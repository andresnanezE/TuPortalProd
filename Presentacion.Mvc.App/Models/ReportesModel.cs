using Dominio.Administracion.Entidades;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Models
{
    public class ReportesModel
    {
        public string ContratoId { get; set; }
        //public IEnumerable<SelectListItem> Contrato { get; set; }

        //[Required(ErrorMessage = "Debe ingresar un contrato.")]
        public IEnumerable<SelectListItem> Contratos { get; set; }

        public IEnumerable<SelectListItem> AniosList { get; set; }

        public string PeriodoFin { get; set; }

        public string PeriodoInicio { get; set; }

        public IEnumerable<SelectListItem> Periodos { get; set; }

        public string NumeroDoc { get; set; }

        public string Canal { get; set; }

        public int Anio { get; set; }

        public int Mes { get; set; }

        public string Fecha { get; set; }

        public string nom_terc { get; set; }

        public int cod_terc { get; set; }

        public string cod_bene { get; set; }

        public IEnumerable<string> BeneficiarioSeleccionados { get; set; }

        public IEnumerable<SelectListItem> BeneficiariosList { get; set; }

        public IEnumerable<Beneficiario> Beneficiarios { get; set; }

        public int cod_mpio { get; set; }

        public IEnumerable<SelectListItem> Ciudades { get; set; }

        public int cod_dpto { get; set; }

        public IEnumerable<SelectListItem> Departamentos { get; set; }

        public int cod_pais { get; set; }

        public IEnumerable<SelectListItem> Paises { get; set; }
    }

    public class beneficiario
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string estado { get; set; }
    }
}