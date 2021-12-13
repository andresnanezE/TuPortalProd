// ----------------------------------------------------------------------------------------------
// <copyright file="ServicioAplicacionRoles.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionRoles : IServicioAplicacionRoles
    {
        #region Constants

        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";

        #endregion Constants

        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioRoles _repositorioRoles;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionRoles(IRepositorioRoles repositorioRoles, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioRoles = repositorioRoles;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
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

        #region IServicioAplicacionRoles Members

        public int AgregarRol(EMA_ROLDto rol)
        {
            try
            {
                var rolEntidad = _adaptadorDeObjetos.Adaptar<Dominio.Administracion.Entidades.ModeloCentralizada.Roles>(rol);
                return _repositorioRoles.AgregarRol(rolEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public void AgregarRolUsuario(IEnumerable<int> rolIds, int usuarioId)
        {
            try
            {
                _repositorioRoles.AgregarRolUsuario(rolIds, usuarioId);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public void ModificarRol(EMA_ROLDto rol)
        {
            try
            {
                var rolEntidad = _adaptadorDeObjetos.Adaptar<Dominio.Administracion.Entidades.ModeloCentralizada.Roles>(rol);
                _repositorioRoles.ModificarRol(rolEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public EMA_ROLDto ObtenerRolId(int rolId)
        {
            EMA_ROLDto rolTurno;
            try
            {
                var rolEntidad = _repositorioRoles.ObtenerRolId(rolId);
                rolTurno = _adaptadorDeObjetos.Adaptar<EMA_ROLDto>(rolEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return rolTurno;
        }

        public IEnumerable<EMA_ROLXMENUDto> ObtenerRolMenu(int menuId)
        {
            IEnumerable<EMA_ROLXMENUDto> listadoRolMenu;
            try
            {
                var listadorolEntidad = _repositorioRoles.ObtenerRolMenu(menuId);
                listadoRolMenu = _adaptadorDeObjetos.Adaptar<IEnumerable<EMA_ROLXMENUDto>>(listadorolEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return listadoRolMenu;
        }

        public IEnumerable<EMA_ROLXUSUARIODto> ObtenerRolUsuario(int usuarioId)
        {
            IEnumerable<EMA_ROLXUSUARIODto> listadoRolUsuario;
            try
            {
                var listadoRolEntidad = _repositorioRoles.ObtenerRolUsuario(usuarioId);
                listadoRolUsuario = _adaptadorDeObjetos.Adaptar<IEnumerable<EMA_ROLXUSUARIODto>>(listadoRolEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return listadoRolUsuario;
        }

        public IEnumerable<EMA_ROLDto> ObtenerRoles()
        {
            IEnumerable<EMA_ROLDto> listadoRoles;
            try
            {
                var listadoRolesEntidad = _repositorioRoles.ObtenerRoles();
                listadoRoles = _adaptadorDeObjetos.Adaptar<IEnumerable<EMA_ROLDto>>(listadoRolesEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return listadoRoles;
        }

        public IEnumerable<EMA_ROLDto> ObtenerRolesActivos()
        {
            IEnumerable<EMA_ROLDto> listaRoles;
            try
            {
                var listadoRolesEntidad = _repositorioRoles.ObtenerRolesActivos();
                listaRoles = _adaptadorDeObjetos.Adaptar<IEnumerable<EMA_ROLDto>>(listadoRolesEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return listaRoles;
        }

        public IEnumerable<EMA_ROLDto> ObtenerRolesFiltros(int rolId, string rol)
        {
            try
            {
                var listadoFiltrosEntidad = _repositorioRoles.ObtenerRolesFiltros(rolId, rol);
                return _adaptadorDeObjetos.Adaptar<IEnumerable<EMA_ROLDto>>(listadoFiltrosEntidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        #endregion IServicioAplicacionRoles Members
    }
}