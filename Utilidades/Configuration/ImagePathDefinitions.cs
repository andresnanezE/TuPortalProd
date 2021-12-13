using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilidades.Configuration
{
    /// <summary>
    /// Image path definitions
    /// </summary>
    public static class ImagePathDefinitions
    {
        /// <summary>
        /// Assets temp folder
        /// </summary>
        public static string AssetsUploadsFolder = ConfigurationManager.AppSettings["Assets.Uploads.Path"];

        /// <summary>
        /// Assets uploads virtual folder
        /// </summary>
        public static string AssetsVirtualPath = ConfigurationManager.AppSettings["Assets.Uploads.Virtual.Path"];
    }
}
