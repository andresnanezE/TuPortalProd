// ----------------------------------------------------------------------------------------------
// <copyright file="RepositorioUsuarios.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Datos.Administracion</project>
// ----------------------------------------------------------------------------------------------

using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloCentralizada;
using Dominio.Administracion.Entidades.Reclutamiento;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        #region IRepositorioUsuarios Members

        //public EMA_USUARIO AgregarUsuario(EMA_USUARIO usuario)
        //{
        //    using (var modelo = new ContextoProcesos())
        //    {
        //        modelo.EMA_USUARIO.Add(usuario);
        //        modelo.SaveChanges();
        //        return usuario;
        //    }
        //}

        //public void EliminarUsuario(int usuarioId)
        //{
        //    using (var modelo = new ContextoProcesos())
        //    {
        //        var usuario = modelo.EMA_USUARIO.FirstOrDefault(u => u.USUARIOID == usuarioId);
        //        modelo.EMA_USUARIO.Remove(usuario);
        //        modelo.SaveChanges();
        //    }
        //}

        //public void ModificarUsuario(Dominio.Administracion.Entidades.ModeloUsuariosCentralizado.Usuarios usuarioModificar)
        //{
        //    using (var modelo = new ContextoProcesos())
        //    {
        //        var usuario = modelo.EMA_USUARIO.FirstOrDefault(u => u.USUARIOID == usuarioModificar.id_usua);
        //        if (usuario == null)
        //        {
        //            return;
        //        }
        //        usuario.NOMBREUSUARIO = usuarioModificar.nom_usua;
        //        usuario.CORREO = usuarioModificar.correo;
        //        usuario.ACTIVO = usuarioModificar.activo;
        //        //usuario.USUARIOMODIFICACION = usuarioModificar.USUARIOMODIFICACION;
        //        usuario.FECHAMODIFICACION = DateTime.Now;
        //        modelo.SaveChanges();
        //    }
        //}

        public usrCentral ObtenerUsuarioDocumento(int documento)
        {
            using (var modelo = new ContextoUsuarios())
            {
                return modelo.usrCentral.FirstOrDefault(u => u.num_doc.Equals(documento));
            }
        }

        //public UsuarioExterno ObtenerUsuarioExterno(string tipoDocumento, int documento)
        //{
        //    Usuarios usuario = null;
        //    RolUsuario rol = null;
        //    Rol rolNombre = null;
        //    using (var modelo = new ContextoUsuarios())
        //    {
        //        usuario = modelo.Usuarios.FirstOrDefault(p => p.num_doc.ToString().Equals(documento));
        //        if (usuario == null)
        //        {
        //            return null;
        //        }
        //        rol = modelo.RolUsuario.FirstOrDefault(r => r.IdUsuario == usuario.IdUsuario);
        //        rolNombre = modelo.Rol.FirstOrDefault(r => r.IdRol == rol.IdRol);
        //    }
        //    EMA_ROL rolAplicacion = null;
        //    using (var modelo2 = new ContextoProcesos())
        //    {
        //        rolAplicacion = modelo2.EMA_ROL.FirstOrDefault(r => r.ROL.Contains(rolNombre.Nombre));
        //    }
        //    string documentoAplicacion = "";
        //    if (rolNombre.Nombre.Contains("Admin"))
        //    {
        //        documentoAplicacion = "";
        //    }
        //    else if (rolNombre.Nombre.Contains("Direc"))
        //    {
        //        documentoAplicacion = usuario.DirectorId.ToString();
        //    }
        //    else if (rolNombre.Nombre.Contains("Ases"))
        //    {
        //        documentoAplicacion = usuario.AsesorId.ToString();
        //    }
        //    var usuarioExterno = new UsuarioExterno
        //    {
        //        TipoDocumento = "1",
        //        Documento = documentoAplicacion,
        //        Usuario = usuario.Login,
        //        Contrasena = usuario.Password,
        //        NombreUsuario = usuario.Login,
        //        Rol = rolAplicacion.ROLID,
        //        Email = usuario.Email
        //    };
        //    return usuarioExterno;
        //}

        public IEnumerable<usrCentral> ObtenerUsuarioFiltros(string usuario, string nombreUsuario, string tipoDocumento, string documento, string correo)
        {
            using (var modelo = new ContextoUsuarios())
            {
                var listadoUsuarios = new List<usrCentral>();

                if (!string.IsNullOrWhiteSpace(usuario))
                {
                    //Obtiene todos los usuarios que contienen el valor usuario
                    listadoUsuarios = modelo.usrCentral.Where(u => u.log_usr.Contains(usuario)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(nombreUsuario))
                {
                    listadoUsuarios = listadoUsuarios.Any() ?
                        listadoUsuarios.Where(u => nombreUsuario.Contains(nombreUsuario)).ToList() :
                        modelo.usrCentral.Where(u => u.nom_usr.Contains(nombreUsuario)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(documento))
                {
                    listadoUsuarios = listadoUsuarios.Any() ?
                        listadoUsuarios.Where(u => u.tip_doc == tipoDocumento && u.num_doc == documento).ToList() :
                        modelo.usrCentral.Where(u => u.tip_doc == tipoDocumento && u.num_doc == documento).ToList();
                }

                if (!string.IsNullOrWhiteSpace(correo))
                {
                    listadoUsuarios = listadoUsuarios.Any() ?
                        listadoUsuarios.Where(u => correo.Contains(correo)).ToList() :
                        modelo.usrCentral.Where(u => u.correo.Contains(correo)).ToList();
                }

                return listadoUsuarios;
            }
        }

        public usrCentral ObtenerUsuarioId(int usuarioId)
        {
            using (var modelo = new ContextoUsuarios())
            {
                var usuario = modelo.usrCentral.FirstOrDefault(u => u.id_usr == usuarioId);
                return usuario;
            }
        }

        public usrCentral ObtenerUsuarioUserName(string usuario)
        {
            using (var modelo = new ContextoUsuarios())
            {
                var usrBD = (from u in modelo.usrCentral select u).Where(x => x.log_usr == usuario).FirstOrDefault();
                return usrBD;
                //return modelo.usrCentral.FirstOrDefault(u => u.log_usr == usuario);
            }
        }

        public string ObtenerCorreoUsr(string userName)
        {
            using (var bd = new ContextoUsuarios())
            {
                var correoUsr = (from u in bd.usrCentral
                                 where u.log_usr == userName
                                 select u.correo).First();

                return correoUsr;
            }
        }

        public IEnumerable<usrCentral> ObtenerUsuarios(int usuarioId)
        {
            using (var modelo = new ContextoUsuarios())
            {
                var usuarios = usuarioId > 0 ? modelo.usrCentral.Where(u => u.id_usr == usuarioId).ToList() : modelo.usrCentral.ToList();

                return usuarios;
            }
        }

        //public EMA_USUARIO ValidarUsuario(string usuario, string contrasena)
        //{
        //    string usuValido = null;

        //    using (var modelo2 = new ContextoCotizador())
        //    {
        //        var ret = modelo2.Usuario.FirstOrDefault(m => m.Password == contrasena && m.Login == usuario);
        //        if (ret != null)
        //        {
        //            usuValido = ret.Login;
        //        }
        //    }
        //    var entidadUsuario = new EMA_USUARIO();

        //    using (var modelo = new ContextoProcesos())
        //    {
        //        entidadUsuario = modelo.EMA_USUARIO.FirstOrDefault(m => m.USUARIO == usuValido);
        //        if (entidadUsuario != null)
        //        {
        //            entidadUsuario.FECHAULTIMASESION = DateTime.Now;
        //            modelo.SaveChanges();
        //        }

        //    }

        //    if (entidadUsuario != null)
        //    {
        //        using (var modeloStone = new ContextoStone())
        //        {
        //            var docu = decimal.Parse(entidadUsuario.DOCUMENTO);
        //            var entidadGN_TERCE =
        //                modeloStone.GN_TERCE.FirstOrDefault(m => m.COD_TERC == docu);
        //            if (entidadGN_TERCE != null)
        //            {
        //                entidadUsuario.NOMBREUSUARIO = entidadGN_TERCE.NOM_COMP;
        //            }
        //        }
        //    }

        //    return entidadUsuario;
        //}

        public usrCentral ObtenerCredencialesUsuario(string documento)
        {
            usrCentral usuario = null;
            //UsuarioXappXrol rol = null;
            //Roles rolNombre = null;
            using (var modelo = new ContextoUsuarios())
            {
                usuario = modelo.usrCentral.FirstOrDefault(p => p.num_doc.Equals(documento));
                if (usuario == null)
                {
                    return null;
                }
                //rol = modelo.UsuarioXappXrol.FirstOrDefault(r => (r.id_usuario == usuario.id_usua && r.id_aplicacion == 1));
                //rolNombre = modelo.Roles.FirstOrDefault(r => r.id_rol == rol.id_rol);
            }
            return usuario;
        }

        /// <summary>
        /// Valida si el usuario Tiene permitido el registro ppor estar en tablas de stone
        /// </summary>
        /// <param name="documento">cedula del usuario</param>
        /// <returns>booleano indicando si tiene o no permitido registrarse</returns>
        public EMSP_ValidarRegistro ValidarPermisoRegistroPorDocumento(int documento)
        {
            using (var bd = new ContextoPortal())
            {
                EMSP_ValidarRegistro res = new EMSP_ValidarRegistro();

                using (var contexto = new ContextoPortal())
                {
                    return contexto.Database.SqlQuery<EMSP_ValidarRegistro>(
                           "EMSP_ValidarRegistro @DOCUMENTO"
                           , new SqlParameter("@DOCUMENTO", documento)
                           ).FirstOrDefault();
                }
            }
        }

        public Tuple<string, string, bool> RegistrarUsuario(RegistroReclutamiento reclutamiento)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<string>("SCISP_RegistrarNuevoReclutamiento @Nombres, @Apellidos, @NumeroDocumento, @Correo, @Telefono, @IdReclutador, @IdDirector, @IdTipoIdentificacion, @TerminosCondiciones, @Ip"
                            , new SqlParameter("@Nombres", reclutamiento.Nombres)
                            , new SqlParameter("@Apellidos", reclutamiento.Apellidos)
                            , new SqlParameter("@NumeroDocumento", reclutamiento.NumeroCedula)
                            , new SqlParameter("@Correo", reclutamiento.Correo)
                            , new SqlParameter("@Telefono", reclutamiento.Telefono)
                            , new SqlParameter("@IdReclutador", reclutamiento.IdReclutador)
                            , new SqlParameter("@IdDirector", reclutamiento.IdDirector)
                            , new SqlParameter("@IdTipoIdentificacion", reclutamiento.IdTipoIdentificacion)
                            , new SqlParameter("@TerminosCondiciones", reclutamiento.TerminosCondiciones)
                            , new SqlParameter("@Ip", reclutamiento.Ip)).FirstOrDefault();
                        
                    var s_result = result.Split('_');

                    bool boolValue = int.Parse(s_result[2]) != 0;
                    return new Tuple<string, string, bool>(s_result[0], s_result[1], boolValue);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion IRepositorioUsuarios Members
    }
}