using ApiCertificadoTributario.Data;
using ApiCertificadoTributario.Models;
using ApiCertificadoTributario.Services;
using MvcReportViewer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using EmailSendinblue.Dto;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Mail;
using Attachment = EmailSendinblue.Dto.Attachment;

namespace ApiCertificadoTributario.Controllers
{
    public class CertificadoController : Controller
    {
        public IServiceCertificado Servicio => new ServiceCertificado();

        public async Task<ActionResult> EjecucionProceso()
        {
            var resultado = await Servicio.ObtenerCertificadosPendientes();
            if (resultado.Success)
            {
                if (resultado.Result != null)
                {
                    var lista = (List<EnvioCertificadosTributarios>)resultado.Result;

                    foreach (var envioPendiente in lista)
                    {
                        var resultadoGeneracion = GenerarCertificado(envioPendiente);
                        if (resultadoGeneracion.Success)
                        {
                            var resultadoEnvio = await EnviarCertificado
                            (resultadoGeneracion.Result.ToString(), envioPendiente.AñoCertificado,
                            envioPendiente.Email, envioPendiente.RmtCont);
                            if (!resultadoEnvio.Success)
                            {
                                envioPendiente.EstadoEnvio = false;
                                envioPendiente.EstadoError = true;
                                envioPendiente.Mensaje = $"Ocurrió un error al intentar enviar el certificado {resultadoEnvio.Message}";
                            }
                            else
                            {
                                envioPendiente.FechaEnvio = DateTime.Now;
                                envioPendiente.EstadoEnvio = true;
                                envioPendiente.EstadoError = false;
                                envioPendiente.Mensaje = "El certificado fue enviado correctamente";
                            }
                        }
                        else
                        {
                            envioPendiente.EstadoEnvio = false;
                            envioPendiente.EstadoError = true;
                            envioPendiente.Mensaje = "Ocurrió un error al intentar generar el certificado";
                        }
                        await Servicio.ActualizarEnvio(envioPendiente);
                    }
                    var procesos = lista.Distinct().Select(p => p.ProcesoId).ToList();
                    foreach (var prc in procesos)
                    {
                        await Servicio.ActualizarProceso(prc, true);
                    }
                }
                return Json( 
                    new ResultadoConsulta
                    {
                        Success = true,
                        Message = "El proceso se ha ejecutado con éxito"
                    }, JsonRequestBehavior.AllowGet);
            }

            return Json(
                    new ResultadoConsulta
                    {
                        Success = false,
                        Message = "Error al ejecutar proceso de generación y envio"
                    }, JsonRequestBehavior.AllowGet);            
        }
        private async Task<ResultadoConsulta> EnviarCertificado(string fileBase64, int año, string email, string rmtCont)
        {
            try
            {
                var EnvioCorreo = new Email
                {
                    sender = new Sender { email = "servcioalcliente@emermedica.com.co" },
                    subject = $"EMERMÉDICA S. A. - CERTIFICADO TRIBUTARIO AÑO {año}",
                    to = new List<To> { new To { email = email } },
                    templateId = Convert.ToInt32(ConfigurationManager.AppSettings["IdPlantillaSendinblue"]),
                    attachment = new List<Attachment>
                                          {
                                              new Attachment
                                              {
                                                  content = fileBase64,
                                                  name = rmtCont + ".pdf"
                                              }
                                          }
                };
                var result = await EmailSendinblue.Send.PostAsync(
                  ConfigurationManager.AppSettings["DirSendinblue"],
                  ConfigurationManager.AppSettings["SendinblueKey"],
                  EnvioCorreo);
                return new ResultadoConsulta
                {
                    Success = result.success,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                Servicio.RegLog($"Error al enviar certificado para el RMT_CONT {rmtCont} del año {año}", ex);
                return new ResultadoConsulta
                {
                    Success = false,
                    Message = "Error al enviar certificado: CertificadoController"
                };
            }
        }
        private ResultadoConsulta GenerarCertificado(EnvioCertificadosTributarios envioPendiente)
        {
            try
            {
                List<KeyValuePair<string, object>> parametros = new List<KeyValuePair<string, object>>();
                parametros.Add(new KeyValuePair<string, object>
                    ("NumIdentificacion", envioPendiente.NumeroDocumento.ToString()));
                parametros.Add(new KeyValuePair<string, object>
                    ("Contrato", envioPendiente.RmtCont.ToString()));
                parametros.Add(new KeyValuePair<string, object>
                    ("anio", envioPendiente.AñoCertificado.ToString()));
                parametros.Add(new KeyValuePair<string, object>
                    ("UltimosTresAnios", false));
                FileStreamResult reporte = this.Report(
                            ReportFormat.Pdf,
                            ConfigurationManager.AppSettings["UrlReporteCertificado"],
                            ConfigurationManager.AppSettings["UrlReportes"],
                            parametros,
                            ConfigurationManager.AppSettings["UsuarioReportes"],
                            ConfigurationManager.AppSettings["PasswordReportes"]
                            );

                int length = Convert.ToInt32(reporte.FileStream.Length);
                byte[] data = new byte[length];
                reporte.FileStream.Read(data, 0, length);
                reporte.FileStream.Close();

                var fileBase64 = Convert.ToBase64String(data);

                return new ResultadoConsulta
                {
                    Success = true,
                    Result = fileBase64
                };
            }
            catch (Exception ex)
            {
                Servicio.RegLog($"Error al generar certificado para el RMT_CONT {envioPendiente?.RmtCont} " +
                    $"del año {envioPendiente?.AñoCertificado}", ex);
                return new ResultadoConsulta
                {
                    Success = false,
                    Message = "Error al generar certificado: CertificadoController"
                };
            }
        }
    }
}