namespace Pumatool
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Panel panelWindows10;
        private Panel panelWindows11;
        private Panel panelGenerales;

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
            this.lblTitulo = new Label();
            this.panelWindows10 = new Panel();
            this.panelWindows11 = new Panel();
            this.panelGenerales = new Panel();

            // Inicializamos los botones
            this.btnOptimizarW10 = new Button();
            this.btnOptimizarW11 = new Button();
            this.btnInicioIzquierda = new Button();
            this.btnLimpiarTemporales = new Button();
            this.btnRestaurarUsuario = new Button();
            this.btnInstalarOfimatica = new Button();

            // FormPrincipal
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "PumaTool - Herramientas";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // lblTitulo
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblTitulo.Text = "PUMATOOLS";
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(300, 20);
            this.Controls.Add(this.lblTitulo);

            // panelWindows10
            this.panelWindows10.BackColor = System.Drawing.Color.White;
            this.panelWindows10.BorderStyle = BorderStyle.FixedSingle;
            this.panelWindows10.Location = new System.Drawing.Point(50, 100);
            this.panelWindows10.Size = new System.Drawing.Size(300, 150);
            this.Controls.Add(this.panelWindows10);

            // panelWindows11
            this.panelWindows11.BackColor = System.Drawing.Color.White;
            this.panelWindows11.BorderStyle = BorderStyle.FixedSingle;
            this.panelWindows11.Location = new System.Drawing.Point(400, 100);
            this.panelWindows11.Size = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.panelWindows11);

            // panelGenerales
            this.panelGenerales.BackColor = System.Drawing.Color.White;
            this.panelGenerales.BorderStyle = BorderStyle.FixedSingle;
            this.panelGenerales.Location = new System.Drawing.Point(50, 320);
            this.panelGenerales.Size = new System.Drawing.Size(650, 200);
            this.Controls.Add(this.panelGenerales);

            //-------------------------
            // Botones Windows 10
            //-------------------------
            FormatearBoton(this.btnOptimizarW10, "Optimizar Windows 10", 20);
            this.btnOptimizarW10.Image = Image.FromFile(@"Resources\Iconos\optimizarW10.png");
            this.panelWindows10.Controls.Add(this.btnOptimizarW10);

            //-------------------------
            // Botones Windows 11
            //-------------------------
            FormatearBoton(this.btnOptimizarW11, "Optimizar Windows 11", 20);
            this.btnOptimizarW11.Image = Image.FromFile(@"Resources\Iconos\optimizarW11.png");
            this.panelWindows11.Controls.Add(this.btnOptimizarW11);

            FormatearBoton(this.btnInicioIzquierda, "Inicio a la izquierda", 80);
            this.btnInicioIzquierda.Image = Image.FromFile(@"Resources\Iconos\inicioIzquierda.png");
            this.panelWindows11.Controls.Add(this.btnInicioIzquierda);

            //-------------------------
            // Botones Configuración General
            //-------------------------
            FormatearBoton(this.btnRestaurarUsuario, "Restaurar Usuario", 20);
            this.btnRestaurarUsuario.Image = Image.FromFile(@"Resources\Iconos\restaurarUsuario.png");
            this.panelGenerales.Controls.Add(this.btnRestaurarUsuario);

            FormatearBoton(this.btnLimpiarTemporales, "Limpiar Temporales", 80);
            this.btnLimpiarTemporales.Image = Image.FromFile(@"Resources\Iconos\limpiarTemporales.png");
            this.panelGenerales.Controls.Add(this.btnLimpiarTemporales);

            FormatearBoton(this.btnInstalarOfimatica, "Instalar Ofimática", 140);
            this.btnInstalarOfimatica.Image = Image.FromFile(@"Resources\Iconos\ofimatica.png");
            this.panelGenerales.Controls.Add(this.btnInstalarOfimatica);

            //-------------------------
            // EVENTOS CLICK
            //-------------------------
            this.btnLimpiarTemporales.Click += (s, e) => LimpiarTemporales();
            this.btnOptimizarW10.Click += new System.EventHandler(this.btnOptimizarWindows_Click);
            this.btnOptimizarW11.Click += new System.EventHandler(this.btnOptimizarWindows11_Click);
            this.btnInicioIzquierda.Click += (s, e) => PonerInicioIzquierda();
            this.btnRestaurarUsuario.Click += new System.EventHandler(this.btnRestaurarUsuario_Click);
            this.btnInstalarOfimatica.Click += btnInstalarOfimatica_Click;
        }

        private void FormatearBoton(Button boton, string texto, int posY)
        {
            boton.Font = new System.Drawing.Font("Segoe UI", 10F);
            boton.Text = texto;
            boton.Size = new System.Drawing.Size(260, 40);
            boton.Location = new System.Drawing.Point(20, posY);
            boton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            boton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            boton.BackColor = System.Drawing.Color.DarkSlateBlue;
            boton.ForeColor = System.Drawing.Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
        }
    }
}
