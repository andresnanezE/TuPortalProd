using Dominio.Administracion.Entidades;
using PagedList;

namespace Presentacion.Mvc.App.Models
{
    public class CargueTarifaPlenaModel
    {
        #region Instance Properties

        public IPagedList<CTB_TARIFAS_PLENAS> ListaCargueTarifasPlenas { get; set; }

        #endregion Instance Properties
    }
}