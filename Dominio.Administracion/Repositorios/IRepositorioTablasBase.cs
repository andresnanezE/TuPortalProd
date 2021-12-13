// ----------------------------------------------------------------------------------------------
// <copyright file="IRepositorioTablasBase.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades.ModelKheiron;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioTablasBase
    {
        #region Instance Methods

        //IEnumerable<AreaMetropolitana> ObtenerAreasMetropolitanas();
        //IEnumerable<EMB_CIUDAD> ObtenerCiudades();
        //IEnumerable<Ciudad> ObtenerCiudadesAreaMetropolitana(string areaId);
        //IEnumerable<Sector> ObtenerSectoresCiudad(string ciudadId);
        //IEnumerable<EMB_SedeTurnos> ObtenerSedesCiudad(string ciudadId);
        IEnumerable<EMB_TIPO_IDENTIFICACION> ObtenerTipoDocumentos();

        //IEnumerable<Unidad> ObtenerUnidades();
        //IEnumerable<Unidad> ObtenerUnidadesDisponibles();
        //IEnumerable<Unidad> ObtenerUnidadesXCiudad(string ciudadId);
        //IEnumerable<Zona> ObtenerZonasSector(string sectorId, string ciudadId);

        #endregion Instance Methods
    }
}