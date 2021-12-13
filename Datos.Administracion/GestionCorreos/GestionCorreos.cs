using Datos.Administracion.Repositorios;
using Dominio.Administracion.Entidades.CorreoDto;
using Transversales.Administracion.Plantillas;

namespace Utilidades.GestionCorreos
{
    public class GestionCorreos : GestionPlantillas
    {
        /// <summary>
        /// Inserta un registrop en la tabla del servicio de envio de correos
        /// </summary>
        /// <param name="correoEnvio">Correo que se enviara con todos los paramnetros</param>
        public static void InsertarEnviarCorreo(CorreoDTO correoEnvio)
        {
            GestionDatosCorreo dllCorreo = new GestionDatosCorreo();
            dllCorreo.InsertarCorreo(correoEnvio);
        }
    }
}