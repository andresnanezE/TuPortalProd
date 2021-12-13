using Dominio.Administracion.Entidades.MapperDto;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

//using Dominio.Administracion.Entidades.MapperDto;
using System.Text;

namespace Presentacion.Mvc.App.Utilidades
{
    public class WSUsuariosCentralizadosManager
    {
        private string Aplicacion { get; set; }
        private string UrlBase { get; set; }
        private string Clave { get; set; }
        private Token Token { get; set; }

        public WSUsuariosCentralizadosManager()
        {
            Aplicacion = ConfiguracionesGlobales.AplicacionWsUsuarios;
            Clave = ConfiguracionesGlobales.ClaveWsUsuarios;//123
            UrlBase = ConfiguracionesGlobales.URLWsUsuarios;
            Token = SolicitarTokenAplicacionAsync();
        }

        public Token SolicitarTokenAplicacionAsync()
        {
            using (var clientToken = new HttpClient())
            {
                try
                {
                    var form = new Dictionary<string, string>
                {
                   {"grant_type", "password"},
                   {"username", Aplicacion},
                   {"password", Clave}
                };

                    var tokenResponse = clientToken.PostAsync(UrlBase + "/token", new FormUrlEncodedContent(form)).Result;
                    Token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                }
                return Token;
            }
        }

        public HttpResponseMessage ValidarAcesoUsuario(string usuario, string clave, string usuario2 = "")
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                   {"grant_type", "password"},
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token.AccessToken);
                client.DefaultRequestHeaders.Add("aplicacion", Aplicacion);
                client.DefaultRequestHeaders.Add("usuario", usuario);
                client.DefaultRequestHeaders.Add("password", clave);

                client.DefaultRequestHeaders.Add("usuario2", usuario2);

                var responseMessage = client.PostAsync(UrlBase + "/api/Acceso/ValidarAcceso", new FormUrlEncodedContent(form)).Result;
                return responseMessage;
            }
        }

        public HttpResponseMessage RegistrarUsuario(RegistroModel usrNuev, string urlSitio)
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                   {"grant_type", "password"},
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token.AccessToken);
                client.DefaultRequestHeaders.Add("aplicacion", Aplicacion);
                client.DefaultRequestHeaders.Add("usuario", usrNuev.Usuario);//username
                client.DefaultRequestHeaders.Add("password", usrNuev.Clave);
                client.DefaultRequestHeaders.Add("correo", usrNuev.Correo);
                client.DefaultRequestHeaders.Add("tipoDoc", usrNuev.TipoDocumento);
                client.DefaultRequestHeaders.Add("documento", usrNuev.Documento.ToString());
                client.DefaultRequestHeaders.Add("NombreUsuario", usrNuev.NombreUsuario);
                client.DefaultRequestHeaders.Add("Rol", usrNuev.Rol.ToString());//el id del rol se pasa a string
                client.DefaultRequestHeaders.Add("urlSitio", urlSitio);

                var responseMessage = client.PostAsync(UrlBase + "/api/Acceso/RegistrarUsuario", new FormUrlEncodedContent(form)).Result;
                return responseMessage;
            }
        }

        public bool SolicitarRecuperacionDeClave(string docusuario, string urlSitio)
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                   {"grant_type", "password"},
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token.AccessToken);
                //client.DefaultRequestHeaders.Add("usuario", usuario);
                client.DefaultRequestHeaders.Add("usuario", docusuario);
                client.DefaultRequestHeaders.Add("urlSitio", urlSitio);
                client.DefaultRequestHeaders.Add("aplicacion", Aplicacion);

                var responseMessage = client.PostAsync(UrlBase + "/api/Acceso/RecuperarClave", new FormUrlEncodedContent(form)).Result;

                return responseMessage.StatusCode == HttpStatusCode.OK;
            }
        }

        public string ValidarSolicitudRestablecimiento(string valores)
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                   {"grant_type", "password"},
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token.AccessToken);
                client.DefaultRequestHeaders.Add("valores", valores);

                var responseMessage = client.PostAsync(UrlBase + "/api/Acceso/ValidarSolicitudRestablecimiento", new FormUrlEncodedContent(form)).Result;
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return responseMessage.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }

        public HttpResponseMessage CambiarClave(string numDocu, string clave, string urlSitio)
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                   {"grant_type", "password"},
                };

                if (numDocu.Contains("?"))
                {
                    string[] chars = new string[] { "?" };
                    for (int i = 0; i < chars.Length; i++)
                    {
                        if (numDocu.Contains(chars[i]))
                        {
                            numDocu = numDocu.Replace(chars[i], "ñ");
                        }
                    }
                }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token.AccessToken);
                client.DefaultRequestHeaders.Add("password", clave);
                client.DefaultRequestHeaders.Add("usuario", numDocu);
                client.DefaultRequestHeaders.Add("urlSitio", urlSitio);
                client.DefaultRequestHeaders.Add("aplicacion", Aplicacion);

                var responseMessage = client.PostAsync(UrlBase + "/api/Acceso/ActualizarClave", new FormUrlEncodedContent(form)).Result;
                return responseMessage;
            }
        }

        //public HttpResponseMessage ClientGetRequest(string urlBase, List<int> requestUri)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            var form = new Dictionary<string, string>
        //            {
        //                  {"grant_type", "password"},
        //            };
        //            var responseMessage = new HttpResponseMessage();
        //            string parametros = null;
        //            int cont = 1;
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            //client.DefaultRequestHeaders.Add("roles", requestUri);
        //            //string parametros=string.Join(string.Format("roles={0}&", requestUri.Select(x => x.ToString()).ToArray()));
        //            for (int i = 0; i < requestUri.Count; i++)
        //            {
        //                //parametros = parametros+string.Format("roles={0}&"+requestUri.ElementAt(i));
        //                if (requestUri.Count>1)
        //                {
        //                    if (cont < requestUri.Count)
        //                    {
        //                        parametros = parametros + "roles=" + requestUri.ElementAt(i) + "&";
        //                        cont++;
        //                    }else if(cont== requestUri.Count)
        //                    {
        //                        parametros = parametros + "roles=" + requestUri.ElementAt(i);
        //                    }
        //                }
        //                else if(requestUri.Count == 1)
        //                {
        //                    parametros = parametros + "roles=" + requestUri.ElementAt(i);
        //                }
        //            }
        //            responseMessage = client.GetAsync(urlBase + "/api/BaseSiteNodeApi/ReturnAll?" + parametros).Result;

        //            IEnumerable<MenuAplicacionDto> rootObj = null;
        //            try
        //            {
        //                dynamic content = JsonConvert.DeserializeObject(
        //               responseMessage.Content.ReadAsStringAsync()
        //               .Result);
        //                rootObj = JsonConvert.DeserializeObject<IEnumerable<MenuAplicacionDto>>(content);
        //            }
        //            catch (System.Exception e1)
        //            {
        //                String a = responseMessage.Content.ReadAsStringAsync().Result;
        //                var content2 = JsonConvert.DeserializeObject(responseMessage.Content.ReadAsStringAsync().Result);
        //                rootObj = JsonConvert.DeserializeObject<IEnumerable<MenuAplicacionDto>>(content2.ToString());

        //            }

        //            return responseMessage;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.NotFound);
        //    }
        //}

        public bool ActualizarUsuario(UsuarioDto usr)
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                   {"grant_type", "password"},
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token.AccessToken);
                client.DefaultRequestHeaders.Add("aplicacion", Aplicacion);
                client.DefaultRequestHeaders.Add("usuario", usr.logIn);
                client.DefaultRequestHeaders.Add("correo", usr.correo);
                client.DefaultRequestHeaders.Add("nombreUsuario", usr.Nombre);
                client.DefaultRequestHeaders.Add("activo", usr.activo.ToString());
                client.DefaultRequestHeaders.Add("idUsr", usr.Id.ToString());

                StringBuilder strBRoles = new StringBuilder();
                foreach (var item in usr.Roles)
                {
                    strBRoles.AppendFormat("{0}|", item);
                }

                client.DefaultRequestHeaders.Add("Rol", strBRoles.ToString());

                var responseMessage = client.PostAsync(UrlBase + "/api/Acceso/ActualizrUsuario", new FormUrlEncodedContent(form)).Result;
                return responseMessage.StatusCode == HttpStatusCode.OK;
            }
        }
    }
}