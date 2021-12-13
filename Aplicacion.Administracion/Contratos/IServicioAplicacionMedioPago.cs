using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoStone.DtoSitioWeb;
using Dominio.Administracion.Entidades.ModeloMedioPago;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionMedioPago
    {
        List<MedioPagoAfiliado> ObtenerMedioPagoActualPorUsuario(string numDocu);
        Task<IEnumerable<BancosDto>> GetBancosAsyc();
        Task<ResponseMedioPago> UpdateMedioPagoAsyc(DatosActualizacionMedioPagoDto model);
        Task<bool> UpdateMedioPagoIvrAsyc(DatosActualizacionMedioPagoIvrDto model);
    }
}
