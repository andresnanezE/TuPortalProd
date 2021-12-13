// ----------------------------------------------------------------------------------------------
// <copyright file="IServicioAplicacionMenu.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades.MapperDto;

//using Dominio.Administracion.Entidades.MapperDto;
using System;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionNoticia
    {
        #region Instance Methods

        /// <summary>
        /// Agrega una noticia
        /// </summary>
        /// <param name="noticia">Noticia</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        void AgregarNoticia(DYK_NOTICIADto noticia, string pathImagen, string pathImagenDestino, string pathBanner, string pathBannerDestino);

        /// <summary>
        /// Modifica una imagen
        /// </summary>
        /// <param name="noticia">Noticia</param>
        /// <param name="pathImagen">Path temporal de la imagen</param>
        /// <param name="pathImagenDestino">Path final de la imagen</param>
        void ModificarNoticia(DYK_NOTICIADto noticia, string pathImagen, string pathImagenDestino, string pathBanner, string pathBannerDestino);

        /// <summary>
        /// Elimina una noticia
        /// </summary>
        /// <param name="noticiaId">Identificador de la noticia</param>
        void EliminarNoticia(Guid noticiaId);

        /// <summary>
        /// Trae una noticia por el identificador
        /// </summary>
        /// <param name="noticiaId">Identificador de la noticia</param>
        /// <returns></returns>
        DYK_NOTICIADto TraerNoticiaPorId(Guid noticiaId);

        /// <summary>
        /// Trae una noticia por el titulo
        /// </summary>
        /// <param name="noticiaId">Identificador de la noticia</param>
        /// <returns></returns>
        DYK_NOTICIADto TraerNoticiaPorTituloQS(string tituloQS);

        /// <summary>
        /// Trae la lista de noticias paginada
        /// </summary>
        /// <param name="soloActivas">Indica si so trae las noticias acrivas</param>
        /// <param name="pageSize">Tamaño de la cantidad de resultados a devolver</param>
        /// <param name="pageIndex">Número de la página</param>
        /// <returns></returns>
        IEnumerable<DYK_NOTICIA_SPDto> TraerNoticias(bool soloActivas, int pageSize, int pageIndex, string orderBy);

        /// <summary>
        /// Trae el numero total de noticias
        /// </summary>
        /// <param name="soloActivas">Indica si so trae las noticias acrivas</param>
        /// <returns></returns>
        int TraerNumeroNoticias(bool soloActivas);

        #endregion Instance Methods
    }
}