// ----------------------------------------------------------------------------------------------
// <copyright file="IRepositorioBanner.cs" company="Dynamic">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades.ModeloPortal;
using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioBanner
    {
        #region Instance Methods

        /// <summary>
        /// Agrega un nuevo banner
        /// </summary>
        /// <param name="banner">Banner</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        void AgregarBanner(DYK_BANNER banner, string pathImagen, string pathImagenDestino);

        /// <summary>
        /// Modifica la informción de un banner
        /// </summary>
        /// <param name="banner">Banner</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        void ModificarBanner(DYK_BANNER banner, string pathImagen, string pathImagenDestino);

        /// <summary>
        /// Elimina un banner
        /// </summary>
        /// <param name="bannerId">Identificador del banner</param>
        void EliminarBanner(Guid bannerId);

        /// <summary>
        /// Tarea un banner por el identificador
        /// </summary>
        /// <param name="bannerId">Identificador del banner</param>
        /// <returns></returns>
        DYK_BANNER TraerBannerPorId(Guid bannerId);

        /// <summary>
        /// Tare la lista de banners
        /// </summary>
        /// <returns></returns>
        IEnumerable<DYK_BANNER> TraerBanners();

        #endregion Instance Methods
    }
}