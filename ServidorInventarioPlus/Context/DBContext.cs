using System.Data.Entity;
using ServidorInventarioPlus.Modelos;

namespace ServidorInventarioPlus.Context
{
    public class DBContext  : DbContext
    {
        public DBContext() : base("name=DefaultConnection") // referencia a App.config
        {
        }

        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Moviento> Movientos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Categoria>  Categorias { get; set; }
        public DbSet<ProductoProveedores> ProductoProveedores { get; set; }
        // Agregas más DbSet según tus modelos
        

    }
}