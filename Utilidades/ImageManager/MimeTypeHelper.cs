using Microsoft.Win32;

namespace Utilidades.ImageManager
{
    /// <summary>
    /// Mime type helper
    /// </summary>
    public class MimeTypeHelper
    {
        /// <summary>
        /// Gets an extension from a mime-type
        /// </summary>
        public static string GetDefaultExtension(string mimeType)
        {
            var key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            var value = key != null ? key.GetValue("Extension", null) : null;
            var result = value != null ? value.ToString() : string.Empty;

            return result;
        }

        /// <summary>
        /// Gets a mime-type from an extension
        /// </summary>
        public static string GetMimeTypeFromExtension(string extension)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;

            var key = Registry.ClassesRoot.OpenSubKey(extension, false);
            var value = key != null ? key.GetValue("Content Type", null) : null;
            var result = value != null ? value.ToString() : string.Empty;

            return result;
        }
    }
}
