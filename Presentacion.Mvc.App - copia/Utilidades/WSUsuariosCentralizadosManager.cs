using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;
using Aplicacion.Administracion.Dto;

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
                var form = new Dictionary<string, string>
                {
                   {"grant_type", "password"},
                   {"username", Aplicacion},
                   {"password", Clave}
                };

                var tokenResponse = clientToken.PostAsync(UrlBase + "/token", new FormUrlEncodedContent(form)).Result;
                Token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                return Token;
            }
        }
        public HttpResponseMessage ValidarAcesoUsuario(string usuario, string clave)
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

        public bool SolicitarRecuperacionDeClave(string usuario, string urlSitio)
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                   {"grant_type", "password"},
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token.AccessToken);
                client.DefaultRequestHeaders.Add("usuario", usuario);
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

        public bool CambiarClave(string usuario, string clave, string urlSitio)
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                   {"grant_type", "password"},
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token.AccessToken);
                client.DefaultRequestHeaders.Add("password", clave);
                client.DefaultRequestHeaders.Add("usuario", usuario);
                client.DefaultRequestHeaders.Add("urlSitio", urlSitio);
                client.DefaultRequestHeaders.Add("aplicacion", Aplicacion);

                var responseMessage = client.PostAsync(UrlBase + "/api/Acceso/ActualizarClave", new FormUrlEncodedContent(form)).Result;
                return responseMessage.StatusCode == HttpStatusCode.OK;
            }
        }

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
                client.DefaultRequestHeaders.Add("usuario", usr.logIn);
                client.DefaultRequestHeaders.Add("correo", usr.correo);
                client.DefaultRequestHeaders.Add("nombreUsuario", usr.Nombre);
                client.DefaultRequestHeaders.Add("activo", usr.activo.ToString());

                var responseMessage = client.PostAsync(UrlBase + "/api/Acceso/ActualizarClave", new FormUrlEncodedContent(form)).Result;
                return responseMessage.StatusCode == HttpStatusCode.OK;
            }
        }
    }
}