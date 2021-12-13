using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

//using Dominio.Administracion.Entidades.MapperDto;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionSesionEnVezDeAdmUsuarios
    {
        IEnumerable<SesionEnVezDeParametrosCrearUsuarioDto> ObtenerParametros(decimal cod_usuario, int id);

        int CrearUsuario(SesionEnVezDeNuevoUsuarioDto usuario);

        IEnumerable<SesionEnVezDeUsuarioRecuperadoDto> ObtUsuarios();

        int ActEstadoUsuario(int id, char estado);

        int ActUsuario(SesionEnVezDeNuevoUsuarioDto usuario);
    }
}