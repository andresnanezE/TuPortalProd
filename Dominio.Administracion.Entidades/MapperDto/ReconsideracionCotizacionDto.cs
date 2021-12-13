using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class ReconsideracionCotizacionDto
    {
        public int UserId { get; set; }
        public IEnumerable<int> RolId { get; set; }
        public string NotaReconsideracion { get; set; }
        public int CotizacionId { get; set; }
        public string DetalleReconsideracion { get; set; }

        public IEnumerable<SedeReconsideracionDto> Sedes { get; set; }


    }
    public class SedeReconsideracionDto
    {
        public int SedeId { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public Nullable<decimal> ValorReconsideracion { get; set; }

    }
}
