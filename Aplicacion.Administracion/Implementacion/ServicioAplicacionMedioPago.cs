using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoStone.DtoSitioWeb;
using Dominio.Administracion.Entidades.ModeloMedioPago;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionMedioPago : IServicioAplicacionMedioPago
    {
        private readonly IRepositorioMedioPago _repositorioMedioPago;
        public ServicioAplicacionMedioPago(IRepositorioMedioPago repositorioMedioPago){
            _repositorioMedioPago = repositorioMedioPago;
        }

        public List<MedioPagoAfiliado> ObtenerMedioPagoActualPorUsuario(string numDocu)
        {
            try
            {
                var resultado = _repositorioMedioPago.ObtenerMedioPagoActualPorUsuario(numDocu);
                return resultado;
            }
            catch (SystemException excepcion)
            {               
                throw new SystemException(excepcion.Message);
            }
        }

        public async Task<IEnumerable<BancosDto>> GetBancosAsyc()
        {
            try
            {
                return await _repositorioMedioPago.GetBancosAsyc();
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async  Task<ResponseMedioPago> UpdateMedioPagoAsyc(DatosActualizacionMedioPagoDto model) {
            try
            {
                return await _repositorioMedioPago.UpdateMedioPagoAsync(model);
            }
            catch (SystemException ex)
            {
                return new ResponseMedioPago();
            }
        }

        public async Task<bool> UpdateMedioPagoIvrAsyc(DatosActualizacionMedioPagoIvrDto model)
        {
            try
            {
               await _repositorioMedioPago.UpdateMedioPagoIvrAsync(model);
                return true;
            }
            catch (SystemException ex)
            {
                return false;
            }
        }
    }
}
