using System.Collections.Generic;
using System.ServiceModel;
using BibliotecaClasesNetframework.ModelosODT;

namespace BibliotecaClasesNetframework.Contratos
{
    [ServiceContract]
    public interface IMovimientoService
    {
        [OperationContract]
        bool RegistrarMovimiento(MovimientoDTO movimiento);
        
        [OperationContract]
        List<MovimientoDTO> ObtenerMovimientos();

        [OperationContract]
        List<MovimientoDTO> BuscarMovimientos(string valorBusqueda);

        [OperationContract]
        bool AnularMovimiento(MovimientoDTO movimiento);
        
        [OperationContract]
        List<ProductoDTO> ObtenerProductosParaMovimiento();

        [OperationContract]
        List<ProductoDTO> BuscarProductosParaMovimiento(string valorBusqueda);
    }
}