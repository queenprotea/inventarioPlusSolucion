namespace ServidorInventarioPlus.Modelos
{
    public class ProductoAtributo
    {
        public int ProductoAtributoID { get; set; }
        public int ProductoID { get; set; }
        public string NombreAtributo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;

        public Producto Producto { get; set; }
    }
}