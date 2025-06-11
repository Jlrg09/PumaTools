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

        private void InitializeComponent()
        {
            this.panelFondo = new Panel();
            this.lblTitulo = new Label();
            this.lblSubtitulo = new Label();
            this.lblInstrucciones = new Label();
            this.btnContinuar = new Button();
            this.panelFondo.SuspendLayout();
            this.SuspendLayout();

            // panelFondo
            this.panelFondo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelFondo.BorderStyle = BorderStyle.FixedSingle;
            this.panelFondo.Controls.Add(this.lblTitulo);
            this.panelFondo.Controls.Add(this.lblSubtitulo);
            this.panelFondo.Controls.Add(this.lblInstrucciones);
            this.panelFondo.Controls.Add(this.btnContinuar);
            this.panelFondo.Location = new System.Drawing.Point(20, 20);
            this.panelFondo.Size = new System.Drawing.Size(500, 400);

            // lblTitulo
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblTitulo.Text = "PUMATOOLS";
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(120, 30);

            // lblSubtitulo
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.Gray;
            this.lblSubtitulo.Text = "Programado por José Romero";
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Location = new System.Drawing.Point(150, 90);

            // lblInstrucciones
            this.lblInstrucciones.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblInstrucciones.Text = "Herramienta de soporte técnico para optimización de Windows.";
            this.lblInstrucciones.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblInstrucciones.Size = new System.Drawing.Size(450, 80);
            this.lblInstrucciones.Location = new System.Drawing.Point(25, 140);

            // btnContinuar
            this.btnContinuar.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnContinuar.Text = "Comenzar";
            this.btnContinuar.Size = new System.Drawing.Size(150, 50);
            this.btnContinuar.Location = new System.Drawing.Point(175, 280);
            this.btnContinuar.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.btnContinuar.ForeColor = System.Drawing.Color.White;
            this.btnContinuar.FlatStyle = FlatStyle.Flat;
            this.btnContinuar.FlatAppearance.BorderSize = 0;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);

            // FormBienvenida
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 450);
            this.Controls.Add(this.panelFondo);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "PumaTool - Bienvenida";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.panelFondo.ResumeLayout(false);
            this.panelFondo.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
