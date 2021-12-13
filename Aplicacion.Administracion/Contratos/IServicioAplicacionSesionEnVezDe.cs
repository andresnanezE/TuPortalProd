// ----------------------------------------------------------------------------------------------
// <copyright file="IServicioAplicacionTerminos.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionSesionEnVezDe
    {
        #region Instance Methods

        string CargaTerminosCondiciones();

        IEnumerable<SesionEnVezDeDto> ValidarDocumento(string documento, string usuario);

        #endregion Instance Methods
    }
}