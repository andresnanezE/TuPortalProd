using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace Presentacion.Mvc.App.Helpers
{
    public class TokenHelper
    {
        public Token SolicitarTokenAplicacion(string aplicacion, string clave)
        {
            string baseAddress = "http://localhost:51881";
            var token = new Token();
            var res = "";
            using (var clientToken = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                   {"grant_type", "password"},
                   {"username", "admin"},
                   {"password", "123"},
                   {"Aplicacion","TuPortal"}
                };
                var tokenResponse = clientToken.PostAsync(baseAddress + "/token", new FormUrlEncodedContent(form)).Result;
                token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                if (string.IsNullOrEmpty(token.Error))
                {
                    res = String.Concat("Token issued is: {0}", token.AccessToken);
                }
                else
                {
                    res = String.Concat("Error : {0}", token.Error);
                }
            }

            return new Token();
        }
    }
}