using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Pumatool
{
    public partial class FormInstaladorOfimatica : Form
    {
        private CheckedListBox programasList;
        private CheckBox chkInstalacionAutomatica;
        private Button btnInstalar;
        private Label lblTitulo;
        private Panel panelFondo;
        private PictureBox picLogo;

        public FormInstaladorOfimatica()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Instalador de Ofimática";
            this.Size = new Size(420, 510);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(235, 237, 245);

            panelFondo = new Panel()
            {
                BackColor = Color.White,
                Size = new Size(370, 440),
                Location = new Point(25, 25)
            };
            panelFondo.Region = System.Drawing.Region.FromHrgn(
                Pumatool.FormPrincipal.CreateRoundRectRgn(0, 0, panelFondo.Width, panelFondo.Height, 20, 20));
            this.Controls.Add(panelFondo);

            picLogo = new PictureBox()
            {
                Image = Image.FromFile(@"Resources\Iconos\ofimatica.png"),
                Size = new Size(50, 50),
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(160, 10)
            };
            panelFondo.Controls.Add(picLogo);

            lblTitulo = new Label()
            {
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(54, 33, 89),
                Text = "Instalador de Ofimática",
                AutoSize = true,
                Location = new Point(75, 65)
            };
            panelFondo.Controls.Add(lblTitulo);

            programasList = new CheckedListBox()
            {
                Location = new Point(30, 110),
                Size = new Size(305, 180),
                CheckOnClick = true,
                Font = new Font("Segoe UI", 10F)
            };

            programasList.Items.Add("Office 365");
            programasList.Items.Add("Office 2019");
            programasList.Items.Add("Chrome");
            programasList.Items.Add("Firefox");
            programasList.Items.Add("Brave");
            programasList.Items.Add("Adobe");
            programasList.Items.Add("PDF Pro");
            programasList.Items.Add("Visual Studio Code");

            panelFondo.Controls.Add(programasList);

            chkInstalacionAutomatica = new CheckBox()
            {
                Text = "Instalación automática (todo de golpe)",
                Location = new Point(30, 302),
                Size = new Size(300, 26),
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(60, 64, 75)
            };
            panelFondo.Controls.Add(chkInstalacionAutomatica);

            btnInstalar = new Button()
            {
                Text = "Instalar Seleccionados",
                Location = new Point(90, 350),
                Size = new Size(190, 44),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(54, 33, 89),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnInstalar.FlatAppearance.BorderSize = 0;
            btnInstalar.Region = System.Drawing.Region.FromHrgn(
                Pumatool.FormPrincipal.CreateRoundRectRgn(0, 0, btnInstalar.Width, btnInstalar.Height, 14, 14));
            btnInstalar.Click += BtnInstalar_Click;
            panelFondo.Controls.Add(btnInstalar);

            // Botón cerrar
            Button btnCerrar = new Button()
            {
                Text = "X",
                Location = new Point(panelFondo.Width - 38, 8),
                Size = new Size(30, 30),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(231, 76, 60),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => this.Close();
            panelFondo.Controls.Add(btnCerrar);
        }

        private void BtnInstalar_Click(object sender, EventArgs e)
        {
            if (programasList.CheckedItems.Count == 0)
            {
                MessageBox.Show("Selecciona al menos un programa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (chkInstalacionAutomatica.Checked)
                InstalarTodoDeGolpe(programasList);
            else
                InstalarUnoPorUno(programasList);
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
                        logFinal.AppendLine($"{programa}: {(exito ? "Instalado correctamente." : "Falló la instalación.")}");
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
                                logFinal.AppendLine($"{programa}: Código de salida {proc.ExitCode}");
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

        private void InstalarTodoDeGolpe(CheckedListBox programasList)
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
                        logFinal.AppendLine($"{programa}: {(exito ? "Instalado correctamente." : "Falló la instalación.")}");
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

                            Process proc = Process.Start(psi);
                            logFinal.AppendLine($"{programa}: Instalación iniciada.");
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

                if (Directory.Exists(destino))
                    Directory.Delete(destino, true);

                CopiarDirectorio(origen, destino);

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