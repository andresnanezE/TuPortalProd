// ----------------------------------------------------------------------------------------------
// <copyright file="IServicioAplicacionRoles.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloProcesos;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionRoles
    {
        #region Instance Methods

        int AgregarRol(EMA_ROLDto rol);
        void AgregarRolUsuario(IEnumerable<int> rolIds, int usuarioId);
        void ModificarRol(EMA_ROLDto rol);
        EMA_ROLDto ObtenerRolId(int rolId);
        IEnumerable<EMA_ROLXMENUDto> ObtenerRolMenu(int menuId);
        IEnumerable<EMA_ROLXUSUARIODto> ObtenerRolUsuario(int usuarioId);
        IEnumerable<EMA_ROLDto> ObtenerRoles();
        IEnumerable<EMA_ROLDto> ObtenerRolesActivos();
        IEnumerable<EMA_ROLDto> ObtenerRolesFiltros(int rolId, string rol);

        #endregion
    }
}
