using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Management;
using System.Drawing;

namespace Pumatool
{
    public partial class FormSeleccionarUsuario : Form
    {
        public string UsuarioSeleccionado { get; private set; }

        public FormSeleccionarUsuario()
        {
            InitializeComponent();
            CargarUsuariosLocales();
        }

        private void CargarUsuariosLocales()
        {
            try
            {
                comboUsuarios.Items.Clear();

                string query = "SELECT * FROM Win32_UserAccount WHERE LocalAccount=True AND Disabled=False";
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject user in searcher.Get())
                    {
                        string name = user["Name"].ToString();
                        string domain = user["Domain"].ToString();

                        if (domain == Environment.MachineName)
                        {
                            comboUsuarios.Items.Add(name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios locales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboUsuarios.Text))
            {
                MessageBox.Show("Debe seleccionar o escribir un usuario.");
                return;
            }
            UsuarioSeleccionado = comboUsuarios.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}