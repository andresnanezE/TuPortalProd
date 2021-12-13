using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioSesionEnVezDeAdmUsuarios
    {
        IEnumerable<SesionEnVezDeParametrosCrearUsuario> ObtenerParametros(decimal cod_usuario, int id);

        int CrearUsuario(SesionEnVezDeNuevoUsuario usuario);

        IEnumerable<SesionEnVezDeUsuarioRecuperado> ObtUsuarios();

        int ActEstadoUsuario(int id, char estado);

        int ActUsuario(SesionEnVezDeNuevoUsuario usuario);
    }
}