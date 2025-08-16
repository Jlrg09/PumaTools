using System;
using System.Diagnostics;
using Microsoft.Win32;

public static class WindowsOptimizer
{
    public static string GetWindowsVersion()
    {
        string productName = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "");
        if (productName.Contains("Windows 10")) return "10";
        if (productName.Contains("Windows 11")) return "11";
        return "Otro";
    }

    public static void Optimizar()
    {
        // Borra todos los temporales
        try
        {
            string tempPath = System.IO.Path.GetTempPath();
            foreach (string file in System.IO.Directory.GetFiles(tempPath, "*", System.IO.SearchOption.AllDirectories))
                try { System.IO.File.Delete(file); } catch { }
            foreach (string dir in System.IO.Directory.GetDirectories(tempPath, "*", System.IO.SearchOption.AllDirectories))
                try { System.IO.Directory.Delete(dir, true); } catch { }
        }
        catch { }

        // Desactiva notificaciones
        Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled", 0, RegistryValueKind.DWord);

        // Desactiva modo juego
        Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0, RegistryValueKind.DWord);
        Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameModeEnabled", 0, RegistryValueKind.DWord);

        // Desactiva widgets (Windows 11)
        if (GetWindowsVersion() == "11")
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 0, RegistryValueKind.DWord);
        }

        // Quitar apps en segundo plano
        Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications", "GlobalUserDisabled", 1, RegistryValueKind.DWord);

        // Desactiva Cortana
        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0, RegistryValueKind.DWord);

        // Desactiva SysMain, WSearch, etc.
        EjecutarComando("sc config \"SysMain\" start= disabled");
        EjecutarComando("sc stop \"SysMain\"");
        EjecutarComando("sc config \"WSearch\" start= disabled");
        EjecutarComando("sc stop \"WSearch\"");
        EjecutarComando("sc config \"MapsBroker\" start= disabled");
        EjecutarComando("sc config \"WdiServiceHost\" start= disabled");
        EjecutarComando("sc config \"WdiSystemHost\" start= disabled");

        // Energía
        EjecutarComando("powercfg -setactive SCHEME_MIN");

        // Efectos visuales
        Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 2, RegistryValueKind.DWord);

        // Más optimizaciones puedes agregarlas aquí...

        System.Windows.Forms.MessageBox.Show("Optimización completada.", "Optimización", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
    }

    private static void EjecutarComando(string cmd)
    {
        var psi = new ProcessStartInfo("cmd.exe", "/c " + cmd)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            Verb = "runas"
        };
        using (var proc = Process.Start(psi))
        {
            proc.WaitForExit();
        }
    }
}