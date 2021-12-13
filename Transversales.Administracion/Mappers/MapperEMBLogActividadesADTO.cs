//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloPortal;

namespace Transversales.Administracion.Mappers
{
    public class MapperEMBLogActividadesADTO
    {
        public static EmbLogActividadesDto AdaptarMenu(EMB_LogActividades origen)
        {
            EmbLogActividadesDto vRetorno = new EmbLogActividadesDto();
            if (origen != null)
            {
                vRetorno = EntidadLogActividadesADTO(origen);
            }
            return vRetorno;
        }

        public static EmbLogActividadesDto EntidadLogActividadesADTO(EMB_LogActividades item)
        {
            EmbLogActividadesDto vRetorno = new EmbLogActividadesDto()
            {
                DatosIngreso = item.DatosIngreso,
                Detalle = item.Detalle,
                DocUsuario1 = item.DocUsuario1,
                DocUsuario2 = item.DocUsuario2,
                EsSesionEnVezDe = item.EsSesionEnVezDe,
                fecha = item.fecha,
                FechaHoraFin = item.FechaHoraFin,
                FechaHoraIni = item.FechaHoraIni,
                idLog = item.idLog,
                idTipoLog = item.idTipoLog,
                ip = item.ip,
                MenuId = item.MenuId,
                NombreUsuario1 = item.NombreUsuario1,
                NombreUsuario2 = item.NombreUsuario2,
                Respuesta = item.Respuesta,
                TiempoSesion = item.TiempoSesion,
                UsuarioId = item.UsuarioId
            };
            return vRetorno;
        }
    }
}