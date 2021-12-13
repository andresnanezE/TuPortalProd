// ----------------------------------------------------------------------------------------------
// <copyright file="RepositorioUsuarios.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Datos.Administracion</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data;
using System.Configuration;
using System.Collections;
using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.ModeloProcesos;
using Dominio.Administracion.Entidades.ModelKheiron;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        #region IRepositorioUsuarios Members

        public EMA_USUARIO AgregarUsuario(EMA_USUARIO usuario)
        {
            using (var modelo = new ContextoPortal())
            {
                modelo.EMA_USUARIO.Add(usuario);
                modelo.SaveChanges();
                return usuario;
            }
        }

        public void EliminarUsuario(int usuarioId)
        {
            using (var modelo = new ContextoPortal())
            {
                var usuario = modelo.EMA_USUARIO.FirstOrDefault(u => u.USUARIOID == usuarioId);
                modelo.EMA_USUARIO.Remove(usuario);
                modelo.SaveChanges();
            }
        }

        public void ModificarUsuario(EMA_USUARIO usuarioModificar)
        {
            using (var modelo = new ContextoPortal())
            {
                var usuario = modelo.EMA_USUARIO.FirstOrDefault(u => u.USUARIOID == usuarioModificar.USUARIOID);
                if (usuario == null)
                {
                    return;
                }
                usuario.NOMBREUSUARIO = usuarioModificar.NOMBREUSUARIO;
                usuario.CORREO = usuarioModificar.CORREO;
                usuario.ACTIVO = usuarioModificar.ACTIVO;
                usuario.USUARIOMODIFICACION = usuarioModificar.USUARIOMODIFICACION;
                usuario.FECHAMODIFICACION = DateTime.Now;
                modelo.SaveChanges();
            }
        }


        public EMA_USUARIO ObtenerUsuarioDocumento(string documento)
        {
            using (var modelo = new ContextoPortal())
            {
                return modelo.EMA_USUARIO.FirstOrDefault(u => u.DOCUMENTO == documento);
            }
        }

        public UsuarioExterno ObtenerUsuarioExterno(string tipoDocumento, string documento)
        {
            using (var modelo = new ContextoKheiron())
            {
                var persona = modelo.EMB_PERSONA.FirstOrDefault(p => p.COD_FUNC_HOMOLOG == documento);

                if (persona == null)
                {
                    return null;
                }

                var usuario = modelo.EMB_USUARIO.FirstOrDefault(u => u.USER_NAME == persona.USUARIO);

                if (usuario == null)
                {
                    return null;
                }

                var usuarioExterno = new UsuarioExterno
                {
                    TipoDocumento = persona.ID_TIPO_IDENTIFICACION,
                    Documento = persona.COD_FUNC_HOMOLOG,
                    Usuario = usuario.USER_NAME,
                    Contrasena = usuario.CONTRASENA_KII,
                    NombreUsuario = usuario.NOMBRE_COMPLETO
                };
                return usuarioExterno;
            }
        }


        public IEnumerable<EMA_USUARIO> ObtenerUsuarioFiltros(string usuario, string nombreUsuario, string tipoDocumento, string documento, string correo)
        {
            using (var modelo = new ContextoPortal())
            {
                var listadoUsuarios = new List<EMA_USUARIO>();

                if (!string.IsNullOrWhiteSpace(usuario))
                {
                    listadoUsuarios = modelo.EMA_USUARIO.Where(u => u.USUARIO.Contains(usuario)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(nombreUsuario))
                {
                    listadoUsuarios = listadoUsuarios.Any() ? listadoUsuarios.Where(u => nombreUsuario.Contains(nombreUsuario)).ToList() : modelo.EMA_USUARIO.Where(u => u.NOMBREUSUARIO.Contains(nombreUsuario)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(documento))
                {
                    listadoUsuarios = listadoUsuarios.Any() ? listadoUsuarios.Where(u => u.TIPODOCUMENTO == tipoDocumento && u.DOCUMENTO == documento).ToList() : modelo.EMA_USUARIO.Where(u => u.TIPODOCUMENTO == tipoDocumento && u.DOCUMENTO == documento).ToList();
                }

                if (!string.IsNullOrWhiteSpace(correo))
                {
                    listadoUsuarios = listadoUsuarios.Any() ? listadoUsuarios.Where(u => correo.Contains(correo)).ToList() : modelo.EMA_USUARIO.Where(u => u.CORREO.Contains(correo)).ToList();
                }

                return listadoUsuarios;
            }
        }

        public EMA_USUARIO ObtenerUsuarioId(int usuarioId)
        {
            using (var modelo = new ContextoPortal())
            {
                var usuario = modelo.EMA_USUARIO.FirstOrDefault(u => u.USUARIOID == usuarioId);
                return usuario;
            }
        }

        public EMA_USUARIO ObtenerUsuarioUserName(string usuario)
        {
            using (var modelo = new ContextoPortal())
            {
                return modelo.EMA_USUARIO.FirstOrDefault(u => u.USUARIO == usuario);
            }
        }

        public IEnumerable<EMA_USUARIO> ObtenerUsuarios(int usuarioId)
        {
            using (var modelo = new ContextoPortal())
            {
                var usuarios = usuarioId > 0 ? modelo.EMA_USUARIO.Where(u => u.USUARIOID == usuarioId).ToList() : modelo.EMA_USUARIO.ToList();

                return usuarios;
            }
        }

        public EMA_USUARIO ValidarUsuario(string usuario, string contrasena)
        {
            string usuValido = null;

            using (var modelo2 = new ContextoKheiron()) 
            {
                usuValido = modelo2.EMB_USUARIO.FirstOrDefault(m => m.CONTRASENA_KII == contrasena && m.USER_NAME == usuario).USER_NAME;

            }
            var entidadUsuario = new EMA_USUARIO();
        
            using (var modelo = new ContextoPortal())
            {
                entidadUsuario = modelo.EMA_USUARIO.FirstOrDefault(m => m.USUARIO == usuValido && m.ACTIVO);
                if (entidadUsuario != null)
                {
                    entidadUsuario.FECHAULTIMASESION = DateTime.Now;
                    modelo.SaveChanges();
                }
                return entidadUsuario;
            }
        }

        #endregion
    }
}
