using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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
                string ruta = ObtenerRutaPrograma(programa);

                if (File.Exists(ruta))
                {
                    try
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
                    catch (Exception ex)
                    {
                        logFinal.AppendLine($"{programa}: Error -> {ex.Message}");
                    }
                }
                else
                {
                    logFinal.AppendLine($"{programa}: No se encontró el instalador.");
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
                case "Office 2019": return Path.Combine(recursos, "office2019.exe");
                case "Chrome": return Path.Combine(recursos, "chrome.exe");
                case "Firefox": return Path.Combine(recursos, "firefox.exe");
                case "Brave": return Path.Combine(recursos, "brave.exe");
                case "Adobe": return Path.Combine(recursos, "adobe.exe");
                case "PDF Pro": return Path.Combine(recursos, "pdfpro.exe");
                case "Visual Studio Code": return Path.Combine(recursos, "vscode.exe");
                default: return "";
            }
        }
    }
}
