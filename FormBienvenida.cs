using System;
using System.Windows.Forms;

namespace Pumatool
{
    public partial class FormBienvenida : Form
    {
        public FormBienvenida()
        {
            InitializeComponent();
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormPrincipal principal = new FormPrincipal();
            principal.ShowDialog();
            this.Close();
        }
    }
}
