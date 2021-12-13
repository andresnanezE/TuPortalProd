// ----------------------------------------------------------------------------------------------
// <copyright file="ServicioAplicacionNoticia.cs" company="SCI Software">
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
    public class ServicioAplicacionNoticia : IServicioAplicacionNoticia
    {
        #region Constants

        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";
        private const string ErrorNoticiaExistente = "El seleccionado ya existe en la Aplicación. Comuníquese con su administrador";
        private const string ErrorNoticiaNoExistente = "El seleccionado no existe en la Aplicación. Comuníquese con su administrador";

        #endregion Constants

        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioNoticia _repositorioNoticia;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionNoticia(IRepositorioNoticia repositorioNoticia, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioNoticia = repositorioNoticia;
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

        #region IServicioAplicacionNoticia Members

        /// <summary>
        /// Agrega una noticia
        /// </summary>
        /// <param name="noticia">Noticia</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        public void AgregarNoticia(DYK_NOTICIADto noticia, string pathImagen, string pathImagenDestino, string pathBanner, string pathBannerDestino)
        {
            string mensaje = null;
            try
            {
                var existeNoticia = _repositorioNoticia.TraerNoticiaPorId(noticia.NOTICIAID);
                if (existeNoticia == null)
                {
                    //var datosEntidad = _adaptadorDeObjetos.Adaptar<DYK_NOTICIA>(noticia);
                    var datosEntidad = Transversales.Administracion.Mappers.MapperDtoADYKNoticia.AdaptarMenu(noticia);
                    _repositorioNoticia.AgregarNoticia(datosEntidad, pathImagen, pathImagenDestino, pathBanner, pathBannerDestino);
                }
                else
                {
                    mensaje = ErrorNoticiaExistente;
                    throw new SystemException();
                }
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, mensaje ?? ErrorProcesandoPeticion);
            }
        }

        /// <summary>
        /// Modifica una imagen
        /// </summary>
        /// <param name="noticia">Noticia</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        public void ModificarNoticia(DYK_NOTICIADto noticia, string pathImagen, string pathImagenDestino, string pathBanner, string pathBannerDestino)
        {
            try
            {
                //var noticiaEntidad = _adaptadorDeObjetos.Adaptar<DYK_NOTICIA>(noticia);
                var noticiaEntidad = Transversales.Administracion.Mappers.MapperDtoADYKNoticia.AdaptarMenu(noticia);
                _repositorioNoticia.ModificarNoticia(noticiaEntidad, pathImagen, pathImagenDestino, pathBanner, pathBannerDestino);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        /// <summary>
        /// Elimina una noticia
        /// </summary>
        /// <param name="noticiaId">Identificador de la noticia</param>
        public void EliminarNoticia(Guid noticiaId)
        {
            string mensaje = null;
            try
            {
                var existeNoticia = _repositorioNoticia.TraerNoticiaPorId(noticiaId);
                if (existeNoticia != null)
                {
                    _repositorioNoticia.EliminarNoticia(noticiaId);
                }
                else
                {
                    mensaje = ErrorNoticiaNoExistente;
                    throw new SystemException();
                }
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, mensaje ?? ErrorProcesandoPeticion);
            }
        }

        /// <summary>
        /// Trae una noticia por el identificador
        /// </summary>
        /// <param name="noticiaId">Identificador de la noticia</param>
        /// <returns></returns>
        public DYK_NOTICIADto TraerNoticiaPorId(Guid noticiaId)
        {
            DYK_NOTICIADto respuestaNoticia;
            try
            {
                var resultadoEntidad = _repositorioNoticia.TraerNoticiaPorId(noticiaId);
                //respuestaNoticia = _adaptadorDeObjetos.Adaptar<DYK_NOTICIADto>(resultadoEntidad);
                respuestaNoticia = Transversales.Administracion.Mappers.MapperDYKNOTICIAADTO.AdaptarMenu(resultadoEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return respuestaNoticia;
        }

        /// <summary>
        /// Trae una noticia por el titulo
        /// </summary>
        /// <param name="noticiaId">Identificador de la noticia</param>
        /// <returns></returns>
        public DYK_NOTICIADto TraerNoticiaPorTituloQS(string tituloQS)
        {
            DYK_NOTICIADto respuestaNoticia;
            try
            {
                var resultadoEntidad = _repositorioNoticia.TraerNoticiaPorTituloQS(tituloQS);
                //respuestaNoticia = _adaptadorDeObjetos.Adaptar<DYK_NOTICIADto>(resultadoEntidad);
                respuestaNoticia = Transversales.Administracion.Mappers.MapperDYKNOTICIAADTO.AdaptarMenu(resultadoEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return respuestaNoticia;
        }

        /// <summary>
        /// Trae la lista de noticias paginada
        /// </summary>
        /// <param name="soloActivas">Indica si so trae las noticias acrivas</param>
        /// <param name="pageSize">Tamaño de la cantidad de resultados a devolver</param>
        /// <param name="pageIndex">Número de la página</param>
        /// <returns></returns>
        public IEnumerable<DYK_NOTICIA_SPDto> TraerNoticias(bool soloActivas, int pageSize, int pageIndex, string orderBy)
        {
            try
            {
                var noticias = _repositorioNoticia.TraerNoticias(soloActivas, pageSize, pageIndex, orderBy);
                //var list = _adaptadorDeObjetos.Adaptar<IEnumerable<DYK_NOTICIA_SPDto>>(noticias);
                var list = Transversales.Administracion.Mappers.MapperListDTOADYKNOTICIA.AdaptarMenu(noticias);
                return list;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        /// <summary>
        /// Trae el numero total de noticias
        /// </summary>
        /// <param name="soloActivas">Indica si so trae las noticias acrivas</param>
        /// <returns></returns>
        public int TraerNumeroNoticias(bool soloActivas)
        {
            try
            {
                var total = _repositorioNoticia.TraerNumeroNoticias(soloActivas);
                return total;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        #endregion IServicioAplicacionNoticia Members
    }
}