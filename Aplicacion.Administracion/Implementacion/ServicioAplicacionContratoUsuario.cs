using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionContratoUsuario : IServicioAplicacionContratoUsuario
    {
        #region Constants

        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";

        #endregion Constants

        #region Readonly & Static Fields

        private readonly IRepositorioContratosUsuarios _repositorioContratosUsuarios;
        private readonly IRepositorioLogs _manejadorLogs;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionContratoUsuario(IRepositorioLogs manejadorLogs, IRepositorioContratosUsuarios repositorioContratosUsuarios)
        {
            _manejadorLogs = manejadorLogs;
            _repositorioContratosUsuarios = repositorioContratosUsuarios;
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

        #region IServicioAplicacionUsuarios Members

        public IEnumerable<EmbUsuarioContrato> ObtenerContratos(string codigoUsuario)
        {
            var entidadUsuario = new List<EmbUsuarioContrato>();
            try
            {
                var result = _repositorioContratosUsuarios.ObtenerTodosContratosDeUsuario(codigoUsuario);
                entidadUsuario = result.ToList();
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return entidadUsuario;
        }

        public IEnumerable<EmbUsuarioContrato> ObtenerContratosContratante(string codigoUsuario, string nit, int tipo)
        {
            try
            {
                var entidades = _repositorioContratosUsuarios.ObtenerTodosContratosDeContratante(codigoUsuario, nit, tipo);
                var contratos = entidades as IList<EmbUsuarioContrato> ?? entidades.ToList();
                //if (!contratos.Any())
                //{
                //    throw new Exception("Apreciado afiliado, el estado actual de tu contrato no permite realizar consultas y transacciones. Por favor comunícate con nuestras líneas de atención al afiliado: Bogotá, Chía y Soacha: (1) 3077089 Línea Nacional: 018000 117098 E-mail: servicioalcliente@emermedica.com.co");
                //}
                return contratos;
            }
            catch (SystemException excepcion)
            {
                throw LogExepxion(excepcion, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<Beneficiario> ObtenerBeneficiariosCartaViajero(string rmt_cont, string cod_terc)
        {
            try
            {
                var entidades = _repositorioContratosUsuarios.ObtenerBeneficiariosCartaViajero(rmt_cont, cod_terc);
                return entidades;
            }
            catch (SystemException excepcion)
            {
                throw LogExepxion(excepcion, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<EmbUsuarioContrato> ObtenerContratosBeneficiario(string codigoUsuario)
        {
            try
            {
                var entidades = _repositorioContratosUsuarios.ObtenerTodosContratosDeBeneficiario(codigoUsuario);
                return entidades;
            }
            catch (SystemException excepcion)
            {
                throw LogExepxion(excepcion, ErrorProcesandoPeticion);
            }
        }

        public bool TieneFacturacionAnioAnterior(string rmtCont)
        {
            try
            {
                var entidades = _repositorioContratosUsuarios.TieneFacturacionAnioAnterior(rmtCont);
                return entidades;
            }
            catch (SystemException excepcion)
            {
                throw LogExepxion(excepcion, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ContratosFacturas> ObtenerContratosFacturasUsuarios(string NumeroDocumento, int? contratoId)
        {
            try
            {
                var entidades = _repositorioContratosUsuarios.ObtenerContratosFacturasCodigoUsuario(NumeroDocumento, contratoId);
                return entidades;
            }
            catch (SystemException excepcion)
            {
                throw LogExepxion(excepcion, ErrorProcesandoPeticion);
            }
        }

        public bool SolicitarEnvioCertificadoTributario(int año, string tipoContrato, DateTime fechaMaximaEnvio, string tipoEnvio, List<RmtContSolicitudCertificado> rmtContRecurrentes)
        {
            try
            {
                _repositorioContratosUsuarios.SolicitarEnvioCertificadoTributario(año,
                   tipoContrato, fechaMaximaEnvio, tipoEnvio, rmtContRecurrentes);
                return true;
            }
            catch (SystemException excepcion)
            {
                LogExepxion(excepcion, ErrorProcesandoPeticion);
                return false;
            }
        }
        public IEnumerable<ReporteRegistroEnvio> ConsultaReporteEnvios()
        {
            try
            {
                var entidades = _repositorioContratosUsuarios.ConsultarEnvioReportes();
                return entidades;
            }
            catch (SystemException excepcion)
            {
                throw LogExepxion(excepcion, ErrorProcesandoPeticion);
            }
        }

        #endregion IServicioAplicacionUsuarios Members
    }
}