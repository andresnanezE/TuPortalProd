// ----------------------------------------------------------------------------------------------
// <copyright file="ServicioAplicacionMenu.cs" company="SCI Software">
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
using Dominio.Administracion.Entidades.ModeloProcesos;
using Dominio.Administracion.Repositorios;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionMenu : IServicioAplicacionMenu
    {
        #region Constants

        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";

        #endregion

        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioMenu _repositorioMenu;
        private readonly IRepositorioRoles _repositorioRoles;

        #endregion

        #region C'tors

        public ServicioAplicacionMenu(IRepositorioMenu repositorioMenu, IRepositorioLogs manejadorLogs, IRepositorioRoles repositorioRoles,IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioMenu = repositorioMenu;
            _manejadorLogs = manejadorLogs;
            _repositorioRoles = repositorioRoles;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion

        #region Instance Methods

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion, null);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }


        #endregion

        #region IServicioAplicacionMenu Members

        public int AgregarMenu(EMA_MENUDto menu, IEnumerable<int> rolesIds)
        {
            try
            {
                var menuEntidad = _adaptadorDeObjetos.Adaptar<EMA_MENU>(menu);
                var menuId = _repositorioMenu.AgregarMenu(menuEntidad);
                if (menuId > 0)
                {
                    _repositorioRoles.AgregarRolMenu(rolesIds, menuId);
                }
                return menuId;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public void ModificarMenu(EMA_MENUDto menu, IEnumerable<int> rolesIds)
        {
            try
            {
                var menuEntidad = _adaptadorDeObjetos.Adaptar<EMA_MENU>(menu);
                _repositorioMenu.ModificarMenu(menuEntidad);
                if (rolesIds != null)
                {
                    _repositorioRoles.ModificarRolesMenu(rolesIds, menu.MENUID);
                }
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<MenuAplicacionDto> ObtenerMenu()
        {
            try
            {
                var entidad = _repositorioMenu.ObtenerMenu();
                var dto = _adaptadorDeObjetos.Adaptar<IEnumerable<MenuAplicacionDto>>(entidad);
                return dto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<MenuAplicacionDto> ObtenerMenuFiltros(int menuId, string descripcion, int? nodoPadreId, int rolId)
        {
            try
            {
                var entidad = _repositorioMenu.ObtenerMenuFiltros(menuId, descripcion, nodoPadreId, rolId);
                var dto = _adaptadorDeObjetos.Adaptar<IEnumerable<MenuAplicacionDto>>(entidad);
                return dto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<EMA_MENUDto> ObtenerMenuPadres()
        {
            IEnumerable<EMA_MENUDto> menuturnos;
            try
            {
                var entidad = _repositorioMenu.ObtenerMenuPadres();
                menuturnos = _adaptadorDeObjetos.Adaptar<IEnumerable<EMA_MENUDto>>(entidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return menuturnos;
        }

        public IEnumerable<MenuAplicacionDto> ObtenerMenuRoles(List<int> rolesIds)
        {
            IEnumerable<MenuAplicacionDto> menuturnos;
            try
            {
                var entidad = _repositorioMenu.ObtenerMenuRoles(rolesIds);
                menuturnos = _adaptadorDeObjetos.Adaptar<IEnumerable<MenuAplicacionDto>>(entidad);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
            return menuturnos;
        }

        #endregion
    }
}
