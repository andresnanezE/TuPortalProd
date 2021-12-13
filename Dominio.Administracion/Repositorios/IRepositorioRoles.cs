// ----------------------------------------------------------------------------------------------
// <copyright file="IRepositorioRoles.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades.ModeloCentralizada;
using Dominio.Administracion.Entidades.ModeloProcesos;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioRoles
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

        IEnumerable<USR_APL_ROL> ObtenerRolUsuario(int usuarioId);

        IEnumerable<Roles> ObtenerRoles();

        IEnumerable<Roles> ObtenerRolesActivos();

        IEnumerable<Roles> ObtenerRolesFiltros(int rolId, string rol);

        #endregion Instance Methods
    }
}