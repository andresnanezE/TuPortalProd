// ----------------------------------------------------------------------------------------------
// <copyright file="IRepositorioMenu.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloProcesos;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioMenu
    {
        #region Instance Methods

        int AgregarMenu(EMA_MENU menu);
        void ModificarMenu(EMA_MENU menu);
        IEnumerable<MenuAplicacion> ObtenerMenu();
        IEnumerable<MenuAplicacion> ObtenerMenuFiltros(int menuId, string descripcion, int? nodoPadreId, int rolId);
        IEnumerable<EMA_MENU> ObtenerMenuPadres();
        IEnumerable<MenuAplicacion> ObtenerMenuRoles(List<int> rolesIds);

        #endregion
    }
}
