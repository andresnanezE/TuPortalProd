// ----------------------------------------------------------------------------------------------
// <copyright file="ServicioAplicacionMenu.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Aplicacion.Administracion.Contratos;

//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionLogs : IServicioAplicacionLogs
    {
        #region Constants

        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";

        #endregion Constants

        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionLogs(IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _manejadorLogs = manejadorLogs;

            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion C'tors

        #region Instance Methods

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion, null);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        #endregion Instance Methods

        #region IServicioAplicacionMenu Members

        public EmbLogActividadesDto AgregarLog(EmbLogActividadesDto log)
        {
            var logActividadesDto = new EmbLogActividadesDto();
            try
            {
                //var dto = _adaptadorDeObjetos.Adaptar<EMB_LogActividades>(log);
                var dto = Transversales.Administracion.Mappers.MapperDTOAEMBLogActividades.AdaptarMenu(log);
                var result = _manejadorLogs.LogActividades(dto);
                //logActividadesDto = _adaptadorDeObjetos.Adaptar<EmbLogActividadesDto>(result);
                logActividadesDto = Transversales.Administracion.Mappers.MapperEMBLogActividadesADTO.AdaptarMenu(result);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return logActividadesDto;
        }

        public IEnumerable<EMB_TipoLogDto> ObtenerTiposLog()
        {
            IEnumerable<EMB_TipoLogDto> dtoTipoLog;
            try
            {
                var tipoLog = _manejadorLogs.ObtenerTiposLog();
                //dtoTipoLog = _adaptadorDeObjetos.Adaptar<IEnumerable<EMB_TipoLogDto>>(tipoLog);
                dtoTipoLog = Transversales.Administracion.Mappers.MapperListTipoLogADTO.AdaptarMenu(tipoLog);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return dtoTipoLog;
        }

        public EmbLogActividadesDto ObtenerObtenerLogSesionEnVezDe(int idLog)
        {
            EmbLogActividadesDto dtoLog;
            try
            {
                var logActividades = _manejadorLogs.ObtenerLogSesionEnVezDe(idLog);
                //dtoLog = _adaptadorDeObjetos.Adaptar<EmbLogActividadesDto>(logActividades);
                dtoLog = Transversales.Administracion.Mappers.MapperEMBLogActividadesADTO.AdaptarMenu(logActividades);
            }
            catch (SystemException ex)
            {
                throw LogExepxion(ex, ErrorProcesandoPeticion);
            }
            return dtoLog;
        }

        public void modificaLogSesionEnVezDe(EmbLogActividadesDto log)
        {
            try
            {
                //var dto = _adaptadorDeObjetos.Adaptar<EMB_LogActividades>(log);
                var dto = Transversales.Administracion.Mappers.MapperDTOAEMBLogActividades.AdaptarMenu(log);
                _manejadorLogs.modificaLogSesionEnVezDe(dto);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        #endregion IServicioAplicacionMenu Members
    }
}