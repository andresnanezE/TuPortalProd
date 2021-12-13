using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioContratosUsuarios : IRepositorioContratosUsuarios
    {

        public IEnumerable<EmbUsuarioContrato> ObtenerTodosContratosDeUsuario(string codigoUsuario)
        {
            try
            {
                var contexto = new ContextoTuEmermedica();
                var entidades = contexto.Database.SqlQuery<EmbUsuarioContrato>("SCISP_CONSULTA_CONTRATOS_USUARIOS @COD_USUARIO", new SqlParameter("@COD_USUARIO", codigoUsuario)).ToList();
                return entidades;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.InnerException.ToString());
            }
        }

        public IEnumerable<EmbUsuarioContrato> ObtenerTodosContratosDeContratante(string codigoUsuario, string nit, int tipo)
        {
            if (nit == "")
            {
                var contexto = new ContextoTuEmermedica();
                var entidades = contexto.Database.SqlQuery<EmbUsuarioContrato>("SCISP_CONSULTA_CONTRATOS_CONTRATANTE @COD_USUARIO, @NIT, @TIPO", new SqlParameter("@COD_USUARIO", codigoUsuario), new SqlParameter("@NIT", nit), new SqlParameter("@TIPO", tipo)).ToList();
                return entidades;
            }
            else
            {
                var contexto = new ContextoTuEmermedica();
                var entidades = contexto.Database.SqlQuery<EmbUsuarioContrato>("SCISP_CONSULTA_CONTRATOS_CONTRATANTE @COD_USUARIO, @NIT, @TIPO", new SqlParameter("@COD_USUARIO", codigoUsuario), new SqlParameter("@NIT", nit), new SqlParameter("@TIPO", tipo)).ToList();
                return entidades;
            }
        }

        public IEnumerable<Beneficiario> ObtenerBeneficiariosCartaViajero(string contratoId, string cod_terc)
        {
            var contexto = new ContextoTuEmermedica();
            var entidades = contexto.Database.SqlQuery<Beneficiario>("SCISP_CONSULTA_BENEFICIARIOS_CARTA_VIAJERO @RMT_CONT, @CONTRATANTE", new SqlParameter("@RMT_CONT", contratoId), new SqlParameter("@CONTRATANTE", cod_terc)).ToList();
            return entidades;
        }

        public IEnumerable<EmbUsuarioContrato> ObtenerTodosContratosDeBeneficiario(string codigoUsuario)
        {
            var contexto = new ContextoTuEmermedica();
            var entidades = contexto.Database.SqlQuery<EmbUsuarioContrato>("SCISP_CONSULTA_CONTRATO_BENEFICIARIOS @NUM_DOC", new SqlParameter("@NUM_DOC", codigoUsuario)).ToList();
            return entidades;
        }

        public bool TieneFacturacionAnioAnterior(string rmtCont)
        {
            var contexto = new ContextoTuEmermedica();
            var resultado = contexto.Database.SqlQuery<bool>("SCISP_CONSULTA_FACTURACION_ANIO_ANTERIOR @RMT_CONT", new SqlParameter("@RMT_CONT", rmtCont)).FirstOrDefault();
            return resultado;
        }

        public IEnumerable<ContratosFacturas> ObtenerContratosFacturasCodigoUsuario(string NumeroDocumento, int? contratoId)
        {
            var contexto = new ContextoTuEmermedica();
            var entidades = contexto.Database.SqlQuery<ContratosFacturas>("EMSP_CONSULTA_CONTRATOS_USUARIOS_FACTURAS_V2 @NumeroDocumento",
                new SqlParameter("@NumeroDocumento", NumeroDocumento)).ToList();
            if (entidades != null)
            {
                if (contratoId != null)
                    entidades = entidades.Where(e => e.RMT_CONT == contratoId).ToList();
            }
            return entidades;
        }


        public void SolicitarEnvioCertificadoTributario(int año, string tipoContrato,
            DateTime fechaMaximaEnvio, string tipoEnvio,
            List<RmtContSolicitudCertificado> rmtContRecurrentes)
        {
            var contexto = new ContextoPortal();
            var sqlQuery = "SP_INSERTAR_PROCESO_ENVIO " +
                "@AÑO," +
                "@TIPO_CONTRATO," +
                "@TODOS," +
                "@FECHA_MAXIMA_ENVIO," +
                "@MENSAJE";
            var parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("@AÑO", año);
            switch (tipoContrato)
            {
                case "PPE":
                    parametros[1] = new SqlParameter("@TIPO_CONTRATO", 1);
                    break;
                case "FAM":
                    parametros[1] = new SqlParameter("@TIPO_CONTRATO", 2);
                    break;
                case "Todos":
                    parametros[1] = new SqlParameter("@TIPO_CONTRATO", Convert.ToInt32(0));
                    break;
            }
            parametros[2] = new SqlParameter("@TODOS", tipoEnvio != "RMT");
            parametros[3] = new SqlParameter("@FECHA_MAXIMA_ENVIO", fechaMaximaEnvio);
            parametros[4] = new SqlParameter("@MENSAJE", "La solicitud de envio de certificado ha sido registrada");

            var idProceso = contexto.Database.SqlQuery<int>(sqlQuery, parametros).FirstOrDefault();

            var listaRmtCont = new List<RmtContSolicitudCertificado>();
            if (tipoEnvio == "RMT")
            {
                listaRmtCont = rmtContRecurrentes;

                listaRmtCont.Select(c => { c.ProcesoId = idProceso; return c; }).ToList();
                var sqlQueryEnvio = "SP_INSERTAR_ENVIO_CERTIFICADOS_TRIBUTARIOS " +
                            "@ENVIOS";
                var tabla = new DataTable();
                tabla.Columns.Add("ProcesoId");
                tabla.Columns.Add("NumeroDocumento");
                tabla.Columns.Add("RmtCont");
                tabla.Columns.Add("Email");
                foreach (var rmt in listaRmtCont)
                {
                    tabla.Rows.Add(idProceso, rmt.NumeroDocumento, rmt.RmtCont,DBNull.Value);
                }
                var parametrosEnvio = new SqlParameter[1];
                var tableParam = new SqlParameter("@ENVIOS", tabla);
                tableParam.SqlDbType = SqlDbType.Structured;
                tableParam.TypeName = "dbo.Param_EnvioCertificado";
                parametrosEnvio[0] = tableParam;

                var entidad = contexto.Database.ExecuteSqlCommand(sqlQueryEnvio, parametrosEnvio);
            }
        }

        public IEnumerable<ReporteRegistroEnvio> ConsultarEnvioReportes()
        {
            var contexto = new ContextoPortal();
            var sqlQuery = "SP_CONSULTAR_REPORTE_ENVIOS";
            var result = contexto.Database.SqlQuery<ReporteRegistroEnvio>(sqlQuery).ToList();
            return result;
        }

    }
}