// ----------------------------------------------------------------------------------------------
// <copyright file="BaseSiteMapNode.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Aplicacion.Administracion.Contratos;
using MvcSiteMapProvider;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class BaseSiteMapNode : DynamicNodeProviderBase
    {
        #region Fields

        private IServicioAplicacionMenu _servicioAplicacionMenu;

        #endregion Fields

        #region Instance Properties

        private IServicioAplicacionMenu ServicioAplicacionMenu
        {
            get { return _servicioAplicacionMenu ?? (_servicioAplicacionMenu = FabricaIoC.Resolver<IServicioAplicacionMenu>()); }
        }

        #endregion Instance Properties

        #region Instance Methods

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            return ReturnAll();
        }

        private IEnumerable<DynamicNode> ReturnAll()
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var claimRol = identity.FindFirst(ClaimTypes.Role).Value;
            var roles = claimRol.Split(',');
            var listaRoles = (from role in roles where !string.IsNullOrWhiteSpace(role) select int.Parse(role)).ToList();
            var menuPrincipal = ServicioAplicacionMenu.ObtenerMenuRoles(listaRoles);

            var nodos = new List<DynamicNode>();
            foreach (var menuTurnos in menuPrincipal)
            {
                var nodo = new DynamicNode
                {
                    Title = menuTurnos.Descripcion,
                    Key = menuTurnos.MenuId.ToString(),
                    Controller = menuTurnos.Controller,
                    Action = menuTurnos.Action,
                    Url = menuTurnos.Url,
                    Description = string.IsNullOrWhiteSpace(menuTurnos.Icono) ? "glyphicon-ok" : menuTurnos.Icono
                };
                nodos.Add(nodo);

                foreach (var menuhijosTurnos in menuTurnos.MenuHijos)
                {
                    var nodoHijo = new DynamicNode
                    {
                        Title = menuhijosTurnos.DESCRIPCION,
                        ParentKey = menuTurnos.MenuId.ToString(),
                        Key = menuhijosTurnos.MENUID.ToString(),
                        Controller = menuhijosTurnos.CONTROLLER,
                        Action = menuhijosTurnos.ACTION,
                        Url = menuhijosTurnos.URL,
                        Description = string.IsNullOrWhiteSpace(menuTurnos.Icono) ? "glyphicon-ok" : menuhijosTurnos.ICONO
                    };
                    nodos.Add(nodoHijo);
                }
            }

            return nodos;
        }

        #endregion Instance Methods
    }
}