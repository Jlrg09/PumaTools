using System.Drawing;
using System.Windows.Forms;

namespace Pumatool
{
    partial class FormBienvenida
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblInstrucciones;
        private Button btnContinuar;
        private Panel panelFondo;
        private PictureBox picLogo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelFondo = new Panel();
            this.lblTitulo = new Label();
            this.lblSubtitulo = new Label();
            this.lblInstrucciones = new Label();
            this.btnContinuar = new Button();
            this.picLogo = new PictureBox();

            // FormBienvenida
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(540, 450);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "PumaTool - Bienvenida";
            this.BackColor = Color.FromArgb(235, 237, 245);
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // panelFondo
            this.panelFondo.BackColor = Color.White;
            this.panelFondo.BorderStyle = BorderStyle.None;
            this.panelFondo.Size = new Size(480, 390);
            this.panelFondo.Location = new Point(30, 25);
            this.panelFondo.Anchor = AnchorStyles.None;
            this.panelFondo.Region = Region.FromHrgn(
                Pumatool.FormPrincipal.CreateRoundRectRgn(0, 0, this.panelFondo.Width, this.panelFondo.Height, 28, 28));
            this.panelFondo.Controls.Add(this.picLogo);
            this.panelFondo.Controls.Add(this.lblTitulo);
            this.panelFondo.Controls.Add(this.lblSubtitulo);
            this.panelFondo.Controls.Add(this.lblInstrucciones);
            this.panelFondo.Controls.Add(this.btnContinuar);
            this.Controls.Add(this.panelFondo);

            // picLogo
            this.picLogo.Image = Image.FromFile(@"Resources\Iconos\pumatool.png");
            this.picLogo.Size = new Size(75, 75);
            this.picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            this.picLogo.Location = new Point(202, 18);

            // lblTitulo
            this.lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(54, 33, 89);
            this.lblTitulo.Text = "PUMATOOLS";
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new Point(132, 100);

            // lblSubtitulo
            this.lblSubtitulo.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            this.lblSubtitulo.ForeColor = Color.FromArgb(120, 120, 120);
            this.lblSubtitulo.Text = "Programado por José Romero";
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Location = new Point(142, 140);

            // lblInstrucciones
            this.lblInstrucciones.Font = new Font("Segoe UI", 12.5F, FontStyle.Regular);
            this.lblInstrucciones.ForeColor = Color.FromArgb(60, 64, 75);
            this.lblInstrucciones.Text = "Bienvenido a PumaTools, la herramienta profesional para dar soporte y optimizar Windows.\n\nHaz clic en Comenzar para iniciar.";
            this.lblInstrucciones.TextAlign = ContentAlignment.MiddleCenter;
            this.lblInstrucciones.Size = new Size(410, 70);
            this.lblInstrucciones.Location = new Point(35, 185);

            // btnContinuar
            this.btnContinuar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnContinuar.Text = "Comenzar";
            this.btnContinuar.Size = new Size(180, 46);
            this.btnContinuar.Location = new Point(150, 295);
            this.btnContinuar.BackColor = Color.FromArgb(54, 33, 89);
            this.btnContinuar.ForeColor = Color.White;
            this.btnContinuar.FlatStyle = FlatStyle.Flat;
            this.btnContinuar.FlatAppearance.BorderSize = 0;
            this.btnContinuar.Cursor = Cursors.Hand;
            this.btnContinuar.Region = Region.FromHrgn(
                Pumatool.FormPrincipal.CreateRoundRectRgn(0, 0, this.btnContinuar.Width, this.btnContinuar.Height, 18, 18));
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);
        }
    }
}