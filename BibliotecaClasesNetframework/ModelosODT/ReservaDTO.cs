using System;

namespace BibliotecaClasesNetframework.ModelosODT
{
    public class ReservaDTO
    {
        public int ReservaID { get; set; }
        public string NumeroReserva { get; set; } = string.Empty;
        public int CantidadReservada { get; set; }
        public DateTime FechaHora { get; set; }
        public int ProductoID { get; set; }
        public string Cliente {get; set;} = string.Empty;
    }
}