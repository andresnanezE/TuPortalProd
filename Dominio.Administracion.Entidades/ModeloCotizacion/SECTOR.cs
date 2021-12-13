using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class SECTOR
    {
        [Key]
        public int Id_sector { get; set; }

        public int Id_Factor { get; set; }
        public string Descripcion { get; set; }

        [DisplayFormat(DataFormatString = "{0:d9}", ApplyFormatInEditMode = true)]
        public decimal Valor { get; set; }

        public decimal Valor_Constante { get; set; }
        public decimal Valor_Exponencial { get; set; }

        [DisplayName("Factor De Ajuste Y")]
        public decimal Factor_Ajuste { get; set; }

        public Boolean Estado { get; set; }
    }
}