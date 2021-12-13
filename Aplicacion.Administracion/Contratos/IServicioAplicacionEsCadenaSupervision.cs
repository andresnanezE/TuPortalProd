using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionEsCadenaSupervision
    {
        IEnumerable<CadenaSupervisionDto> EsCadenaSupervision(string cc);
    }
}