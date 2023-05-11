using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
			List<Articulo> lista = new List<Articulo>();
			SqlConnection conexion = new SqlConnection();
			SqlCommand comando = new SqlCommand();
			SqlDataReader lector;

			try
			{
				conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true";
				comando.CommandType = System.Data.CommandType.Text;
				comando.CommandText = "Select A.Id, A.Codigo, A.Nombre, A.Descripcion, I.ImagenUrl as Imagen, M.Descripcion as Marca, C.Descripcion as Categoria, A.Precio From ARTICULOS A Inner Join IMAGENES I ON A.Id = I.IdArticulo Inner Join MARCAS M ON A.IdMarca = M.Id Inner Join CATEGORIAS C ON A.IdCategoria = C.Id";
				comando.Connection = conexion;

				conexion.Open();
				lector = comando.ExecuteReader();

				while (lector.Read())
				{
					Articulo aux = new Articulo();

					aux.Id = (int)lector["Id"];
					aux.Codigo = (string)lector["Codigo"];
					aux.Nombre = (string)lector["Nombre"];
					aux.Descripcion = (string)lector["Descripcion"];
					aux.Imagen = new Imagen();
					aux.Imagen.Url = (string)lector["Imagen"];
					aux.Marca = new Marca();
					aux.Marca.Descripcion = (string)lector["Marca"];
					aux.Categoria = new Categoria();
					aux.Categoria.Descripcion = (string)lector["Categoria"];

                    //decimal dosDecimal;
                    //dosDecimal = (decimal)lector["Precio"];
                    //aux.Precio = Decimal.Parse(dosDecimal.ToString("0.00"));
                    aux.Precio = (decimal)lector["Precio"];

					lista.Add(aux);
				}

				conexion.Close();
                return lista;
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

		public void agregar(Articulo nuevo)
		{
			AccesoDatos datos = new AccesoDatos();

			try
			{
				datos.setearConsulta("Insert Into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria) Values('" + nuevo.Codigo + "', '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', @IdMarca, @IdCategoria)");
				datos.setearParametros("@IdMarca", nuevo.Marca.Id);
				datos.setearParametros("@IdCategoria", nuevo.Categoria.Id);
				datos.ejecutarAccion();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				datos.cerrarConexion();
			}
		}



    }
}
