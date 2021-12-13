
using Dominio.Administracion.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioArchivosVentas
    {
        Task<List<string>> GetArchivosVentas(string documento);

        Task<ArchivoBlob> GetArchivoBlob(string fileName);


    }

}
