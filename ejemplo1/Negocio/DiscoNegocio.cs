using ejemplo1.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ejemplo1
{
    class DiscoNegocio
    {
        public List<Disco> listar()
        {
            List<Disco> lista = new List<Disco>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select d.Id, D.Titulo, D.UrlImagenTapa, E.Descripcion as Estilo, T.Descripcion AS Formato from DISCOS d, ESTILOS e, TIPOSEDICION t where d.IdEstilo = e.Id and t.Id = d.IdTipoEdicion");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Disco aux = new Disco();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Titulo = (string)datos.Lector["Titulo"];
                    aux.UrlImagen = (string)datos.Lector["UrlImagenTapa"];

                    aux.Estilo = new Estilo();
                    aux.Estilo.Id = (int)datos.Lector["Id"];
                    aux.Estilo.Descripcion = (string)datos.Lector["Estilo"];

                    aux.TipoEdicion = new TipoEdicion();
                    aux.TipoEdicion.Id = (int)datos.Lector["Id"];
                    aux.TipoEdicion.Descripcion= (string)datos.Lector["Formato"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
