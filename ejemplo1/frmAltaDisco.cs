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
        public frmAltaDisco()
        {
            InitializeComponent();
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            Disco discoNuevo = new Disco();
            DiscoNegocio negocio = new DiscoNegocio();
            try
            {
                discoNuevo.Titulo = txtTitulo.Text;
                discoNuevo.UrlImagen = txtUrlImagen.Text;
                discoNuevo.Estilo =(Estilo)cbxEstilo.SelectedItem;
                discoNuevo.TipoEdicion = (TipoEdicion)cbxTipo.SelectedItem;//slected item devuelve un object. Casteo directo que tipo de objeto devuelve, porque lo programamos nosotros
                negocio.agregar(discoNuevo);
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
                cbxTipo.DataSource = tipoEdicionNegocio.listar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
