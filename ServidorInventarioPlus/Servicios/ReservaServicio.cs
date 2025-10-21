using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using ServidorInventarioPlus.Context;
using ServidorInventarioPlus.Modelos;
using Xceed.Wpf.Toolkit;


namespace ServidorInventarioPlus.Servicios
{
    public class ReservaServicio : IReservaService
    {
        private static List<ReservaDTO> _reservas = new List<ReservaDTO>();


        public bool CrearReserva(ReservaDTO reserva)
        {
            using (var db = new DBContext())
            {
                try
                {
                    // 1️⃣ Buscar el producto asociado a la reserva
                    var producto = db.Productos.FirstOrDefault(p => p.ProductoID == reserva.ProductoID);
                    if (producto == null)
                    {
                        MessageBox.Show("El producto seleccionado no existe.");
                        return false;
                    }

                    // 2️⃣ Verificar stock disponible
                    if (producto.Stock < reserva.CantidadReservada)
                    {
                        MessageBox.Show("La cantidad a reservar excede el stock actual.");
                        return false;
                    }

                    // 3️⃣ Crear la reserva
                    var nuevaReserva = new Reserva
                    {
                        NumeroReserva = reserva.NumeroReserva,
                        CantidadReservada = reserva.CantidadReservada,
                        FechaHora = reserva.FechaHora == default(DateTime)
                            ? DateTime.Now  // 👈 asigna la fecha actual si viene vacía
                            : reserva.FechaHora,
                        ProductoID = reserva.ProductoID,
                        Cliente = reserva.Cliente
                    };

                    db.Reservas.Add(nuevaReserva);

                    // 5️⃣ Guardar los cambios
                    db.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error al crear la reserva: {e.Message}");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.Source);
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.InnerException);
                    if (e.InnerException != null)
                        Console.WriteLine($"Detalles: {e.InnerException.Message}");
                    return false;
                }
            }
        }


        public List<ReservaDTO> ObtenerReservas()
        {
            using (var context = new DBContext())
            {
                try
                {
                    var reservas = context.Reservas.Select(u => new ReservaDTO
                    {
                        ReservaID = u.ReservaID,
                        NumeroReserva = u.NumeroReserva,
                        CantidadReservada = u.CantidadReservada,
                        FechaHora = u.FechaHora,
                        ProductoID = u.ProductoID,
                        Cliente = u.Cliente
                    })
                        .ToList();
                    
                    return reservas;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error al consultar Proveedores: {e.Message}");
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.InnerException);
                    return new List<ReservaDTO>();
                }
            }
        }

        public bool CancelarReserva(int reservaID)
        {
            using (var db  = new DBContext())
            {
                try
                {
                    var reservaCancelar = db.Reservas.FirstOrDefault(reserva => reserva.ReservaID == reservaID);
                    if(reservaCancelar == null)
                        return false;
                    
                    var produto = db.Productos.FirstOrDefault(producto => reservaCancelar.ProductoID == producto.ProductoID);
                    if (produto == null)
                        return false;
                    
                    produto.StockApartado = (produto.StockApartado - reservaCancelar.CantidadReservada);
                    produto.Stock = (produto.Stock +  reservaCancelar.CantidadReservada);
                    
                    db.Reservas.Remove(reservaCancelar);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error al consultar Reserva: {e.Message}");
                    return false;
                }
            }
        }
    }
}