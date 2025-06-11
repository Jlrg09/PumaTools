namespace Pumatool
{
    partial class FormSeleccionarUsuario
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox comboUsuarios;
        private Button btnAceptar;

        private void InitializeComponent()
        {
            this.comboUsuarios = new ComboBox();
            this.btnAceptar = new Button();

            this.SuspendLayout();

            // comboUsuarios
            this.comboUsuarios.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboUsuarios.Location = new System.Drawing.Point(20, 20);
            this.comboUsuarios.Width = 250;

            // btnAceptar
            this.btnAceptar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.Location = new System.Drawing.Point(100, 70);
            this.btnAceptar.Click += btnAceptar_Click;

            // FormSeleccionarUsuario
            this.ClientSize = new System.Drawing.Size(300, 130);
            this.Controls.Add(this.comboUsuarios);
            this.Controls.Add(this.btnAceptar);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Text = "Seleccionar Usuario";

            this.ResumeLayout(false);
        }
    }
}
