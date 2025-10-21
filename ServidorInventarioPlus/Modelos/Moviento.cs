using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServidorInventarioPlus.Modelos
{
    [Table("Movimientos")]
    public class Moviento
    {
        [Key]
        public int MovimientoID { get; set; }
        public DateTime FechaHora { get; set; }
        public int UsuarioID { get; set; }
        public int ProductoID { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty; // Entrada / Salida
        public int Cantidad { get; set; }

        public Usuario Usuario { get; set; }
        public Producto Producto { get; set; }
    }
}