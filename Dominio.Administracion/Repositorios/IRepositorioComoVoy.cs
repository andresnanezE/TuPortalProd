using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioComoVoy
    {
        IEnumerable<ComoVoy> CantidadVentasAsesorMesActual(decimal ccAsesor, string rol, string pathImagenesComoVoy, string urlActionDownloadPdf);

        string ObtenerPlanesDeVuelo();

        void ModificarPlanesDeVuelo(string plan1, string plan2);
    }
}