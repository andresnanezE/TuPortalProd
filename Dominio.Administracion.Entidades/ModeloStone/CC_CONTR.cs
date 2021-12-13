using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloStone
{
    public class CC_CONTR
    {
        [Key]
        public int RMT_CONT { get; set; }

        public decimal COD_ASES { get; set; }
        public char ACT_ESTA { get; set; }
        public decimal COD_TERC { get; set; }
        public string EST_CONT { get; set; }
        public DateTime FEC_VENC { get; set; }
        public int COD_TIPC { get; set; }
    }
}