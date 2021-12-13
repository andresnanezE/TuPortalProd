using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades.MapperDto.DtoStone.DtoSitioWeb
{
    public class DatosActualizacionMedioPagoIvrDto
    {
        public int Id_CC_ACTFP { get; set; }
        public string EST_PASARELA { get; set; }
        public string SESION_IVR { get; set; }
    }
}
