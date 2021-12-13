using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using System.Collections.Generic;

//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class AfiliacionesFiltroDto
    {
        public DatosGeneralesFiltroAfiliacionesDto DatosFiltro { get; set; }
        public IEnumerable<EM_FILTRO_OPCION_AFILIACIONESDto> ListaOpciones { get; set; }
        public string FiltroDefault { get; set; }
        public string Mostar { get; set; }
        public IEnumerable<NovedadesHomologadasDto> ListaNovedadesHomologadas { get; set; }
    }
}