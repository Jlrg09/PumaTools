using System.Drawing;
using System.Windows.Forms;

namespace Pumatool
{
    partial class FormSeleccionarUsuario
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox comboUsuarios;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblTitulo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new Label();
            this.comboUsuarios = new ComboBox();
            this.btnAceptar = new Button();
            this.btnCancelar = new Button();

            // FormSeleccionarUsuario
            this.Text = "Seleccionar usuario";
            this.Size = new Size(340, 170);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            // lblTitulo
            this.lblTitulo.Text = "Seleccione el usuario a restaurar:";
            this.lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblTitulo.Location = new Point(20, 15);
            this.lblTitulo.AutoSize = true;
            this.Controls.Add(lblTitulo);

            // comboUsuarios
            this.comboUsuarios.Location = new Point(20, 45);
            this.comboUsuarios.Size = new Size(280, 28);
            this.comboUsuarios.Font = new Font("Segoe UI", 10F);
            this.comboUsuarios.DropDownStyle = ComboBoxStyle.DropDown;
            this.Controls.Add(comboUsuarios);

            // btnAceptar
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.Location = new Point(40, 90);
            this.btnAceptar.Size = new Size(100, 32);
            this.btnAceptar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAceptar.BackColor = Color.FromArgb(54, 33, 89);
            this.btnAceptar.ForeColor = Color.White;
            this.btnAceptar.Click += btnAceptar_Click;
            this.Controls.Add(btnAceptar);

            // btnCancelar
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Location = new Point(180, 90);
            this.btnCancelar.Size = new Size(100, 32);
            this.btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnCancelar.BackColor = Color.FromArgb(231, 76, 60);
            this.btnCancelar.ForeColor = Color.White;
            this.btnCancelar.Click += btnCancelar_Click;
            this.Controls.Add(btnCancelar);
        }
    }
}