using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Entidades.ModeloProcesos;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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

        public EMB_LogActividades LogActividades(EMB_LogActividades log)
        {
            using (var model = new ContextoPortal())
            {
                model.EMB_LogActividades.Add(log);
                model.SaveChanges();
            }

            return log;
        }

        public IEnumerable<EMB_TipoLog> ObtenerTiposLog()
        {
            using (var model = new ContextoProcesos())
            {
                var listTipos = new List<EMB_TipoLog>();
                listTipos = model.EMB_TipoLog.ToList();
                return listTipos;
            }
        }

        public EMB_LogActividades ObtenerLogSesionEnVezDe(int idLog)
        {
            using (var model = new ContextoProcesos())
            {
                var listLog = new EMB_LogActividades();
                listLog = model.EMB_LogActividades.Where(n => n.idLog == idLog).FirstOrDefault();
                return listLog;
            }
        }

        public void modificaLogSesionEnVezDe(EMB_LogActividades logActividad)
        {
            using (var model = new ContextoProcesos())
            {
                model.Entry(logActividad).State = System.Data.Entity.EntityState.Modified;
                model.SaveChanges();
            }
        }
    }
}