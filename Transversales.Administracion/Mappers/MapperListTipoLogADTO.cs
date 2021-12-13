//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloPortal;
using System.Collections.Generic;

namespace Transversales.Administracion.Mappers
{
    public class MapperListTipoLogADTO
    {
        public static IEnumerable<EMB_TipoLogDto> AdaptarMenu(IEnumerable<EMB_TipoLog> origen)
        {
            List<EMB_TipoLogDto> vRetorno = new List<EMB_TipoLogDto>();
            if (origen != null)
            {
                foreach (var index in origen)
                {
                    vRetorno.Add(EntidadTipoLogADTO(index));
                }
            }
            return vRetorno;
        }

        public static EMB_TipoLogDto EntidadTipoLogADTO(EMB_TipoLog item)
        {
            EMB_TipoLogDto vRetorno = new EMB_TipoLogDto()
            {
                descripcion = item.descripcion,
                idTipoLog = item.idTipoLog,
                tipo = item.tipo
            };
            return vRetorno;
        }
    }
}