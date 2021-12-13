using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Reflection;
using System.Runtime.Serialization.Json;

namespace NormalizacionDirecciones
{
    public class Methods
    {
        public static byte[] Create<T>(T value)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, value);
                return stream.ToArray();
            }
        }

        public EMB_DIRECCIONES ObtenerDireccionWS(string strDireccion, string stringCiudad, string strRMT_CON, string strRMT_CCB, string strAppid)
        {
            string strIdAplicacion = strAppid;

            try
            {
                var direccionDesdeBd = ObtenerDireccionDesdeBD(strRMT_CON, strRMT_CCB, strDireccion, stringCiudad, strAppid);

                if (direccionDesdeBd != null)
                {
                    return direccionDesdeBd;
                }

                string strToken = ConfigurationManager.AppSettings["TokenNormalizacion"].ToString();
                string strUrl = ConfigurationManager.AppSettings["URLNormalizacion"].ToString();

                Dictionary<string, string> objS = new Dictionary<string, string>();
                objS.Add("address", strDireccion);
                objS.Add("city", stringCiudad);
                string sb = JsonConvert.SerializeObject(objS);
                byte[] data = Encoding.UTF8.GetBytes(sb);

                HttpWebRequest serviceRequest = WebRequest.Create(strUrl) as HttpWebRequest;
                IWebProxy proxy = serviceRequest.Proxy;

                serviceRequest.Headers.Add("Authorization", strToken);
                //serviceRequest.Timeout = 10 * 1000;
                serviceRequest.Method = "POST";
                serviceRequest.ContentLength = data.Length;
                serviceRequest.ContentType = "application/json";
                serviceRequest.Host = "sitidata-stdr.appspot.com";

                Stream st = serviceRequest.GetRequestStream();
                st.Write(data, 0, data.Length);

                using (HttpWebResponse rpResult = serviceRequest.GetResponse() as HttpWebResponse)
                {
                    if (rpResult.StatusCode != HttpStatusCode.OK) throw new Exception(String.Format(
                        "Error en el servidor: (HTTP {0}: {1}).", rpResult.StatusCode,
                    rpResult.StatusDescription));

                    StreamReader srResult = new StreamReader(rpResult.GetResponseStream());
                    string strWebResult = srResult.ReadToEnd();

                    Entities.objJsonResponse.Container objJson = JsonConvert.DeserializeObject<Entities.objJsonResponse.Container>(strWebResult);

                    EMB_DIRECCIONES objDireccion = new EMB_DIRECCIONES()
                    {
                        RMTCON = strRMT_CON,
                        RMTRCCB = strRMT_CCB,
                        nivsocio = objJson.Data.Nivsocio,
                        dirAlterna = objJson.Data.DirAlterna,
                        zonapostal = objJson.Data.Zonapostal,
                        coddane = objJson.Data.Coddane,
                        coddireccion = objJson.Data.Coddireccion,
                        fuente = objJson.Data.Fuente,
                        validacionPlaca = objJson.Data.ValidacionPlaca,
                        zona = objJson.Data.zona,
                        longitude = objJson.Data.Longitude,
                        barrioTraducido = objJson.Data.BarrioTraducido,
                        latitude = objJson.Data.Latitude,
                        dirtrad = objJson.Data.Dirtrad,
                        coddirplaca = objJson.Data.Coddirplaca,
                        localidad = objJson.Data.Localidad,
                        validacion = objJson.Data.Validacion,
                        estado = objJson.Data.Estado,
                        barrio = objJson.Data.Barrio,
                        esambigua = objJson.Data.Esambigua,
                        dirIngresada = strDireccion,
                        ciudadIngresada = stringCiudad.Trim(),
                        fechaConsulta = DateTime.Now
                    };

                    AgregarDireccionBD(objDireccion, strAppid);
                    EscribeLogError(Entities.objLogCodes.iWebService.ToString(), Entities.objLogMessage.LogWS_OK, strIdAplicacion, Environment.UserName.ToString());

                    return objDireccion;
                }
            }
            catch (Exception ex)
            {
                EscribeLogError(Entities.objLogCodes.iGeneral.ToString(), Entities.objLogMessage.LogWS_Fail + ex.Message, strIdAplicacion, Environment.UserName.ToString());
                return null;
            }
        }

        private EMB_DIRECCIONES ObtenerDireccionDesdeBD(string iContrato, string iBeneficiario, string strDireccion, string strCiudad, string strAppid)
        {
            try
            {
                EMB_DIRECCIONES resp = null;
                using (DireccionEntities context = new DireccionEntities())
                {
                    resp = (from EMB_DIRECCIONES in context.EMB_DIRECCIONES
                            where (EMB_DIRECCIONES.dirtrad == strDireccion || EMB_DIRECCIONES.dirIngresada == strDireccion)
                                   && EMB_DIRECCIONES.ciudadIngresada == strCiudad.Trim()
                                   && EMB_DIRECCIONES.RMTCON == iContrato && EMB_DIRECCIONES.RMTRCCB == iBeneficiario
                            select EMB_DIRECCIONES).FirstOrDefault();

                    if (resp != null)
                    {
                        EscribeLogError(Entities.objLogCodes.iDataBase.ToString(), Entities.objLogMessage.LogOB_OK, strAppid, Environment.UserName.ToString());
                    }

                };
                return resp;
            }
            catch (Exception ex)
            {
                EscribeLogError(Entities.objLogCodes.iDataBase.ToString(), Entities.objLogMessage.LogDB_Fail + ex.Message, strAppid, Environment.UserName.ToString());
                return null;
            }
        }

        private bool AgregarDireccionBD(EMB_DIRECCIONES objDatos, string strIdapp)
        {
            try
            {
                using (DireccionEntities context = new DireccionEntities())
                {
                    context.EMB_DIRECCIONES.Add(objDatos);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                EscribeLogError(Entities.objLogCodes.iDataBase.ToString(), Entities.objLogMessage.LogDB_Fail + ex.Message, strIdapp, Environment.UserName.ToString());
                return false;
            }
        }

        private bool EscribeLogError(string strCodError, string strDescripcion, string strIdAplicacion, string strUsuario)
        {
            try
            {
                var dbContext = new LogEntities();

                EMB_NORMALIZA_DIRECCIONES_LOG objLog = new EMB_NORMALIZA_DIRECCIONES_LOG()
                {
                    idAplicacion = strIdAplicacion,
                    idErrorLog = strCodError,
                    descripcionError = strDescripcion,
                    fechaHora = System.DateTime.Now,
                    usuarioLog = strUsuario
                };

                dbContext.EMB_NORMALIZA_DIRECCIONES_LOG.Add(objLog);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string HomologarBarrio(string strCodBarrio, string strCiudad)
        {
            try
            {
                string strResultado = "";

                using (DireccionEntities context = new DireccionEntities())
                {
                    var direccionQuery = from EMB_HOMOLOGACION_BARRIO in context.EMB_HOMOLOGACION_BARRIO
                                         where EMB_HOMOLOGACION_BARRIO.barrioServicio == strCodBarrio && EMB_HOMOLOGACION_BARRIO.ciudad == strCiudad
                                         select EMB_HOMOLOGACION_BARRIO;

                    foreach (var result in direccionQuery)
                    {
                        strResultado = result.barrioEmermedica.ToString();
                    }

                    if (strResultado == "")
                    {
                        return "";
                    }
                };
                return strResultado;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

    }
}
