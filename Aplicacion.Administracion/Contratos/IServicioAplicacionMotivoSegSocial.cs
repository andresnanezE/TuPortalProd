using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

//
// j0HNn3LS0N r0DRIGU3Z
// 2016-10-25
namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionMotivoSegSocial
    {
        IEnumerable<MotivoSoporteSegSocialDto> MotivosSoporteSegSocial();

        void AgregarLog(MotivoSoporteSegSocialDto mss);
    }
}