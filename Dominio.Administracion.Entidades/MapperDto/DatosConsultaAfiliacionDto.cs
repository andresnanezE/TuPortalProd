using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class DatosConsultaAfiliacionDto
    {
        public DateTime Periodo { get; set; }
        public DateTime Periodo2 { get; set; }
        public List<string> Comisiona { get; set; }

        public List<string> Novedad { get; set; }

        public List<string> EstBenef { get; set; }

        public List<string> TipContrato { get; set; }

        public string Documento { get; set; }

        public int Rol { get; set; }

        public string TipContr { get; set; }
    }
}