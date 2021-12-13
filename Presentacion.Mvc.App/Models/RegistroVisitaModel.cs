using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using Dominio.Administracion.Entidades.ReportesDto;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Models
{
    public class RegistroVisitaModel
    {
        //[Required(ErrorMessage = "Por favor ingrese el NIT.")]

        public string NIT { get; set; }

        [Display(Name = "D.V. ")]
        public int DV { get; set; }

        [Display(Name = "Ciudad")]
        public string ciudad { get; set; }

        [Display(Name = "Canal")]
        public Boolean validarNit { get; set; }

        public Boolean validarMdDetalleFactor { get; set; }
        public Boolean validarModalReserva { get; set; }
        public Boolean validarFormularioReserva { get; set; }
        public int validarMsgDetalleFactor { get; set; }

        public string ObservacionReserva { get; set; }
        public int TelefonoExt { get; set; }

        public string canal { get; set; }
        public string factor { get; set; }
        public string nombreEmpresa { get; set; }
        public string nombreDirectorAsesor { get; set; }
        public string mensaje { get; set; }
        public List<String> lstMensajeReserva { get; set; }
        public string Contacto { get; set; }
        public string NombreProductoCotizacion { get; set; }
        public List<string> lstIdRol { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Sólo números telefónicos")]
        //[Required(ErrorMessage = "Por favor ingrese el número de telefono de contacto")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Este campo solo acepta números (0 al 9)")]
        public string telefono { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Los  números celulares tienen 10 dígitos")]
        //[Required(ErrorMessage = "Por favor ingrese el número de telefono de contacto")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Este campo solo acepta números (0 al 9)")]
        public string celular { get; set; }

        //[Required(ErrorMessage = "Por favor ingrese el cargo del contacto")]
        public string Cargo { get; set; }

        //[Required(ErrorMessage = "Por favor seleccione por lo menos un producto")]

        public int[] productosIds { get; set; }
        public int[] validacionIds { get; set; }
        public int[] copiaDirectIds { get; set; }

        [Display(Name = "Productos")]
        public IEnumerable<ProductosSelector> productos { get; set; }

        public IEnumerable<Validacion> validacionInfo { get; set; }
        public IEnumerable<ValidacionCopiaDirector> validacionCopiaDirector { get; set; }

        //public string nivelInteres { get; set; }
        //public IEnumerable<NivelesInteres> nivelesInteres { get; set; }
        public DateTime FechaVisita { get; set; }

        public DateTime FechaExpiracion { get; set; }
        public DateTime FechaCambioClave { get; set; }

        //[Required(ErrorMessage = "Por favor ingrese una descripción de la visita")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public string MotivoVisita { get; set; }

        public string ObservacionesPricing { get; set; }

        [DataType(DataType.EmailAddress)]
        //[Required(ErrorMessage = "Por favor ingrese el correo electrónico del contacto.")]
        [Display(Name = "Correo")]
        [EmailAddress(ErrorMessage = "Por favor ingrese un correo válido.")]
        public string correoElectronico { get; set; }

        public bool Seleccionado { get; set; }
        public string estado { get; set; }
        public IPagedList<EMA_Cotizacion> misCotizaciones { get; set; }
        public IPagedList<Usp_ObtenerCotizaciones> lstCotizaciones { get; set; }
        public IPagedList<EMA_CotizacionXAsesor> cotizaAsesor { get; set; }

        public IEnumerable<EstadosCotizacions> estadosCotizacion { get; set; }
        public IEnumerable<Productos> listadoProductos { get; set; }
        public IEnumerable<Productos> listadoProductosDisponibles { get; set; }

        public IEnumerable<ESTADOS_COTI_PRIC> estadosPricing { get; set; }
        public string IdCotizacion { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public int? estadoCotiID { get; set; }

        [DisplayFormat(DataFormatString = "{0:D9}", ApplyFormatInEditMode = true)]
        public int? estadoDetalleFactorID { get; set; }

        public int? estadoPricingID { get; set; }
        public int productoSelectID { get; set; }
        public string productoSelectName { get; set; }

        public String DescripcionFiltro { get; set; }

        //[Required(ErrorMessage = "Por favor ingrese el número de documento.")]
        [RegularExpression("([0-9]+)")]
        public string numeroDocumento { get; set; }

        public string nombreAsesor { get; set; }
        public string nombreCliente { get; set; }
        public string tipoAreaProtegida { get; set; }
        public string sectorEconomico { get; set; }

        //Variables para el calculo de las cotizaciones
        [RegularExpression("([0-9]+)", ErrorMessage = "Este campo solo acepta números (0 al 9)")]
        public string numeroCapacitaciones { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Este campo solo acepta números (0 al 9)")]
        [Display(Name = "No. Eventos")]
        public string numeroEventos { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Este campo solo acepta números (0 al 9)")]
        [Display(Name = "No. Sedes")]
        public string numeroSedes { get; set; }

        //Variables de Mis Cotizaciones
        public DateTime? fechaInicioFiltroCoti { get; set; }

        public DateTime? fechaFinFiltroCoti { get; set; }
        public IEnumerable<EstadosCotizacion> estadosCotizacionSCI { get; set; }
        public int productoFiltroCotiSelectID { get; set; }
        public int estadoReserFiltroCotiSelectID { get; set; }
        public int estadoCotiFiltroCotiSelectID { get; set; }
        public string nitFiltroCoti { get; set; }
        public string estadoCotizacionAdjunto { get; set; }

        //Sedes Cotización
        public List<Cotizacion_Sedes> listadoAdicionarSede { get; set; }

        //Cotización / Sede 1
        [RegularExpression("([0-9]+)", ErrorMessage = "Este campo solo acepta números (0 al 9)")]
        [Display(Name = "No. Personas")]
        public string NumeroExpuestos { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Este campo solo acepta números (0 al 9)")]
        public string numeroPromedioVisitantes { get; set; }

        public IEnumerable<Tipos_AP> areas_protegidas { get; set; }
        public int? areasProtegID { get; set; }
        public IEnumerable<CiudadesFactor> ciudadesFactor { get; set; }
        public int? ciudadFactorID { get; set; }
        public IEnumerable<TipoRiesgo> tipo_riesgo { get; set; }
        public int? tipoRiesgoID { get; set; }

        //Factores
        public IEnumerable<SECTOR> sectores { get; set; }

        public IEnumerable<Factores> factores { get; set; }
        public List<Factores> factores_ { get; set; }
        public IPagedList<Usp_ObtenerDetalleValorFactor> listadoFactores { get; set; }

        public int? sectorID { get; set; }
        public int? productoID { get; set; }

        public string tipoAreaProt { get; set; }
        public string sectorSelec { get; set; }

        public string Total { get; set; }
        public string rutaArchivo { get; set; }

        public HttpPostedFileBase File { get; set; }

        public int banderaAreasProtegidas { get; set; }

        //Variables para el descargue de reporte de cotizaciones de asesores por director
        public DateTime FechaInicial { get; set; }

        public DateTime FechaFinal { get; set; }
        public int asesoID { get; set; }

        //Variables de Detalle Factor Seleccionado.
        public int IdDetalleFactor { get; set; }

        public int IdFactor { get; set; }
        public int Id_TipoFactor { get; set; }
        public string NombreDetalleFactor { get; set; }
        public string DescripcionDetalleFactor { get; set; }

        //[DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 9)")]
        //[RegularExpression("^[0-9]{18}(,)?[0-9]{9}$", ErrorMessage = "¡Este campo acepta números enteros y dígitos con máximo 9 decimales después de la coma!")]
        [DisplayFormat(DataFormatString = "{0:D9}", ApplyFormatInEditMode = true)]
        public string ValorDetalleFactorString { get; set; }

        [DisplayFormat(DataFormatString = "{0:D9}", ApplyFormatInEditMode = true)]
        public decimal ValorDetalleFactor => decimal.TryParse(ValorDetalleFactorString, out decimal var) ? decimal.Parse(ValorDetalleFactorString) : 0;

        //[DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 2)")]
        //[RegularExpression("^[0-9]{18}(,)?[0-9]{2}$", ErrorMessage = "Este campo acepta números enteros y dígitos con máximo 2 decimales después de la coma!")]
        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public string ValorConstanteFactorString { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal ValorConstanteFactor => decimal.TryParse(ValorConstanteFactorString, out decimal var) ? decimal.Parse(ValorConstanteFactorString) : 0;

        //[DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 2)")]
        [RegularExpression("^[+-]?[0-9]{0,15}(?:/,[0-9]{0,2})?$", ErrorMessage = "Este campo acepta números enteros y dígitos con máximo 2 decimales después de la coma!")]
        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public string ValorExponenteFactorString { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal ValorExponenteFactor => decimal.TryParse(ValorExponenteFactorString, out decimal var) ? decimal.Parse(ValorExponenteFactorString) : 0;

        public string EstadoDetalleFactor { get; set; }

        ////[DataType(DataType.Currency)]
        ////[Column(TypeName = "decimal(18, 2)")]
        //[RegularExpression("^[0-9]{18}(,)?[0-9]{2}?$", ErrorMessage = "Este campo acepta números enteros y dígitos con máximo 2 decimales después de la coma!")]
        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public string ValorDetalleConstanteString { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal ValorDetalleConstante => decimal.TryParse(ValorDetalleConstanteString, out decimal var) ? decimal.Parse(ValorDetalleConstanteString) : 0;

        //[DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 2)")]
        //[RegularExpression("^[0-9]{18}(,)?[0-9]{2}?$", ErrorMessage = "Este campo acepta números enteros y dígitos con máximo 2 decimales después de la coma!")]
        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public string ValorDetalleExponencialString { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal ValorDetalleExponencial => decimal.TryParse(ValorDetalleExponencialString, out decimal var) ? decimal.Parse(ValorDetalleExponencialString) : 0;

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public string ValorFactor_AjusteString { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal Factor_Ajuste => decimal.TryParse(ValorFactor_AjusteString, out decimal var) ? decimal.Parse(ValorFactor_AjusteString) : 0;

      
        public IEnumerable<SelectListItem> listEstadosDetalleFactor { get; set; }

        // Nuevos campos
        public string anotacion { get; set; }

        public int ext { get; set; }

        public string msgErrorReservaNit { get; set; }
        public int validMsgErrorReservaNit { get; set; }

        //reportes
        public IEnumerable<Reporte> DatosReportes { get; set; }
    }
}