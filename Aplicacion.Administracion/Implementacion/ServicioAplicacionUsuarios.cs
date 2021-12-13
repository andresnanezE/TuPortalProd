// ----------------------------------------------------------------------------------------------
// <copyright file="ServicioAplicacionUsuarios.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Aplicacion.Administracion.Contratos;

//using Dominio.Administracion.Entidades.MapperDto;
//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.Reclutamiento;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
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

        #endregion Constants

        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioRoles _repositorioRoles;
        private readonly IRepositorioUsuarios _repositorioUsuarios;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionUsuarios(IRepositorioUsuarios repositorioUsuarios, IRepositorioLogs manejadorLogs, IRepositorioRoles repositorioRoles, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioUsuarios = repositorioUsuarios;
            _manejadorLogs = manejadorLogs;
            _repositorioRoles = repositorioRoles;
            _adaptadorDeObjetos = adaptadorDeObjetos;
            _repositorioUsuarios = repositorioUsuarios;
        }

        #endregion C'tors

        #region Instance Methods

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        #endregion Instance Methods

        #region IServicioAplicacionUsuarios Members

        //public void ModificarUsuario(EMA_USUARIODto usuarioModificar, IEnumerable<int> rolesIds)
        //{
        //    //TODO:Nsarmiento:LLAMAR A WEBSERVICE PARA MODIFICAR EL USUARIO
        //    try
        //    {
        //        var datosEntidad = _adaptadorDeObjetos.Adaptar<Dominio.Administracion.Entidades.ModeloUsuariosCentralizado.Usuarios>(usuarioModificar);
        //        // _repositorioUsuarios.ModificarUsuario(datosEntidad);
        //        if (rolesIds != null)
        //        {
        //            _repositorioRoles.ModificarRolesUsuario(rolesIds, usuarioModificar.USUARIOID);
        //        }
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }
        //}

        //public EMA_USUARIODto ObtenerUsuarioDocumento(string documento)
        //{
        //    EMA_USUARIODto respuestaUsuario;
        //    try
        //    {
        //        var respuestaEntidad = _repositorioUsuarios.ObtenerUsuarioDocumento(documento);
        //        respuestaUsuario = _adaptadorDeObjetos.Adaptar<EMA_USUARIODto>(respuestaEntidad);
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }
        //    return respuestaUsuario;
        //}

        //public UsuarioExternoDto ObtenerUsuarioExterno(string tipoDocumento, string documento)
        //{
        //    UsuarioExternoDto respuestaUsuario;
        //    try
        //    {
        //        var respuestaEntidad = _repositorioUsuarios.ObtenerUsuarioExterno(tipoDocumento, documento);
        //        respuestaUsuario = _adaptadorDeObjetos.Adaptar<UsuarioExternoDto>(respuestaEntidad);
        //        if (respuestaUsuario == null)
        //        {
        //            throw new SystemException();
        //        }
        //        if (respuestaUsuario.Contrasena == null)
        //        {
        //            throw new SystemException();
        //        }
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorUsuarioExterno);
        //    }
        //    return respuestaUsuario;
        //}

        public IEnumerable<EMA_USUARIODto> ObtenerUsuarioFiltros(string usuario, string nombreUsuario, string tipoDocumento, string documento, string correo)
        {
            try
            {
                var resultadoEntidad = _repositorioUsuarios.ObtenerUsuarioFiltros(usuario, nombreUsuario, tipoDocumento, documento, correo);
                //return _adaptadorDeObjetos.Adaptar<IEnumerable<EMA_USUARIODto>>(resultadoEntidad);
                return Transversales.Administracion.Mappers.MapperListUSRCentralADTO.AdaptarMenu(resultadoEntidad);
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
                //respuestaUsuario = _adaptadorDeObjetos.Adaptar<EMA_USUARIODto>(resultadoEntidad);
                respuestaUsuario = Transversales.Administracion.Mappers.MapperUSRCentralADTO.AdaptarMenu(resultadoEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return respuestaUsuario;
        }

        public IEnumerable<EMA_ROLXUSUARIODto> ObtenerRolUsuario(int usuarioId)
        {
            IEnumerable<EMA_ROLXUSUARIODto> listadoRolUsuario;
            try
            {
                var resultadoRol = _repositorioRoles.ObtenerRolUsuario(usuarioId);
                //listadoRolUsuario = _adaptadorDeObjetos.Adaptar<IEnumerable<EMA_ROLXUSUARIODto>>(resultadoRol);
                listadoRolUsuario = Transversales.Administracion.Mappers.MapperUSRAPLRolADTO.AdaptarMenu(resultadoRol);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return listadoRolUsuario;
        }

        public EMA_USUARIODto ObtenerUsuarioUserName(string usuario)
        {
            EMA_USUARIODto respuestaUsuario;
            try
            {
                var resultadoEntidad = _repositorioUsuarios.ObtenerUsuarioUserName(usuario);
                //respuestaUsuario = _adaptadorDeObjetos.Adaptar<EMA_USUARIODto>(resultadoEntidad);
                respuestaUsuario = Transversales.Administracion.Mappers.MapperUSRCentralADTO.AdaptarMenu(resultadoEntidad);
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
                //respuestaUsuarios = _adaptadorDeObjetos.Adaptar<IEnumerable<EMA_USUARIODto>>(resutladoEntidad);
                respuestaUsuarios = Transversales.Administracion.Mappers.MapperListUSRCentralADTO.AdaptarMenu(resutladoEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return respuestaUsuarios;
        }

        //public EMA_USUARIODto ValidarUsuario(string usuario, string contrasena)
        //{
        //    EMA_USUARIODto respuestaUsuario;
        //    try
        //    {
        //        var resultadoEntidad = _repositorioUsuarios.ValidarUsuario(usuario, contrasena);
        //        respuestaUsuario = _adaptadorDeObjetos.Adaptar<EMA_USUARIODto>(resultadoEntidad);
        //    }
        //    catch (SystemException exception)
        //    {
        //        throw LogExepxion(exception, ErrorProcesandoPeticion);
        //    }
        //    return respuestaUsuario;
        //}

        public EMA_USUARIODto ObtenerCredencialesUsuario(string documento)
        {
            EMA_USUARIODto respuestaUsuario;
            try
            {
                var respuestaEntidad = _repositorioUsuarios.ObtenerCredencialesUsuario(documento);
                //respuestaUsuario = _adaptadorDeObjetos.Adaptar<EMA_USUARIODto>(respuestaEntidad);
                respuestaUsuario = Transversales.Administracion.Mappers.MapperUSRCentralADTO.AdaptarMenu(respuestaEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return respuestaUsuario;
        }

        ///// <summary>
        /// Valida si el usuario esta en las tablas de stone para permitir el registro en tuportal
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public EMA_USUARIODto ValidarPermisoRegistro(int usuario)
        {
            try
            {
                var resultadoEntidad = _repositorioUsuarios.ValidarPermisoRegistroPorDocumento(usuario);
                if (resultadoEntidad != null)
                {
                    return new EMA_USUARIODto
                    {
                        NOMBREUSUARIO = resultadoEntidad.Nombre,
                        ROL = resultadoEntidad.ROL
                    };
                }
                return null;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public string ObtenerCorreoUsr(string userName)
        {
            try
            {
                var resultadoCorreo = _repositorioUsuarios.ObtenerCorreoUsr(userName);

                return resultadoCorreo;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public Tuple<string, string, bool> RegistrarUsuario(RegistroReclutamiento reclutamiento)
        {
            try
            {
                var resultRegistro = _repositorioUsuarios.RegistrarUsuario(reclutamiento);

                return resultRegistro;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        #endregion IServicioAplicacionUsuarios Members
    }
}