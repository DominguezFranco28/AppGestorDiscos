using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;
namespace ejemplo1
{
    public partial class frmAltaDisco : Form
    {
        //atributo privado 
        private Disco disco = null;
        OpenFileDialog archivo = null;
        public frmAltaDisco()
        {
            InitializeComponent();
            Text = "Agregar Disco";
        }
        public frmAltaDisco(Disco modificacionDisco)
        {
            InitializeComponent();
            this.disco = modificacionDisco; //carga el atributo privado de esta clase con un parametro (disco)
                                            //que viene de la otra ventana
            Text = "Modificar Disco";
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            // Disco discoNuevo = new Disco();
            DiscoNegocio negocio = new DiscoNegocio();
            try
            {
                if (disco == null)
                {
                    disco = new Disco();//Si se llega aca desde el boton de Agregar, se asigna un nuevo disco a la isntancia vacia, y entocnes volvemos a agregar uno en vez de modif
                }
                disco.Titulo = txtTitulo.Text;
                disco.UrlImagen = txtUrlImagen.Text;
                disco.Estilo = (Estilo)cbxEstilo.SelectedItem;
                disco.TipoEdicion = (TipoEdicion)cbxTipo.SelectedItem;//slected item devuelve un object. Casteo directo que tipo de objeto devuelve, porque lo programamos nosotros

                //Hasta aca, tanto agregar como modif te agregan un item
                if (disco.Id != 0) //identifico con su lectura en la base de datos si no tiene ID. SI la tiene, entonces modifica 
                {
                    negocio.modificar(disco);
                    MessageBox.Show("Modificado con exito.");
                }
                else
                { 
                    negocio.agregar(disco);
                    MessageBox.Show("Agregado con exito.");
                }
                //Guardo imagen del disco si se levanto localmente con el boton +
                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP"))) //convierot en mayus el string y filtro el contenido con mayus tambien
                guardarImagenLocal();
                                         //si el archivo no es nulo Y no contiene el string http, quiere decir que el archivo es local y lo guarda
                                         //lo agrega solo si se da a confirmar en el alta del disco, no con solo pooner la ruta con el boton + ahora
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void frmAltaDisco_Load(object sender, EventArgs e)
        {
            EstiloNegocio estiloNegocio = new EstiloNegocio();
            TipoEdicionNegocio tipoEdicionNegocio = new TipoEdicionNegocio();
            try
            {
                cbxEstilo.DataSource = estiloNegocio.listar();
                cbxEstilo.ValueMember = "Id"; //agregar value y display fue necesario para que se preselecciones el tipo y estilo cuando se le da a modificar.
                cbxEstilo.DisplayMember = "Descripcion";
                cbxTipo.DataSource = tipoEdicionNegocio.listar();
                cbxTipo.ValueMember = "Id";
                cbxTipo.DisplayMember = "Descripcion";
                if (disco != null) //si el disco es distinto de nulo, es decir, se uso el boton Modificar, precargo los datos del disco que vino
                {

                    txtTitulo.Text = disco.Titulo;
                    txtUrlImagen.Text = disco.UrlImagen;
                    cbxEstilo.SelectedValue = disco.Estilo.Id;
                    cbxTipo.SelectedValue = disco.TipoEdicion.Id;
                    cargarImagen(disco.UrlImagen);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }
        private void btnImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog(); //ventana de dialogo q permite al usuario carga run archivo
            archivo.Filter = "jpg|*.jpg | png|* .png"; //le filtras el tipo de dato que permite cargar, en este caso se permiteen todos los jpg. y png.
            if (archivo.ShowDialog() == DialogResult.OK) //dialogresult permite detectar si el usuario selecciono un archivo del buscador.
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
                //guardado de imagen en una carpet,a no base de datos //uso de libreria IO
                //se estblece la ruta a la cual se guarda la imagente desde el archivo de configuracion del proyecto (App.config) (C:\discos-app)
                //se linkea con addreferences>Assemblie> y se agrega System.Configuration, para obtener la libreria dll, y una vez utilizada en la clase
                // poder leer la ruta desde el archivo de config (gracias a esto, ahora nos aparece la clase ConfigManager y llamamos al appsettings)
               // File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-discos-folder"] + archivo.SafeFileName); //appsetings funciona como una suerte de lector de una coleccion clave-valor,
                                                                                                                              //podes tener bastantes key y leerlas como un strings               //se concatena el nombre del archivo. Crea una copia de la imagen en la carpeta en cuestion
               //el guardado de imagen se termino modularizando 
            }
        }
        private void cargarImagen(string imagen)  //metodo repe en el script del otro form, podria modularizarse con una clase helper
        {
            try
            {
                pbxImagenAltaDisco.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxImagenAltaDisco.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }
        private void guardarImagenLocal()
        {
            File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-discos-folder"] + archivo.SafeFileName);
        }
    }
    }
