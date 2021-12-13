namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class CalcularTarifaSede
    {
        public int Id_Ciudad { get; set; }
        public int Id_Sector { get; set; }
        public int Id_TipoRiesgo { get; set; }
        public int Id_TipoAP { get; set; }
        public int NumeroEmpleados { get; set; }
        public int NumeroVisitantes { get; set; }
    }
}