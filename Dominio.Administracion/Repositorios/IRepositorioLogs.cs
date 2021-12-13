using Dominio.Administracion.Entidades.ModeloPortal;
using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioLogs
    {
        void LogAplicacion(string llave, string descripcion, Exception excepcion, string parametroXml = null);

        EMB_LogActividades LogActividades(EMB_LogActividades log);

        IEnumerable<EMB_TipoLog> ObtenerTiposLog();

        EMB_LogActividades ObtenerLogSesionEnVezDe(int idLog);

        void modificaLogSesionEnVezDe(EMB_LogActividades logActividad);
    }
}