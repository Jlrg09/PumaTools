using System.Drawing;
using System.Windows.Forms;

namespace Pumatool
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelBarraSuperior;
        private Label lblTitulo;
        private PictureBox picAppIcon;
        private Button btnMinimizar;
        private Button btnMaximizar;
        private Button btnCerrar;

        private Panel panelWindows10;
        private Panel panelWindows11;
        private Panel panelGenerales;
        private Button btnLimpiarRegistrosOffice;
        private Button btnOptimizarW10;
        private Button btnOptimizarW11;
        private Button btnInicioIzquierda;
        private Button btnLimpiarTemporales;
        private Button btnRestaurarUsuario;
        private Button btnInstalarOfimatica;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelBarraSuperior = new Panel();
            this.lblTitulo = new Label();
            this.picAppIcon = new PictureBox();
            this.btnMinimizar = new Button();
            this.btnMaximizar = new Button();
            this.btnCerrar = new Button();

            this.ClientSize = new Size(820, 610);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "";
            this.BackColor = Color.FromArgb(245, 246, 250);

            // Barra Superior
            this.panelBarraSuperior.BackColor = Color.FromArgb(36, 37, 42);
            this.panelBarraSuperior.Dock = DockStyle.Top;
            this.panelBarraSuperior.Height = 44;
            this.panelBarraSuperior.MouseDown += BarraSuperior_MouseDown;
            this.Controls.Add(this.panelBarraSuperior);

            // App Icon
            this.picAppIcon.Image = Image.FromFile(@"Resources\Iconos\pumatool.png");
            this.picAppIcon.Size = new Size(34, 34);
            this.picAppIcon.SizeMode = PictureBoxSizeMode.Zoom;
            this.picAppIcon.Location = new Point(8, 5);
            this.panelBarraSuperior.Controls.Add(this.picAppIcon);

            // Título
            this.lblTitulo.Text = "PUMATOOLS PRO V0.2";
            this.lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(255, 255, 255);
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new Point(48, 6);
            this.panelBarraSuperior.Controls.Add(this.lblTitulo);

            // Botón Minimizar
            this.btnMinimizar.FlatStyle = FlatStyle.Flat;
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.Text = "—";
            this.btnMinimizar.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            this.btnMinimizar.ForeColor = Color.White;
            this.btnMinimizar.BackColor = Color.Transparent;
            this.btnMinimizar.Size = new Size(36, 36);
            this.btnMinimizar.Location = new Point(this.ClientSize.Width - 108, 4);
            this.btnMinimizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnMinimizar.Click += BtnMinimizar_Click;
            this.panelBarraSuperior.Controls.Add(this.btnMinimizar);

            // Botón Maximizar
            this.btnMaximizar.FlatStyle = FlatStyle.Flat;
            this.btnMaximizar.FlatAppearance.BorderSize = 0;
            this.btnMaximizar.Text = "☐";
            this.btnMaximizar.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnMaximizar.ForeColor = Color.White;
            this.btnMaximizar.BackColor = Color.Transparent;
            this.btnMaximizar.Size = new Size(36, 36);
            this.btnMaximizar.Location = new Point(this.ClientSize.Width - 72, 4);
            this.btnMaximizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnMaximizar.Click += BtnMaximizar_Click;
            this.panelBarraSuperior.Controls.Add(this.btnMaximizar);

            // Botón Cerrar
            this.btnCerrar.FlatStyle = FlatStyle.Flat;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.Text = "X";
            this.btnCerrar.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnCerrar.ForeColor = Color.White;
            this.btnCerrar.BackColor = Color.FromArgb(231, 76, 60);
            this.btnCerrar.Size = new Size(36, 36);
            this.btnCerrar.Location = new Point(this.ClientSize.Width - 36, 4);
            this.btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnCerrar.Click += BtnCerrar_Click;
            this.panelBarraSuperior.Controls.Add(this.btnCerrar);

            // PANEL WINDOWS 10
            this.panelWindows10 = new Panel();
            this.panelWindows10.BackColor = Color.White;
            this.panelWindows10.BorderStyle = BorderStyle.None;
            this.panelWindows10.Location = new Point(32, 80);
            this.panelWindows10.Size = new Size(245, 112);
            this.panelWindows10.Region = Region.FromHrgn(
                Pumatool.FormPrincipal.CreateRoundRectRgn(0, 0, this.panelWindows10.Width, this.panelWindows10.Height, 16, 16));
            this.Controls.Add(this.panelWindows10);

            Label lblW10 = CrearEtiquetaPanel("Windows 10", 14, 10, Color.FromArgb(33, 150, 243));
            this.panelWindows10.Controls.Add(lblW10);

            this.btnOptimizarW10 = new Button();
            FormatearBoton(this.btnOptimizarW10, "Optimizar Windows 10", 50);
            this.btnOptimizarW10.Image = Image.FromFile(@"Resources\Iconos\optimizarW10.png");
            this.btnOptimizarW10.Click += new System.EventHandler(this.btnOptimizarWindows_Click);
            this.panelWindows10.Controls.Add(this.btnOptimizarW10);

            // PANEL WINDOWS 11
            this.panelWindows11 = new Panel();
            this.panelWindows11.BackColor = Color.White;
            this.panelWindows11.BorderStyle = BorderStyle.None;
            this.panelWindows11.Location = new Point(287, 80);
            this.panelWindows11.Size = new Size(245, 160);
            this.panelWindows11.Region = Region.FromHrgn(
                Pumatool.FormPrincipal.CreateRoundRectRgn(0, 0, this.panelWindows11.Width, this.panelWindows11.Height, 16, 16));
            this.Controls.Add(this.panelWindows11);

            Label lblW11 = CrearEtiquetaPanel("Windows 11", 14, 10, Color.FromArgb(0, 200, 83));
            this.panelWindows11.Controls.Add(lblW11);

            this.btnOptimizarW11 = new Button();
            FormatearBoton(this.btnOptimizarW11, "Optimizar Windows 11", 50);
            this.btnOptimizarW11.Image = Image.FromFile(@"Resources\Iconos\optimizarW11.png");
            this.btnOptimizarW11.Click += new System.EventHandler(this.btnOptimizarWindows11_Click);
            this.panelWindows11.Controls.Add(this.btnOptimizarW11);

            this.btnInicioIzquierda = new Button();
            FormatearBoton(this.btnInicioIzquierda, "Inicio a la izquierda", 92);
            this.btnInicioIzquierda.Image = Image.FromFile(@"Resources\Iconos\inicioIzquierda.png");
            this.btnInicioIzquierda.Click += (s, e) => PonerInicioIzquierda();
            this.panelWindows11.Controls.Add(this.btnInicioIzquierda);

            // PANEL GENERALES
            this.panelGenerales = new Panel();
            this.panelGenerales.BackColor = Color.White;
            this.panelGenerales.BorderStyle = BorderStyle.None;
            this.panelGenerales.Location = new Point(542, 80);
            this.panelGenerales.Size = new Size(245, 336);
            this.panelGenerales.Region = Region.FromHrgn(
                Pumatool.FormPrincipal.CreateRoundRectRgn(0, 0, this.panelGenerales.Width, this.panelGenerales.Height, 16, 16));
            this.Controls.Add(this.panelGenerales);

            Label lblGen = CrearEtiquetaPanel("Herramientas Generales", 14, 10, Color.FromArgb(255, 193, 7));
            this.panelGenerales.Controls.Add(lblGen);

            int botonY = 50;
            int espacio = 42;

            this.btnRestaurarUsuario = new Button();
            FormatearBoton(this.btnRestaurarUsuario, "Restaurar Usuario", botonY);
            this.btnRestaurarUsuario.Image = Image.FromFile(@"Resources\Iconos\restaurarUsuario.png");
            this.btnRestaurarUsuario.Click += new System.EventHandler(this.btnRestaurarUsuario_Click);
            this.panelGenerales.Controls.Add(this.btnRestaurarUsuario);
            botonY += espacio;

            this.btnLimpiarTemporales = new Button();
            FormatearBoton(this.btnLimpiarTemporales, "Limpiar Temporales", botonY);
            this.btnLimpiarTemporales.Image = Image.FromFile(@"Resources\Iconos\limpiarTemporales.png");
            this.btnLimpiarTemporales.Click += (s, e) => LimpiarTemporales();
            this.panelGenerales.Controls.Add(this.btnLimpiarTemporales);
            botonY += espacio;

            this.btnInstalarOfimatica = new Button();
            FormatearBoton(this.btnInstalarOfimatica, "Instalar Ofimática", botonY);
            this.btnInstalarOfimatica.Image = Image.FromFile(@"Resources\Iconos\ofimatica.png");
            this.btnInstalarOfimatica.Click += btnInstalarOfimatica_Click;
            this.panelGenerales.Controls.Add(this.btnInstalarOfimatica);
            botonY += espacio;

            this.btnLimpiarRegistrosOffice = new Button();
            FormatearBoton(this.btnLimpiarRegistrosOffice, "Limpiar Registros Office", botonY);
            this.btnLimpiarRegistrosOffice.Image = Image.FromFile(@"Resources\Iconos\officeClean.png");
            this.btnLimpiarRegistrosOffice.Click += (s, e) => LimpiarRegistrosOffice();
            this.panelGenerales.Controls.Add(this.btnLimpiarRegistrosOffice);
        }

        private Label CrearEtiquetaPanel(string texto, int x, int y, Color color)
        {
            Label lbl = new Label();
            lbl.Text = texto;
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lbl.ForeColor = color;
            lbl.Location = new Point(x, y);
            return lbl;
        }

        private void FormatearBoton(Button boton, string texto, int posY)
        {
            boton.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            boton.Text = texto;
            boton.Size = new Size(217, 36);
            boton.Location = new Point(14, posY);
            boton.TextAlign = ContentAlignment.MiddleLeft;
            boton.ImageAlign = ContentAlignment.MiddleRight;
            boton.BackColor = Color.FromArgb(52, 73, 94);
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.Cursor = Cursors.Hand;
            boton.Padding = new Padding(6, 0, 8, 0);
            boton.Region = Region.FromHrgn(
                Pumatool.FormPrincipal.CreateRoundRectRgn(0, 0, boton.Width, boton.Height, 12, 12));
            boton.Margin = new Padding(0, 8, 0, 0);
            boton.TabStop = false;
        }
    }
}