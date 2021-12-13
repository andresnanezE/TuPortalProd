namespace Dominio.Administracion.Entidades.MapperDto.DtoSitioWeb
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class SPEM_CONSULTACONTRATOSDto
    {
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Identificacion { get; set; }

        public string Nombre { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public string Email { get; set; }

        public int RmtCont { get; set; }

        public int num_pers { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal cod_ases { get; set; }

        public string nom_comp { get; set; }
        public string TipoContrato { get; set; }
        public string Estado { get; set; }
        public DateTime FechaI { get; set; }
        public DateTime FechaV { get; set; }
        public string FormaPago { get; set; }
        public string ModoPago { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal CuotaMensual { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal ValorContrato { get; set; }

        public string EstadoAse { get; set; }
        public string TelefonoAse { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal? Cartera { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal TarifaSinIva { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal TarifaConIva { get; set; }

        public string Tipo { get; set; }

        public string Num_cont { get; set; }

        public string NombreDirector { get; set; }

        public string TelDirector { get; set; }

        public string TelAsesor { get; set; }

        public string SubCanal { get; set; }

        public string CiudadAsesor { get; set; }

        public string EstadoAsesor { get; set; }

        public string PrefijoCont { get; set; }

        public bool MostrarBoton { get; set; }

        public decimal cod_dire { get; set; }

        public bool Ver_Detalle { get; set; }

        public DateTime? fCorte { get; set; }

        public decimal? vIva { get; set; }
        public int? diasFaltantes { get; set; }

        public int? diasProrrateo { get; set; }

        public string NOM_BENE { get; set; }

    }
}