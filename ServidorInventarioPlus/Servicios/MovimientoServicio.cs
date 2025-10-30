using System;
using System.Collections.Generic;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using ServidorInventarioPlus.Context;
using System.Linq;
using ServidorInventarioPlus.Modelos;
using System.Data.Entity;



namespace ServidorInventarioPlus.Servicios
{
    public class MovimientoServicio : IMovimientoService
    {
        public bool RegistrarMovimiento(MovimientoDTO movimiento)
        {
            using (var db = new DBContext())
            {
                try
                {
                    var nuevoMovimiento = new Moviento
                    {
                        FechaHora = movimiento.FechaHora,
                        UsuarioID = movimiento.UsuarioID,
                        ProductoID = movimiento.ProductoID,
                        TipoMovimiento = movimiento.TipoMovimiento,
                        Cantidad = movimiento.Cantidad
                    };

                    db.Movientos.Add(nuevoMovimiento);
                    db.SaveChanges();

                    Console.WriteLine("✅ Movimiento registrado (el trigger actualizó el stock).");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al registrar el movimiento: {ex.Message}");
                    return false;
                }
            }
        }

        public List<MovimientoDTO> ObtenerMovimientos()
        {
            using (var context = new DBContext())
            {
                try
                {
                    var movimientos = context.Movientos
                        .Include(m => m.Usuario)
                        .Include(m => m.Producto)
                        .Select(m => new MovimientoDTO
                        {
                            MovimientoID = m.MovimientoID,
                            FechaHora = m.FechaHora,
                            UsuarioID = m.UsuarioID,
                            ProductoID = m.ProductoID,
                            TipoMovimiento = m.TipoMovimiento,
                            Cantidad = m.Cantidad,

                            Usuario = m.Usuario == null
                                ? null
                                : new UsuarioDTO
                                {
                                    UsuarioID = m.Usuario.UsuarioID,
                                    NombreUsuario = m.Usuario.NombreUsuario
                                },

                            Producto = m.Producto == null
                                ? null
                                : new ProductoDTO
                                {
                                    ProductoID = m.Producto.ProductoID,
                                    Nombre = m.Producto.Nombre
                                }
                        })
                        .ToList();

                    return movimientos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consultar movimientos: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"Detalle interno: {ex.InnerException.Message}");

                    return new List<MovimientoDTO>();
                }
            }
        }

        public List<MovimientoDTO> BuscarMovimientos(string valorBusqueda)
        {
            using (var context = new DBContext())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(valorBusqueda))
                    {
                        // Si no se escribe nada, devolver todos los movimientos
                        return ObtenerMovimientos();
                    }

                    valorBusqueda = valorBusqueda.ToLower();
                    
                    var movimientosFiltrados = context.Movientos
                        .Include("Usuario")
                        .Include("Producto")
                        .Where(m =>
                            m.TipoMovimiento.ToLower().Contains(valorBusqueda) ||
                            m.Usuario.Nombre.ToLower().Contains(valorBusqueda) ||
                            m.Usuario.NombreUsuario.ToLower().Contains(valorBusqueda) ||
                            m.Producto.Nombre.ToLower().Contains(valorBusqueda) ||
                            m.Producto.Codigo.ToLower().Contains(valorBusqueda)
                        )
                        .Select(m => new MovimientoDTO
                        {
                            MovimientoID = m.MovimientoID,
                            FechaHora = m.FechaHora,
                            UsuarioID = m.UsuarioID,
                            ProductoID = m.ProductoID,
                            TipoMovimiento = m.TipoMovimiento,
                            Cantidad = m.Cantidad,

                            Usuario = m.Usuario == null
                                ? null
                                : new UsuarioDTO
                                {
                                    UsuarioID = m.Usuario.UsuarioID,
                                    NombreUsuario = m.Usuario.NombreUsuario
                                },

                            Producto = m.Producto == null
                                ? null
                                : new ProductoDTO
                                {
                                    ProductoID = m.Producto.ProductoID,
                                    Nombre = m.Producto.Nombre
                                }
                        })
                        .ToList();

                    return movimientosFiltrados;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al buscar movimientos: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"Detalle interno: {ex.InnerException.Message}");
                    return new List<MovimientoDTO>();
                }
            }
        }

        public bool AnularMovimiento(MovimientoDTO movimiento)
        {
            using (var db = new DBContext())
            {
                using (var transaccion = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Buscar el movimiento en la base de datos
                        var movimientoExistente = db.Movientos.FirstOrDefault(m => m.MovimientoID == movimiento.MovimientoID);
                        if (movimientoExistente == null)
                        {
                            Console.WriteLine("No se encontró el movimiento a anular.");
                            return false;
                        }

                        // Buscar el producto relacionado
                        var producto = db.Productos.FirstOrDefault(p => p.ProductoID == movimientoExistente.ProductoID);
                        if (producto == null)
                        {
                            Console.WriteLine("No se encontró el producto relacionado al movimiento.");
                            return false;
                        }

                        // Ajustar el stock según el tipo de movimiento
                        if (movimientoExistente.TipoMovimiento.ToLower() == "entrada")
                        {
                            // Si fue una entrada, al anular se resta del stock
                            producto.Stock -= movimientoExistente.Cantidad;
                        }
                        else if (movimientoExistente.TipoMovimiento.ToLower() == "salida")
                        {
                            // Si fue una salida, al anular se suma al stock
                            producto.Stock += movimientoExistente.Cantidad;
                        }

                        // Eliminar el movimiento
                        db.Movientos.Remove(movimientoExistente);

                        // Guardar cambios y confirmar transacción
                        db.SaveChanges();
                        transaccion.Commit();

                        Console.WriteLine($"Movimiento {movimientoExistente.MovimientoID} anulado correctamente. Stock actualizado.");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();
                        Console.WriteLine($"Error al anular el movimiento: {ex.Message}");
                        return false;
                    }
                }
            }
        }


        public List<ProductoDTO> ObtenerProductosParaMovimiento()
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
                            IDCategoria = u.IDCategoria
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
        
        public List<ProductoDTO> BuscarProductosParaMovimiento(string valorBusqueda)
        {
            using (var context = new DBContext())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(valorBusqueda))
                    {
                        // Si no hay texto de búsqueda, devuelve todos
                        return ObtenerProductosParaMovimiento();
                    }

                    var ProductosFiltrados = context.Productos
                        .Where(u => u.Codigo.Contains(valorBusqueda) || u.Nombre.Contains(valorBusqueda) || u.Descripcion.Contains(valorBusqueda))
                        .Select(u => new ProductoDTO
                        {
                            ProductoID = u.ProductoID,
                            Codigo = u.Codigo,
                            Nombre = u.Nombre,
                            Descripcion = u.Descripcion,
                            Stock = u.Stock,
                            StockApartado = u.StockApartado,
                            StockMinimo = u.StockMinimo
                        })
                        .ToList();

                    return ProductosFiltrados;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al buscar usuarios: {ex.Message}");
                    return new List<ProductoDTO>();
                }
            }
        }
        
        public List<MovimientoDTO> ObtenerMovimientosPorRangoFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            using (var context = new DBContext())
            {
                try
                {
                    var movimientos = context.Movientos
                        .Include(m => m.Usuario)
                        .Include(m => m.Producto)
                        .Where(m => m.FechaHora >= fechaInicio && m.FechaHora <= fechaFin)
                        .OrderBy(m => m.FechaHora)
                        .Select(m => new MovimientoDTO
                        {
                            MovimientoID = m.MovimientoID,
                            FechaHora = m.FechaHora,
                            UsuarioID = m.UsuarioID,
                            ProductoID = m.ProductoID,
                            TipoMovimiento = m.TipoMovimiento,
                            Cantidad = m.Cantidad,

                            Usuario = m.Usuario == null
                                ? null
                                : new UsuarioDTO
                                {
                                    UsuarioID = m.Usuario.UsuarioID,
                                    NombreUsuario = m.Usuario.NombreUsuario
                                },

                            Producto = m.Producto == null
                                ? null
                                : new ProductoDTO
                                {
                                    ProductoID = m.Producto.ProductoID,
                                    Nombre = m.Producto.Nombre
                                }
                        })
                        .OrderByDescending(m => m.FechaHora)
                        .ToList();

                    return movimientos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consultar movimientos entre fechas: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"Detalle interno: {ex.InnerException.Message}");

                    return new List<MovimientoDTO>();
                }
            }
        }

        
        
        
    }
}