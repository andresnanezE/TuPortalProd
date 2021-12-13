//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

namespace Transversales.Administracion.Mappers
{
    public class MapperListComoVoyADTO
    {
        public static IEnumerable<ComoVoyDto> AdaptarMenu(IEnumerable<ComoVoy> origen)
        {
            List<ComoVoyDto> vRetorno = new List<ComoVoyDto>();
            if (origen != null)
            {
                foreach (var index in origen)
                {
                    vRetorno.Add(EntidadDTOAComoVoy(index));
                }
            }
            return vRetorno;
        }

        public static ComoVoyDto EntidadDTOAComoVoy(ComoVoy item)
        {
            ComoVoyDto vRetorno = new ComoVoyDto()
            {
                ESTATUS = item.ESTATUS,
                ESTATUS_HOMOLOG = item.ESTATUS_HOMOLOG,
                HTML = item.HTML,
                MOSTRAR = item.MOSTRAR,
                MSGERR = item.MSGERR,
                NOMBRE_CUADRO = item.NOMBRE_CUADRO,
                PATHMETA0 = item.PATHMETA0,
                PATHMETA1 = item.PATHMETA1,
                PATHMETA2 = item.PATHMETA2,
                POS = item.POS,
                TEXTO = item.TEXTO
            };
            return vRetorno;
        }
    }
}