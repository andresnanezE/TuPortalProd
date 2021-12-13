namespace Presentacion.Mvc.App.Models
{
    /// <summary>
    /// marzo 2017
    /// John Nelson Rodriguez G
    /// </summary>
    public class DetalleContrato
    {
        public decimal? Busqueda { get; set; }
        public int Rmt { get; set; }
        public decimal Contratante { get; set; }
        public string TipoContrato { get; set; }
        public string TipoBusqueda { get; set; }
        public string NombreContratante { get; set; }
        public string NumCont { get; set; }
        public string Prefijo { get; set; }

        public string Mensaje { get; set; }

      }
}