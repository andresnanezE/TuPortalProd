using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio.Administracion.Entidades;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioArchivosVentas
    {

        Task<List<string>> GetArchivosVentas(string documento);

        Task<ArchivoBlob> GetArchivoBlob(string fileName);
    }
}