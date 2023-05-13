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

namespace GestionDeArticulosApp
{
    public partial class frmPrincipal : Form
    {
        private List<Articulo> listaArticulo;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                listaArticulo = negocio.listar();
                dgvArticulos.DataSource = listaArticulo;
                dgvArticulos.Columns["Imagen"].Visible = false;
                cargarImagen(listaArticulo[0].Imagen.Url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.Imagen.Url);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pboxArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pboxArticulo.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Placeholder_view_vector.svg/681px-Placeholder_view_vector.svg.png");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            pboxArticulo.Visible = false;
            alta.ShowDialog();
            pboxArticulo.Visible = true;
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo articuloSeleccionado = new Articulo();
            articuloSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            frmAltaArticulo modificar = new frmAltaArticulo(articuloSeleccionado);
            pboxArticulo.Visible = false;
            modificar.ShowDialog();
            pboxArticulo.Visible = true;
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo articuloSeleccionado;

            try
            {
                DialogResult respuesta = MessageBox.Show("¿Eliminar articulo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(respuesta == DialogResult.Yes) 
                { 
                    articuloSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(articuloSeleccionado.Id);
                    MessageBox.Show("Articulo eliminado");
                    cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
