using Dominio.Administracion.Entidades;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentacion.Mvc.App.Models
{
    public class CargueTarifaCampañaModel
    {
        #region Instance Properties
              
        public IPagedList<CTB_TARIFAS_CAMPANA> ListaCargueTarifasCampaña { get; set; }

        #endregion
    }
}