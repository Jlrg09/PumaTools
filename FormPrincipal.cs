using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace Pumatool
{
    public partial class FormPrincipal : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public FormPrincipal()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 16, 16));
        }

        private void BarraSuperior_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0xA1, 0x2, 0);
        }
        private void BtnCerrar_Click(object sender, EventArgs e) { this.Close(); }
        private void BtnMinimizar_Click(object sender, EventArgs e) { this.WindowState = FormWindowState.Minimized; }
        private void BtnMaximizar_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void btnRestaurarUsuario_Click(object sender, EventArgs e)
        {
            using (var seleccionar = new FormSeleccionarUsuario())
            {
                if (seleccionar.ShowDialog() == DialogResult.OK)
                {
                    using (var carga = new FormPantallaCarga())
                    {
                        carga.Show();
                        Application.DoEvents(); // Muestra la pantalla antes de iniciar
                        RestaurarUsuario(seleccionar.UsuarioSeleccionado, carga);
                        carga.Close();
                    }
                }
            }
        }

        private void RestaurarUsuario(string usuario, FormPantallaCarga carga = null)
        {
            try
            {
                void Mensaje(string txt)
                {
                    if (carga != null) carga.SetMensaje(txt);
                }
                var confirmar = MessageBox.Show(
                    $"¿Está seguro que desea eliminar y recrear el usuario {usuario}?\nEsto eliminará TODA la información de TODAS las carpetas de perfil con ese nombre y variantes.",
                    "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmar == DialogResult.No)
                    return;

                Mensaje("Cerrando sesión y procesos de usuario...");
                CerrarSesionUsuario(usuario);
                KillProcesosUsuario(usuario);
                Mensaje("Desmontando perfil del registro...");
                UnloadUserProfileRegistryHive(usuario);

                Mensaje("Eliminando carpetas de perfil...");
                // 1. Eliminar TODAS las carpetas que comiencen con el nombre del usuario en C:\Users
                string usuariosPath = @"C:\Users";
                if (Directory.Exists(usuariosPath))
                {
                    foreach (string carpeta in Directory.GetDirectories(usuariosPath, usuario + "*", SearchOption.TopDirectoryOnly))
                    {
                        try
                        {
                            Mensaje($"Eliminando: {carpeta}");
                            TomarPermisos(carpeta);
                            ForzarPermisosRecursivo(carpeta);
                            EliminarDirectorioForzado(carpeta);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error eliminando carpeta de perfil: {carpeta}\n{ex.Message}");
                        }
                    }
                }

                Mensaje("Eliminando claves de registro del perfil...");
                // 2. Eliminar todas las claves de ProfileList que apunten a alguna de esas carpetas
                string keyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";
                using (RegistryKey profileListKey = Registry.LocalMachine.OpenSubKey(keyPath, true))
                {
                    var clavesAEliminar = new List<string>();
                    foreach (string subKeyName in profileListKey.GetSubKeyNames())
                    {
                        using (RegistryKey subKey = profileListKey.OpenSubKey(subKeyName))
                        {
                            string profileImagePath = subKey.GetValue("ProfileImagePath") as string;
                            if (!string.IsNullOrEmpty(profileImagePath) &&
                                profileImagePath.StartsWith($@"C:\Users\{usuario}", StringComparison.OrdinalIgnoreCase))
                            {
                                clavesAEliminar.Add(subKeyName);
                            }
                        }
                    }
                    foreach (var subKeyName in clavesAEliminar)
                    {
                        try
                        {
                            profileListKey.DeleteSubKeyTree(subKeyName);
                        }
                        catch { }
                    }
                }

                Mensaje("Eliminando cuentas de usuario...");
                // 3. Eliminar el usuario y todas sus cuentas variantes (Ej: Usuario, Usuario.PCNAME, etc)
                foreach (var nombre in ObtenerUsuariosWindows(usuario))
                {
                    EjecutarComando($"net user \"{nombre}\" /delete");
                }

                Mensaje("Creando cuenta de usuario limpia...");
                // 4. Crear el usuario "limpio"
                EjecutarComando($"net user \"{usuario}\" \"\" /add");
                EjecutarComando($"net localgroup Users \"{usuario}\" /add");

                Mensaje("¡Finalizado!");
                MessageBox.Show($"El usuario {usuario} y todas sus variantes fueron restaurados correctamente.\nLa carpeta será C:\\Users\\{usuario} cuando inicie sesión por primera vez.",
                    "Proceso Completado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Mata todos los procesos del usuario antes de intentar borrar la carpeta
        private void KillProcesosUsuario(string username)
        {
            try
            {
                var procesos = Process.GetProcesses();
                foreach (var proc in procesos)
                {
                    try
                    {
                        if (proc.SessionId == 0) continue; // Sistema
                        string owner = GetProcessOwner(proc.Id);
                        if (!string.IsNullOrEmpty(owner) && owner.Equals(username, StringComparison.OrdinalIgnoreCase))
                        {
                            proc.Kill();
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }

        // Obtiene el usuario propietario de un proceso
        private string GetProcessOwner(int processId)
        {
            try
            {
                var query = $"Select * From Win32_Process Where ProcessID = {processId}";
                var searcher = new System.Management.ManagementObjectSearcher(query);
                foreach (System.Management.ManagementObject process in searcher.Get())
                {
                    object[] outParameters = new object[2];
                    int returnVal = Convert.ToInt32(process.InvokeMethod("GetOwner", outParameters));
                    if (returnVal == 0)
                        return outParameters[0].ToString();
                }
            }
            catch { }
            return null;
        }

        // Desmonta el hive del perfil de usuario si quedó enganchado (por ejemplo, ntuser.dat)
        private void UnloadUserProfileRegistryHive(string username)
        {
            try
            {
                string keyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";
                using (RegistryKey profileListKey = Registry.LocalMachine.OpenSubKey(keyPath, false))
                {
                    foreach (string subKeyName in profileListKey.GetSubKeyNames())
                    {
                        using (RegistryKey subKey = profileListKey.OpenSubKey(subKeyName))
                        {
                            string profileImagePath = subKey.GetValue("ProfileImagePath") as string;
                            if (profileImagePath != null && profileImagePath.EndsWith(@"\" + username, StringComparison.OrdinalIgnoreCase))
                            {
                                string sid = subKeyName;
                                if (Registry.Users.OpenSubKey(sid) != null)
                                {
                                    EjecutarComando($"reg unload \"HKEY_USERS\\{sid}\"");
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        // Cambia los permisos de todos los archivos/carpetas dentro de la carpeta
        private void ForzarPermisosRecursivo(string dir)
        {
            EjecutarComando($"icacls \"{dir}\" /grant Administrators:F /T /C /Q");
        }

        // Elimina carpetas incluso si hay archivos solo lectura, ocultos, bloqueados, etc. usando métodos nativos y fallback a cmd
        private void EliminarDirectorioForzado(string ruta)
        {
            try
            {
                QuitarSoloLecturaOculto(ruta);
                Directory.Delete(ruta, true);
            }
            catch (Exception ex1)
            {
                try
                {
                    EjecutarComando($"rd /s /q \"{ruta}\"");
                    if (Directory.Exists(ruta))
                        throw new IOException("No se pudo borrar el directorio con rd /s /q.");
                }
                catch (Exception ex2)
                {
                    MessageBox.Show("Error al eliminar directorio: " + ex1.Message + "\n" + ex2.Message);
                }
            }
        }

        // Quita atributos solo lectura y oculto a todo el contenido de la carpeta
        private void QuitarSoloLecturaOculto(string ruta)
        {
            foreach (string file in Directory.GetFiles(ruta, "*", SearchOption.AllDirectories))
            {
                try
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                }
                catch { }
            }
            foreach (string dir in Directory.GetDirectories(ruta, "*", SearchOption.AllDirectories))
            {
                try
                {
                    File.SetAttributes(dir, FileAttributes.Normal);
                }
                catch { }
            }
            try
            {
                File.SetAttributes(ruta, FileAttributes.Normal);
            }
            catch { }
        }

        private List<string> ObtenerUsuariosWindows(string nombreBase)
        {
            var lista = new List<string>();
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c net user")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process proc = Process.Start(psi))
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();

                    var lineas = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    bool tabla = false;
                    foreach (var linea in lineas)
                    {
                        if (linea.StartsWith("---")) { tabla = true; continue; }
                        if (!tabla) continue;
                        if (string.IsNullOrWhiteSpace(linea)) break;
                        var usuariosEnLinea = linea.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var usuario in usuariosEnLinea)
                        {
                            if (usuario.Equals(nombreBase, StringComparison.OrdinalIgnoreCase) ||
                                usuario.StartsWith(nombreBase + ".", StringComparison.OrdinalIgnoreCase))
                            {
                                lista.Add(usuario);
                            }
                        }
                    }
                }
            }
            catch { }
            return lista;
        }

        private void TomarPermisos(string ruta)
        {
            EjecutarComando($"takeown /F \"{ruta}\" /R /D Y");
            EjecutarComando($"icacls \"{ruta}\" /grant administrators:F /T /C /Q");
        }

        private void CerrarSesionUsuario(string usuario)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c query session")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process proc = Process.Start(psi))
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();

                    string[] lineas = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string linea in lineas)
                    {
                        if (linea.Contains(usuario))
                        {
                            string[] partes = linea.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            string sessionId = partes[2];
                            EjecutarComando($"logoff {sessionId}");
                            MessageBox.Show($"Sesión del usuario {usuario} cerrada correctamente.", "Sesión Cerrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                    }
                }
            }
            catch
            {
                // Ignorar si no está logueado
            }
        }

        private void EjecutarComando(string comando)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + comando)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Verb = "runas"
            };

            using (Process proc = Process.Start(psi))
            {
                proc.WaitForExit();
            }
        }

        private void LimpiarTemporales()
        {
            try
            {
                string tempPath = Path.GetTempPath();

                string[] archivos = Directory.GetFiles(tempPath);
                int archivosEliminados = 0;
                foreach (string archivo in archivos)
                {
                    try { File.Delete(archivo); archivosEliminados++; }
                    catch { }
                }

                string[] carpetas = Directory.GetDirectories(tempPath);
                int carpetasEliminadas = 0;
                foreach (string carpeta in carpetas)
                {
                    try { Directory.Delete(carpeta, true); carpetasEliminadas++; }
                    catch { }
                }

                MessageBox.Show($"Se eliminaron {archivosEliminados} archivos y {carpetasEliminadas} carpetas temporales.",
                    "Limpieza completada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar los temporales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOptimizarWindows_Click(object sender, EventArgs e)
        {
            try
            {
                string rutaTemp = Path.Combine(Path.GetTempPath(), "OptimizarWindows10.bat");

                File.WriteAllText(rutaTemp, ObtenerScriptOptimizar());

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = rutaTemp,
                    UseShellExecute = true,
                    Verb = "runas"
                };
                Process proceso = Process.Start(psi);
                proceso.WaitForExit();

                File.Delete(rutaTemp);
                MessageBox.Show("Optimización completada correctamente.", "Optimización", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar optimización: " + ex.Message);
            }
        }
        private void btnOptimizarWindows11_Click(object sender, EventArgs e)
        {
            try
            {
                string rutaTemp = Path.Combine(Path.GetTempPath(), "OptimizarWindows11.bat");

                File.WriteAllText(rutaTemp, ObtenerScriptOptimizarWindows11());

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = rutaTemp,
                    UseShellExecute = true,
                    Verb = "runas"
                };
                Process proceso = Process.Start(psi);
                proceso.WaitForExit();

                File.Delete(rutaTemp);
                MessageBox.Show("Optimización de Windows 11 completada correctamente.", "Optimización", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar optimización: " + ex.Message);
            }
        }
        private string ObtenerScriptOptimizar()
        {
            return @"@echo off
        title OPTIMIZACIÓN AVANZADA PARA WINDOWS 10
        echo ========================================
        echo      APLICANDO OPTIMIZACIONES AVANZADAS
        echo ========================================
        timeout /t 2

        :: Desactivar Telemetría
        sc config ""DiagTrack"" start= disabled
        sc stop ""DiagTrack""
        reg add ""HKLM\Software\Policies\Microsoft\Windows\DataCollection"" /v AllowTelemetry /t REG_DWORD /d 0 /f

        :: Desactivar Cortana
        reg add ""HKLM\Software\Policies\Microsoft\Windows\Windows Search"" /v AllowCortana /t REG_DWORD /d 0 /f

        :: Deshabilitar servicios innecesarios
        sc config ""SysMain"" start= disabled
        sc stop ""SysMain""
        sc config ""WSearch"" start= disabled
        sc stop ""WSearch""
        sc config ""MapsBroker"" start= disabled
        sc config ""WdiServiceHost"" start= disabled
        sc config ""WdiSystemHost"" start= disabled

        :: Mejorar rendimiento de energía
        powercfg -setactive SCHEME_MIN

        :: Ajustar efectos visuales
        reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"" /v VisualFXSetting /t REG_DWORD /d 2 /f
        reg add ""HKCU\Control Panel\Desktop"" /v UserPreferencesMask /t REG_BINARY /d 9012038010000000 /f

        :: Desactivar Widgets y recomendaciones
        reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\Feeds"" /v ShellFeedsTaskbarViewMode /t REG_DWORD /d 2 /f
        reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications"" /v GlobalUserDisabled /t REG_DWORD /d 1 /f
        reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"" /v SubscribedContent-338388Enabled /t REG_DWORD /d 0 /f
        reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"" /v SubscribedContent-353694Enabled /t REG_DWORD /d 0 /f
        reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement"" /v ScoobeSystemSettingEnabled /t REG_DWORD /d 0 /f
        reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent"" /v DisableTailoredExperiencesWithDiagnosticData /t REG_DWORD /d 1 /f
        reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent"" /v DisableSoftLanding /t REG_DWORD /d 1 /f

        :: Desactivar Notificaciones
        reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\PushNotifications"" /v ToastEnabled /t REG_DWORD /d 0 /f

        :: Desactivar Juegos
        reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR"" /v AllowGameDVR /t REG_DWORD /d 0 /f
        reg add ""HKCU\Software\Microsoft\GameBar"" /v AllowAutoGameMode /t REG_DWORD /d 0 /f

        :: Desactivar Personalización de escritura y estado
        reg add ""HKCU\Software\Microsoft\InputPersonalization"" /v RestrictImplicitInkCollection /t REG_DWORD /d 1 /f
        reg add ""HKCU\Software\Microsoft\InputPersonalization"" /v RestrictImplicitTextCollection /t REG_DWORD /d 1 /f

        :: Desactivar activación por voz
        reg add ""HKCU\Software\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps"" /v AgentActivationEnabled /t REG_DWORD /d 0 /f

        :: Desactivar ubicación
        reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location"" /v Value /t REG_SZ /d Deny /f

        :: Desactivar llamadas, mensajería y correos
        reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\phoneCall"" /v Value /t REG_SZ /d Deny /f
        reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\contacts"" /v Value /t REG_SZ /d Deny /f
        reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\chat"" /v Value /t REG_SZ /d Deny /f
        reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\email"" /v Value /t REG_SZ /d Deny /f

        :: Desactivar diagnóstico de aplicaciones
        reg add ""HKLM\Software\Policies\Microsoft\Windows\AppCompat"" /v DisableInventory /t REG_DWORD /d 1 /f

        :: Limpiar temporales
        echo Limpiando archivos temporales...
        del /s /f /q %temp%\* >nul 2>&1
        for /d %%x in (%temp%\*) do @rd /s /q ""%%x"" >nul 2>&1
        del /s /f /q C:\Windows\Temp\* >nul 2>&1
        for /d %%x in (C:\Windows\Temp\*) do @rd /s /q ""%%x"" >nul 2>&1
        del /s /f /q C:\Windows\Prefetch\* >nul 2>&1
        cleanmgr /sagerun:1

        echo.
        echo ========================================
        echo      OPTIMIZACIÓN COMPLETA - WIN10 OK
        echo ========================================
        pause";
        }
        private string ObtenerScriptOptimizarWindows11()
        {
            return @"@echo off
        title OPTIMIZACIÓN AVANZADA PARA WINDOWS 11
        echo ========================================
        echo      APLICANDO OPTIMIZACIONES W11
        echo ========================================
        timeout /t 2

        :: Desactivar Telemetría
        sc config ""DiagTrack"" start= disabled
        sc stop ""DiagTrack""
        reg add ""HKLM\Software\Policies\Microsoft\Windows\DataCollection"" /v AllowTelemetry /t REG_DWORD /d 0 /f

        :: Desactivar Widgets
        reg add ""HKLM\Software\Policies\Microsoft\Windows\Windows Feeds"" /v EnableFeeds /t REG_DWORD /d 0 /f

        :: Desactivar Cortana
        reg add ""HKLM\Software\Policies\Microsoft\Windows\Windows Search"" /v AllowCortana /t REG_DWORD /d 0 /f

        :: Deshabilitar SysMain
        sc config ""SysMain"" start= disabled
        sc stop ""SysMain""
        sc config ""WSearch"" start= disabled
        sc stop ""WSearch""
        sc config ""MapsBroker"" start= disabled
        sc config ""WdiServiceHost"" start= disabled
        sc config ""WdiSystemHost"" start= disabled

        :: Mejorar energía
        powercfg -setactive SCHEME_MIN

        :: Ajustar efectos visuales
        reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"" /v VisualFXSetting /t REG_DWORD /d 2 /f
        reg add ""HKCU\Control Panel\Desktop"" /v UserPreferencesMask /t REG_BINARY /d 9012038010000000 /f

        :: Desactivar recomendaciones
        reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent"" /v DisableSoftLanding /t REG_DWORD /d 1 /f
        reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent"" /v DisableTailoredExperiencesWithDiagnosticData /t REG_DWORD /d 1 /f

        :: Desactivar Notificaciones
        reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\PushNotifications"" /v ToastEnabled /t REG_DWORD /d 0 /f

        :: Desactivar Juegos
        reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR"" /v AllowGameDVR /t REG_DWORD /d 0 /f
        reg add ""HKCU\Software\Microsoft\GameBar"" /v AllowAutoGameMode /t REG_DWORD /d 0 /f

        :: Desactivar Personalización de escritura y estado
        reg add ""HKCU\Software\Microsoft\InputPersonalization"" /v RestrictImplicitInkCollection /t REG_DWORD /d 1 /f
        reg add ""HKCU\Software\Microsoft\InputPersonalization"" /v RestrictImplicitTextCollection /t REG_DWORD /d 1 /f

        :: Desactivar activación por voz
        reg add ""HKCU\Software\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps"" /v AgentActivationEnabled /t REG_DWORD /d 0 /f

        :: Desactivar ubicación
        reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location"" /v Value /t REG_SZ /d Deny /f

        :: Desactivar llamadas, mensajería y correos
        reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\phoneCall"" /v Value /t REG_SZ /d Deny /f
        reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\contacts"" /v Value /t REG_SZ /d Deny /f
        reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\chat"" /v Value /t REG_SZ /d Deny /f
        reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\email"" /v Value /t REG_SZ /d Deny /f

        :: Desactivar diagnóstico de aplicaciones
        reg add ""HKLM\Software\Policies\Microsoft\Windows\AppCompat"" /v DisableInventory /t REG_DWORD /d 1 /f

        :: Limpiar temporales
        echo Limpiando archivos temporales...
        del /s /f /q %temp%\* >nul 2>&1
        for /d %%x in (%temp%\*) do @rd /s /q ""%%x"" >nul 2>&1
        del /s /f /q C:\Windows\Temp\* >nul 2>&1
        for /d %%x in (C:\Windows\Temp\*) do @rd /s /q ""%%x"" >nul 2>&1
        del /s /f /q C:\Windows\Prefetch\* >nul 2>&1

        echo.
        echo ========================================
        echo     OPTIMIZACIÓN COMPLETA - WINDOWS 11 OK
        echo ========================================
        pause";
        }

        private void PonerInicioIzquierda()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true);
                if (key != null)
                {
                    key.SetValue("TaskbarAl", 0, RegistryValueKind.DWord);
                    key.Close();
                }

                foreach (Process proc in Process.GetProcessesByName("explorer"))
                {
                    proc.Kill();
                }
                Process.Start("explorer.exe");

                MessageBox.Show("El menú de inicio ha sido movido a la izquierda.", "Inicio a la izquierda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mover el inicio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInstalarOfimatica_Click(object sender, EventArgs e)
        {
            using (var formInstalar = new FormInstaladorOfimatica())
            {
                formInstalar.ShowDialog();
            }
        }

        private void LimpiarRegistrosOffice()
        {
            try
            {
                var confirmar = MessageBox.Show("Este proceso eliminará registros de Office antiguos para solucionar errores de instalación.\n¿Desea continuar?",
                    "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmar == DialogResult.No)
                    return;

                string[] clavesOffice = new string[]
                {
                    @"SOFTWARE\Microsoft\Office",
                    @"SOFTWARE\WOW6432Node\Microsoft\Office",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                    @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
                };

                foreach (string clave in clavesOffice)
                {
                    try
                    {
                        Registry.LocalMachine.DeleteSubKeyTree(clave, false);
                    }
                    catch { }
                }

                MessageBox.Show("Los registros de Office han sido limpiados correctamente.",
                    "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar registros de Office: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}