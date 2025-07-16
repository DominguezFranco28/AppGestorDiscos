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
            cboCampo.Items.Add("Id Disco");
            cboCampo.Items.Add("Título");
            cboCampo.Items.Add("Estilo");
            cboCampo.Items.Add("Tipo de edición");
            
        }
        private void filtrar()
        {
            List<Disco> listaFiltrada;
            string filtro = txtFiltro.Text;
            //hace una suerte de foreach contra la lista,
            //en cada vuelta aloja en la X (var disco) un objeto, si el contenido de su propiedad, en este caso, Titulo es igual al filtro de la caja de texto,
            //entonces lo devuelve (basicamente, lo filtra)

            if (filtro != "") //si se pone algo en la caja de texto, entonces muestra la lista filtrada
            {
                listaFiltrada = listaDiscos.FindAll(disco => disco.Titulo.ToUpper().Contains(filtro.ToUpper()) || disco.Estilo.Descripcion.ToUpper().Contains(filtro.ToUpper())); //EXPRESION LAMBDA + condicion logica para que bussque filtros por estilo o titulo
                //el toupper fue agregado para que lea todo en mayuscula, y el filtro sirva sin necesidad de respetar exactamente las minusculas o maayus           
                //Contains para que no sea necesarioe scribir la palabra completa. El .Contains devuelve v o f si lo que viene en el campo (filtro) esta contenido en el campo anterior (Titulo)
            }
            else //si no hay nada escrito, muestra la lista orignal
            {
                listaFiltrada = listaDiscos;
            }

            dgvDiscos.DataSource = null; //se limpia el listado previo
            dgvDiscos.DataSource = listaFiltrada; //y se le pone la info de la nueva lista filtrada    
            ocultarColumnas();
        }
        private void ocultarColumnas()
        {
            dgvDiscos.Columns["UrlImagen"].Visible = false;
            //dgvDiscos.Columns["Id"].Visible = false;
            //dgvDiscos.Columns["Prueba"].Visible = false;
            
        }
        private void cargar() //para actualizar la grilla
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
            catch (Exception)
            {
                pbxDisco.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }
        private void eliminar(bool logico = false) //Esta definicion convierte al parametro en OPCIONAL, si no se pasa, el parametro se pasa en false
        {
            Disco discoSeleccionado;
            discoSeleccionado = (Disco)dgvDiscos.CurrentRow.DataBoundItem;
            DiscoNegocio negocio = new DiscoNegocio();
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Desea eliminar el Disco?", "Eliminando Objeto", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    if (logico)
                    {
                        negocio.eliminarLogico(discoSeleccionado);
                    }
                    else
                    {
                        negocio.eliminar(discoSeleccionado);
                    }
                    cargar();
                    MessageBox.Show("Disco eliminado", "Eliminación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex <0){
                MessageBox.Show("Por favor, seleccione el campo para filtrar");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Id Disco")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                { 
                    MessageBox.Show("Debes cargar el filtro (solo númericos)!");
                    return true;
                }
                if (!(validarNumeros(txtFiltroAvanzado.Text)))  //si no son son numeros...
                { 
                    MessageBox.Show("Por favor, escriba solo números para filtrar por un campo númerico..");
                    return true; 
                }             
            }
                
            return false;
        }
        private bool validarNumeros(string cadena)
        {
            foreach (char caracter in cadena) //ciclo foreach para recorrer cada caracter dentro del texto del filtro. Si encuentra alguno que no sea numero, retorna falso
            {
                if (!(char.IsNumber(caracter))) //si el caracter NO es un numero..
                        return false;
            }
            return true;
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaDisco frmAlta = new frmAltaDisco();
            frmAlta.ShowDialog();
            cargar(); //PARA QUE lo actualice en el momento
        }

        private void dgvDiscos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDiscos.CurrentRow != null)
            //Fue necesario agregar esto si no saltaba un error al tener un grilla seleccionada y
            //justo aplicar el filtro. Al no tener ningun elemento disco seleccionmado, y posteriormente inter cargar la imagen de un objeto nulo,
            //daba error, por eso ahora lo dejo entrar solo si no es null la seleccion
            {
                Disco seleccionado = (Disco)dgvDiscos.CurrentRow.DataBoundItem;
                //le indicamos que cada fila tiene un objeto enlazado. 
                //pERO Como no sabe que tipo de objeto es, hacemos un casteo explicito par aindicarle que es un objeto de tipo disoc
                //(xq asi tenemos programada la ap, sabemos por el metodo cargar que lo que esta cargado en al grilla son discos)
                cargarImagen(seleccionado.UrlImagen);
            }

        }
        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            Disco discoSeleccionado;
            discoSeleccionado = (Disco)dgvDiscos.CurrentRow.DataBoundItem; //encapsulo en un objeto especifco la selecion del usuario
            frmAltaDisco frmModificar = new frmAltaDisco(discoSeleccionado);//llamo al constructor q puede recibir un objeto de tipo disco
            frmModificar.ShowDialog();
            cargar(); //PARA QUE lo actualice en el momento
        }

        private void btnEliminar_Click(object sender, EventArgs e) //Eliminacion fisica del objeto. Incluida en la base de dato
        {
            eliminar();
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true); //se pasa en True para activar el condicional dentro del metodo eliminar
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
        
            DiscoNegocio negocio = new DiscoNegocio();
            try
            {
                if (validarFiltro())
                    return; ; //chequea las validaciones preestablecidas, si alguna retorna true, se sale del try
            string campo = cboCampo.SelectedItem.ToString();
            string criterio = cboCriterio.SelectedItem.ToString();
            string filtro = txtFiltroAvanzado.Text;
            dgvDiscos.DataSource = negocio.filtrar(campo,criterio,filtro);

            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            filtrar();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            // chequeamos la opcion del primer criterio del filtro, para quede opciones acorde para el sig criterio del filtro
            //en mi ejemplo, no uso numeros, si los usara podria crear otra condicion con cosas como "Mayor a" etc.
            if (opcion == "Id Disco")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Es mayor a ");
                cboCriterio.Items.Add("Es menor a ");
                cboCriterio.Items.Add("Es igual");
            }
            else //son todos los demas textos, asi que no hago mas condiciones, todos comparten el mismo criterio en este caso
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
    }
}
