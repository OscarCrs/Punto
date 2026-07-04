using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Punto.Forms
{
    internal class Conexion
    {
        //Cadena de conexion a la base de datos con los 5 parametros
        private readonly string Cadena;

        //constructor
        public Conexion()
        {
            Cadena = "Server=127.0.0.1; Database=PuntoDB; Uid=root; Pwd=; Port=3306;";
        }

        //metodo para conectar a la base de datos
        public MySqlConnection GetConeccion()
        {
            try
            {
                MySqlConnection con = new MySqlConnection(Cadena);
                con.Open();
                MessageBox.Show("Conexion Exitosa...");
                return con;
            }
            catch(MySqlException ex)
            {
                MessageBox.Show("error al conectarse con la base de datos \n"+ ex.Message);
                return null;
            }

        }
    }
}
