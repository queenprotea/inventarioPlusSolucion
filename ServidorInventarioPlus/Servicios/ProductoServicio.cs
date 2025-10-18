using System;
using System.Collections.Generic;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using ServidorInventarioPlus.Context;
using ServidorInventarioPlus.Modelos;
using System.Linq;

namespace ServidorInventarioPlus.Servicios
{
    public class ProductoServicio : IProductoService
    {
        public List<ProductoDTO> ObtenerProductos()
        {
            using (var context = new DBContext())
            {
                try
                {
                    var productos = context.Productos
                        .Select(u => new ProductoDTO
                        {
                            ProductoID = u.ProductoID,
                            Codigo = u.Codigo,
                            Nombre = u.Nombre,
                            Descripcion = u.Descripcion,
                            Stock = u.Stock,
                            StockApartado = u.StockApartado,
                            StockMinimo = u.StockMinimo,
                           
                            IDCategoria = u.IDCategoria,
                        })
                        .ToList();

                    return productos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consultar productos: {ex.Message}");
                    return new List<ProductoDTO>();
                }
            }
        }

        public List<ProductoDTO> BuscarProductos(string valorBusqueda)
        {
            throw new NotImplementedException();
        }

        public bool EliminarProducto(int productoID)
        {   
            using (var context = new DBContext())
            {
                try
                {
                    var producto = context.Productos.FirstOrDefault(p => p.ProductoID == productoID);
                    if (producto == null)
                        return false;

                    context.Productos.Remove(producto);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar producto: {ex.Message}");
                    return false;
                }
            }
        }
        
        public bool RegistrarProducto(ProductoDTO producto)
        {
            using (var db = new DBContext())
            {
                try
                {
                    var nuevoProducto = new Producto
                    {
                        Nombre = producto.Nombre,
                        Descripcion = producto.Descripcion,
                        Stock = producto.Stock,
                        StockApartado = producto.StockApartado,
                        StockMinimo = producto.StockMinimo,
                        Codigo = producto.Codigo,
                        IDCategoria = producto.IDCategoria,
                    };

// 1️⃣ Agregamos el producto y guardamos para obtener el ID generado
                    db.Productos.Add(nuevoProducto);
                    db.SaveChanges();
//
// 2️⃣ Ahora usamos el ProductoID real generado
/*
                    foreach (var proveedor in producto.proveedores)
                    {
                        var relacion = new ProductoProveedores
                        {
                            ProductoID = nuevoProducto.ProductoID,
                            ProveedorID = proveedor.ProveedorID
                        };
                        db.ProductoProveedores.Add(relacion);
                    }

                    db.SaveChanges();*/
//
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al registrar producto: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.InnerException);
                    return false;
                }
            }
        }
        
        public bool ActualizarProducto(ProductoDTO producto)
        {
            return false;
        }

        public List<ProductoDTO> BuscarProductosPorNombre(string textoBusqueda)
        {
            throw new NotImplementedException();
        }

        public List<ProductoDTO> BuscarProductosPorCodigo(string textoBusqueda)
        {
            throw new NotImplementedException();
        }

        public List<ProductoDTO> BuscarProductosPorCategoria(string textoBusqueda)
        {
            throw new NotImplementedException();
        }

        public List<CategoriasDTO> ObtenerCategorias()
        {
            using (var context = new DBContext())
            {
                try
                {
                    var categorias =  context.Categorias
                        .Select(u => new CategoriasDTO
                    {
                        IDCategoria = u.IDCategoria.ToString(),
                        Nombre = u.Nombre,
                    })
                    .ToList();
                    
                    return categorias;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consultar categorias: {ex.Message}");
                    return new List<CategoriasDTO>();
                }
            }
        }
    }
}