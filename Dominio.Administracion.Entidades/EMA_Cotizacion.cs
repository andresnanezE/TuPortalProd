using System;

namespace Dominio.Administracion.Entidades
{
    public class EMA_Cotizacion
    {
        public int id_cotizacion { get; set; }
        public string NIT { get; set; }
        public string nombreEmpresa { get; set; }
        public string productos { get; set; }
        public string nombreAsesor { get; set; }
        public Nullable<System.DateTime> fechaVencimiento { get; set; }
        public string estado { get; set; }
        public string estadoCotizacion { get; set; }
        public Nullable<System.DateTime> FechaVisita { get; set; }
        public bool validarVencida { get; set; }
    }
}