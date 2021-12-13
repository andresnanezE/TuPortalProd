//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloCentralizada;

namespace Transversales.Administracion.Mappers
{
    public class MapperUSRCentralADTO
    {
        public static EMA_USUARIODto AdaptarMenu(usrCentral origen)
        {
            EMA_USUARIODto vRetorno = new EMA_USUARIODto();
            if (origen != null)
            {
                vRetorno = EntidadUSRCentralADTO(origen);
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
                FECHAREGISTRO = item.fec_regis,
                FECHAULTIMASESION = item.fec_ultima_sesion,
                NOMBREUSUARIO = item.nom_usr,
                USUARIO = item.log_usr,
                USUARIOID = item.id_usr
            };
            return vRetorno;
        }
    }
}