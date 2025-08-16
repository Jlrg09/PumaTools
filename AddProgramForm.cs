using System.Windows.Forms;

namespace Pumatool
{
    public class AddProgramForm : Form
    {
        public AddProgramForm()
        {
            var lbl = new Label() { Text = "Nombre:", Location = new System.Drawing.Point(10, 10) };
            var txtNombre = new TextBox() { Location = new System.Drawing.Point(80, 10), Width = 200 };
            var lbl2 = new Label() { Text = "URL descarga:", Location = new System.Drawing.Point(10, 40) };
            var txtUrl = new TextBox() { Location = new System.Drawing.Point(80, 40), Width = 200 };
            var lbl3 = new Label() { Text = "Ejecutable:", Location = new System.Drawing.Point(10, 70) };
            var txtExe = new TextBox() { Location = new System.Drawing.Point(80, 70), Width = 200 };
            var btn = new Button() { Text = "Agregar", Location = new System.Drawing.Point(80, 100) };

            btn.Click += (s, e) =>
            {
                if (txtNombre.Text != "" && txtExe.Text != "")
                {
                    ProgramDownloader.AgregarPrograma(txtNombre.Text, txtUrl.Text, txtExe.Text);
                    MessageBox.Show("Programa agregado.");
                    this.Close();
                }
            };

            Controls.Add(lbl); Controls.Add(txtNombre);
            Controls.Add(lbl2); Controls.Add(txtUrl);
            Controls.Add(lbl3); Controls.Add(txtExe);
            Controls.Add(btn);
            Size = new System.Drawing.Size(300, 160);
        }
    }
}