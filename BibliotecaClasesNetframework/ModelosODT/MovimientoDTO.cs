using System;

namespace BibliotecaClasesNetframework.ModelosODT
{
    public class MovimientoDTO
    {
        public int MovimientoID { get; set; }
        public DateTime FechaHora { get; set; }
        public int UsuarioID { get; set; }
        public int ProductoID { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        
        public UsuarioDTO Usuario { get; set; }
        public ProductoDTO Producto { get; set; }
        
        public string UsuarioDisplay => Usuario != null
            ? $"{Usuario.NombreUsuario} ({UsuarioID})"
            : $"ID {UsuarioID}";

        public string ProductoDisplay => Producto != null
            ? $"{Producto.Nombre} ({ProductoID})"
            : $"ID {ProductoID}";

    }

    public class CrearMovimientoDTO
    {
        public DateTime FechaHora { get; set; }
        public int UsuarioID { get; set; }
        public int ProductoID { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}