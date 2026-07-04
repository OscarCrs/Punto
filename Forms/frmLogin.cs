using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Punto.Forms
{
    public partial class frmLogin : Form
    {
        private frmPrincipal principal;
        public frmLogin()
        {
            InitializeComponent();
            Conexion coneccion  = new Conexion();
            MySqlConnection Conex = coneccion.GetConeccion();
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            string usuario = txtUser.Text.Trim();
            string contra = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contra))
            {
                MessageBox.Show("Por favor, llena todos los campos.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Conexion con = new Conexion();

            try
            {
                using (var conexion = con.GetConeccion())
                {
                    if (conexion == null) return;

                    string query = "SELECT nombre_completo FROM usuarios WHERE username = @user AND password = @pass";

                    using (MySqlCommand comando = new MySqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@user", usuario);
                        comando.Parameters.AddWithValue("@pass", contra);

                        object resultado = comando.ExecuteScalar();

                        if (resultado != null)
                        {
                            string nombreUsuario = resultado.ToString();

                            MessageBox.Show($"¡Bienvenido al Sistema, {nombreUsuario}!", "Acceso Concedido",MessageBoxButtons.OK, MessageBoxIcon.Information);

                            frmProductos formularioPrincipal = new frmProductos();
                            formularioPrincipal.Show();

                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Usuario o contraseña incorrectos. Intente de nuevo.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            txtPassword.Clear();
                            txtUser.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }
    }
}
