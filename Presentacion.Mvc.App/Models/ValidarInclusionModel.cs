using Dominio.Administracion.Entidades;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Models
{
    public class ValidarInclusionModel
    {
        public int RmtCont { get; set; }

        public List<SelectListItem> TipoConsulta { get; set; }
        public string TipoConsultaValor { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "ingrese un valor numérico")]
        public string CodTerc { get; set; }

        [RegularExpression("^[0-9,]*$", ErrorMessage = "ingrese un valor numérico")]
        public string CodBene { get; set; }

        public List<ValidarContratante> ContratanteList { get; set; }
        public List<ValidarBeneficiario> BeneficiarioList { get; set; }

        public string Mensaje { get; set; }
    }
}