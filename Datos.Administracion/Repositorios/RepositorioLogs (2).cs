using System;
using System.Data;
using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.ModeloProcesos;
using Dominio.Administracion.Repositorios;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioLogs : IRepositorioLogs
    {
        public void LogAplicacion(string llave, string descripcion, Exception excepcion, string parametroXml = null)
        {
            using (var model = new ContextoPortal())
            {
                var registroLog = new EMA_LOG
                {
                    LLAVE = llave,
                    DESCRIPCION = descripcion,
                    EXCEPCION = excepcion.ToString(),
                    PARAMETROXML = parametroXml
                };
                model.EMA_LOG.Add(registroLog);
                model.SaveChanges();
            }
        }
    }
}