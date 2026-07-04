using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Punto.Forms
{
    public partial class frmProductos : Form
    {
        public frmProductos()
        {
            InitializeComponent();
        }

        private void frmProductos_Load(object sender, System.EventArgs e)
        {
            CargarProductos();
        }
        private void CargarProductos()
        {
            Conexion conClase = new Conexion();
            MySqlConnection dbConn = conClase.GetConeccion();

            if (dbConn == null)
            {
                return;
            }
            try
            {
                string query = "SELECT producto_id, codigo, descripcion, precio, stock FROM productos";

                using (MySqlCommand comando = new MySqlCommand(query, dbConn))
                {
                    using (MySqlDataAdapter adaptador = new MySqlDataAdapter(comando))
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        adaptador.Fill(dt);
                        dgvProductos.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbConn.Close();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {

        }
    }
}
