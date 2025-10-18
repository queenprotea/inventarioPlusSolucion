using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BibliotecaClasesNetframework.ModelosODT;
using BibliotecaClasesNetframework.Contratos;
using ServidorInventarioPlus.Context;
using ServidorInventarioPlus.Modelos;

namespace ServidorInventarioPlus.Servicios
{
    public class ProveedorServicio : IProveedorService
    {
        private static List<ProveedorDTO> _proveedores = new List<ProveedorDTO>();

        public bool AgregarProveedor(ProveedorDTO proveedor)
        {
            using (var db = new DBContext())
            {
                try
                {
                    // Validar si ya existe un usuario con el mismo NombreUsuario
                    db.Proveedores.Add(new Proveedor
                    {
                        Nombre = proveedor.Nombre,
                        Telefono = proveedor.Telefono,
                        Direccion = proveedor.Direccion,
                        Correo = proveedor.Correo,
                        
                    });

                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al registrar usuario: {ex.Message}");
                    return false;
                }
            }
        }

        public List<ProveedorDTO> ListarProveedores()
        {
            return _proveedores;
        }
        
        public List<ProveedorDTO> BuscarProveedores(string valorBusqueda)
        {
            using (var context = new DBContext())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(valorBusqueda))
                    {
                        // Si no hay texto de búsqueda, devuelve todos
                        return ObtenerProveedores();
                    }
                    
                    var proveedoresFiltrador = context.Proveedores
                        .Where(u => u.Nombre.Contains(valorBusqueda) || u.Direccion.Contains(valorBusqueda) || u.Telefono.Contains(valorBusqueda) || u.Correo.Contains(valorBusqueda))
                        .Select(u => new ProveedorDTO()
                        {
                            ProveedorID = u.ProveedorID,
                            Nombre = u.Nombre,
                            Direccion = u.Direccion,
                            Telefono = u.Telefono,
                            Correo = u.Correo,
                            
                        })
                        .ToList();

                    return proveedoresFiltrador;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al buscar proveedores: {ex.Message}");
                    return new List<ProveedorDTO>();
                }
            }
        }

        public ProveedorDTO ObtenerProveedor(int id)
        {
            return _proveedores.Find(p => p.ProveedorID == id);
        }

        public bool ActualizarProveedor(ProveedorDTO proveedorDto)
        {
            using (var db = new DBContext())
            {
                try
                {
                    var proveedor  = db.Proveedores.FirstOrDefault(p => p.ProveedorID == proveedorDto.ProveedorID);
                    if (proveedor == null)
                        return false;

                    // Actualizamos campos
                    proveedor.Nombre = proveedorDto.Nombre;
                    proveedor.Correo = proveedorDto.Correo;
                    proveedor.Direccion = proveedorDto.Direccion;
                    proveedor.Telefono = proveedorDto.Telefono;

                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al modificar usuario: {ex.Message}");
                    return false;
                }
            }
        }

        public bool EliminarProveedor(int id)
        {
            using (var db = new DBContext())
            {
                try
                {
                    // No permitir eliminar el proveedor por defecto
                    if (id == 1)
                        throw new InvalidOperationException("No se puede eliminar el proveedor por defecto.");

                    var proveedor = db.Proveedores
                        .Include(p => p.Productos)
                        .FirstOrDefault(p => p.ProveedorID == id);

                    if (proveedor == null)
                        return false;

                    // Obtener proveedor por defecto
                    var proveedorDefault = db.Proveedores.FirstOrDefault(p => p.ProveedorID == 1);
                    if (proveedorDefault == null)
                        throw new InvalidOperationException("No existe un proveedor por defecto en la base de datos.");

                    // Reasignar productos al proveedor por defecto
                    foreach (var producto in proveedor.Productos)
                    

                    // Guardar cambios de productos
                    db.SaveChanges();

                    // Ahora eliminar el proveedor
                    db.Proveedores.Remove(proveedor);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar proveedor: {ex.Message}");
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine(ex.StackTrace);
                    return false;
                }
            }
        }

        
        public List<ProveedorDTO> ObtenerProveedores()
        {
            using (var context = new DBContext())
            {
                try
                {
                    var proveedores = context.Proveedores
                        .Select(u => new ProveedorDTO
                        {
                            ProveedorID = u.ProveedorID,
                            Nombre = u.Nombre,
                            Correo = u.Correo,
                            Direccion = u.Direccion,
                            Telefono = u.Telefono,
                            
                        })
                        .ToList();

                    return proveedores;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consultar Proveedores: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.InnerException);
                    return new List<ProveedorDTO>();
                }
            }
        }
        
    }
}