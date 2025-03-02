using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class Form1 : Form
    {

        private List<Articulos> listaArticulos;

        public Form1()
        {
            InitializeComponent();
        }

        

        private void dgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulos = negocio.listadoArticulos();
                dgvArticulos.DataSource = listaArticulos;
                ocultarColumnas();
                CargarImagen(listaArticulos[0].ImagenUrl);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        public void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();

            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Precio");
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pbImagen.Load(imagen);
            }
            catch (Exception)
            {

                pbImagen.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
            }
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                CargarImagen(seleccionado.ImagenUrl);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregar agregarfrm = new frmAgregar();
            agregarfrm.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulos seleccionado;
            seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;

            frmAgregar modificar = new frmAgregar(seleccionado);
            modificar.ShowDialog();
            cargar();
            
        }

        private void btnDetalles_Click(object sender, EventArgs e)
        {
            Articulos seleccionado;
            seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;

            frmDetalle detalle = new frmDetalle(seleccionado);
            detalle.ShowDialog();
            cargar();
        }



        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulos seleccionado;

            try
            {
                DialogResult resultado;
                resultado = MessageBox.Show("De verdad quieres eliminarlo?","Eliminando",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado);
                    cargar();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        private void txtFiltradoRapido_TextChanged(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            string filtrarRapido = txtFiltradoRapido.Text;

            List<Articulos> filtrado;

            if (filtrarRapido.Length >= 3)
            {
                filtrado = listaArticulos.FindAll(articulo => articulo.Nombre.ToUpper().Contains(filtrarRapido.ToUpper()) || articulo.Categoria.Descripcion.ToUpper().Contains(filtrarRapido.ToUpper()));
            }
            else
            {
                filtrado = listaArticulos;

            }
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = filtrado;
            ocultarColumnas();
            
        }        

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor que");
                cboCriterio.Items.Add("Menor que");
                cboCriterio.Items.Add("Igual");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }

        }

        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un campo para filtrar");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un criterio para filtrar");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtBuscar.Text))
                {
                    MessageBox.Show("Debes cargar el filtro para numericos...");
                    return true;
                }
                if (!(soloNumeros(txtBuscar.Text)))
                {
                    MessageBox.Show("Solo se permiten numeros y un punto decimal (ej: 90.000)");
                    return true;
                }

            }
            return false;
        }
        
        private bool soloNumeros(string cadena)
        {

            bool puntoEncontrado = false;

            foreach (char caracter in cadena)
            {
                if (caracter == '.')
                {
                    if (puntoEncontrado)
                    {
                        return false;
                    }
                    puntoEncontrado = true;
                }
                else if (!(char.IsNumber(caracter)))
                {
                    return false;
                }                                       
            }
            return true;
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                // cancela ejecucion del evento
                if (validarFiltro())
                {
                    return;
                }

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string buscar = txtBuscar.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo,criterio,buscar);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
