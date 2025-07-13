using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
namespace ejemplo1
{
    public partial class frmAltaDisco: Form
    {
        private Disco disco = null;
        //atributo privado 
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
                    cbxEstilo.SelectedValue= disco.Estilo.Id;
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
    }
}
