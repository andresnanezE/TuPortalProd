using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class ConsultaDobleDto
    {
        public IEnumerable<resultadosConsultaAfiliacionResumenDto> listaResumenAfiliaciones { get; set; }

        public IEnumerable<resultadosConsultaAfiliacionEstatusDto> listaResumenAfiliacionesEstatus { get; set; }
    }
}