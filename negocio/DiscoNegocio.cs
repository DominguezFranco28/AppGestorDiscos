
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using dominio;

namespace negocio
{
    public class DiscoNegocio 
    {
        //CLASE NEGOCIO DE CADA OBJETO SE HACE EN UNA CLASE APARTE AL OBJETO
        //PARA CREAR METODOS DE ACCESO A LA BASE DE DATOS 
        public List<Disco> listar()
        {
            List<Disco> lista = new List<Disco>();
            AccesoDatos datos = new AccesoDatos();

            try //CONEXION A BASE DE DATOS CON MANEJO DE EXCEPCIONES, PARA LA CEACION DE LA LISTA
            {
                datos.setearConsulta("select D.Id, D.Titulo, D.UrlImagenTapa, E.Descripcion as Estilo, T.Descripcion AS Tipo, D.IdEstilo, d.IdTipoEdicion from DISCOS d, ESTILOS e, TIPOSEDICION t where d.IdEstilo = e.Id and t.Id = d.IdTipoEdicion and D.Active = 1 ");
                //es mala practica el * para pedir todo de la tabla, hay que usar un comando especifico de columnas a obtener
               //consulta joineada con rel entre tablas
               //EN la conlsuta se filtra tambien solo los discos activos para mostrar
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                    //si existe lectura proveniente del lector (ver clase accesodatos),
                    //posisiona el entero en la primer fila de la consulta dentro de la base de datos, y devuelve true
                    //se recorre asi todos los registros llamados
                {
                    Disco aux = new Disco();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Titulo = (string)datos.Lector["Titulo"]; //necesario el casteo explicito al inicio con arentesis, sino da error.
                     //El compilador no puede sabver de antemano que tipo de dato esta en la Base de datos, asi que lo casteamos explicitamente
                    //lo que se pone entre corchetes
                    //es el nombre que hayamos creado a nuestra tabla virtual en el seteo de la consulta. Por eso en Descripcion de tipoedicion Formato
                   
                    if (!(datos.Lector["UrlImagenTapa"] is DBNull)) 
                        //Sirve para validar si la columna que se esta leyendo en Nula //Validacion de null
                        //En este caso, si urlimagen no es nul, entonces lo trata de leer. Si es null, lo salteo
                        //Esto es para evitar que se rompa la app con la lectura de un null cargado por error
                        //Es una validacion pensada para agregar en las columnas que den lugar a la carga de un Null. Si la columna dentro de la base de datos, es notnull(es decir, no admite la carga de null), esto es innecesario)
                    {
                        aux.UrlImagen = (string)datos.Lector["UrlImagenTapa"];
                    }

                    aux.Estilo = new Estilo();//cuando nace el obj disco, su Estilo no tiene instancia.
                                              //Se genera aca para evitar una null reference. Podria refactorizarse y hacer que el objeto Disco nazca con un constructor para la prop de tipo Estilo (relacion de herencia / composicion)
                    aux.Estilo.Id = (int)datos.Lector["IdEstilo"];
                    aux.Estilo.Descripcion = (string)datos.Lector["Estilo"]; 
                    //hubo que sobreescribir el emtodo ToString del objeto. Cuando el compilador retorna el valor del objeto, por defecto retorna su metodo tostrings
                    // retornando la definicion textual del obj
                    //como se busca que lea el 'Estilo', se sobreescribe ese metodo heradado para que retorne la pro Descripcion

                    aux.TipoEdicion = new TipoEdicion();
                    aux.TipoEdicion.Id = (int)datos.Lector["IdTipoEdicion"];
                    aux.TipoEdicion.Descripcion= (string)datos.Lector["Tipo"];

                    lista.Add(aux);

                }

                return lista; //cuando no hay mas datos para leer, da false y sale del while.
                              //Se retorna la el bojet de tipo Lista de discos, invocado por el meotodo cargar del 
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
        public void modificar (Disco discoModificar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update DISCOS set Titulo = @titulo, UrlImagenTapa = @urlImagen, IdEstilo = @idEstilo, IdTipoEdicion = @idTipo  where Id = @id ");
                datos.setearParametro("@titulo", discoModificar.Titulo);            
                datos.setearParametro("@urlImagen", discoModificar.UrlImagen);            
                datos.setearParametro("@idEstilo", discoModificar.Estilo.Id);            
                datos.setearParametro("@idTipo", discoModificar.TipoEdicion.Id);
                datos.setearParametro("@id", discoModificar.Id);
                datos.ejecutarAccion();
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
        public void agregar( Disco nuevoDisco) 
        {
            AccesoDatos datos = new AccesoDatos();
            //no es necesario lsitado, se agregan de a uno
            try
            {
                datos.setearConsulta("insert into DISCOS (Titulo, UrlImagenTapa, IdEstilo, IdTipoEdicion, Active) values ('"+nuevoDisco.Titulo+"','"+nuevoDisco.UrlImagen+"',@idEstilo,@idTipoEdicion, 1)");
                //con los @ se crea una suerte de variable en la consulta, son Parametros.
                //Cuando la consulta se ejecute, estos parametro seran cargados con un metodo creado
                //para cargar parametros, en la clase AccesoDatos.
                //El ultimo valor es 1porque quiero que siempre se agregen activos los discos
                datos.setearParametro("@idEstilo",nuevoDisco.Estilo.Id); //se da valor al aarametro, tomamos la prop Estilo.Id del nuevo disco creado desde el form
                datos.setearParametro("@idTipoEdicion", nuevoDisco.TipoEdicion.Id);
                datos.ejecutarAccion();          
            }
            catch (Exception ex )
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void eliminar (Disco discoEliminar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("delete from DISCOS where id = @id");
                datos.setearParametro("@id", discoEliminar.Id);
                datos.ejecutarAccion();
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
        public void eliminarLogico( Disco disco)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update DISCOS  set Active = 0 where Id = @id");
                datos.setearParametro("@id", disco.Id);
                datos.ejecutarAccion();
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
        public List<Disco> filtrar(string campo, string criterio, string filtro)
        {
            List<Disco> listaNegocio = new List<Disco>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select D.Id, D.Titulo, D.UrlImagenTapa, E.Descripcion as Estilo, T.Descripcion AS Tipo, D.IdEstilo, d.IdTipoEdicion from DISCOS d, ESTILOS e, TIPOSEDICION t where d.IdEstilo = e.Id and t.Id = d.IdTipoEdicion and D.Active = 1 and ";
                //la base d ela consulta es la misma que el metodo listar,
                //ya que son los discos que tenemos lsitados los que hay que filtrar. Solo se agrega "and" al final para concatenar el string que sigue en el condicional
                if (campo == "Título")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "D.Titulo like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "D.Titulo like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "D.Titulo like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Estilo")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "E.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "E.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "E.Descripcion like '%" + filtro + "%'";
                            break;
                    }

                }
                else if (campo == "Tipo de edición")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "T.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "T.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "T.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Es mayor a ":
                            consulta += "D.Id > " + filtro;
                            break;
                        case "Es menor a ":
                            consulta += "D.Id < " + filtro;              
                            break;
                        default:
                            consulta += "D.Id = " + filtro;
                                break;
                    }
                }
                    //si los botoes de campo y criterio tuvieran mas opciones, podria usarse un switch para esta parte.
                    datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                //si existe lectura proveniente del lector (ver clase accesodatos),
                //posisiona el entero en la primer fila de la consulta dentro de la base de datos, y devuelve true
                //se recorre asi todos los registros llamados
                {
                    Disco aux = new Disco();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Titulo = (string)datos.Lector["Titulo"]; 
                    if (!(datos.Lector["UrlImagenTapa"] is DBNull))
                    {
                        aux.UrlImagen = (string)datos.Lector["UrlImagenTapa"];
                    }

                    aux.Estilo = new Estilo();
                    aux.Estilo.Id = (int)datos.Lector["IdEstilo"];
                    aux.Estilo.Descripcion = (string)datos.Lector["Estilo"];

                    aux.TipoEdicion = new TipoEdicion();
                    aux.TipoEdicion.Id = (int)datos.Lector["IdTipoEdicion"];
                    aux.TipoEdicion.Descripcion = (string)datos.Lector["Tipo"];

                    listaNegocio.Add(aux);
                    //misma logica que el metodo listar, pero con filtro aplicado
                }
                return listaNegocio;
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
