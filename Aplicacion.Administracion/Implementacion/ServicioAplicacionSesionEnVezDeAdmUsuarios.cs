using Aplicacion.Administracion.Contratos;

//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionSesionEnVezDeAdmUsuarios : IServicioAplicacionSesionEnVezDeAdmUsuarios
    {
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioSesionEnVezDeAdmUsuarios _repositorioAdmUsuarios;

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        public ServicioAplicacionSesionEnVezDeAdmUsuarios(IRepositorioSesionEnVezDeAdmUsuarios repositorioAdmUsuarios, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioAdmUsuarios = repositorioAdmUsuarios;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        IEnumerable<SesionEnVezDeParametrosCrearUsuarioDto> IServicioAplicacionSesionEnVezDeAdmUsuarios.ObtenerParametros(decimal cod_usuario, int id)
        {
            try
            {
                List<SesionEnVezDeParametrosCrearUsuarioDto> listaDto = new List<SesionEnVezDeParametrosCrearUsuarioDto>();
                var parametros = _repositorioAdmUsuarios.ObtenerParametros(cod_usuario, id);
                return _adaptadorDeObjetos.Adaptar<IEnumerable<SesionEnVezDeParametrosCrearUsuarioDto>>(parametros);
            }
            catch (SystemException e)
            {
                throw LogExepxion(e, e.Message);
            }
        }

        public int CrearUsuario(SesionEnVezDeNuevoUsuarioDto usuario)
        {
            try
            {
                var us = new SesionEnVezDeNuevoUsuario()
                {
                    CodUsuario = usuario.CodUsuario,
                    Ciudad = usuario.Ciudad,
                    Canal = usuario.Canal,
                    Perfil = usuario.Perfil,
                    Segmento = usuario.Segmento
                };
                return _repositorioAdmUsuarios.CrearUsuario(us);
            }
            catch (SystemException e)
            {
                throw LogExepxion(e, e.Message);
            }
        }

        public IEnumerable<SesionEnVezDeUsuarioRecuperadoDto> ObtUsuarios()
        {
            try
            {
                IEnumerable<SesionEnVezDeUsuarioRecuperado> usuarios = _repositorioAdmUsuarios.ObtUsuarios();
                var usuariosDto = _adaptadorDeObjetos.Adaptar<IEnumerable<SesionEnVezDeUsuarioRecuperadoDto>>(usuarios);

                return usuariosDto;
            }
            catch (SystemException e)
            {
                throw LogExepxion(e, e.Message);
            }
        }

        public int ActEstadoUsuario(int id, char estado)
        {
            try
            {
                return _repositorioAdmUsuarios.ActEstadoUsuario(id, estado);
            }
            catch (SystemException e)
            {
                throw LogExepxion(e, e.Message);
            }
        }

        public int ActUsuario(SesionEnVezDeNuevoUsuarioDto usuario)
        {
            try
            {
                var us = new SesionEnVezDeNuevoUsuario()
                {
                    Id = usuario.Id,
                    CodUsuario = usuario.CodUsuario,
                    Ciudad = usuario.Ciudad,
                    Canal = usuario.Canal,
                    Perfil = usuario.Perfil,
                    Segmento = usuario.Segmento
                };
                return _repositorioAdmUsuarios.ActUsuario(us);
            }
            catch (SystemException e)
            {
                throw LogExepxion(e, e.Message);
            }
        }
    }
}