using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloStone
{
    public class GN_TERCE
    {
        [Key]
        public decimal COD_TERC { get; set; }

        public string NUM_IDEN { get; set; }
        public Int16? DIG_VERI { get; set; }
        public string NOM_TERC { get; set; }
        public string NOM_COMP { get; set; }
    }
}