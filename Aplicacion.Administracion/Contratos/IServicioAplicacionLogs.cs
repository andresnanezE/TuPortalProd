// ----------------------------------------------------------------------------------------------
// <copyright file="IServicioAplicacionMenu.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using System.Collections.Generic;

//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionLogs
    {
        #region Instance Methods

        EmbLogActividadesDto AgregarLog(EmbLogActividadesDto log);

        IEnumerable<EMB_TipoLogDto> ObtenerTiposLog();

        EmbLogActividadesDto ObtenerObtenerLogSesionEnVezDe(int idLog);

        void modificaLogSesionEnVezDe(EmbLogActividadesDto log);

        #endregion Instance Methods
    }
}