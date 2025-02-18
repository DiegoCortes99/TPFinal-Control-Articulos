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
    public partial class frmAgregar : Form
    {

        private Articulos nuevoArticulo = null;


        public frmAgregar()
        {
            InitializeComponent();
            Text = "Agregar";
        }

        public frmAgregar(Articulos articulos)
        {
            InitializeComponent();
            this.nuevoArticulo = articulos;
            Text = "Modificar";
        }

        private void frmAgregar_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                cboMarca.DataSource = marcaNegocio.listarMarca();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";

                cboCategoria.DataSource = categoriaNegocio.ListarCategoria();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (nuevoArticulo != null)
                {
                    txtCodigo.Text = nuevoArticulo.codigoArticulo;
                    txtNombre.Text = nuevoArticulo.Nombre;
                    txtDescripcion.Text = nuevoArticulo.Descripcion;
                    cboMarca.SelectedValue = nuevoArticulo.Marca.Id;
                    cboCategoria.SelectedValue = nuevoArticulo.Categoria.Id;
                    txtImagen.Text = nuevoArticulo.ImagenUrl;
                    CargarImagen(nuevoArticulo.ImagenUrl);

                    txtPrecio.Text = nuevoArticulo.precio.ToString();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAcceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (nuevoArticulo == null)
                {
                    nuevoArticulo = new Articulos();
                }

                nuevoArticulo.codigoArticulo = txtCodigo.Text;
                nuevoArticulo.Nombre = txtNombre.Text;
                nuevoArticulo.Descripcion = txtDescripcion.Text;
                nuevoArticulo.Marca = (Marcas)cboMarca.SelectedItem;
                nuevoArticulo.Categoria = (Categorias)cboCategoria.SelectedItem;
                nuevoArticulo.ImagenUrl = txtImagen.Text;
                CargarImagen(txtImagen.Text);
                nuevoArticulo.precio = decimal.Parse(txtPrecio.Text);

                if (nuevoArticulo.Id != 0)
                {
                    negocio.modificar(nuevoArticulo);
                    MessageBox.Show("Modificado Exitosamente");
                }
                else
                {
                    negocio.agregar(nuevoArticulo);
                    MessageBox.Show("Agregado Exitosamente");
                }                
                Close();
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

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
        
        private void txtImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtImagen.Text);
        }
    }
}
