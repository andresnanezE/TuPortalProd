// ----------------------------------------------------------------------------------------------
// <copyright file="ServicioAplicacionSesionEnVezDe.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Aplicacion.Administracion.Contratos;
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
    public class ServicioAplicacionSesionEnVezDe : IServicioAplicacionSesionEnVezDe
    {
        #region Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioSesionEnVezDe _repositorioEnVezDe;
#pragma warning disable CS0649 // Field 'ServicioAplicacionSesionEnVezDe._manejadorLogs' is never assigned to, and will always have its default value null
        private readonly IRepositorioLogs _manejadorLogs;
#pragma warning restore CS0649 // Field 'ServicioAplicacionSesionEnVezDe._manejadorLogs' is never assigned to, and will always have its default value null

        #endregion Fields

        #region Instace properties

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        public ServicioAplicacionSesionEnVezDe(IAdaptadorDeObjetos adaptadorDeObjetos, IRepositorioSesionEnVezDe repositorioTerminos)
        {
            _adaptadorDeObjetos = adaptadorDeObjetos;
            _repositorioEnVezDe = repositorioTerminos;
        }

        #endregion Instace properties

        public string CargaTerminosCondiciones()
        {
            return _repositorioEnVezDe.CargaTerminosCondiciones();
        }

        public IEnumerable<SesionEnVezDeDto> ValidarDocumento(string documento, string usuario)
        {
            try
            {
                var lista = _repositorioEnVezDe.ValidarDocumento(documento, usuario);

                var listaDto = _adaptadorDeObjetos.Adaptar<IEnumerable<SesionEnVezDeDto>>(lista);
                return listaDto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error validando el documento");
            }
        }
    }
}