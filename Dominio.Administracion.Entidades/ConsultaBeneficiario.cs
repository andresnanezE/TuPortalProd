using System;

namespace Dominio.Administracion.Entidades
{
    public class ConsultaBeneficiario
    {
        public decimal Num_iden { get; set; }
        public DateTime Fec_naci { get; set; }

        public string Nom_bene { get; set; }

        public string Ape_bene { get; set; }

        public string Tel_bene { get; set; }
    }
}