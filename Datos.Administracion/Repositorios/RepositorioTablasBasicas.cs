// ----------------------------------------------------------------------------------------------
// <copyright file="RepositorioTablasBasicas.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.ModelKheiron;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioTablasBasicas : IRepositorioTablasBase
    {
        #region IRepositorioTablasBase Members

        public IEnumerable<EMB_TIPO_IDENTIFICACION> ObtenerTipoDocumentos()
        {
            using (var modelo = new ContextoKheiron())
            {
                return modelo.EMB_TIPO_IDENTIFICACION.ToList();
            }
        }

        #endregion IRepositorioTablasBase Members
    }
}