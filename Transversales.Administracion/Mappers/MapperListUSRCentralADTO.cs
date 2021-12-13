//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloCentralizada;
using System.Collections.Generic;

namespace Transversales.Administracion.Mappers
{
    public class MapperListUSRCentralADTO
    {
        public static IEnumerable<EMA_USUARIODto> AdaptarMenu(IEnumerable<usrCentral> origen)
        {
            List<EMA_USUARIODto> vRetorno = new List<EMA_USUARIODto>();
            if (origen != null)
            {
                foreach (var index in origen)
                {
                    vRetorno.Add(EntidadUSRCentralADTO(index));
                }
            }
            return vRetorno;
        }

        public static EMA_USUARIODto EntidadUSRCentralADTO(usrCentral item)
        {
            EMA_USUARIODto vRetorno = new EMA_USUARIODto()
            {
                ACTIVO = item.activo,
                CLAVE = item.clave,
                CORREO = item.correo,
                DOCUMENTO = item.num_doc,
                FECHAEXPIRACLAVE = item.fec_expira_clave,
                FECHAULTIMASESION = item.fec_ultima_sesion,
                FECHAREGISTRO = item.fec_regis,
                NOMBREUSUARIO = item.nom_usr,
                USUARIO = item.log_usr,
                USUARIOID = item.id_usr
            };
            return vRetorno;
        }
    }
}