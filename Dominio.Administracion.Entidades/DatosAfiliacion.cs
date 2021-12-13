using System;

namespace Dominio.Administracion.Entidades
{
    public class DatosAfiliacion
    {
        public string @DOCUMENTO { get; set; }
        public string @ROL { get; set; }
        public string @TIPO { get; set; }
        public DateTime @FECH_PERIODO { get; set; }

        public DateTime FECH_PERIODO2 { get; set; }
        public string @COMISIONA { get; set; }
        public string @TIP_CONTR { get; set; }
        public string TIP_NOVEDAD { get; set; }
        public string EST_BENEF { get; set; }
    }
}