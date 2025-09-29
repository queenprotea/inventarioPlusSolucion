using System;
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

        
        
        
        
    }
}