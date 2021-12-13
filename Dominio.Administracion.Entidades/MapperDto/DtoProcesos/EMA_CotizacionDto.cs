using Dominio.Administracion.Entidades.ModeloCotizacion;
using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.MapperDto.DtoProcesos
{
    public class EMA_CotizacionDto
    {
        public int id_cotizacion { get; set; }
        public int NIT { get; set; }
        public string nombreEmpresa { get; set; }
        public List<Productos> productos { get; set; }
        public string nombreAsesor { get; set; }
        public Nullable<System.DateTime> fechaVencimiento { get; set; }
        public string estado { get; set; }
    }
}