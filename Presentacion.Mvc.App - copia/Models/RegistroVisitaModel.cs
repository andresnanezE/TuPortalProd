using Aplicacion.Administracion.Dto.DtoProcesos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Presentacion.Mvc.App.Models
{
    public class RegistroVisitaModel
    {
        //[Required(ErrorMessage = "Por favor ingrese el NIT.")]
        [RegularExpression("([0-9]+)")]
        public string NIT { get; set; }
        [Display(Name = "D.V.")]
        public int DV { get; set; }
        //[Required(ErrorMessage = "Por favor ingrese el nombre de la empresa cliente")]
        public string nombreEmpresa { get; set; }
        public string mensaje { get; set; }
        //[Required(ErrorMessage = "Por favor ingrese el nombre del contacto")]
        public string Contacto { get; set; }

        //[Required(ErrorMessage = "Por favor ingrese el número de telefono de contacto")]
        [RegularExpression("([0-9]+)")]
        public string telefono { get; set; }
        //[Required(ErrorMessage = "Por favor ingrese el número de telefono de contacto")]
        [RegularExpression("([0-9]+)")]
        public string celular { get; set; }
        //[Required(ErrorMessage = "Por favor ingrese el cargo del contacto")]
        public string Cargo { get; set; }
        //[Required(ErrorMessage = "Por favor seleccione por lo menos un producto")]
        public int[] productosIds { get; set; }

        public IEnumerable<ProductosSelector> productos { get; set; }

        //public string nivelInteres { get; set; }
        //public IEnumerable<NivelesInteres> nivelesInteres { get; set; }
        public DateTime FechaVisita { get; set; }
        public DateTime FechaCambioClave { get; set; }
        //[Required(ErrorMessage = "Por favor ingrese una descripción de la visita")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public string MotivoVisita { get; set; }
        [DataType(DataType.EmailAddress)]
        //[Required(ErrorMessage = "Por favor ingrese el correo.")]
        [EmailAddress(ErrorMessage = "Por favor ingrese un correo valido.")]
        public string correoElectronico { get; set; }
        public bool Seleccionado { get; set; }
        public string estado { get; set; }
        public IPagedList<EMA_Cotizacion> misCotizaciones { get; set; }
        public IPagedList<EMA_CotizacionXAsesor> cotizaAsesor { get; set; }
        public IEnumerable<EstadosCotizacion> estadosCotizacion { get; set; }       
        public string IdCotizacion { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public int? estadoCotiID { get; set; }
        public String DescripcionFiltro { get; set; }
        //[Required(ErrorMessage = "Por favor ingrese el número de documento.")]
        [RegularExpression("([0-9]+)")]
        public string numeroDocumento { get; set; }
    }
}