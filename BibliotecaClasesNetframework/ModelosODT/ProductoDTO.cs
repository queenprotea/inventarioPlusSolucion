using System;
using System.Collections.Generic;

namespace BibliotecaClasesNetframework.ModelosODT
{
    public class ProductoDTO
    {
        public int ProductoID { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public int StockApartado { get; set; }
        public int StockMinimo { get; set; }
        
        public int? IDCategoria  { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
         public string NombreCategoria { get; set; }
         public string FirstProveedor { get; set; }
        public List<ProveedorDTO> proveedores { get; set; }
     
    }

    public class CrearProductoDTO
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public int ProveedorID { get; set; }
        public int IDCategoria { get; set; }
    }

    public class ActualizarProductoDTO
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? Stock {get; set;}
        public int? StockMinimo { get; set; }
        public int? ProveedorID { get; set; }
        public int? IDCategoria { get; set; }
    }
}