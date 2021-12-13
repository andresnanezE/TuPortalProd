// ----------------------------------------------------------------------------------------------
// <copyright file="IServicioAplicacionUsuarios.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

//using Dominio.Administracion.Entidades.MapperDto;
//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.Reclutamiento;
using System;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionUsuarios
    {
        #region Instance Methods

        //void ModificarUsuario(EMA_USUARIODto usuarioModificar, IEnumerable<int> rolesIds);
        //EMA_USUARIODto ObtenerUsuarioDocumento(string documento);
        //UsuarioExternoDto ObtenerUsuarioExterno(string tipoDocumento, string documento);
        IEnumerable<EMA_USUARIODto> ObtenerUsuarioFiltros(string usuario, string nombreUsuario, string tipoDocumento, string documento, string correo);

        EMA_USUARIODto ObtenerUsuarioId(int usuarioId);

        IEnumerable<EMA_ROLXUSUARIODto> ObtenerRolUsuario(int usuarioId);

        EMA_USUARIODto ObtenerUsuarioUserName(string usuario);

        IEnumerable<EMA_USUARIODto> ObtenerUsuarios(int usuarioId);

        //EMA_USUARIODto ValidarUsuario(string usuario, string contrasena);
        EMA_USUARIODto ObtenerCredencialesUsuario(string documento);

        EMA_USUARIODto ValidarPermisoRegistro(int usuario);

        string ObtenerCorreoUsr(string userName);

        Tuple<string, string, bool> RegistrarUsuario(RegistroReclutamiento reclutamiento);

        #endregion Instance Methods
    }
}