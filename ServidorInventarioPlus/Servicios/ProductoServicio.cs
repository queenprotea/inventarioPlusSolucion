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
                            ProveedorID = u.ProveedorID,
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
            
        }
        
        public bool ActualizarProducto(ProductoDTO producto)
        {
            
        }
    }
}