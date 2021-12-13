using Dominio.Administracion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Models
{
    public class EnvioCertificadoTributarioModel
    {
        public string Año { get; set; }
        public IEnumerable<SelectListItem> Años => GetAnnios();
        public string TipoContrato { get; set; }
        public IEnumerable<SelectListItem> Tipos => new List<SelectListItem>()
        {
            new SelectListItem { Text = "PPE",Value="PPE" },
            new SelectListItem { Text = "Familiar",Value="FAM" },
            new SelectListItem { Text = "Todos",Value="Todos" }
        };
        public DateTime FechaMaximaEnvio { get; set; }
        public string TipoEnvio { get; set; }
        public IEnumerable<SelectListItem> TiposEnvio => new List<SelectListItem>()
        {
            new SelectListItem { Text = "RMT-CONT de clientes recurrentes",Value="RMT" },
            new SelectListItem { Text = "Todos",Value="Todos" }
        };
        public List<RmtContSolicitudCertificado> RmtContRecurrentes { get; set; }

        public List<SelectListItem> GetAnnios()
        {
            var listaAnnios = new List<SelectListItem>();
            var añoActual = DateTime.Today.Year;
            for (int i = 0; i < 100; i++)
            {
                añoActual--;
                listaAnnios.Add(new SelectListItem { Text = añoActual.ToString(), Value = añoActual.ToString() });
            }
            return listaAnnios;
        }
    }
}