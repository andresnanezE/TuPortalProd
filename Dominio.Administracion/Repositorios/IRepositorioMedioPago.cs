using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoStone.DtoSitioWeb;
using Dominio.Administracion.Entidades.ModeloMedioPago;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioMedioPago
    {
        List<MedioPagoAfiliado> ObtenerMedioPagoActualPorUsuario(string numDocu);
        Task<IEnumerable<BancosDto>> GetBancosAsyc();
        Task<ResponseMedioPago> UpdateMedioPagoAsync(DatosActualizacionMedioPagoDto model);
        Task UpdateMedioPagoIvrAsync(DatosActualizacionMedioPagoIvrDto model);
    }
}
