using Dominio.Administracion.Entidades;
using PagedList;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Mvc.App.Models
{
    public class CaracterizacionCampañasModel
    {
        #region Instance Properties

        [DisplayName("Tipo tarifa")]
        public string @TIPO_TARIFA { get; set; }

        [DisplayName("Campaña")]
        public string @CAMPANA_TARIFA { get; set; }

        public string @RUTA_IMAGEN { get; set; }

        [Required(ErrorMessage = "Por favor ingrese la caracterización")]
        public string @CARACTERIZACION { get; set; }

        public IEnumerable<TIPO_TARIFAtb> TipoTarifas { get; set; }
        public IEnumerable<CAMPAÑA> Campañas { get; set; }
        public IPagedList<CTB_CAMPANA_CARACTERIZACION> ListaCampanaCaracterizacion { get; set; }
        public string Mensaje { get; set; }

        public CaracterizacionCampañasModel()
        {
            TipoTarifas = new List<TIPO_TARIFAtb>();
            Campañas = new List<CAMPAÑA>();
            TIPO_TARIFA = string.Empty;
            CAMPANA_TARIFA = string.Empty;
        }

        #endregion Instance Properties
    }
}