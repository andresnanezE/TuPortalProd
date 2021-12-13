// ----------------------------------------------------------------------------------------------
// <copyright file="IServicioAplicacionMenu.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionDestacado
    {
        #region Instance Methods

        /// <summary>
        /// Modifica la lista de destacados
        /// </summary>
        /// <param name="destacados">Lista de descatacod</param>
        /// <param name="pathsImagenes">Losta de paths de las imagenes de cada destacado</param>
        /// <param name="pathImagenesDestino">Path de destino para la imagen</param>
        void ModificarDestacados(IEnumerable<DYK_DESTACADODto> destacados, IEnumerable<string> pathsImagenes, string pathImagenesDestino);

        /// <summary>
        /// Trae la lista de destacados
        /// </summary>
        /// <returns></returns>
        IEnumerable<DYK_DESTACADODto> TraerDestacados();

        #endregion Instance Methods
    }
}