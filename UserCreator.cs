using System.Diagnostics;
using System.Windows.Forms;

public static class UserCreator
{
    public static void CrearUsuario(string nombre, string contrasena)
    {
        EjecutarComando($"net user \"{nombre}\" \"{contrasena}\" /add");
        EjecutarComando($"net localgroup Users \"{nombre}\" /add");
        MessageBox.Show($"Usuario '{nombre}' creado correctamente.", "Usuario creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static void CrearEstudiante() => CrearUsuario("ESTUDIANTE", "");
    public static void CrearDocente() => CrearUsuario("DOCENTE", "");

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