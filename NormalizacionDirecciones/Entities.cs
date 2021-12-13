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

namespace NormalizacionDirecciones
{
    public class Entities
    {
        public class objJsonResponse
        {
            public partial class Container
            {
                [JsonProperty("message")]
                public string Message { get; set; }

                [JsonProperty("data")]
                public Data Data { get; set; }

                [JsonProperty("success")]
                public bool Success { get; set; }
            }

            public partial class Data
            {
                [JsonProperty("esambigua")]
                public string Esambigua { get; set; }

                [JsonProperty("coddireccion")]
                public string Coddireccion { get; set; }

                [JsonProperty("barrioTraducido")]
                public string BarrioTraducido { get; set; }

                [JsonProperty("barrio")]
                public string Barrio { get; set; }

                [JsonProperty("coddane")]
                public string Coddane { get; set; }

                [JsonProperty("dirAlterna")]
                public string DirAlterna { get; set; }

                [JsonProperty("coddirplaca")]
                public string Coddirplaca { get; set; }

                [JsonProperty("dirtrad")]
                public string Dirtrad { get; set; }

                [JsonProperty("localidad")]
                public string Localidad { get; set; }

                [JsonProperty("fuente")]
                public string Fuente { get; set; }

                [JsonProperty("estado")]
                public string Estado { get; set; }

                [JsonProperty("latitude")]
                public string Latitude { get; set; }

                [JsonProperty("nivsocio")]
                public string Nivsocio { get; set; }

                [JsonProperty("validacionPlaca")]
                public string ValidacionPlaca { get; set; }

                [JsonProperty("zona")]
                public string zona { get; set; }

                [JsonProperty("longitude")]
                public string Longitude { get; set; }

                [JsonProperty("validacion")]
                public string Validacion { get; set; }

                [JsonProperty("zonapostal")]
                public string Zonapostal { get; set; }
            }
        }

        public class objLog
        {
            public string idAplicaicon { get; set; }
            public string CordError { get; set; }
            public string Mensaje { get; set; }
            public DateTime Fecha { get; set; }
            public string Usuario { get; set; }
        }

        public class objLogMessage
        {
            public static string LogOB_OK = "Direccion obtenida de base de datos";
            public static string LogWS_OK = "Direccion obtenida de servicio web";
            public static string LogDB_Saved = "Direccion almacenada en la base de datos";
            public static string LogDB_Fail = "Error al almacenar direccion en la base de datos: ";
            public static string LogWS_Fail = "Error al obtener la direccion del servicio web: ";
        }

        public class objLogCodes
        {
            public static int iDataBase = 1;
            public static int iWebService = 2;
            public static int iGeneral = 0;
        }
    }
}
