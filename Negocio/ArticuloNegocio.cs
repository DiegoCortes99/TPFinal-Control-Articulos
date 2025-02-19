using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulos> listadoArticulos()
        {
            List<Articulos> lista = new List<Articulos>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.crearConsulta("Select Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria,A.IdMarca,A.IdCategoria, A.ImagenUrl, A.Precio, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M where M.Id = IdMarca And C.Id = IdCategoria");
                datos.ejecutarConsulta();

                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.codigoArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    aux.Marca = new Marcas();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl"))))
                    {
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    }
                    aux.precio = (decimal)datos.Lector["Precio"];
                    aux.Id = (int)datos.Lector["Id"];

                    lista.Add(aux);
                }               
                return lista;
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

        public void agregar(Articulos nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.crearConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion,IdMarca,IdCategoria,ImagenUrl,Precio) values (@codigo,@nombre,@descripcion,@idmarca,@idcategoria,@imagenurl,@precio)");
                datos.setearParametros("@codigo",nuevo.codigoArticulo);
                datos.setearParametros("@nombre", nuevo.Nombre);
                datos.setearParametros("@descripcion", nuevo.Descripcion);
                datos.setearParametros("@idmarca", nuevo.Marca.Id);
                datos.setearParametros("@idcategoria", nuevo.Categoria.Id);
                datos.setearParametros("@imagenurl",nuevo.ImagenUrl);
                datos.setearParametros("@precio",nuevo.precio);
                datos.ejecutarConsultaParametro();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar articulo: " + ex.Message, ex);
            }
            
        }

        public void modificar(Articulos articulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.crearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @desc, IdMarca = @idmarca, IdCategoria = @idcategoria, ImagenUrl = @img, Precio = @precio where id = @id");
                datos.setearParametros("@codigo",articulo.codigoArticulo);
                datos.setearParametros("@nombre",articulo.Nombre);
                datos.setearParametros("@desc",articulo.Descripcion);
                datos.setearParametros("@idmarca",articulo.Marca.Id);
                datos.setearParametros("@idcategoria",articulo.Categoria.Id);
                datos.setearParametros("@img",articulo.ImagenUrl);
                datos.setearParametros("@precio",articulo.precio);
                datos.setearParametros("@id", articulo.Id);
                datos.ejecutarConsultaParametro();
            }
            catch (Exception ex)
            {

                throw ex;
            }           
        }

        public void eliminar(Articulos articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.crearConsulta("delete from ARTICULOS where Id = @id");
                datos.setearParametros("@id",articulo.Id);
                datos.ejecutarConsultaParametro();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulos> filtrar(string campo, string criterio, string buscar)
        {
            List<Articulos> articulosFiltrado = new List<Articulos>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria,A.IdMarca,A.IdCategoria, A.ImagenUrl, A.Precio, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M where M.Id = IdMarca And C.Id = IdCategoria AND ";

                if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + buscar + "%' ";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + buscar + "' ";
                            break;
                        case "Contiene":
                            consulta += "Nombre like '%" + buscar + "%' ";
                            break;
                    }
                }
                if (campo == "Categoria")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "C.Descripcion like '" + buscar + "%' ";
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion like '%" + buscar + "' ";
                            break;
                        case "Contiene":
                            consulta += "C.Descripcion like '%" + buscar + "%' ";
                            break;
                    }
                }
                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor que":
                            consulta += "A.Precio > " + buscar;
                            break;
                        case "Menor que":
                            consulta += "A.Precio < " + buscar;
                            break;
                        case "Igual":
                            consulta += "A.Precio = " + buscar;
                            break;
                    }
                }

                datos.crearConsulta(consulta);
                datos.ejecutarConsulta();

                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.codigoArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    aux.Marca = new Marcas();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl"))))
                    {
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    }
                    aux.precio = (decimal)datos.Lector["Precio"];
                    aux.Id = (int)datos.Lector["Id"];

                    articulosFiltrado.Add(aux);
                }

                return articulosFiltrado;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
