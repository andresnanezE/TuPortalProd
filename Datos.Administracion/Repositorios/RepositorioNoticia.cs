// ----------------------------------------------------------------------------------------------
// <copyright file="RepositorioNoticia.cs" company="Dynamic">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Utilidades.Extensions;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioNoticia : IRepositorioNoticia
    {
        #region IRepositorioNoticia Members

        /// <summary>
        /// Agrega una noticia
        /// </summary>
        /// <param name="noticia">Noticia</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        public void AgregarNoticia(DYK_NOTICIA noticia, string pathImagen, string pathImagenDestino, string pathBanner, string pathBannerDestino)
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

                    noticia.IMAGEN = imageName;
                }

                // Banner
                string bannerName = string.Empty;
                if (!string.IsNullOrWhiteSpace(pathBanner))
                {
                    bannerName = Path.GetFileName(pathBanner);
                    File.Copy(pathBanner, pathBannerDestino + bannerName);
                    File.Delete(pathBanner);

                    noticia.BANNER = bannerName;
                }

                noticia.TITULOQS = StringExtension.GenerateQueryString(noticia.TITULO);
                modelo.DYK_NOTICIA.Add(noticia);
                modelo.SaveChanges();
            }
        }

        /// <summary>
        /// Modifica una imagen
        /// </summary>
        /// <param name="noticia">Noticia</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        public void ModificarNoticia(DYK_NOTICIA noticia, string pathImagen, string pathImagenDestino, string pathBanner, string pathBannerDestino)
        {
            using (var modelo = new ContextoPortal())
            {
                var noticiaEntidad = modelo.DYK_NOTICIA.FirstOrDefault(m => m.NOTICIAID == noticia.NOTICIAID);

                // Imagen
                string imageName = string.Empty;
                if (!string.IsNullOrWhiteSpace(pathImagen))
                {
                    imageName = Path.GetFileName(pathImagen);
                    File.Copy(pathImagen, pathImagenDestino + imageName);

                    noticiaEntidad.IMAGEN = imageName;
                }

                // Banner
                string bannerName = string.Empty;
                if (!string.IsNullOrWhiteSpace(pathBanner))
                {
                    bannerName = Path.GetFileName(pathBanner);
                    File.Copy(pathBanner, pathBannerDestino + bannerName);

                    noticiaEntidad.BANNER = bannerName;
                }

                noticiaEntidad.TITULO = noticia.TITULO;
                noticiaEntidad.TITULOQS = StringExtension.GenerateQueryString(noticia.TITULO);
                noticiaEntidad.DESCRIPCION = noticia.DESCRIPCION;
                noticiaEntidad.CONTENIDO = noticia.CONTENIDO;
                noticiaEntidad.ACTIVO = noticia.ACTIVO;
                modelo.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina una noticia
        /// </summary>
        /// <param name="noticiaId">Identificador de la noticia</param>
        public void EliminarNoticia(Guid noticiaId)
        {
            using (var modelo = new ContextoPortal())
            {
                var noticia = modelo.DYK_NOTICIA.FirstOrDefault(u => u.NOTICIAID == noticiaId);
                modelo.DYK_NOTICIA.Remove(noticia);
                modelo.SaveChanges();
            }
        }

        /// <summary>
        /// Trae una noticia por el identificador
        /// </summary>
        /// <param name="noticiaId">Identificador de la noticia</param>
        /// <returns></returns>
        public DYK_NOTICIA TraerNoticiaPorId(Guid noticiaId)
        {
            using (var modelo = new ContextoPortal())
            {
                return modelo.DYK_NOTICIA.FirstOrDefault(u => u.NOTICIAID == noticiaId);
            }
        }

        /// <summary>
        /// Trae una noticia por el titulo
        /// </summary>
        /// <param name="noticiaId">Identificador de la noticia</param>
        /// <returns></returns>
        public DYK_NOTICIA TraerNoticiaPorTituloQS(string tituloQS)
        {
            using (var modelo = new ContextoPortal())
            {
                return modelo.DYK_NOTICIA.FirstOrDefault(u => u.TITULOQS == tituloQS);
            }
        }

        /// <summary>
        /// Trae la lista de noticias paginada
        /// </summary>
        /// <param name="soloActivas">Indica si so trae las noticias acrivas</param>
        /// <param name="pageSize">Tamaño de la cantidad de resultados a devolver</param>
        /// <param name="pageIndex">Número de la página</param>
        /// <returns></returns>
        public IEnumerable<DYK_NOTICIA_SP> TraerNoticias(bool soloActivas, int pageSize, int pageIndex, string orderBy)
        {
            using (var modelo = new ContextoPortal())
            {
                var noticias = new List<DYK_NOTICIA_SP>();
                noticias = modelo.ConsultaSql<DYK_NOTICIA_SP>("DYK_OBTENER_NOTICIAS @SoloActivas,@PageSize,@PageIndex,@Order",
                    new SqlParameter("@SoloActivas", soloActivas),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex),
                    new SqlParameter("@Order", orderBy)
                ).ToList();

                return noticias;
            }
        }

        /// <summary>
        /// Trae el numero total de noticias
        /// </summary>
        /// <param name="soloActivas">Indica si so trae las noticias acrivas</param>
        /// <returns></returns>
        public int TraerNumeroNoticias(bool soloActivas)
        {
            using (var modelo = new ContextoPortal())
            {
                var total = 0;
                if (soloActivas)
                {
                    total = modelo.DYK_NOTICIA.Count();
                }
                else
                {
                    total = modelo.DYK_NOTICIA.Where(n => n.ACTIVO).Count();
                }

                return total;
            }
        }

        #endregion IRepositorioNoticia Members
    }
}