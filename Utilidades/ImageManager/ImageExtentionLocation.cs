using System.Configuration;
using System.Text;
using Utilidades.Enums;
using Utilidades.Configuration;

namespace Utilidades.ImageManager
{
    /// <summary>
    /// Image Location
    /// </summary>
    public static class ImageExtentionLocation
    {
        /// <summary>
        /// Gets an image
        /// </summary>
        public static string GetImage(this string str, ImageType imagetype)
        {
            var typePath = ConfigurationManager.AppSettings["Image.Path." + imagetype.ToString()];
            var path = WebPathCombainer(ImagePathDefinitions.AssetsVirtualPath, typePath, str);
            path = path.Replace("\\\\", "/");
            path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        /// Generates a web path with the given parts
        /// </summary>
        public static string WebPathCombainer(params string[] parts)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];

                if (string.IsNullOrEmpty(part) || string.IsNullOrWhiteSpace(part)) continue;
                sb.Append(part);

                if (!(i == parts.Length - 1 && part.IndexOf(".") != -1))
                {
                    if (!part.EndsWith("/")) sb.Append("/");
                    if (part.EndsWith("\\")) sb.Append("/");
                }
            }

            return sb.ToString();
        }
    }
}