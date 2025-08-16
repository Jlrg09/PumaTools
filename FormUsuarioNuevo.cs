using System.Windows.Forms;

public class FormUsuarioNuevo : Form
{
    public FormUsuarioNuevo()
    {
        var lbl = new Label() { Text = "Nombre:", Location = new System.Drawing.Point(10, 10) };
        var txtNombre = new TextBox() { Location = new System.Drawing.Point(80, 10), Width = 200 };
        var lbl2 = new Label() { Text = "Contraseña:", Location = new System.Drawing.Point(10, 40) };
        var txtPass = new TextBox() { Location = new System.Drawing.Point(80, 40), Width = 200, PasswordChar = '*' };
        var btn = new Button() { Text = "Crear Usuario", Location = new System.Drawing.Point(80, 70) };
        var btnEst = new Button() { Text = "Crear ESTUDIANTE", Location = new System.Drawing.Point(80, 100) };
        var btnDoc = new Button() { Text = "Crear DOCENTE", Location = new System.Drawing.Point(80, 130) };

        btn.Click += (s, e) => { UserCreator.CrearUsuario(txtNombre.Text, txtPass.Text); };
        btnEst.Click += (s, e) => { UserCreator.CrearEstudiante(); };
        btnDoc.Click += (s, e) => { UserCreator.CrearDocente(); };

        Controls.Add(lbl); Controls.Add(txtNombre);
        Controls.Add(lbl2); Controls.Add(txtPass);
        Controls.Add(btn); Controls.Add(btnEst); Controls.Add(btnDoc);
        Size = new System.Drawing.Size(320, 180);
    }
}