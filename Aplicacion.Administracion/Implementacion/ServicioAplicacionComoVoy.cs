using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;

//using Aplicacion.Administracion.Dto.DtoSitioWeb;
using Transversales.Administracion;

//using Dominio.Administracion.Entidades.MapperDto;
//
// j0HNn3LS0N r0DRIGU3Z
// 2016-10-25
//
namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionComoVoy : IServicioAplicacionComoVoy
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioComoVoy _repositorioComoVoy;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionComoVoy(IRepositorioComoVoy repositorioComoVoy, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioComoVoy = repositorioComoVoy;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion C'tors

        #region Instance Methods

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        #endregion Instance Methods

        public IEnumerable<ComoVoyDto> CantidadVentasAsesorMesActual(decimal ccAsesor, string rol, string pathImagenesComoVoy, string urlActionDownloadPdf)
        {
            try
            {
                var resultado = _repositorioComoVoy.CantidadVentasAsesorMesActual(ccAsesor, rol, pathImagenesComoVoy, urlActionDownloadPdf);
                //return _adaptadorDeObjetos.Adaptar<IEnumerable<ComoVoyDto>>(resultado);
                return Transversales.Administracion.Mappers.MapperListComoVoyADTO.AdaptarMenu(resultado);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo datos para : ComoVoy " + exception.Message);
            }
        }

        public string ObtenerPlanesDeVuelo()
        {
            try
            {
                return _repositorioComoVoy.ObtenerPlanesDeVuelo();
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo datos para : Plan de vuelo " + exception.Message);
            }
        }

        public void ModificarPlanesDeVuelo(string plan1, string plan2)
        {
            try
            {
                _repositorioComoVoy.ModificarPlanesDeVuelo(plan1, plan2);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo datos para : Plan de vuelo " + exception.Message);
            }
        }
    }
}