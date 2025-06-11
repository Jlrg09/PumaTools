using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Pumatool
{
    public partial class FormInstaladorOfimatica : Form
    {
        public FormInstaladorOfimatica()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Instalador de Ofimática";
            this.Size = new System.Drawing.Size(400, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            CheckedListBox programasList = new CheckedListBox()
            {
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(340, 250),
                CheckOnClick = true
            };

            programasList.Items.Add("Office 365");
            programasList.Items.Add("Office 2019");
            programasList.Items.Add("Chrome");
            programasList.Items.Add("Firefox");
            programasList.Items.Add("Brave");
            programasList.Items.Add("Adobe");
            programasList.Items.Add("PDF Pro");
            programasList.Items.Add("Visual Studio Code");

            this.Controls.Add(programasList);

            Button btnInstalar = new Button()
            {
                Text = "Instalar Seleccionados",
                Location = new System.Drawing.Point(100, 300),
                Size = new System.Drawing.Size(180, 40)
            };

            btnInstalar.Click += (s, e) => InstalarUnoPorUno(programasList);
            this.Controls.Add(btnInstalar);
        }

        private void InstalarUnoPorUno(CheckedListBox programasList)
        {
            StringBuilder logFinal = new StringBuilder();

            foreach (var item in programasList.CheckedItems)
            {
                string programa = item.ToString();

                try
                {
                    if (programa == "Office 2019")
                    {
                        bool exito = InstalarOffice2019();
                        if (exito)
                            logFinal.AppendLine($"{programa}: Instalado correctamente.");
                        else
                            logFinal.AppendLine($"{programa}: Falló la instalación.");
                    }
                    else
                    {
                        string ruta = ObtenerRutaPrograma(programa);
                        if (File.Exists(ruta))
                        {
                            ProcessStartInfo psi = new ProcessStartInfo()
                            {
                                FileName = ruta,
                                UseShellExecute = true,
                                Verb = "runas"
                            };

                            using (Process proc = Process.Start(psi))
                            {
                                proc.WaitForExit();
                                if (proc.ExitCode == 0)
                                {
                                    logFinal.AppendLine($"{programa}: Instalado correctamente.");
                                }
                                else
                                {
                                    logFinal.AppendLine($"{programa}: El instalador terminó con código {proc.ExitCode}.");
                                }
                            }
                        }
                        else
                        {
                            logFinal.AppendLine($"{programa}: No se encontró el instalador.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logFinal.AppendLine($"{programa}: Error -> {ex.Message}");
                }
            }

            MessageBox.Show(logFinal.ToString(), "Resumen de Instalación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string ObtenerRutaPrograma(string programa)
        {
            string recursos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/Instalables");
            switch (programa)
            {
                case "Office 365": return Path.Combine(recursos, "office365.exe");
                case "Chrome": return Path.Combine(recursos, "chrome.exe");
                case "Firefox": return Path.Combine(recursos, "firefox.exe");
                case "Brave": return Path.Combine(recursos, "brave.exe");
                case "Adobe": return Path.Combine(recursos, "adobe.exe");
                case "PDF Pro": return Path.Combine(recursos, "pdfpro.exe");
                case "Visual Studio Code": return Path.Combine(recursos, "vscode.exe");
                default: return "";
            }
        }

        private bool InstalarOffice2019()
        {
            try
            {
                string origen = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Instalables\Office2019");
                string destino = @"C:\Office2019";

                // Si ya existe, eliminamos primero la carpeta destino
                if (Directory.Exists(destino))
                    Directory.Delete(destino, true);

                CopiarDirectorio(origen, destino);

                // Ejecutar setup /configure configuracion.xml
                string setupPath = Path.Combine(destino, "setup.exe");
                string configPath = Path.Combine(destino, "configuracion.xml");

                if (!File.Exists(setupPath) || !File.Exists(configPath))
                {
                    MessageBox.Show("No se encontraron los archivos necesarios para instalar Office 2019.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = setupPath,
                    Arguments = "/configure configuracion.xml",
                    WorkingDirectory = destino,
                    UseShellExecute = true,
                    Verb = "runas"
                };

                using (Process proc = Process.Start(psi))
                {
                    proc.WaitForExit();

                    if (proc.ExitCode != 0)
                    {
                        MessageBox.Show($"La instalación de Office 2019 terminó con código {proc.ExitCode}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                // Limpiar la carpeta temporal
                Directory.Delete(destino, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error durante instalación de Office 2019: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void CopiarDirectorio(string origenDir, string destinoDir)
        {
            Directory.CreateDirectory(destinoDir);

            foreach (string archivo in Directory.GetFiles(origenDir))
            {
                string nombreArchivo = Path.GetFileName(archivo);
                string destinoArchivo = Path.Combine(destinoDir, nombreArchivo);
                File.Copy(archivo, destinoArchivo, true);
            }

            foreach (string subDir in Directory.GetDirectories(origenDir))
            {
                string nombreSubDir = Path.GetFileName(subDir);
                string nuevoDestinoSubDir = Path.Combine(destinoDir, nombreSubDir);
                CopiarDirectorio(subDir, nuevoDestinoSubDir);
            }
        }
    }
}
