using System.Drawing;
using System.Windows.Forms;

namespace Pumatool
{
    partial class FormPantallaCarga
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMensaje;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMensaje = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = false;
            this.lblMensaje.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMensaje.Location = new System.Drawing.Point(15, 30);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(320, 40);
            this.lblMensaje.TabIndex = 0;
            this.lblMensaje.Text = "Procesando...";
            this.lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormPantallaCarga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 110);
            this.Controls.Add(this.lblMensaje);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPantallaCarga";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Procesando...";
            this.BackColor = Color.White;
            this.ResumeLayout(false);
        }
    }
}