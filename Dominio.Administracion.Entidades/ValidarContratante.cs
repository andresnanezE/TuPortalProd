using System;

namespace Dominio.Administracion.Entidades
{
    public class ValidarContratante
    {
        public Int16 Cod_Empr { get; set; }
        public int Rmt_Cont { get; set; }
        public int Num_Cont { get; set; }
        public char Est_Cont { get; set; }
        public string Est_Contt { get; set; }
        public DateTime Fec_Inic { get; set; }
        public DateTime Fec_Venc { get; set; }
        public char Doc_Admi { get; set; }
        public char Tip_Deau { get; set; }
        public char Mod_Pago { get; set; }
        public string Vig_Cont { get; set; }
        public string Mod_PagoT { get; set; }
        public string Tip_Pago { get; set; }
        public decimal Tar_Cont { get; set; }
        public decimal Cod_Contra { get; set; }
        public string Nom_Contra { get; set; }
        public string Tel_Terc { get; set; }
        public string Nro_Celu { get; set; }
        public decimal Saldo_Cartera { get; set; }
        public char Con_Mora { get; set; }
        public string Con_Morat { get; set; }
        public DateTime? Fec_Mora { get; set; }
        public string Cal_Terc { get; set; }
        public Int16 Nro_Bene { get; set; }
    }
}