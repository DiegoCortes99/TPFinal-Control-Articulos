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
    public partial class frmDetalle: Form
    {
        private Articulos articulosDetalle;
        public frmDetalle(Articulos articulos)
        {
            InitializeComponent();
            articulosDetalle = articulos;
        }

        private void frmDetalle_Load(object sender, EventArgs e)
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

                txtCodigo.Text = articulosDetalle.codigoArticulo;
                txtNombre.Text = articulosDetalle.Nombre;
                txtDescripcion.Text = articulosDetalle.Descripcion;
                cboMarca.SelectedValue = articulosDetalle.Marca.Id;
                cboCategoria.SelectedValue = articulosDetalle.Categoria.Id;
                txtImagen.Text = articulosDetalle.ImagenUrl;
                CargarImagen(articulosDetalle.ImagenUrl);
                txtPrecio.Text = articulosDetalle.precio.ToString();

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Close();
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
    }

}
