using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Tipos_AP
    {
        [Key]
        public int Id_area { get; set; }

        public int Id_Factor { get; set; }
        public string Tipo_area { get; set; }

        [DisplayFormat(DataFormatString = "{0:D9}", ApplyFormatInEditMode = true)]
        public decimal Valor { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal Valor_Constante { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal Valor_Exponencial { get; set; }

        public Boolean Estado { get; set; }
    }
}