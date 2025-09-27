using System;

namespace BibliotecaClasesNetframework.ModelosODT
{
    public class MovimientoDTO
    {
        public int MovimientoID { get; set; }
        public DateTime FechaHora { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int ProductoID { get; set; }
        public int UsuarioID { get; set; }
    }

    public class CrearMovimientoDTO
    {
        public int UsuarioID { get; set; }
        public int ProductoID { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}