using Dominio.Administracion.Entidades;
using System;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionContratoUsuario
    {
        IEnumerable<EmbUsuarioContrato> ObtenerContratos(string codigoUsuario);

        IEnumerable<EmbUsuarioContrato> ObtenerContratosContratante(string codigoUsuario, string nit, int tipo);

        IEnumerable<Beneficiario> ObtenerBeneficiariosCartaViajero(string rmt_cont, string cod_terc);

        IEnumerable<EmbUsuarioContrato> ObtenerContratosBeneficiario(string codigoUsuario);

        bool TieneFacturacionAnioAnterior(string rmtCont);

        IEnumerable<ContratosFacturas> ObtenerContratosFacturasUsuarios(string NumeroDocumento, int? contratoId);

        bool SolicitarEnvioCertificadoTributario(int año, string tipoContrato, DateTime fechaMaximaEnvio, string tipoEnvio, List<RmtContSolicitudCertificado> rmtContRecurrentes);
        IEnumerable<ReporteRegistroEnvio> ConsultaReporteEnvios();
    }
}