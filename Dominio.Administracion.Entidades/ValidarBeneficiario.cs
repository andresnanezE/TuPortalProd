using System;

namespace Dominio.Administracion.Entidades
{
    public class ValidarBeneficiario
    {
        public int Rmt_Cont { get; set; }
        public int Num_Cont { get; set; }
        public char Est_Cont { get; set; }
        public string Est_Contt { get; set; }
        public DateTime Fec_Inic { get; set; }
        public DateTime Fec_Venc { get; set; }
        public string Vig_Cont { get; set; }
        public string Con_Morat { get; set; }
        public decimal Tar_Bene { get; set; }
        public int Rmt_Rccc { get; set; }
        public int Rmt_Rccb { get; set; }
        public char Est_Bene { get; set; }
        public string Est_Benet { get; set; }
        public decimal Cod_bene { get; set; }
        public string Nom_Bene { get; set; }
        public string Ape_Bene { get; set; }
        public string Nom_Comp_Bene { get; set; }
        public string Tel_Bene { get; set; }
        public string Cal_Bene { get; set; }
    }
}