// ----------------------------------------------------------------------------------------------
// <copyright file="IServicioAplicacionUsuarios.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Aplicacion.Administracion.Dto;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloProcesos;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionUsuarios
    {
        #region Instance Methods

        int AgregarUsuario(EMA_USUARIODto usuario, IEnumerable<int> rolesIds);
        void ModificarUsuario(EMA_USUARIODto usuarioModificar, IEnumerable<int> rolesIds);
        EMA_USUARIODto ObtenerUsuarioDocumento(string documento);
        UsuarioExternoDto ObtenerUsuarioExterno(string tipoDocumento, string documento);
        IEnumerable<EMA_USUARIODto> ObtenerUsuarioFiltros(string usuario, string nombreUsuario, string tipoDocumento, string documento, string correo);
        EMA_USUARIODto ObtenerUsuarioId(int usuarioId);
        EMA_USUARIODto ObtenerUsuarioUserName(string usuario);
        IEnumerable<EMA_USUARIODto> ObtenerUsuarios(int usuarioId);
        EMA_USUARIODto ValidarUsuario(string usuario, string contrasena);

        #endregion
    }
}
