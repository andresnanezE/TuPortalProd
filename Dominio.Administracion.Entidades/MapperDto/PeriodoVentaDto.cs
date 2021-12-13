using System;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class PeriodoVentaDto
    {
        public int Anio { get; set; }
        public int Periodo { get; set; }
        public int Semana { get; set; }
        public int Dia { get; set; }
        public DateTime Fecha { get; set; }
        public bool EsHabil { get; set; }
    }
}