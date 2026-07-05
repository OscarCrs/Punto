using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Punto.Forms
{
    public partial class frmProductos : Form
    {
        //Este es un variable para sasber sobre el id del producto para eliminarlo o modificar 
        int IdProducSelecc = 0;
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
            Conexion conClase = new Conexion();
            MySqlConnection dbConn = conClase.GetConeccion();

            //Si falla la conexion
            if (dbConn == null) return;

            try
            {
                string sql = "INSERT INTO productos (codigo, descripcion, precio, stock) VALUES (@codigo, @descripcion, @precio, @stock)";
                MySqlCommand cmd = new MySqlCommand(sql, dbConn);

                cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text.Trim());
                cmd.Parameters.AddWithValue("@descripcion", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text));
                cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(txtStock.Text));
                int filas = cmd.ExecuteNonQuery();
                dbConn.Close();

                //Validamos si se guardó correctamente
                if (filas > 0)
                {
                    MessageBox.Show("Producto registrado con éxito.", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarProductos();

                    txtCodigo.Clear();
                    txtNombre.Clear();
                    txtPrecio.Clear();
                    txtStock.Clear();
                    txtCodigo.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    dbConn.Close();
                }
            }
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { 
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

                txtCodigo.Text = fila.Cells["codigo"].Value.ToString();
                txtNombre.Text = fila.Cells["descripcion"].Value.ToString();
                txtPrecio.Text = fila.Cells["precio"].Value.ToString();
                txtStock.Text = fila.Cells["stock"].Value.ToString();

                IdProducSelecc = Convert.ToInt32(fila.Cells["producto_id"].Value);
            }
        }
    
    }
}
