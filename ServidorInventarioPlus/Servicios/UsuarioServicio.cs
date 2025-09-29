using System;
using System.Collections.Generic;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using ServidorInventarioPlus.Context;
using ServidorInventarioPlus.Modelos;
using System.Linq;
using System.Data.Entity;

namespace ServidorInventarioPlus.Servicios
{
    public class UsuarioServicio : IUsuarioService
    {
        public UsuarioDTO IniciarSesion(string nombreUsuario, string password)
        {
            Console.WriteLine("Iniciando sesion");
            using (var context = new DBContext())
            {
                Console.WriteLine("entro aqui?");
                // Busca usuario por nombre y contraseña sin encriptar (temporal)
                try
                {
                    var usuario = context.Usuarios
                        .FirstOrDefault(u => u.NombreUsuario == nombreUsuario 
                                             && u.Contrasena == password);
                    if (usuario != null)
                    {
                        return new UsuarioDTO
                        {
                            UsuarioID = usuario.UsuarioID,
                            Nombre = usuario.NombreUsuario,
                            Rol = usuario.Rol
                        };
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                Console.WriteLine("qu tal aqui");
               
                return null;
            }
        }
        
        public bool RegistrarUsuario(UsuarioDTO usuario)
        {
            using (var db = new DBContext())
            {
                try
                {
                    // Validar si ya existe un usuario con el mismo NombreUsuario
                    if (db.Usuarios.Any(u => u.NombreUsuario == usuario.NombreUsuario))
                        return false;

                    db.Usuarios.Add(new Usuario
                    {
                        Nombre = usuario.Nombre,
                        NombreUsuario = usuario.NombreUsuario,
                        Contrasena = usuario.Contrasena,
                        Rol = usuario.Rol
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

        public List<UsuarioDTO> ObtenerUsuarios()
        {
            using (var context = new DBContext())
            {
                try
                {
                    var usuarios = context.Usuarios
                        .Select(u => new UsuarioDTO
                        {
                            UsuarioID = u.UsuarioID,
                            Nombre = u.Nombre,
                            NombreUsuario = u.NombreUsuario,
                            Rol = u.Rol,
                            Contrasena = u.Contrasena
                        })
                        .ToList();

                    return usuarios;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consultar usuarios: {ex.Message}");
                    return new List<UsuarioDTO>();
                }
            }
        }
        
        
        public List<UsuarioDTO> BuscarUsuarios(string valorBusqueda)
        {
            using (var context = new DBContext())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(valorBusqueda))
                    {
                        // Si no hay texto de búsqueda, devuelve todos
                        return ObtenerUsuarios();
                    }

                    var usuariosFiltrados = context.Usuarios
                        .Where(u => u.Nombre.Contains(valorBusqueda) || u.NombreUsuario.Contains(valorBusqueda))
                        .Select(u => new UsuarioDTO
                        {
                            UsuarioID = u.UsuarioID,
                            Nombre = u.Nombre,
                            NombreUsuario = u.NombreUsuario,
                            Rol = u.Rol,
                            Contrasena = u.Contrasena
                        })
                        .ToList();

                    return usuariosFiltrados;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al buscar usuarios: {ex.Message}");
                    return new List<UsuarioDTO>();
                }
            }
        }

        public bool EliminarUsuario(int usuarioId)
        {
            using (var db = new DBContext())
            {
                try
                {
                    var usuario = db.Usuarios.FirstOrDefault(u => u.UsuarioID == usuarioId);
                    if (usuario == null)
                        return false;

                    db.Usuarios.Remove(usuario);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
                    return false;
                }
            }
        }

        public bool ModificarUsuario(UsuarioDTO usuarioDto)
        {
            using (var db = new DBContext())
            {
                try
                {
                    var usuario = db.Usuarios.FirstOrDefault(u => u.UsuarioID == usuarioDto.UsuarioID);
                    if (usuario == null)
                        return false;

                    // Actualizamos campos
                    usuario.Nombre = usuarioDto.Nombre;
                    usuario.NombreUsuario = usuarioDto.NombreUsuario;
                    usuario.Contrasena = usuarioDto.Contrasena;
                    usuario.Rol = usuarioDto.Rol;

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

        
        
    }
}