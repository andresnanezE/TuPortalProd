// ----------------------------------------------------------------------------------------------
// <copyright file="ServicioAplicacionNovedades.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Aplicacion.Administracion.Contratos;

//using Dominio.Administracion.Entidades.MapperDto;
//using Aplicacion.Administracion.Dto.DtoSitioWeb;
//using Aplicacion.Administracion.Dto.DtoStone.DtoSitioWeb;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoSitioWeb;
using Dominio.Administracion.Entidades.MapperDto.DtoStone.DtoSitioWeb;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Transversales.Administracion;
using Transversales.Administracion.Mappers;

//
// j0HNn3LS0N r0DRIGU3Z
// 2016-10-25
namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionContratos : IServicioAplicacionContratos
    {
        #region Constants

        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";

        #endregion Constants

        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioContratos _repositorioContratos;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionContratos(IRepositorioContratos repositorioContratos, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioContratos = repositorioContratos;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion C'tors

        #region Instance Methods

        public FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        #endregion Instance Methods

        #region IServicioAplicacionNovedades Members

        public IEnumerable<SPEM_CONSULTACONTRATOSDto> ObtenerContratos(decimal codigo)
        {
            try
            {
                var resultado = _repositorioContratos.ObtenerContratos(codigo);
                //return _adaptadorDeObjetos.Adaptar<IEnumerable<SPEM_CONSULTACONTRATOSDto>>(resultado);
                return Transversales.Administracion.Mappers.MapperConsultaContratosADTO.AdaptarMenu(resultado);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<SPEM_CONSULTACONTRATOSDto> ObtenerConsultaContratos(decimal codigo, decimal usuario, int rol)
        {
            try
            {
                var resultado = _repositorioContratos.ObtenerConsultaContratos(codigo, usuario, rol);
                //return _adaptadorDeObjetos.Adaptar<IEnumerable<SPEM_CONSULTACONTRATOSDto>>(resultado);
                return Transversales.Administracion.Mappers.MapperConsultaContratosADTO.AdaptarMenu(resultado);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<SPEM_CONSULTACONTRATOSDto> ObtenerConsultaContratosPorNombre(string nombre, decimal usuario, int rol)
        {
            try
            {
                var resultado = _repositorioContratos.ObtenerConsultaContratosPorNombre(nombre, usuario, rol);
                //return _adaptadorDeObjetos.Adaptar<IEnumerable<SPEM_CONSULTACONTRATOSDto>>(resultado);
                return Transversales.Administracion.Mappers.MapperConsultaContratosADTO.AdaptarMenu(resultado);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<SPEM_CONSULTACONTRATOSDto> ObtenerContratosContratante(decimal codigo, int rmt)
        {
            try
            {
                var resultado = _repositorioContratos.ObtenerContratosContratante(codigo, rmt);
                //return _adaptadorDeObjetos.Adaptar<IEnumerable<SPEM_CONSULTACONTRATOSDto>>(resultado);
                return Transversales.Administracion.Mappers.MapperConsultaContratosADTO.AdaptarMenu(resultado);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<SPEM_CONSULTACONTRATOSDto> ObtenerContratosBeneficiario(decimal codigo, int rmt, decimal CCcontratante, string contratante)
        {
            try
            {
                var resultado = _repositorioContratos.ObtenerContratosBeneficiario(codigo, rmt, CCcontratante, contratante);
                //return _adaptadorDeObjetos.Adaptar<IEnumerable<SPEM_CONSULTACONTRATOSDto>>(resultado);
                return Transversales.Administracion.Mappers.MapperConsultaContratosADTO.AdaptarMenu(resultado);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ConsultaBeneficiarioDto> ObtenerBeneficiario(string benef)
        {
            try
            {
                var resultado = _repositorioContratos.ObtenerBeneficiario(benef);
                //return _adaptadorDeObjetos.Adaptar<IEnumerable<ConsultaBeneficiarioDto>>(resultado);
                return MapperConsultaBeneficiarioADTO.AdaptarMenu(resultado);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ConsultaFacturaDto> ObtenerFacturas(string doc)
        {
            try
            {
                var resultado = _repositorioContratos.ObtenerFacturas(doc);
                //return _adaptadorDeObjetos.Adaptar<IEnumerable<ConsultaFacturaDto>>(resultado);
                return Transversales.Administracion.Mappers.MapperConsultaFacturaADTO.AdaptarMenu(resultado);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<SPCCBENEFINCLUSIONTUPORTALDto> ValidacionBeneficiarioInclusion(int rmtCont, string codTerc, string codBene)
        {
            try
            {
                var resultado = _repositorioContratos.ValidacionBeneficiarioInclusion(rmtCont, codTerc, codBene);
                return _adaptadorDeObjetos.Adaptar<IEnumerable<SPCCBENEFINCLUSIONTUPORTALDto>>(resultado);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        #endregion IServicioAplicacionNovedades Members
    }
}