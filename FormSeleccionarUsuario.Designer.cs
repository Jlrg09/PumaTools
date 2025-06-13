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
        private Panel panelFondo;
        private Label lblTitulo;
        private PictureBox picUser;

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
            this.picUser = new PictureBox();
            this.comboUsuarios = new ComboBox();
            this.btnAceptar = new Button();
            this.btnCancelar = new Button();

            // FormSeleccionarUsuario
            this.ClientSize = new Size(350, 210);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Seleccionar Usuario";
            this.BackColor = Color.FromArgb(235, 237, 245);

            // panelFondo
            this.panelFondo.BackColor = Color.White;
            this.panelFondo.Size = new Size(310, 170);
            this.panelFondo.Location = new Point(20, 20);
            this.panelFondo.Region = System.Drawing.Region.FromHrgn(
                Pumatool.FormPrincipal.CreateRoundRectRgn(0, 0, panelFondo.Width, panelFondo.Height, 18, 18));
            this.Controls.Add(panelFondo);

            // picUser
            this.picUser.Image = Image.FromFile(@"Resources\Iconos\user.png");
            this.picUser.Size = new Size(36, 36);
            this.picUser.SizeMode = PictureBoxSizeMode.Zoom;
            this.picUser.Location = new Point(20, 13);
            panelFondo.Controls.Add(picUser);

            // lblTitulo
            this.lblTitulo.Text = "Selecciona un usuario local";
            this.lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(54, 33, 89);
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new Point(70, 20);
            panelFondo.Controls.Add(lblTitulo);

            // comboUsuarios
            this.comboUsuarios.Font = new Font("Segoe UI", 11F);
            this.comboUsuarios.Location = new Point(30, 65);
            this.comboUsuarios.Size = new Size(250, 28);
            this.comboUsuarios.DropDownStyle = ComboBoxStyle.DropDown;
            panelFondo.Controls.Add(comboUsuarios);

            // btnAceptar
            this.btnAceptar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.Size = new Size(110, 36);
            this.btnAceptar.Location = new Point(35, 115);
            this.btnAceptar.BackColor = Color.FromArgb(54, 33, 89);
            this.btnAceptar.ForeColor = Color.White;
            this.btnAceptar.FlatStyle = FlatStyle.Flat;
            this.btnAceptar.FlatAppearance.BorderSize = 0;
            this.btnAceptar.Cursor = Cursors.Hand;
            this.btnAceptar.Region = System.Drawing.Region.FromHrgn(
                Pumatool.FormPrincipal.CreateRoundRectRgn(0, 0, btnAceptar.Width, btnAceptar.Height, 12, 12));
            this.btnAceptar.Click += btnAceptar_Click;
            panelFondo.Controls.Add(btnAceptar);

            // btnCancelar
            this.btnCancelar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Size = new Size(110, 36);
            this.btnCancelar.Location = new Point(165, 115);
            this.btnCancelar.BackColor = Color.FromArgb(231, 76, 60);
            this.btnCancelar.ForeColor = Color.White;
            this.btnCancelar.FlatStyle = FlatStyle.Flat;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.Cursor = Cursors.Hand;
            this.btnCancelar.Region = System.Drawing.Region.FromHrgn(
                Pumatool.FormPrincipal.CreateRoundRectRgn(0, 0, btnCancelar.Width, btnCancelar.Height, 12, 12));
            this.btnCancelar.Click += btnCancelar_Click;
            panelFondo.Controls.Add(btnCancelar);
        }
    }
}