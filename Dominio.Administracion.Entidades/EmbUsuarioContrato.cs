using System;

namespace Dominio.Administracion.Entidades
{
    public class EmbUsuarioContrato
    {
        #region Instance Properties

        public int NumeroContrato { get; set; }
        public int RmtCont { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string TipoContrato { get; set; }
        public string CONTRATOCOMBO { get; set; }
        public string EST_CONT { get; set; }

        #endregion Instance Properties
    }
}