using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.CorreoDto;
using Dominio.Administracion.Entidades.ModelKheiron_Logs;
using System;
using Transversales.Administracion.Mappers;

namespace Datos.Administracion.Repositorios
{
    public class GestionDatosCorreo
    {
        public void InsertarCorreo(CorreoDTO correo)
        {
            using (var bd = new ContextoKHEIRON_LOGSEntities())
            {
                try
                {
                    EMH_ENVIO_CORREO nuevoMensaje = new EMH_ENVIO_CORREO();
                    nuevoMensaje = Mapper.DtoToEntity(correo);
                    bd.EMH_ENVIO_CORREO.Add(nuevoMensaje);
                    bd.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}