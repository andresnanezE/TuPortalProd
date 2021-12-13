// ----------------------------------------------------------------------------------------------
// <copyright file="IRepositorioRoles.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloProcesos;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioRoles
    {
        #region Instance Methods

        int AgregarRol(EMA_ROL rol);
        void AgregarRolMenu(IEnumerable<int> rolesIds, int menuId);
        void AgregarRolUsuario(IEnumerable<int> rolesIds, int usuarioId);
        void ModificarRol(EMA_ROL rol);
        void ModificarRolesMenu(IEnumerable<int> rolesIds, int menuId);
        void ModificarRolesUsuario(IEnumerable<int> rolesIds, int usuarioId);
        EMA_ROL ObtenerRolId(int rolId);
        IEnumerable<EMA_ROLXMENU> ObtenerRolMenu(int menuId);
        IEnumerable<EMA_ROLXUSUARIO> ObtenerRolUsuario(int usuarioId);
        IEnumerable<EMA_ROL> ObtenerRoles();
        IEnumerable<EMA_ROL> ObtenerRolesActivos();
        IEnumerable<EMA_ROL> ObtenerRolesFiltros(int rolId, string rol);

        #endregion
    }
}
