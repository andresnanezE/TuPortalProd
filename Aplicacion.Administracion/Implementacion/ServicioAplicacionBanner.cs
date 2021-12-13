// ----------------------------------------------------------------------------------------------
// <copyright file="ServicioAplicacionBanner.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Aplicacion.Administracion.Contratos;

//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionBanner : IServicioAplicacionBanner
    {
        #region Constants

        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";
        private const string ErrorBannerExistente = "El seleccionado ya existe en la Aplicación. Comuníquese con su administrador";
        private const string ErrorBannerNoExistente = "El seleccionado no existe en la Aplicación. Comuníquese con su administrador";

        #endregion Constants

        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioBanner _repositorioBanner;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionBanner(IRepositorioBanner repositorioBanner, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioBanner = repositorioBanner;
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

        #region IServicioAplicacionBanner Members

        /// <summary>
        /// Agrega un nuevo banner
        /// </summary>
        /// <param name="banner">Banner</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        public void AgregarBanner(DYK_BANNERDto banner, string pathImagen, string pathImagenDestino)
        {
            string mensaje = null;
            try
            {
                var existeBanner = _repositorioBanner.TraerBannerPorId(banner.BANNERID);
                if (existeBanner == null)
                {
                    //var datosEntidad = _adaptadorDeObjetos.Adaptar<DYK_BANNER>(banner);
                    var datosEntidad = Transversales.Administracion.Mappers.MapperDYKBannerADTO.AdaptarMenu(banner);
                    _repositorioBanner.AgregarBanner(datosEntidad, pathImagen, pathImagenDestino);
                }
                else
                {
                    mensaje = ErrorBannerExistente;
                    throw new SystemException();
                }
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, mensaje ?? ErrorProcesandoPeticion);
            }
        }

        /// <summary>
        /// Modifica la informción de un banner
        /// </summary>
        /// <param name="banner">Banner</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        public void ModificarBanner(DYK_BANNERDto banner, string pathImagen, string pathImagenDestino)
        {
            try
            {
                //var bannerEntidad = _adaptadorDeObjetos.Adaptar<DYK_BANNER>(banner);
                var bannerEntidad = Transversales.Administracion.Mappers.MapperDYKBannerADTO.AdaptarMenu(banner);
                _repositorioBanner.ModificarBanner(bannerEntidad, pathImagen, pathImagenDestino);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        /// <summary>
        /// Elimina un banner
        /// </summary>
        /// <param name="bannerId">Identificador del banner</param>
        public void EliminarBanner(Guid bannerId)
        {
            string mensaje = null;
            try
            {
                var existeBanner = _repositorioBanner.TraerBannerPorId(bannerId);
                if (existeBanner != null)
                {
                    _repositorioBanner.EliminarBanner(bannerId);
                }
                else
                {
                    mensaje = ErrorBannerNoExistente;
                    throw new SystemException();
                }
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, mensaje ?? ErrorProcesandoPeticion);
            }
        }

        /// <summary>
        /// Tarea un banner por el identificador
        /// </summary>
        /// <param name="bannerId">Identificador del banner</param>
        /// <returns></returns>
        public DYK_BANNERDto TraerBannerPorId(Guid bannerId)
        {
            DYK_BANNERDto respuestaBanner;
            try
            {
                var resultadoEntidad = _repositorioBanner.TraerBannerPorId(bannerId);
                //respuestaBanner = _adaptadorDeObjetos.Adaptar<DYK_BANNERDto>(resultadoEntidad);
                respuestaBanner = Transversales.Administracion.Mappers.MapperDTOADYKBanner.AdaptarMenu(resultadoEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return respuestaBanner;
        }

        /// <summary>
        /// Tare la lista de banners
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DYK_BANNERDto> TraerBanners()
        {
            try
            {
                var entidades = _repositorioBanner.TraerBanners();
                //var list = _adaptadorDeObjetos.Adaptar<IEnumerable<DYK_BANNERDto>>(entidades);
                var list = Transversales.Administracion.Mappers.MapperListDYKADTO.AdaptarMenu(entidades);
                return list;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        #endregion IServicioAplicacionBanner Members
    }
}