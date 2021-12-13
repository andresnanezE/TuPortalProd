using Dominio.Administracion.Entidades.MapperDto.DtoSitioWeb;

//using Dominio.Administracion.Entidades.MapperDto.DtoSitioWeb;
using PagedList;
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Mvc.App.Models
{
    /// <summary>
    /// JohnNelsonRodriguex
    /// </summary>
    public class ContratosModel
    {
        //[Required(ErrorMessage = "Por favor seleccione un Tipo Proceso.")]
        public string NumCont { get; set; }

        public decimal Codigo { get; set; }
        public string Criterio { get; set; }
        public string Mensaje { get; set; }
        public IPagedList<SPEM_CONSULTACONTRATOSDto> ListaContratos { get; set; }

        public Contratos Contrato { get; set; }

        public string TipoContrato { get; set; }
        public string Rmt_Cont { get; set; }
        public string Identificacion { get; set; }
        public string Contratante { get; set; }
        public string Telefono { get; set; }

        public string Estado { get; set; }
        public string Cantidad { get; set; }
        public string Asesor { get; set; }
        public string IdentificacionAse { get; set; }

        public string FormaPago { get; set; }

        public string ModoPago { get; set; }

        public string TelefonoAse { get; set; }
        public string EstadoAse { get; set; }

        public string CuotaMensual { get; set; }
        public string ValorContrato { get; set; }
        public string ValorCartera { get; set; }

        #region Instance Properties

        //[Required(ErrorMessage = "Por favor seleccione Fecha de Ejecución.")]
        //public DateTime? FechaEjecucion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione una Fecha Fin.")]
        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "Por favor seleccione una Fecha Inicial.")]
        [DataType(DataType.Date)]
        public DateTime? FechaInicio { get; set; }

        #endregion Instance Properties

        public string Num_idenBen { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fec_naciBen { get; set; }

        public string Nom_bene { get; set; }
        public string Tel_bene { get; set; }
        public string Titulo { get; set; }
        public decimal Busqueda { get; set; }

        public string viewBene { get; set; }
        public string viewPMP { get; set; }
        public string viewAP { get; set; }

        public DateTime? fCorte { get; set; }

        public decimal valIvaRecurrente { get; set; }
        public decimal @valIvaProrrateo { get; set; }
        public decimal @valIvaProxCuota { get; set; }

        public int? diasFaltantes { get; set; }

        public int? diasProrrateo { get; set; }

        public string NOMBRE_RTE_ATENCIONES { get; set; }
    }

    public class Contratos
    {
        public string TipoContrato { get; set; }
        public string Rmt_Cont { get; set; }
        public string Identificacion { get; set; }
        public string Contratante { get; set; }
        public string Estado { get; set; }
        public string Cantidad { get; set; }
        public string Asesor { get; set; }
        public string IdentificacionAse { get; set; }
        public string NOMBRE_RTE_ATENCIONES { get; set; }
    }

    public class Motivos
    {
        public int MotivoId { get; set; }
        public string Descripcion { get; set; }
    }

    public class Estados
    {
        public int EstadoId { get; set; }
        public string Estado { get; set; }
    }
}