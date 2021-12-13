using Dominio.Administracion.Entidades.CorreoDto;
using Dominio.Administracion.Entidades.ModelKheiron_Logs;

namespace Transversales.Administracion.Mappers
{
    public static class Mapper
    {
        public static EMH_ENVIO_CORREO DtoToEntity(CorreoDTO dto)
        {
            return new EMH_ENVIO_CORREO
            {
                ASUNTO = dto.Asunto,
                CUERPO = dto.Body,
                ADJUNTOS = dto.Adjuntos,
                CORREOS_DES = dto.Destinos,
                CORREOS_COP = dto.DestinosCopia,
                CORREOS_COO = dto.DestinosCopiaOculta,
                TIPO = dto.Tipo
            };
        }
    }
}