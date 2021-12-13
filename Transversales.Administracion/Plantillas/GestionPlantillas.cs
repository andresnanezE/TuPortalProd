using System.IO;

namespace Transversales.Administracion.Plantillas
{
    public class GestionPlantillas
    {
        /// <summary>
        /// Obtiene una plantilla de email
        /// </summary>
        /// <param name="rutaPlantillaEmail">ruta de la plantilla</param>
        /// <returns></returns>
        public static string ObtenerHtmlPlantilla(string rutaPlantillaEmail)
        {
            string html = "";

            if (File.Exists(rutaPlantillaEmail))
            {
                string temp = "";
                StreamReader file = new StreamReader(rutaPlantillaEmail);
                while ((temp = file.ReadLine()) != null)
                {
                    html += temp;
                }
                file.Close();
            }

            return html;
        }
    }
}