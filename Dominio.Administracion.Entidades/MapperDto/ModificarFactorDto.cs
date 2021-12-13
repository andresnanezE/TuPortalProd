namespace Dominio.Administracion.Entidades.MapperDto
{
    public class ModificarFactorDto
    {
        public int IdFactor { get; set; }
        public int IdDetalleFactor { get; set; }
        public string NombreDetalleFactor { get; set; }
        public decimal ValorDetalleFactor { get; set; }
        public decimal ValorDetalleConstante { get; set; }
        public decimal ValorDetalleExponencial { get; set; }
        public decimal FactorAjuste { get; set; }
        public bool Estado { get; set; }

        //Factores adicionales para ciudad
        public decimal ValorIndicadorFuga { get; set; }
        public decimal GastosAdministrativos { get; set; }
        public decimal GastosComerciales { get; set; }
        public decimal FactorUtilidad { get; set; }

        public decimal ValorMinimoCotizacion { get; set; }
    }
}
