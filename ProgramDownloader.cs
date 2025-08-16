using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;

namespace Pumatool
{
    public class ProgramaInfo
    {
        public string Nombre { get; set; } = "";
        public string Url { get; set; } = "";
        public string Ejecutable { get; set; } = "";
    }

    public static class ProgramDownloader
    {
        private static string ConfigPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Instalables", "descargas.json");
        private static string DownloadDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Instalables");

        public static List<ProgramaInfo> CargarProgramas()
        {
            if (!File.Exists(ConfigPath))
                File.WriteAllText(ConfigPath, "[]");

            var json = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<List<ProgramaInfo>>(json) ?? new List<ProgramaInfo>();
        }

        public static void GuardarProgramas(List<ProgramaInfo> programas)
        {
            var json = JsonSerializer.Serialize(programas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
        }

        public static bool DescargarPrograma(ProgramaInfo programa)
        {
            try
            {
                string destino = Path.Combine(DownloadDir, programa.Ejecutable);
                using (var wc = new WebClient())
                {
                    wc.DownloadFile(programa.Url, destino);
                }
                return File.Exists(destino);
            }
            catch
            {
                return false;
            }
        }

        public static void AgregarPrograma(string nombre, string url, string ejecutable)
        {
            var list = CargarProgramas();
            list.Add(new ProgramaInfo { Nombre = nombre, Url = url, Ejecutable = ejecutable });
            GuardarProgramas(list);
        }
    }
}