using System;

namespace Dominio.Administracion.Entidades
{
    public class ContratosFacturas
    {
        #region Instance Properties

        public string ABR_DOCU { get; set; }
        public string CLA_CONT { get; set; }
        public decimal COD_ASES { get; set; }
        public Int16 COD_CCOS { get; set; }
        public Int16 COD_DETE { get; set; }
        public Int16 COD_DOCU { get; set; }
        public Int16 COD_EMPR { get; set; }
        public Int16 COD_MDUR { get; set; }
        public Int16 COD_MONE { get; set; }
        public int COD_PLAN { get; set; }
        public Int16 COD_PVTA { get; set; }
        public int COD_REFE { get; set; }
        public decimal COD_TERC { get; set; }
        public int COD_TIPC { get; set; }
        public string DES_CONT { get; set; }
        public string DES_TIPC { get; set; }
        public Int16 DIA_FACT { get; set; }
        public string DIR_COBR { get; set; }
        public string DOC_ADMI { get; set; }
        public DateTime FECHA_FINAL { get; set; }
        public DateTime FECHA_INICIAL { get; set; }
        public string FEC_INIC { get; set; }
        public DateTime FEC_TASA { get; set; }
        public DateTime FEC_VENC { get; set; }
        public string MOD_PAGO { get; set; }
        public string NOM_TIPC { get; set; }
        public int NUM_CONT { get; set; }
        public int NUM_DOCU { get; set; }
        public Int16 NUM_FACT { get; set; }
        public Int16 NUM_PERS { get; set; }
        public string PERIODO_FACTURA { get; set; }
        public int RMT_CAFA { get; set; }
        public int RMT_CONT { get; set; }
        public string TER_EMER { get; set; }
        public decimal VAL_MDUR { get; set; }
        public decimal VAL_SALD { get; set; }
        public decimal VAL_TASA { get; set; }
        public decimal VAL_TOTA { get; set; }
        public string PREFIJO { get; set; }
        public int TipoDocumento { get; set; }

        #endregion Instance Properties
    }
}