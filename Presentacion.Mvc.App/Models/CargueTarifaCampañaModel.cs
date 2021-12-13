using Dominio.Administracion.Entidades;
using PagedList;

namespace Presentacion.Mvc.App.Models
{
    public class CargueTarifaCampañaModel
    {
        #region Instance Properties

        public IPagedList<CTB_TARIFAS_CAMPANA> ListaCargueTarifasCampaña { get; set; }

        #endregion Instance Properties
    }
}