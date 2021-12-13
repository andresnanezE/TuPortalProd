using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioSesionEnVezDeAdmUsuarios : IRepositorioSesionEnVezDeAdmUsuarios
    {
        public IEnumerable<SesionEnVezDeParametrosCrearUsuario> ObtenerParametros(decimal cod_usuario, int id)
        {
            using (var modelo = new ContextoPortal())
            {
                return modelo.ConsultaSql<SesionEnVezDeParametrosCrearUsuario>("EMESP_SESIONENVEZDE_PARAMS @COD_USUARIO, @ID",
                    new SqlParameter("@COD_USUARIO", cod_usuario),
                    new SqlParameter("@ID", id)
                    ).ToList();
            }
        }

        public int CrearUsuario(SesionEnVezDeNuevoUsuario usuario)
        {
            using (var modelo = new ContextoPortal())
            {
                var registrosMod = modelo.ConsultaSql<SesionEnVezDeNuevoUsuario>(@"EMESP_SESIONENVEZDE_ADD_USUARIOS
                                                                                        @COD_USUARIO,
                                                                                        @CIUDAD,
                                                                                        @CANAL,
                                                                                        @PERFIL, @SEGMENTO",
                    new SqlParameter("@COD_USUARIO", usuario.CodUsuario),
                    new SqlParameter("@CIUDAD", usuario.Ciudad),
                    new SqlParameter("@CANAL", usuario.Canal),
                    new SqlParameter("@PERFIL", usuario.Perfil),
                    new SqlParameter("@SEGMENTO", usuario.Segmento)
                    ).ToList();

                return registrosMod.First().registrosMod;
            }
        }

        public IEnumerable<SesionEnVezDeUsuarioRecuperado> ObtUsuarios()
        {
            using (var modelo = new ContextoPortal())
            {
                var lst = modelo.ConsultaSql<SesionEnVezDeUsuarioRecuperado>(@"EMESP_SESIONENVEZDE_USUARIOS_TODOS").ToList();

                return lst;
            }
        }

        public int ActEstadoUsuario(int id, char estado)
        {
            using (var modelo = new ContextoPortal())
            {
                var lst = modelo.ConsultaSql<SesionEnVezDeUsuarioRecuperado>(@"EMESP_SESIONENVEZDE_ACT_USUARIO @ID_REG, @ESTADO",
                    new SqlParameter("@ID_REG", id),
                    new SqlParameter("@ESTADO", estado)
                    ).ToList();

                return lst.Single().registrosMod;
            }
        }

        public int ActUsuario(SesionEnVezDeNuevoUsuario usuario)
        {
            using (var modelo = new ContextoPortal())
            {
                var registrosMod = modelo.ConsultaSql<SesionEnVezDeNuevoUsuario>(@"EMESP_SESIONENVEZDE_ACT_USUARIO
                                                                                        @ID_REG,
                                                                                        @ESTADO,
                                                                                        @CIUDAD,
                                                                                        @CANAL,
                                                                                        @PERFIL,@SEGMENTO",
                    new SqlParameter("@ID_REG", usuario.Id),
                    new SqlParameter("@ESTADO", ""),
                    new SqlParameter("@CIUDAD", usuario.Ciudad),
                    new SqlParameter("@CANAL", usuario.Canal),
                    new SqlParameter("@PERFIL", usuario.Perfil),
                    new SqlParameter("@SEGMENTO", usuario.Segmento)
                    ).ToList();

                return registrosMod.First().registrosMod;
            }
        }
    }
}