// ----------------------------------------------------------------------------------------------
// <copyright file="ServicioAplicacionUsuarios.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using Dominio.Administracion.Entidades.ModeloProcesos;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionUsuarios : IServicioAplicacionUsuarios
    {
        #region Constants

        private const string ErrorPersona = "El usuario no existe. Comuníquese con su administrador ";
        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";
        private const string ErrorUsuarioExistente = "El usuario ya existe en la aplicación.";
        private const string ErrorUsuarioExterno = "El usuario no existe como usuario de Aplicación. Comuníquese con su administrador ";

        #endregion

        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioRoles _repositorioRoles;
        private readonly IRepositorioUsuarios _repositorioUsuarios;

        #endregion

        #region C'tors

        public ServicioAplicacionUsuarios(IRepositorioUsuarios repositorioUsuarios, IRepositorioLogs manejadorLogs, IRepositorioRoles repositorioRoles, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioUsuarios = repositorioUsuarios;
            _manejadorLogs = manejadorLogs;
            _repositorioRoles = repositorioRoles;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion

        #region Instance Methods

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        #endregion

        #region IServicioAplicacionUsuarios Members

        public int AgregarUsuario(EMA_USUARIODto usuario, IEnumerable<int> rolesIds)
        {
            string mensaje = null;
            try
            {
                var existeUsuario = _repositorioUsuarios.ObtenerUsuarioUserName(usuario.USUARIO);
                if (existeUsuario == null)
                {
                    var datosEntidad = _adaptadorDeObjetos.Adaptar<EMA_USUARIO>(usuario);
                    var usuarioEntidad = _repositorioUsuarios.AgregarUsuario(datosEntidad);
                    _repositorioRoles.AgregarRolUsuario(rolesIds, usuarioEntidad.USUARIOID);
                    return usuarioEntidad.USUARIOID;
                }
                mensaje = ErrorUsuarioExistente;
                throw new SystemException();
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, mensaje ?? ErrorProcesandoPeticion);
            }
        }

        public void ModificarUsuario(EMA_USUARIODto usuarioModificar, IEnumerable<int> rolesIds)
        {
            try
            {
                var datosEntidad = _adaptadorDeObjetos.Adaptar<EMA_USUARIO>(usuarioModificar);
                _repositorioUsuarios.ModificarUsuario(datosEntidad);
                if (rolesIds != null)
                {
                    _repositorioRoles.ModificarRolesUsuario(rolesIds, usuarioModificar.USUARIOID);
                }
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public EMA_USUARIODto ObtenerUsuarioDocumento(string documento)
        {
            EMA_USUARIODto respuestaUsuario;
            try
            {
                var respuestaEntidad = _repositorioUsuarios.ObtenerUsuarioDocumento(documento);
                respuestaUsuario = _adaptadorDeObjetos.Adaptar<EMA_USUARIODto>(respuestaEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return respuestaUsuario;
        }

        public UsuarioExternoDto ObtenerUsuarioExterno(string tipoDocumento, string documento)
        {
            UsuarioExternoDto respuestaUsuario;
            try
            {
                var respuestaEntidad = _repositorioUsuarios.ObtenerUsuarioExterno(tipoDocumento, documento);
                respuestaUsuario = _adaptadorDeObjetos.Adaptar<UsuarioExternoDto>(respuestaEntidad);
                if (respuestaUsuario == null)
                {
                    throw new SystemException();
                }
                if (respuestaUsuario.Contrasena == null)
                {
                    throw new SystemException();
                }
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorUsuarioExterno);
            }
            return respuestaUsuario;
        }

        public IEnumerable<EMA_USUARIODto> ObtenerUsuarioFiltros(string usuario, string nombreUsuario, string tipoDocumento, string documento, string correo)
        {
            try
            {
                var resultadoEntidad = _repositorioUsuarios.ObtenerUsuarioFiltros(usuario, nombreUsuario, tipoDocumento, documento, correo);
                return _adaptadorDeObjetos.Adaptar<IEnumerable<EMA_USUARIODto>>(resultadoEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public EMA_USUARIODto ObtenerUsuarioId(int usuarioId)
        {
            EMA_USUARIODto respuestaUsuario;
            try
            {
                var resultadoEntidad = _repositorioUsuarios.ObtenerUsuarioId(usuarioId);
                respuestaUsuario = _adaptadorDeObjetos.Adaptar<EMA_USUARIODto>(resultadoEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return respuestaUsuario;
        }

        public EMA_USUARIODto ObtenerUsuarioUserName(string usuario)
        {
            EMA_USUARIODto respuestaUsuario;
            try
            {
                var resultadoEntidad = _repositorioUsuarios.ObtenerUsuarioUserName(usuario);
                respuestaUsuario = _adaptadorDeObjetos.Adaptar<EMA_USUARIODto>(resultadoEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return respuestaUsuario;
        }

        public IEnumerable<EMA_USUARIODto> ObtenerUsuarios(int usuarioId)
        {
            IEnumerable<EMA_USUARIODto> respuestaUsuarios;
            try
            {
                var resutladoEntidad = _repositorioUsuarios.ObtenerUsuarios(usuarioId);
                respuestaUsuarios = _adaptadorDeObjetos.Adaptar<IEnumerable<EMA_USUARIODto>>(resutladoEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return respuestaUsuarios;
        }

        public EMA_USUARIODto ValidarUsuario(string usuario, string contrasena)
        {
            EMA_USUARIODto respuestaUsuario;
            try
            {
                var resultadoEntidad = _repositorioUsuarios.ValidarUsuario(usuario, contrasena);
                respuestaUsuario = _adaptadorDeObjetos.Adaptar<EMA_USUARIODto>(resultadoEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return respuestaUsuario;
        }

        #endregion
    }
}
