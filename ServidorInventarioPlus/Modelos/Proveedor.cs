using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServidorInventarioPlus.Modelos
{
    
    [Table("Proveedores")] 
    public class Proveedor
    {
        public int ProveedorID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string Categoria { get; set; }
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}