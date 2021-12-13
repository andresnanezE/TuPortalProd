using System;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioLogs
    {
       void LogAplicacion(string llave, string descripcion, Exception excepcion, string parametroXml = null);
    }
}