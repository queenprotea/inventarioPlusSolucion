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
    }

    public class CrearProductoDTO
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; }
        public int StockMinimo { get; set; }
    }

    public class ActualizarProductoDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? StockMinimo { get; set; }
    }
}