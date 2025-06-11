using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace Pumatool
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnRestaurarUsuario_Click(object sender, EventArgs e)
        {
            using (var seleccionar = new FormSeleccionarUsuario())
            {
                if (seleccionar.ShowDialog() == DialogResult.OK)
                {
                    RestaurarUsuario(seleccionar.UsuarioSeleccionado);
                }
            }
        }

        private void RestaurarUsuario(string usuario)
        {
            try
            {
                var confirmar = MessageBox.Show($"¿Está seguro que desea eliminar y recrear el usuario {usuario}?",
                    "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmar == DialogResult.No)
                    return;

                CerrarSesionUsuario(usuario);

                string rutaPerfil = $@"C:\Users\{usuario}";
                if (Directory.Exists(rutaPerfil))
                {
                    TomarPermisos(rutaPerfil);
                    EliminarDirectorio(rutaPerfil);
                }

                string keyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";
                using (RegistryKey profileListKey = Registry.LocalMachine.OpenSubKey(keyPath, true))
                {
                    foreach (string subKeyName in profileListKey.GetSubKeyNames())
                    {
                        using (RegistryKey subKey = profileListKey.OpenSubKey(subKeyName))
                        {
                            string profileImagePath = subKey.GetValue("ProfileImagePath") as string;
                            if (profileImagePath != null && profileImagePath.Contains(usuario))
                            {
                                profileListKey.DeleteSubKeyTree(subKeyName);
                                break;
                            }
                        }
                    }
                }

                EjecutarComando($"net user {usuario} /delete");
                EjecutarComando($"net user {usuario} \"\" /add");
                EjecutarComando($"net localgroup Users {usuario} /add");

                MessageBox.Show($"El usuario {usuario} fue restaurado correctamente.",
                    "Proceso Completado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarDirectorio(string ruta)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(ruta);

                foreach (FileInfo file in di.GetFiles("*", SearchOption.AllDirectories))
                {
                    file.Attributes = FileAttributes.Normal;
                    file.Delete();
                }

                foreach (DirectoryInfo dir in di.GetDirectories("*", SearchOption.AllDirectories))
                {
                    EliminarDirectorio(dir.FullName);
                }

                di.Attributes = FileAttributes.Normal;
                di.Delete(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar directorio: " + ex.Message);
            }
        }

        private void TomarPermisos(string ruta)
        {
            EjecutarComando($"takeown /F \"{ruta}\" /R /D Y");
            EjecutarComando($"icacls \"{ruta}\" /grant administrators:F /T");
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

            :: Desactivar Cortana (si está presente)
            reg add ""HKLM\Software\Policies\Microsoft\Windows\Windows Search"" /v AllowCortana /t REG_DWORD /d 0 /f

            :: Deshabilitar SysMain (Superfetch)
            sc config ""SysMain"" start= disabled
            sc stop ""SysMain""

            :: Desactivar servicios innecesarios
            sc config ""WSearch"" start= disabled
            sc stop ""WSearch""
            sc config ""MapsBroker"" start= disabled
            sc config ""WdiServiceHost"" start= disabled
            sc config ""WdiSystemHost"" start= disabled

            :: Mejorar rendimiento de energía
            powercfg -setactive SCHEME_MIN

            :: Ajustar efectos visuales para mejor rendimiento
            reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"" /v VisualFXSetting /t REG_DWORD /d 2 /f
            reg add ""HKCU\Control Panel\Desktop"" /v UserPreferencesMask /t REG_BINARY /d 9012038010000000 /f

            :: Desactivar recomendaciones del menú inicio
            reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent"" /v DisableSoftLanding /t REG_DWORD /d 1 /f
            reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent"" /v DisableTailoredExperiencesWithDiagnosticData /t REG_DWORD /d 1 /f

            :: Limpiar archivos temporales y prefetch
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
                // Cambiar en el registro
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true);
                if (key != null)
                {
                    key.SetValue("TaskbarAl", 0, RegistryValueKind.DWord);
                    key.Close();
                }

                // Reiniciar el Explorador para aplicar cambios
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

        private string ObtenerScriptOptimizar()
        {
            return @"@echo off
            title OPTIMIZACIÓN AVANZADA PARA WINDOWS 10
            echo ========================================
            echo      APLICANDO OPTIMIZACIONES AVANZADAS
            echo ========================================
            timeout /t 2

            sc config ""DiagTrack"" start= disabled
            sc stop ""DiagTrack""
            reg add ""HKLM\Software\Policies\Microsoft\Windows\DataCollection"" /v AllowTelemetry /t REG_DWORD /d 0 /f

            reg add ""HKLM\Software\Policies\Microsoft\Windows\Windows Search"" /v AllowCortana /t REG_DWORD /d 0 /f

            sc config ""SysMain"" start= disabled
            sc stop ""SysMain""
            sc config ""WSearch"" start= disabled
            sc stop ""WSearch""
            sc config ""MapsBroker"" start= disabled
            sc config ""WdiServiceHost"" start= disabled
            sc config ""WdiSystemHost"" start= disabled

            powercfg -setactive SCHEME_MIN

            reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"" /v VisualFXSetting /t REG_DWORD /d 2 /f
            reg add ""HKCU\Control Panel\Desktop"" /v UserPreferencesMask /t REG_BINARY /d 9012038010000000 /f

            reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\Feeds"" /v ShellFeedsTaskbarViewMode /t REG_DWORD /d 2 /f

            reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications"" /v GlobalUserDisabled /t REG_DWORD /d 1 /f

            reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"" /v SubscribedContent-338388Enabled /t REG_DWORD /d 0 /f
            reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"" /v SubscribedContent-353694Enabled /t REG_DWORD /d 0 /f
            reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement"" /v ScoobeSystemSettingEnabled /t REG_DWORD /d 0 /f

            reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent"" /v DisableTailoredExperiencesWithDiagnosticData /t REG_DWORD /d 1 /f
            reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent"" /v DisableSoftLanding /t REG_DWORD /d 1 /f

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
        private void btnInstalarOfimatica_Click(object sender, EventArgs e)
        {
            using (var formInstalar = new FormInstaladorOfimatica())
            {
                formInstalar.ShowDialog();
            }
        }

    }
}
