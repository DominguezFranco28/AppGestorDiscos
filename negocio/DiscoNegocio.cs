
using System;
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
                datos.setearConsulta("select D.Titulo, D.UrlImagenTapa, E.Descripcion as Estilo, T.Descripcion AS Tipo from DISCOS d, ESTILOS e, TIPOSEDICION t where d.IdEstilo = e.Id and t.Id = d.IdTipoEdicion");
                //es mala practica el * para pedir todo de la tabla, hay que usar un comando especifico de columnas a obtener
               //consulta joineada con rel entre tablas
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                    //si existe lectura proveniente del lector (ver clase accesodatos),
                    //posisiona el entero en la primer fila de la consulta dentro de la base de datos, y devuelve true
                    //se recorre asi todos los registros llamados
                {
                    Disco aux = new Disco();
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
                    aux.Estilo.Descripcion = (string)datos.Lector["Estilo"]; 
                    //hubo que sobreescribir el emtodo ToString del objeto. Cuando el compilador retorna el valor del objeto, por defecto retorna su metodo tostrings
                    // retornando la definicion textual del obj
                    //como se busca que lea el 'Estilo', se sobreescribe ese metodo heradado para que retorne la pro Descripcion

                    aux.TipoEdicion = new TipoEdicion();
                    aux.TipoEdicion.Descripcion= (string)datos.Lector["Tipo"];

                    aux.Prueba = "gay el q lee";

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
        public void agregar( Disco nuevoDisco) 
        {
            AccesoDatos datos = new AccesoDatos();
            //no es necesario lsitado, se agregan de a uno
            try
            {
                datos.setearConsulta("insert into DISCOS (Titulo, UrlImagenTapa, IdEstilo, IdTipoEdicion) values ('"+nuevoDisco.Titulo+"','"+nuevoDisco.UrlImagen+"',@idEstilo,@idTipoEdicion)");
                //con los @ se crea una suerte de variable en la consulta, son Parametros.
                //Cuando la consulta se ejecute, estos parametro seran cargados con un metodo creado
                //para cargar parametros, en la clase AccesoDatos.
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
    }
}
