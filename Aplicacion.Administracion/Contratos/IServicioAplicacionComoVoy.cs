using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

//using Dominio.Administracion.Entidades.MapperDto;
//
// j0HNn3LS0N r0DRIGU3Z
// 2016-10-25
//

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionComoVoy
    {
        IEnumerable<ComoVoyDto> CantidadVentasAsesorMesActual(decimal ccAsesor, string rol, string pathImagenesComoVoy, string urlActionDownloadPdf);

        string ObtenerPlanesDeVuelo();

        void ModificarPlanesDeVuelo(string plan1, string plan2);
    }
}