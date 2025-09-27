namespace ServidorInventarioPlus.Modelos
{
    public class Proveedor
    {
        public int ProveedorID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string Categoria { get; set; }
    }
}