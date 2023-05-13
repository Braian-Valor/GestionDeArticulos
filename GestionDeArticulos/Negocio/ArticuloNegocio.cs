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
				comando.CommandText = "Select A.Id, A.Codigo, A.Nombre, A.Descripcion, I.ImagenUrl as Imagen, M.Descripcion as Marca, C.Descripcion as Categoria, A.Precio, M.Id as IDMarca, C.Id as IDCategoria From ARTICULOS A Inner Join IMAGENES I ON A.Id = I.IdArticulo Inner Join MARCAS M ON A.IdMarca = M.Id Inner Join CATEGORIAS C ON A.IdCategoria = C.Id";
				comando.Connection = conexion;

				conexion.Open();
				lector = comando.ExecuteReader();

				while (lector.Read())
				{
					Articulo aux = new Articulo();

					aux.Id = (int)lector["Id"];

					if (!(lector["Codigo"] is DBNull))
						aux.Codigo = (string)lector["Codigo"];

                    if (!(lector["Nombre"] is DBNull))
                        aux.Nombre = (string)lector["Nombre"];

                    if (!(lector["Descripcion"] is DBNull))
                        aux.Descripcion = (string)lector["Descripcion"];

					aux.Imagen = new Imagen();
					if (!(lector["Imagen"] is DBNull))
                        aux.Imagen.Url = (string)lector["Imagen"];

					aux.Marca = new Marca();
                    if (!(lector["Marca"] is DBNull))
						aux.Marca.Id = (int)lector["IDMarca"];
                        aux.Marca.Descripcion = (string)lector["Marca"];

					aux.Categoria = new Categoria();
					if (!(lector["Categoria"] is DBNull))
						aux.Categoria.Id = (int)lector["IDCategoria"];
                        aux.Categoria.Descripcion = (string)lector["Categoria"];

                    //decimal dosDecimal;
                    //dosDecimal = (decimal)lector["Precio"];
                    //aux.Precio = Decimal.Parse(dosDecimal.ToString("0.00"));
                    if (!(lector["Precio"] is DBNull))
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
				datos.setearConsulta("Insert Into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) Values(@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @Precio); SELECT SCOPE_IDENTITY();");
				datos.setearParametros("@Codigo", nuevo.Codigo);
				datos.setearParametros("@Nombre", nuevo.Nombre);
				datos.setearParametros("@Descripcion", nuevo.Descripcion);
				datos.setearParametros("@IdMarca", nuevo.Marca.Id);
				datos.setearParametros("@IdCategoria", nuevo.Categoria.Id);
				datos.setearParametros("@Precio", nuevo.Precio);

				nuevo.Imagen.IdArticulo = datos.ejecutarReturnQuery();

				datos.cerrarConexion();

                datos.setearConsulta("Insert Into IMAGENES (IdArticulo, ImagenUrl) Values (@IdArticulo, @Imagen)");
                datos.setearParametros("@IdArticulo", nuevo.Imagen.IdArticulo);
                datos.setearParametros("@Imagen", nuevo.Imagen.Url);

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

		public void modificar(Articulo articulo)
		{
			AccesoDatos datos = new AccesoDatos();

			try
			{
				datos.setearConsulta("Update ARTICULOS Set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, Precio = @Precio Where Id = @Id");
				datos.setearParametros("@Codigo", articulo.Codigo);
                datos.setearParametros("@Nombre", articulo.Nombre);
                datos.setearParametros("@Descripcion", articulo.Descripcion);
                datos.setearParametros("@IdMarca", articulo.Marca.Id);
                datos.setearParametros("@IdCategoria", articulo.Categoria.Id);
                datos.setearParametros("@Precio", articulo.Precio);
                datos.setearParametros("@Id", articulo.Id);

                articulo.Imagen.IdArticulo = datos.ejecutarReturnQuery();

                datos.cerrarConexion();

				datos.setearConsulta("Update IMAGENES Set ImagenUrl = @Imagen Where IdArticulo = @IdArticulo");
				datos.setearParametros("@Imagen", articulo.Imagen.Url);
				datos.setearParametros("@IdArticulo", articulo.Id);

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

		public void eliminar(int id)
		{
			bool b = false;
			AccesoDatos datos = new AccesoDatos();

			try
			{
				if (b == false)
				{
					datos.setearConsulta("Delete From ARTICULOS Where Id = @Id");
					datos.setearParametros("@Id", id);

					datos.ejecutarAccion();
					datos.cerrarConexion();
					b = true;
				}
				if (b == true)
				{
					datos.setearConsulta("Delete From IMAGENES Where IdArticulo = @IdArticulo");
					datos.setearParametros("@IdArticulo", id);

					datos.ejecutarAccion();
				}
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
