using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aplicacion.Administracion.Dto;

namespace Presentacion.Mvc.App.Models
{
    public class AfiliacionesModel
    {
        public IEnumerable<AfiliacionesPeriodoDto> ListaPeriodos { get; set; }
        public List<SelectListItem> ListaComisiona {
            get { 
                return new List<SelectListItem>()
                {
                    new SelectListItem(){Text = "SI",Value = "S"},
                    new SelectListItem(){Text = "NO",Value = "N"}
                };
            }
        }

        public List<SelectListItem> ListaTipoContrato {
            get { 
                return new List<SelectListItem>(){
                    new SelectListItem(){Text = "FAMILIAR",Value = "FAMILIAR"},
                    new SelectListItem(){Text = "PPE",Value = "PPE"},
                    new SelectListItem(){Text = "AREAS PROTEGIDAS",Value = "AREAS PROTEGIDAS"}
                };
            }
        }

        public List<SelectListItem> ListaTipoNovedad {
            get{
                return new List<SelectListItem>(){
                    new SelectListItem(){Text = "CREACION CONTRATO",Value = "270"},
                    new SelectListItem(){Text = "CREACION CONTRATO (SIN COMISION)",Value = "271"},
                    new SelectListItem(){Text = "INCLUSION CON COMISION",Value = "210"},
                    new SelectListItem(){Text = "INCLUSION CON COMISION SIN PRIMERA CUOTA",Value = "295"},
                    new SelectListItem(){Text = "INCLUSION SIN COMISION SIN PRIMERA CUOTA",Value = "205"},
                    new SelectListItem(){Text = "REACTIVACION BENEFICIARIO",Value = "340"},
                    new SelectListItem(){Text = "REACT BENEF CON COMIS SIN 1RA",Value = "350"},
                    new SelectListItem(){Text = "INCLUSION SIN COMISION",Value = "200"}
                };
            } 
        }

        public List<SelectListItem> ListaEstadoBeneficiario
        {
            get
            {
                return new List<SelectListItem>(){
                    new SelectListItem(){Text = "ACTIVO",Value = "ACTIVO"},
                    new SelectListItem(){Text = "CANCELADO",Value = "CANCELADO"}
                };
            } 
        }

        public IEnumerable<AfiliacionesFiltroDto> ListaFiltros { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un periodo")]
        public string Periodo { get; set; }

        public string Periodo2 { get; set; }

        public List<string> Comisiona { get; set; }

        public List<string> Novedad { get; set; }

        public List<string> EstBenef { get; set; }

        public List<string> TipContrato { get; set; }

        public string Mensaje { get; set; }
        public List<string> BajasComerciales { get; set; }
        public IEnumerable<resultadosConsultaAfiliacionResumenDto> ListaResultadoResumen { get; set; }
        public IEnumerable<ResultadosConsultaAfiliacionResumenTablaDto> ListaResultadoResumenTabla { get; set; }
        public IEnumerable<resultadosConsultaAfiliacionEstatusDto> ListaResultadoEstatus { get; set; }
        public IEnumerable<string> ListaEstatusAsesor { get; set; }
        public IEnumerable<string> ListaGroup { get; set; }

        public string TipoInforme { get; set; }
        public string ExportarFormato { get; set; }

        public string MensajeError { get; set; }

        public  string Canal { get; set; }

        public  string Ciudad { get; set; }
        public IEnumerable<CanalesDto> Canales { get; set; }
        public IEnumerable<CiudadesDto> Ciudades { get; set; }

        public string NOMBRE_REPORTE_DETALLE { get; set; }
        public string NOMBRE_REPORTE_RESUMEN { get; set; }
        public string NOMBRE_REPORTE { get; set; }
        public string NOMBRE_REPORTE_PDF { get; set; }
    }
}