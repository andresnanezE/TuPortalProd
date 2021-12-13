// ----------------------------------------------------------------------------------------------
// <copyright file="RepositorioRoles.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.ModeloCentralizada;
using Dominio.Administracion.Entidades.ModeloProcesos;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioRoles : IRepositorioRoles
    {
        #region IRepositorioRoles Members

        public int AgregarRol(Roles rol)
        {
            using (var modelo = new ContextoUsuarios())
            {
                modelo.Roles.Add(rol);
                modelo.SaveChanges();
                return rol.id_rol;
            }
        }

        public void AgregarRolMenu(IEnumerable<int> rolesIds, int menuId)
        {
            using (var modelo = new ContextoUsuarios())
            {
                foreach (var rolMenu in rolesIds.Select(rolId => new EMA_ROLXMENU
                {
                    ROLID = rolId,
                    MENUID = menuId
                }))
                {
                    modelo.EMA_ROLXMENU.Add(rolMenu);
                }
                modelo.SaveChanges();
            }
        }

        public void AgregarRolUsuario(IEnumerable<int> rolesIds, int usuarioId)
        {
            using (var modelo = new ContextoUsuarios())
            {
                foreach (var rolUsuario in rolesIds.Select(rolId => new USR_APL_ROL
                {
                    id_rol = rolId,
                    id_usr = usuarioId,
                    id_apl = 1
                }))
                {
                    modelo.UsuarioXappXrol.Add(rolUsuario);
                }
                modelo.SaveChanges();
            }
        }

        public void ModificarRol(Roles rol)
        {
            using (var modelo = new ContextoUsuarios())
            {
                var rolEntidad = modelo.Roles.Where(r => r.id_rol.Equals(rol.id_rol) && r.id_apl.Equals(1)).FirstOrDefault();
                rolEntidad.nom_rol = rol.nom_rol;
                rolEntidad.activo = rol.activo;
                modelo.SaveChanges();
            }
        }

        public void ModificarRolesMenu(IEnumerable<int> rolesIds, int menuId)
        {
            using (var model = new ContextoUsuarios())
            {
                var rolesMenu = model.EMA_ROLXMENU.Where(ru => ru.MENUID == menuId).ToList();
                var rolesRemove = rolesMenu.Where(ru => !rolesIds.Contains(ru.ROLID));
                model.EMA_ROLXMENU.RemoveRange(rolesRemove);
                foreach (var rolMenuEntidad in from rolId in rolesIds
                                               where rolesMenu.All(ru => ru.ROLID != rolId)
                                               select new EMA_ROLXMENU()
                                               {
                                                   ROLID = rolId,
                                                   MENUID = menuId
                                               })
                {
                    model.EMA_ROLXMENU.Add(rolMenuEntidad);
                }

                model.SaveChanges();
            }
        }

        public void ModificarRolesUsuario(IEnumerable<int> rolesIds, int usuarioId)
        {
            using (var model = new ContextoUsuarios())
            {
                var rolesUsuario = model.UsuarioXappXrol.Where(ru => ru.id_usr == usuarioId && ru.id_apl.Equals(1)).ToList();
                var rolesRemove = rolesUsuario.Where(ru => !rolesIds.Contains(ru.id_rol));
                model.UsuarioXappXrol.RemoveRange(rolesRemove);

                var lst = (from rolId in rolesIds
                           where rolesUsuario.All(ru => ru.id_rol != rolId)
                           select new USR_APL_ROL
                           {
                               id_rol = rolId,
                               id_usr = usuarioId,
                               id_apl = 1
                           });
                foreach (var rolUsuarioEntidad in lst)
                {
                    model.UsuarioXappXrol.Add(rolUsuarioEntidad);
                }

                model.SaveChanges();
            }
        }

        public Roles ObtenerRolId(int rolId)
        {
            using (var modelo = new ContextoUsuarios())
            {
                var rol = modelo.Roles.FirstOrDefault(r => r.id_rol == rolId && r.id_apl.Equals(1));
                return rol;
            }
        }

        public IEnumerable<EMA_ROLXMENU> ObtenerRolMenu(int menuId)
        {
            using (var modelo = new ContextoUsuarios())
            {
                var listadoRolesMenu = modelo.EMA_ROLXMENU.Where(ru => ru.MENUID == menuId).ToList();
                return listadoRolesMenu;
            }
        }

        public IEnumerable<USR_APL_ROL> ObtenerRolUsuario(int usuarioId)
        {
            using (var modelo = new ContextoUsuarios())
            {
                var roles = modelo.UsuarioXappXrol.Where(x => (x.id_usr.Equals(usuarioId) && x.id_apl.Equals(1))).ToList();
                return roles;
            }
        }

        public IEnumerable<Roles> ObtenerRoles()
        {
            using (var modelo = new ContextoUsuarios())
            {
                var listadoRoles = modelo.Roles.Where(e => e.id_apl.Equals(1)).ToList();
                return listadoRoles;
            }
        }

        public IEnumerable<Roles> ObtenerRolesActivos()
        {
            using (var modelo = new ContextoUsuarios())
            {
                var listadoRoles = modelo.Roles.Where(r => r.activo && r.id_apl.Equals(1)).ToList();
                return listadoRoles;
            }
        }

        public IEnumerable<Roles> ObtenerRolesFiltros(int rolId, string rol)
        {
            using (var modelo = new ContextoUsuarios())
            {
                var listadoRoles = new List<Roles>();
                if (rolId > 0)
                {
                    listadoRoles = modelo.Roles.Where(r => r.id_rol == rolId && r.id_apl.Equals(1)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(rol))
                {
                    listadoRoles = listadoRoles.Any() ?
                        listadoRoles.Where(r => r.nom_rol.Contains(rol)).ToList() :
                        modelo.Roles.Where(r => r.nom_rol.Contains(rol) && r.id_apl.Equals(1)).ToList();
                }

                if (rolId == 0 && string.IsNullOrWhiteSpace(rol))
                {
                    listadoRoles = modelo.Roles.Where(r => r.id_apl.Equals(1)).ToList();
                }

                return listadoRoles;
            }
        }

        #endregion IRepositorioRoles Members
    }
}