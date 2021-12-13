// ----------------------------------------------------------------------------------------------
// <copyright file="ServicioAplicacionDestacado.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionDestacado : IServicioAplicacionDestacado
    {
        #region Constants

        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";

        #endregion Constants

        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioDestacado _repositorioDestacado;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionDestacado(IRepositorioDestacado repositorioDestacado, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioDestacado = repositorioDestacado;
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

        #region IServicioAplicacionDestacado Members

        /// <summary>
        /// Modifica la lista de destacados
        /// </summary>
        /// <param name="destacados">Lista de descatacod</param>
        /// <param name="pathsImagenes">Losta de paths de las imagenes de cada destacado</param>
        /// <param name="pathImagenesDestino">Path de destino para la imagen</param>
        public void ModificarDestacados(IEnumerable<DYK_DESTACADODto> destacados, IEnumerable<string> pathsImagenes, string pathImagenesDestino)
        {
            try
            {
                var list = _adaptadorDeObjetos.Adaptar<IEnumerable<DYK_DESTACADO>>(destacados);
                _repositorioDestacado.ModificarDestacados(list, pathsImagenes, pathImagenesDestino);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        /// <summary>
        /// Trae la lista de destacados
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DYK_DESTACADODto> TraerDestacados()
        {
            try
            {
                var entidades = _repositorioDestacado.TraerDestacados();
                var list = _adaptadorDeObjetos.Adaptar<IEnumerable<DYK_DESTACADODto>>(entidades);
                return list;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        #endregion IServicioAplicacionDestacado Members
    }
}