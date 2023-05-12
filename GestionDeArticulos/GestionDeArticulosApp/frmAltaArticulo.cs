using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionDeArticulosApp
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo;

        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = tboxCodigo.Text;
                articulo.Nombre = tboxNombre.Text;
                articulo.Descripcion = tboxDescripcion.Text;
                articulo.Imagen = new Imagen();
                articulo.Imagen.Url = tboxImagen.Text;
                articulo.Marca = (Marca)cboxMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboxCategoria.SelectedItem;
                articulo.Precio = decimal.Parse(tboxPrecio.Text);

                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Articulo modificado");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Articulo agregado");
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                cboxMarca.DataSource = marcaNegocio.listar();
                cboxMarca.ValueMember = "Id";
                cboxMarca.DisplayMember = "Descripcion";
                cboxCategoria.DataSource = categoriaNegocio.listar();
                cboxCategoria.ValueMember = "Id";
                cboxCategoria.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    tboxCodigo.Text = articulo.Codigo;
                    tboxNombre.Text = articulo.Nombre;
                    tboxDescripcion.Text = articulo.Descripcion;
                    tboxImagen.Text = articulo.Imagen.Url;
                    cboxMarca.SelectedValue = articulo.Marca.Id;
                    cboxCategoria.SelectedValue = articulo.Categoria.Id;
                    tboxPrecio.Text = articulo.Precio.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void tboxImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(tboxImagen.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pboxImagenAlta.Load(imagen);
            }
            catch (Exception)
            {
                pboxImagenAlta.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Placeholder_view_vector.svg/681px-Placeholder_view_vector.svg.png");
            }
        }
    }
}
