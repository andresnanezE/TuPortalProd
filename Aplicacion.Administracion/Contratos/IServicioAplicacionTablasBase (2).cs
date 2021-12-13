// ----------------------------------------------------------------------------------------------
// <copyright file="IServicioAplicacionTablasBase.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Aplicacion.Administracion.Dto.DtoKheiron;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModelKheiron;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionTablasBase
    {
        #region Instance Methods

        //IEnumerable<AreaMetropolitana> ObtenerAreasMetropolitanas();
        //IEnumerable<EMB_CIUDAD> ObtenerCiudades();
        //IEnumerable<Ciudad> ObtenerCiudadesAreaMetropolitana(string areaId);
        //IEnumerable<Sector> ObtenerSectoresCiudad(string ciudadId);
        //IEnumerable<EMB_SedeTurnos> ObtenerSedesCiudad(string ciudadId);
        IEnumerable<EMB_TIPO_IDENTIFICACIONDto> ObtenerTipoDocumentos();
        //IEnumerable<Unidad> ObtenerUnidades();
        //IEnumerable<Unidad> ObtenerUnidadesDisponibles();
        //IEnumerable<Unidad> ObtenerUnidadesXCiudad(string ciudadId);
        //IEnumerable<Zona> ObtenerZonasSector(string sectorId, string ciudadId);

        #endregion
    }
}
