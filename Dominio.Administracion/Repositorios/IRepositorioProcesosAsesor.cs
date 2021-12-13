using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModelKheiron;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioProcesosAsesor
    {
        IEnumerable<ProcesosAsesor> ProcesosAsesor(int mes, int anio, int ccAsesor);

        IEnumerable<EMB_CIUDAD> ObtenerCiudades();
    }
}