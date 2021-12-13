// ----------------------------------------------------------------------------------------------
// <copyright file="IRepositorioUsuarios.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Dominio.Turnos</project>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloCentralizada;
using Dominio.Administracion.Entidades.Reclutamiento;
using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioUsuarios
    {
        #region Instance Methods

        usrCentral ObtenerUsuarioDocumento(int documento);

        IEnumerable<usrCentral> ObtenerUsuarioFiltros(string usuario, string nombreUsuario, string tipoDocumento, string documento, string correo);

        usrCentral ObtenerUsuarioId(int usuarioId);

        usrCentral ObtenerUsuarioUserName(string usuario);

        IEnumerable<usrCentral> ObtenerUsuarios(int usuarioId);

        usrCentral ObtenerCredencialesUsuario(string documento);

        EMSP_ValidarRegistro ValidarPermisoRegistroPorDocumento(int documento);

        string ObtenerCorreoUsr(string userName);

        Tuple<string, string, bool> RegistrarUsuario(RegistroReclutamiento reclutamiento);

        #endregion Instance Methods
    }
}