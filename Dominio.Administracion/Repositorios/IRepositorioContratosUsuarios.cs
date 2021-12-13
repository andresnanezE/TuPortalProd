using Dominio.Administracion.Entidades;
using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioContratosUsuarios
    {
        IEnumerable<EmbUsuarioContrato> ObtenerTodosContratosDeUsuario(string codigoUsuario);

        IEnumerable<EmbUsuarioContrato> ObtenerTodosContratosDeContratante(string codigoUsuario, string nit, int tipo);

        IEnumerable<Beneficiario> ObtenerBeneficiariosCartaViajero(string contratoId, string cod_terc);

        IEnumerable<EmbUsuarioContrato> ObtenerTodosContratosDeBeneficiario(string codigoUsuario);

        bool TieneFacturacionAnioAnterior(string rmtCont);

        IEnumerable<ContratosFacturas> ObtenerContratosFacturasCodigoUsuario(string NumeroDocumento, int? contratoId);

        void SolicitarEnvioCertificadoTributario(int año, string tipoContrato, DateTime fechaMaximaEnvio, string tipoEnvio, List<RmtContSolicitudCertificado> rmtContRecurrentes);

        IEnumerable<ReporteRegistroEnvio> ConsultarEnvioReportes();
    }
}