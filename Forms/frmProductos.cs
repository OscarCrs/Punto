using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Punto.Forms
{
    public partial class frmProductos : Form
    {
        int IdProducSelecc = 0;
        public frmProductos()
        {
            InitializeComponent();
        }

        //Para logar la carga de los productos en el datagridview
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

        //Para agregar productos.
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

        //Para seleccionar un producto del datagridview y mostrarlo en los textbox
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

        //Para editar un producto seleccionado
        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Si no se ha seleccionado ningún producto, no hace nada y muestra un mensaje de advertencia
            if (IdProducSelecc == 0)
            {
                MessageBox.Show("Por favor, selecciona primero un producto de la tabla para modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Conexion conClase = new Conexion();
            MySqlConnection conn = conClase.GetConeccion();
            if (conn == null) return;

            try
            {
                string sql = "UPDATE productos SET codigo = @codigo, descripcion = @descripcion, precio = @precio, stock = @stock WHERE producto_id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text.Trim());
                cmd.Parameters.AddWithValue("@descripcion", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text));
                cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(txtStock.Text));
                cmd.Parameters.AddWithValue("@id", IdProducSelecc);
                int filas = cmd.ExecuteNonQuery();
                conn.Close();

                if (filas > 0)
                {
                    MessageBox.Show("Producto modificado con éxito.", "Editar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();

                    txtCodigo.Clear();
                    txtNombre.Clear();
                    txtPrecio.Clear();
                    txtStock.Clear();
                    IdProducSelecc = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }
        }

        //Para eliminar un producto seleccionado
        private void btnEliminar_Click(object sender, EventArgs e)
        { 
            if (dgvProductos.SelectedRows.Count == 0) return;

            int idSeleccionado = Convert.ToInt32(dgvProductos.SelectedRows[0].Cells["producto_id"].Value);

            DialogResult confirm = MessageBox.Show("¿Seguro que deseas eliminar este producto?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No) return;

            Conexion conClase = new Conexion();
            MySqlConnection conn = conClase.GetConeccion();
            if (conn == null) return;

            string sql = "DELETE FROM productos WHERE producto_id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", idSeleccionado);

            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Producto eliminado con éxito.", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarProductos();

            txtCodigo.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
        }
    }
}
