using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos.DtoKheiron;
using System.Collections.Generic;

//using Aplicacion.Administracion.Dto.DtoKheiron;
//
// j0HNn3LS0N r0DRIGU3Z
// 2016-10-25
namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionProcesosAsesor
    {
        IEnumerable<ProcesosAsesorDto> ProcesosAsesor(int mes, int anio, int ccAsesor);

        IEnumerable<EMB_CIUDADDto> ObtenerCiudades();
    }
}