// ----------------------------------------------------------------------------------------------
// <copyright file="RepositorioBanner.cs" company="Dynamic">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioBanner : IRepositorioBanner
    {
        #region IRepositorioBanner Members

        /// <summary>
        /// Agrega un nuevo banner
        /// </summary>
        /// <param name="banner">Banner</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        public void AgregarBanner(DYK_BANNER banner, string pathImagen, string pathImagenDestino)
        {
            using (var modelo = new ContextoPortal())
            {
                // Imagen
                string imageName = string.Empty;
                if (!string.IsNullOrWhiteSpace(pathImagen))
                {
                    imageName = Path.GetFileName(pathImagen);
                    File.Copy(pathImagen, pathImagenDestino + imageName);
                    File.Delete(pathImagen);

                    banner.IMAGEN = imageName;
                }

                modelo.DYK_BANNER.Add(banner);
                modelo.SaveChanges();
            }
        }

        /// <summary>
        /// Modifica la informción de un banner
        /// </summary>
        /// <param name="banner">Banner</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        public void ModificarBanner(DYK_BANNER banner, string pathImagen, string pathImagenDestino)
        {
            using (var modelo = new ContextoPortal())
            {
                var bannerEntidad = modelo.DYK_BANNER.FirstOrDefault(m => m.BANNERID == banner.BANNERID);

                // Imagen
                string imageName = string.Empty;
                if (!string.IsNullOrWhiteSpace(pathImagen))
                {
                    imageName = Path.GetFileName(pathImagen);
                    File.Copy(pathImagen, pathImagenDestino + imageName);
                    File.Delete(pathImagen);

                    bannerEntidad.IMAGEN = imageName;
                }

                bannerEntidad.URL = banner.URL;
                bannerEntidad.POSICION = banner.POSICION;
                modelo.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina un banner
        /// </summary>
        /// <param name="bannerId">Identificador del banner</param>
        public void EliminarBanner(Guid bannerId)
        {
            using (var modelo = new ContextoPortal())
            {
                var banner = modelo.DYK_BANNER.FirstOrDefault(u => u.BANNERID == bannerId);
                modelo.DYK_BANNER.Remove(banner);
                modelo.SaveChanges();
            }
        }

        /// <summary>
        /// Tarea un banner por el identificador
        /// </summary>
        /// <param name="bannerId">Identificador del banner</param>
        /// <returns></returns>
        public DYK_BANNER TraerBannerPorId(Guid bannerId)
        {
            using (var modelo = new ContextoPortal())
            {
                return modelo.DYK_BANNER.FirstOrDefault(u => u.BANNERID == bannerId);
            }
        }

        /// <summary>
        /// Tare la lista de banners
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DYK_BANNER> TraerBanners()
        {
            using (var modelo = new ContextoPortal())
            {
                var banners = modelo.DYK_BANNER.ToList();
                return banners;
            }
        }

        #endregion IRepositorioBanner Members
    }
}