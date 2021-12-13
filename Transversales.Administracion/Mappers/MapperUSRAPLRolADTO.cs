//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloCentralizada;
using System.Collections.Generic;

namespace Transversales.Administracion.Mappers
{
    public class MapperUSRAPLRolADTO
    {
        public static IEnumerable<EMA_ROLXUSUARIODto> AdaptarMenu(IEnumerable<USR_APL_ROL> origen)
        {
            List<EMA_ROLXUSUARIODto> vRetorno = new List<EMA_ROLXUSUARIODto>();
            if (origen != null)
            {
                foreach (var index in origen)
                {
                    vRetorno.Add(EntidadUsrAplRolADTO(index));
                }
            }
            return vRetorno;
        }

        public static EMA_ROLXUSUARIODto EntidadUsrAplRolADTO(USR_APL_ROL item)
        {
            EMA_ROLXUSUARIODto vRetorno = new EMA_ROLXUSUARIODto()
            {
                ROLID = item.id_rol,
                USUARIOID = item.id_usr,
            };
            return vRetorno;
        }
    }
}