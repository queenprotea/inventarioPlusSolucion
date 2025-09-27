using System.Collections.Generic;

namespace ServidorInventarioPlus.Modelos
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;

        public ICollection<Moviento> Movimientos { get; set; }
    }
}