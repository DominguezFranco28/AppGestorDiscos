using dominio;
using negocio;
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
        //atributo privado
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
                //le indicamos que cada fila tiene un objeto enlazado. 
                //pERO Como no sabe que tipo de objeto es, hacemos un casteo explicito par aindicarle que es un objeto de tipo disoc
                //(xq asi tenemos programada la ap, sabemos por el metodo cargar que lo que esta cargado en al grilla son discos)
                cargarImagen(seleccionado.UrlImagen);
            }

        }
        private void ocultarColumnas()
        {
            dgvDiscos.Columns["UrlImagen"].Visible = false;
            //dgvDiscos.Columns["Prueba"].Visible = false;
            
        }
        private void cargar()
        {
            DiscoNegocio negocio = new DiscoNegocio();
            try
            {
                listaDiscos = negocio.listar();//pasaje a lista para poder usarla para mas cosas que solo leer los datos en la grilla
                dgvDiscos.DataSource = listaDiscos;
                //se le pasa la lista de objetos al datasource, y la grilla del form analiza la estructrua del objeto dentro de la lista
                // con una tenica llamada 'reflection' en sistemas,
                // emula la estructura de la clase Disco generando las columnas correspondientes a cada propiedad
                //ver Propiedad 'Prueba'
                cargarImagen(listaDiscos[0].UrlImagen);
                //el 0 por indicar el primer elmeento de la lista, para qe cague la url del disco seleecionado

                ocultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void cargarImagen(string imagen) //Se modularizo para gestionar excepciones (se llaaba desde 3 metodos a la carga de imagen de la Url)
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaDisco frmAlta = new frmAltaDisco();
            frmAlta.ShowDialog();
        }
    }
}
