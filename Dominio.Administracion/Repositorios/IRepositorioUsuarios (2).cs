// ----------------------------------------------------------------------------------------------
// <copyright file="IRepositorioUsuarios.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Dominio.Turnos</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloProcesos;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioUsuarios
    {
        #region Instance Methods

        EMA_USUARIO AgregarUsuario(EMA_USUARIO usuario);
        void EliminarUsuario(int usuarioId);
        void ModificarUsuario(EMA_USUARIO usuarioModificar);
        EMA_USUARIO ObtenerUsuarioDocumento(string documento);
        UsuarioExterno ObtenerUsuarioExterno(string tipoDocumento, string documento);
        IEnumerable<EMA_USUARIO> ObtenerUsuarioFiltros(string usuario, string nombreUsuario, string tipoDocumento, string documento, string correo);
        EMA_USUARIO ObtenerUsuarioId(int usuarioId);
        EMA_USUARIO ObtenerUsuarioUserName(string usuario);
        IEnumerable<EMA_USUARIO> ObtenerUsuarios(int usuarioId);
        EMA_USUARIO ValidarUsuario(string usuario, string contrasena);

        #endregion
    }
}
