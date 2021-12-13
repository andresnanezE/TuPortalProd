using System.Collections.Generic;

namespace Dominio.Administracion.Entidades
{
    public class ConsultaDoble
    {
        public IEnumerable<resultadosConsultaAfiliacionResumen> listaResumenAfiliaciones { get; set; }

        public IEnumerable<resultadosConsultaAfiliacionEstatus> listaResumenAfiliacionesEstatus { get; set; }
    }
}