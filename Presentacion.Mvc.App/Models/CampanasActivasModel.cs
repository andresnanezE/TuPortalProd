using Dominio.Administracion.Entidades.ModeloCampaign;
using System.Collections.Generic;

namespace Presentacion.Mvc.App.Models
{
    public class CampanasActivasModel
    {
        public IEnumerable<Campaign> campaigns { get; set; }
        public int? campaignID { get; set; }
    }
}