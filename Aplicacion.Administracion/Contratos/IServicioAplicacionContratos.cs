using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;

//using Dominio.Administracion.Entidades.MapperDto;
//using Aplicacion.Administracion.Dto.DtoSitioWeb;
//using Aplicacion.Administracion.Dto.DtoStone.DtoSitioWeb;
using Dominio.Administracion.Entidades.MapperDto.DtoSitioWeb;
using Dominio.Administracion.Entidades.MapperDto.DtoStone.DtoSitioWeb;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;

//
// j0HNn3LS0N r0DRIGU3Z
// 2016-10-25
namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionContratos
    {
        #region Instance Methods

        IEnumerable<SPEM_CONSULTACONTRATOSDto> ObtenerConsultaContratos(decimal codigo, decimal usuario, int rol);

        IEnumerable<SPEM_CONSULTACONTRATOSDto> ObtenerConsultaContratosPorNombre(string nombre, decimal usuario, int rol);

        IEnumerable<SPEM_CONSULTACONTRATOSDto> ObtenerContratos(decimal codigo);

        IEnumerable<SPEM_CONSULTACONTRATOSDto> ObtenerContratosContratante(decimal codigo, int rmt);

        IEnumerable<SPEM_CONSULTACONTRATOSDto> ObtenerContratosBeneficiario(decimal codigo, int rmt, decimal CCcontratante,
            string contratante);

        IEnumerable<ConsultaBeneficiarioDto> ObtenerBeneficiario(string benef);

        IEnumerable<ConsultaFacturaDto> ObtenerFacturas(string doc);

        IEnumerable<SPCCBENEFINCLUSIONTUPORTALDto> ValidacionBeneficiarioInclusion(int rmtCont, string codTerc, string codBene);

        FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "");
        #endregion Instance Methods
    }
}