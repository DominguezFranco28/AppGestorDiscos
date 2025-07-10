using ejemplo1.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ejemplo1
{
    public partial class Form1 : Form
    {
        private List<Disco> listaDiscos;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void dgvDiscos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDiscos.CurrentRow != null)
            {
                Disco seleccionado = (Disco)dgvDiscos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
            }

        }
        private void ocultarColumnas()
        {
            dgvDiscos.Columns["UrlImagen"].Visible = false;
            dgvDiscos.Columns["Id"].Visible = false;
        }
        private void cargar()
        {
            DiscoNegocio negocio = new DiscoNegocio();
            try
            {
                listaDiscos = negocio.listar();
                dgvDiscos.DataSource = listaDiscos;
                cargarImagen(listaDiscos[0].UrlImagen);
                ocultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxDisco.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxDisco.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }
    }
}
