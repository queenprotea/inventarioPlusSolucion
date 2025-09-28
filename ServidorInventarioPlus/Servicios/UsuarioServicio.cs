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
                            Nombre = usuario.NombreUsuario
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
    }
}