using System;
using System.Windows.Forms;

namespace Pumatool
{
    public partial class FormPantallaCarga : Form
    {
        public FormPantallaCarga()
        {
            InitializeComponent();
        }

        public void SetMensaje(string texto)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => lblMensaje.Text = texto));
            }
            else
            {
                lblMensaje.Text = texto;
            }
        }
    }
}