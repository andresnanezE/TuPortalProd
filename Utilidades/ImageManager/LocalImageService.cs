using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using Utilidades.Enums;
using Utilidades.Configuration;

namespace Utilidades.ImageManager
{
    /// <summary>
    /// Local Image Service
    /// </summary>
    public class LocalImageService
    {
        /// <summary>
        /// Generates images
        /// </summary>
        public static string GenerateImages(string imagePath, ImageType imageType)
        {
            var folderPath = CreateFolders(imageType);

            // + Getting the image extension
            var extension = Path.GetExtension(imagePath);
            var imageMetadata = ImageMetadata.CreateFromConfigurationFile(imagePath, extension, imageType);

            SaveImage(Path.Combine(folderPath, imageMetadata.ImageName), new Bitmap(imagePath), 85L);

            return imageMetadata.ImageName;
        }

        /// <summary>
        /// Generates images
        /// </summary>
        public static string GenerateImage(string imagePath, ImageSize size, string folderPath)
        {
            // + Getting the image extension
            var extension = Path.GetExtension(imagePath);
            var imageName = Guid.NewGuid().ToString() + extension;

            var image = Generate(new Size { Height = size.Height, Width = size.Width }, imagePath, true);
            SaveImage(Path.Combine(folderPath, imageName), new Bitmap(image), 85L);

            //File.Delete(imagePath);

            return imageName;
        }

        /// <summary>
        /// Remove images
        /// </summary>
        public static void RemoveImages(ImageType imageType, string imageName)
        {
            var predefinedPath = ImagePathDefinitions.AssetsUploadsFolder + ConfigurationManager.AppSettings["Image.Meta.Path." + imageType.ToString()];
            if (!Directory.Exists(predefinedPath))
                return;

            var orginal = Path.Combine(predefinedPath, imageName);
            if (File.Exists(orginal))
            {
                File.Delete(orginal);
            }
        }

        /// <summary>
        /// Generates the image with the specified size and in the given path
        /// </summary>
        /// <param name="size">Size for the image. If size is null, the image will be generated with the original size</param>
        /// <param name="imagePath">Full path for the image to save</param>
        /// <param name="isLocal">True if the file must be gotten from a local directory, false from the internet</param>
        public static Image Generate(Size? size, string imagePath, bool isLocal)
        {
            // + If needed to get the file from the internet            
            Stream stream = null;
            if (!isLocal)
            {
                var req = WebRequest.Create(imagePath);
                var response = req.GetResponse();
                stream = response.GetResponseStream();
            }

            // + Get the image
            stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            var imageTmp = Image.FromStream(stream);
            Image ret;
            if (size != null && size.Value.Height != 0 && size.Value.Height != 0)
            {
                // + Resize the image
                imageTmp = ResizeImage(imageTmp, size.Value);

                // + Crop the image
                var point = new Point(0, 0);
                if (imageTmp.Width == size.Value.Width)
                {
                    point.X = 0;
                    point.Y = 0; //To no crop faces (image.Height - size.Value.Height) / 2;
                }
                else
                {
                    point.X = (imageTmp.Width - size.Value.Width) / 2;
                    point.Y = 0;
                }
                ret = CropImage(imageTmp, new Rectangle(point, size.Value));
            }
            else
            {
                ret = new Bitmap(imageTmp);
            }

            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
            }
            return ret;
        }

        /// <summary>
        /// Crops an image
        /// </summary>
        /// <param name="image">Image to crop</param>
        /// <param name="cropArea">Crop area</param>
        /// <returns>Returns the cropped image</returns>
        private static Image CropImage(Image image, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(image);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        /// <summary>
        /// Saves an image with JPG format
        /// </summary>
        /// <param name="path">Path to save the image to</param>
        /// <param name="image">Image to save</param>
        /// <param name="quality">Quality for the image</param>
        private static void SaveImage(string path, Bitmap image, long quality)
        {
            //Sets the encoder mimeType
            string encoder = "";
            string extension = Path.GetExtension(path);
            switch (extension)
            {
                case ".jpg":
                    encoder = "image/jpeg";
                    break;
                case ".gif":
                    encoder = "image/gif";
                    break;
                case ".png":
                    encoder = "image/png";
                    break;
                default:
                    extension = ".jpg";
                    encoder = "image/jpeg";
                    break;
            }
            // + Save the images
            // Encoder parameter for image quality
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);

            // Jpeg image codec
            var imageCodec = GetEncoderInfo(encoder);

            if (imageCodec == null)
            {
                image.Dispose();
                return;
            }

            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            image.Save(path, imageCodec, encoderParams);
            image.Dispose();
        }

        /// <summary>
        /// Gets the encoder information for the given MIME type
        /// </summary>
        /// <param name="mimeType">MIME type</param>
        /// <returns>Retusn the encoder information for the given MIME type</returns>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            var codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            return codecs.FirstOrDefault(t => t.MimeType == mimeType);
        }

        /// <summary>
        /// Resizes an image
        /// </summary>
        /// <param name="image">Image to resize</param>
        /// <param name="size">New size for the resized image</param>
        /// <returns>Returns the resized image</returns>
        private static Image ResizeImage(Image image, Size size)
        {
            var sourceWidth = image.Width;
            var sourceHeight = image.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (size.Width / (float)sourceWidth);
            nPercentH = (size.Height / (float)sourceHeight);

            nPercent = nPercentH > nPercentW ? nPercentH : nPercentW;

            var destWidth = (int)Math.Ceiling(sourceWidth * nPercent);
            var destHeight = (int)Math.Ceiling(sourceHeight * nPercent);

            var b = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(image, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        /// <summary>
        /// Create folder for an image type
        /// </summary>
        public static string CreateFolders(ImageType imageType)
        {
            //creating folders
            var saveFolder = ConfigurationManager.AppSettings["Image.Path." + imageType.ToString()];
            var configPath = string.IsNullOrEmpty(saveFolder) ? imageType.ToString() + "\\" : saveFolder;
            var predefinedPath = ImagePathDefinitions.AssetsUploadsFolder + configPath;
            if (!Directory.Exists(predefinedPath))
            {
                Directory.CreateDirectory(predefinedPath);
            }

            var imageOriginalPath = Path.Combine(predefinedPath, "Original");
            if (!Directory.Exists(imageOriginalPath))
            {
                Directory.CreateDirectory(imageOriginalPath);
            }

            return predefinedPath;
        }
    }
}