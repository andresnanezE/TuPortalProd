// ----------------------------------------------------------------------------------------------
// <copyright file="IServicioAplicacionMenu.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades.MapperDto;

//using Dominio.Administracion.Entidades.MapperDto;
//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionMenu
    {
        #region Instance Methods

        int AgregarMenu(EMA_MENUDto menu, IEnumerable<int> rolesIds);

        void ModificarMenu(EMA_MENUDto menu, IEnumerable<int> rolesIds);

        IEnumerable<MenuAplicacionDto> ObtenerMenu();

        IEnumerable<MenuAplicacionDto> ObtenerMenuFiltros(int menuId, string descripcion, int? nodoPadreId, int rolId);

        IEnumerable<EMA_MENUDto> ObtenerMenuPadres();

        IEnumerable<MenuAplicacionDto> ObtenerMenuRoles(List<int> rolesIds);

        #endregion Instance Methods
    }
}