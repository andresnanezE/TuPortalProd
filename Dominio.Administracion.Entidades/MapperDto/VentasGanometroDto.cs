using System;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class VentasGanometroDto
    {
        public int Id { get; set; }
        public int Anio { get; set; }
        public int Periodo { get; set; }
        public int Semana { get; set; }
        public int Dia { get; set; }
        public DateTime FechaVenta { get; set; }
        public bool EsHabil { get; set; }
        public int IdCiudadHomologada { get; set; }
        public int IdDirector { get; set; }
        public int CantidadVentas { get; set; }
        public DateTime FechaIngreso { get; set; }
    }

    public class MetasGanometroDto : VentasGanometroDto
    {
        public int CantidadMetaVentas { get; set; }
    }
}