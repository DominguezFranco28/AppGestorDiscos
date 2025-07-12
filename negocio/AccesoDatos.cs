using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{  
   public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=DISCOS_DB; integrated security=true");
            comando = new SqlCommand();
            //consturctor d ela clase que permite la conexion de datos, para cuando sea llamada d eotro objeto
            //el lector no va en el construccion porque no se puede instanciar esa clase
        }
        public SqlDataReader Lector
        {
            get { return lector; } //prop para leer el lectr desde el exterior
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text; //sentencia sql que quiero usar, tipo text el mas comun
            comando.CommandText = consulta;
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader(); //devuelve un SqlDataReader, se asigna a la variable lector
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void cerrarConexion()
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
        }

    }
}

