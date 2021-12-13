// ----------------------------------------------------------------------------------------------
// <copyright file="ServicioAplicacionTablasBase.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto.DtoKheiron;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModelKheiron;
using Dominio.Administracion.Repositorios;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionTablasBase : IServicioAplicacionTablasBase
    {
        #region Constants

        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";

        #endregion

        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioTablasBase _repositorioTablasBase;

        #endregion

        #region C'tors

        public ServicioAplicacionTablasBase(IRepositorioLogs manejadorLogs, IRepositorioTablasBase repositorioTablasBase, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _manejadorLogs = manejadorLogs;
            _repositorioTablasBase = repositorioTablasBase;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion

        #region Instance Methods

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        #endregion

        #region IServicioAplicacionTablasBase Members

        //public IEnumerable<AreaMetropolitana> ObtenerAreasMetropolitanas()
        //{
        //    IEnumerable<AreaMetropolitana> respuestaAreasMetropolitanas;
        //    try
        //    {
        //        respuestaAreasMetropolitanas = _repositorioTablasBase.ObtenerAreasMetropolitanas();
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }

        //    return respuestaAreasMetropolitanas;
        //}

        //public IEnumerable<EMB_CIUDAD> ObtenerCiudades()
        //{
        //    IEnumerable<EMB_CIUDAD> respuestaCiudades;
        //    try
        //    {
        //        respuestaCiudades = _repositorioTablasBase.ObtenerCiudades();
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }

        //    return respuestaCiudades;
        //}

        //public IEnumerable<Ciudad> ObtenerCiudadesAreaMetropolitana(string areaId)
        //{
        //    IEnumerable<Ciudad> respuestaCiudades;
        //    try
        //    {
        //        respuestaCiudades = _repositorioTablasBase.ObtenerCiudadesAreaMetropolitana(areaId);
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }

        //    return respuestaCiudades;
        //}

        //public IEnumerable<Sector> ObtenerSectoresCiudad(string ciudadId)
        //{
        //    IEnumerable<Sector> respuestaSectores;
        //    try
        //    {
        //        respuestaSectores = _repositorioTablasBase.ObtenerSectoresCiudad(ciudadId);
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }

        //    return respuestaSectores;
        //}

        //public IEnumerable<EMB_SedeTurnos> ObtenerSedesCiudad(string ciudadId)
        //{
        //    IEnumerable<EMB_SedeTurnos> respuestaCiudades;
        //    try
        //    {
        //        respuestaCiudades = _repositorioTablasBase.ObtenerSedesCiudad(ciudadId);
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }

        //    return respuestaCiudades;
        //}

        public IEnumerable<EMB_TIPO_IDENTIFICACIONDto> ObtenerTipoDocumentos()
        {
            IEnumerable<EMB_TIPO_IDENTIFICACIONDto> respuestaTipoDocumentos;
            try
            {
                var respuestaEntidad = _repositorioTablasBase.ObtenerTipoDocumentos();
                respuestaTipoDocumentos =
                    _adaptadorDeObjetos.Adaptar<IEnumerable<EMB_TIPO_IDENTIFICACIONDto>>(respuestaEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }

            return respuestaTipoDocumentos;
        }

        //public IEnumerable<Unidad> ObtenerUnidades()
        //{
        //    IEnumerable<Unidad> respuestaunidades;
        //    try
        //    {
        //        respuestaunidades = _repositorioTablasBase.ObtenerUnidades();
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }

        //    return respuestaunidades;
        //}

        //public IEnumerable<Unidad> ObtenerUnidadesDisponibles()
        //{
        //    IEnumerable<Unidad> respuestaunidades;
        //    try
        //    {
        //        respuestaunidades = _repositorioTablasBase.ObtenerUnidadesDisponibles();
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }

        //    return respuestaunidades;
        //}

        //public IEnumerable<Unidad> ObtenerUnidadesXCiudad(string ciudadId)
        //{
        //    IEnumerable<Unidad> respuestaunidades;
        //    try
        //    {
        //        respuestaunidades = _repositorioTablasBase.ObtenerUnidadesXCiudad(ciudadId);
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }

        //    return respuestaunidades;
        //}

        //public IEnumerable<Zona> ObtenerZonasSector(string sectorId, string ciudadId)
        //{
        //    IEnumerable<Zona> respuestaZonas;
        //    try
        //    {
        //        respuestaZonas = _repositorioTablasBase.ObtenerZonasSector(sectorId, ciudadId);
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }

        //    return respuestaZonas;
        //}

        #endregion
    }
}
