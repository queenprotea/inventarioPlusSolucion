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
                    var produto = db.Productos.FirstOrDefault(producto => reserva.ProductoID == producto.ProductoID);
                    if (produto == null)
                        return false;
                    if (produto.Stock < reserva.CantidadReservada)
                    {
                        MessageBox.Show("La cantidad a reservar excede el stock actual");
                        return false;
                    }

                    db.Reservas.Add(new Reserva
                    {
                        NumeroReserva = reserva.NumeroReserva,
                        CantidadReservada = reserva.CantidadReservada,
                        FechaHora = reserva.FechaHora,
                        ProductoID = reserva.ProductoID,
                    });
                    
                    produto.Codigo = produto.Codigo;
                    produto.Nombre = produto.Nombre;
                    produto.StockApartado = (produto.StockApartado + reserva.CantidadReservada);
                    produto.Stock = (produto.Stock -  reserva.CantidadReservada);
                    produto.Descripcion = produto.Descripcion;
                    produto.StockMinimo = produto.StockMinimo;
                    
                    db.SaveChanges();
                    
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error al registrar usuario: {e.Message}");
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
                    
                    produto.Codigo = produto.Codigo;
                    produto.Nombre = produto.Nombre;
                    produto.StockApartado = (produto.StockApartado - reservaCancelar.CantidadReservada);
                    produto.Stock = (produto.Stock +  reservaCancelar.CantidadReservada);
                    produto.Descripcion = produto.Descripcion;
                    produto.StockMinimo = produto.StockMinimo;
                    
                    db.Reservas.Remove(reservaCancelar);
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