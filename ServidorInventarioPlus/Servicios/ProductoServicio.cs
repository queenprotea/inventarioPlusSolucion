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
                        .Select(p => new ProductoDTO
                        {
                            ProductoID = p.ProductoID,
                            Codigo = p.Codigo,
                            Nombre = p.Nombre,
                            Descripcion = p.Descripcion,
                            Stock = p.Stock,
                            IDCategoria = p.IDCategoria,
                            PrecioCompra = p.PrecioCompra,
                            PrecioVenta = p.PrecioVenta
                        })
                        .ToList();
                    
                    foreach (var prod in productos)
                    {
                        prod.NombreCategoria = context.Categorias
                            .Where(c => c.IDCategoria == prod.IDCategoria)
                            .Select(c => c.Nombre)
                            .FirstOrDefault();

                        prod.proveedores = context.ProductoProveedores
                            .Where(pp => pp.ProductoID == prod.ProductoID)
                            .Select(pp => new ProveedorDTO
                            {
                                ProveedorID = pp.Proveedor.ProveedorID,
                                Nombre = pp.Proveedor.Nombre,
                                Direccion = pp.Proveedor.Direccion,
                                Telefono = pp.Proveedor.Telefono,
                                Correo = pp.Proveedor.Correo,
                                
                            })
                            .ToList();
                        var primerProveedor = prod.proveedores.FirstOrDefault();
                        prod.FirstProveedor = primerProveedor != null ? primerProveedor.Nombre : "Sin proveedor";
                    }

                    return productos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consultar productos: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine(ex.Source);
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
                        PrecioCompra = producto.PrecioCompra,
                        PrecioVenta = producto.PrecioVenta
                    };
                    // Agregamos el producto y guardamos para obtener el ID generado
                    db.Productos.Add(nuevoProducto);
                    db.SaveChanges();
                    // Ahora usamos el ProductoID real generado
                    foreach (var proveedor in producto.proveedores)
                    {
                        var relacion = new ProductoProveedores
                        {
                            ProductoID = nuevoProducto.ProductoID,
                            ProveedorID = proveedor.ProveedorID,
                        };
                        db.ProductoProveedores.Add(relacion);
                    }

                    db.SaveChanges();
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
            using (var db = new DBContext())
            {
                try
                {
                    // 1️⃣ Buscar el producto existente
                    var productoExistente = db.Productos
                        .FirstOrDefault(p => p.ProductoID == producto.ProductoID);

                    if (productoExistente == null)
                        throw new Exception("El producto no existe en la base de datos.");

                    // 2️⃣ Actualizar los campos básicos
                    productoExistente.Nombre = producto.Nombre;
                    productoExistente.Descripcion = producto.Descripcion;
                    productoExistente.Stock = producto.Stock;
                    productoExistente.StockApartado = producto.StockApartado;
                    productoExistente.StockMinimo = producto.StockMinimo;
                    productoExistente.Codigo = producto.Codigo;
                    productoExistente.IDCategoria = producto.IDCategoria;
                    productoExistente.PrecioCompra = producto.PrecioCompra;
                    productoExistente.PrecioVenta = producto.PrecioVenta;

                    // 3️⃣ Actualizar las relaciones con proveedores
                    // Eliminar relaciones anteriores
                    var relacionesAnteriores = db.ProductoProveedores
                        .Where(pp => pp.ProductoID == producto.ProductoID)
                        .ToList();

                    db.ProductoProveedores.RemoveRange(relacionesAnteriores);

                    // Agregar las nuevas relaciones
                    foreach (var proveedor in producto.proveedores)
                    {
                        var nuevaRelacion = new ProductoProveedores
                        {
                            ProductoID = producto.ProductoID,
                            ProveedorID = proveedor.ProveedorID
                        };
                        db.ProductoProveedores.Add(nuevaRelacion);
                    }

                    // 4️⃣ Guardar los cambios
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al modificar producto: {ex.Message}");
                    Console.WriteLine(ex.InnerException?.Message);
                    return false;
                }
            }
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