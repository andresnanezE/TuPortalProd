using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class WsRespuestaOperacionDto
    {
        public WsRespuestaOperacionDto()
        {
            Mensajes = new List<string>(); Exito = true;
        }

        public bool Exito { get; set; }
        public List<string> Mensajes { get; set; }
    }
}