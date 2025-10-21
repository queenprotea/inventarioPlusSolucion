using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServidorInventarioPlus.Modelos
{
    [Table("Reservas")]
    public class Reserva
    {
        public int ReservaID { get; set; }
        public string NumeroReserva { get; set; } = string.Empty;
        public int ProductoID { get; set; }
        public int CantidadReservada { get; set; }
        public DateTime FechaHora { get; set; }

        public Producto Producto { get; set; }
        public string Cliente { get; set; }
    }
}