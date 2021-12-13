using Dominio.Administracion.Repositorios;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using Microsoft.Azure;
using System.Collections.Generic;
using Dominio.Administracion.Entidades;
using System.Net;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioArchivosVentas : IRepositorioArchivosVentas
    {
        #region private readonly field
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudBlobClient _storageClient;
        private readonly CloudBlobContainer _container;
        private readonly string _key;
        private readonly string _containerReference;
        #endregion


        public RepositorioArchivosVentas()
        {
            _key = CloudConfigurationManager.GetSetting("StoregeFileAccount");
            _containerReference = CloudConfigurationManager.GetSetting("StorageContainer");
            _storageAccount = CloudStorageAccount.Parse(_key);
            _storageClient = _storageAccount.CreateCloudBlobClient();
            _container = _storageClient.GetContainerReference(_containerReference);
        }

        public async Task<List<string>> GetArchivosVentas(string documento)
        {
            try
            {


                var List = new List<string>();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.SystemDefault;
                var ListaArchivos = _container.ListBlobs();

                foreach (var item in ListaArchivos)
                {
                    if (item is CloudBlockBlob blob)
                    {

                        var blobString = blob.Name;
                        if (blobString.Contains(documento))
                        {
                            List.Add(blobString);
                        }
                    }
                }
                return List;
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
        public async Task<ArchivoBlob> GetArchivoBlob(string fileName)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.SystemDefault;
            var blobServer = await _container.GetBlobReferenceFromServerAsync(fileName);
            var blob = new ArchivoBlob
            {
                ContentType = blobServer.Properties.ContentType,
                File = await blobServer.OpenReadAsync(),
                Lengthfile = blobServer.Properties.Length,

            };

            return blob;
        }

    }
}
