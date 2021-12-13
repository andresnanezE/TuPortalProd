// ----------------------------------------------------------------------------------------------
// <copyright file="RepositorioDestacado.cs" company="Dynamic">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioDestacado : IRepositorioDestacado
    {
        #region IRepositorioDestacado Members

        /// <summary>
        /// Modifica la lista de destacados
        /// </summary>
        /// <param name="destacados">Lista de descatacod</param>
        /// <param name="pathsImagenes">Losta de paths de las imagenes de cada destacado</param>
        /// <param name="pathImagenesDestino">Path de destino para la imagen</param>
        public void ModificarDestacados(IEnumerable<DYK_DESTACADO> destacados, IEnumerable<string> pathsImagenes, string pathImagenesDestino)
        {
            using (var modelo = new ContextoPortal())
            {
                var i = 0;
                foreach (var destacado in destacados)
                {
                    var destacadoEntidad = modelo.DYK_DESTACADO.FirstOrDefault(m => m.DESTACADOID == destacado.DESTACADOID);

                    // Imagen
                    string imageName = string.Empty;
                    string imagePath = pathsImagenes.ElementAt(i);
                    if (!string.IsNullOrWhiteSpace(imagePath))
                    {
                        imageName = Path.GetFileName(imagePath);
                        File.Copy(imagePath, pathImagenesDestino + imageName);
                        File.Delete(imagePath);
                    }

                    if (destacadoEntidad == null)
                    {
                        destacado.IMAGEN = imageName;

                        modelo.DYK_DESTACADO.Add(destacado);
                        modelo.SaveChanges();
                    }
                    else
                    {
                        destacadoEntidad.URL = destacado.URL;
                        destacadoEntidad.ABRIRENSITIO = destacado.ABRIRENSITIO;
                        if (!string.IsNullOrWhiteSpace(imageName))
                        {
                            destacadoEntidad.IMAGEN = imageName;
                        }

                        modelo.SaveChanges();
                    }

                    ++i;
                }
            }
        }

        /// <summary>
        /// Trae la lista de destacados
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DYK_DESTACADO> TraerDestacados()
        {
            using (var modelo = new ContextoPortal())
            {
                var destacados = modelo.DYK_DESTACADO.ToList();
                return destacados;
            }
        }

        #endregion IRepositorioDestacado Members
    }
}