using Dominio.Administracion.Entidades.ModeloProcesos;
using Dominio.Administracion.Entidades.ModeloUsuariosCentralizado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Repositorios
{
    interface IRepositorioUsrRoles
    {
        #region Instance Methods

        int AgregarRol(Roles rol);
        void AgregarRolMenu(IEnumerable<int> rolesIds, int menuId);
        void AgregarRolUsuario(IEnumerable<int> rolesIds, int usuarioId);
        void ModificarRol(Roles rol);
        void ModificarRolesMenu(IEnumerable<int> rolesIds, int menuId);
        void ModificarRolesUsuario(IEnumerable<int> rolesIds, int usuarioId);
        Roles ObtenerRolId(int rolId);
        IEnumerable<EMA_ROLXMENU> ObtenerRolMenu(int menuId);
        IEnumerable<UsrXappXrol> ObtenerRolUsuario(int usuarioId);
        IEnumerable<Roles> ObtenerRoles();
        IEnumerable<Roles> ObtenerRolesActivos();
        IEnumerable<Roles> ObtenerRolesFiltros(int rolId, string rol);

        #endregion
    }
}
