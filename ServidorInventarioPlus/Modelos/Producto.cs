using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServidorInventarioPlus.Modelos
{
    [Table("Productos")]
    public class Producto
    {
        public int ProductoID { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public int StockApartado { get; set; }
        public int StockMinimo { get; set; }
        public int ProveedorID { get; set; }
        public int IDCategoria { get; set; }
        public ICollection<ProductoAtributo> Atributos { get; set; }
        public ICollection<Moviento> Movimientos { get; set; }
        public ICollection<Reserva> Reservas { get; set; }
    }
}