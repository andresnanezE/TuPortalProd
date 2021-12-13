using System;
using Utilidades.Configuration;
using Utilidades.Enums;

namespace Utilidades.ImageManager
{
    /// <summary>
    /// Image metadata
    /// </summary>
    public class ImageMetadata
    {
        /// <summary>
        /// Original image path
        /// </summary>
        public string OriginalImagePath { get; set; }

        /// <summary>
        /// Image name
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Create an ImageMetadata from the configuration file
        /// </summary>
        public static ImageMetadata CreateFromConfigurationFile(string originalImagePath, string format, ImageType imageType, string name = "")
        {
            IResourceManager appSettings = new AppSettings();
            var imageMetadata = new ImageMetadata();
            imageMetadata = appSettings.Get("Image.Size." + imageType.ToString(), imageMetadata);

            imageMetadata.OriginalImagePath = originalImagePath;
            imageMetadata.ImageName = String.IsNullOrEmpty(name) ? Guid.NewGuid().ToString() + format : name + format;
       
            return imageMetadata;
        }
    }
}
