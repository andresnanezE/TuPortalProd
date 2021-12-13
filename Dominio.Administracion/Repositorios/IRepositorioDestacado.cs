// ----------------------------------------------------------------------------------------------
// <copyright file="IRepositorioDestacado.cs" company="Dynamic">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades.ModeloPortal;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioDestacado
    {
        #region Instance Methods

        /// <summary>
        /// Modifica la lista de destacados
        /// </summary>
        /// <param name="destacados">Lista de descatacod</param>
        /// <param name="pathsImagenes">Losta de paths de las imagenes de cada destacado</param>
        /// <param name="pathImagenesDestino">Path de destino para la imagen</param>
        void ModificarDestacados(IEnumerable<DYK_DESTACADO> destacados, IEnumerable<string> pathsImagenes, string pathImagenesDestino);

        /// <summary>
        /// Trae la lista de destacados
        /// </summary>
        /// <returns></returns>
        IEnumerable<DYK_DESTACADO> TraerDestacados();

        #endregion Instance Methods
    }
}