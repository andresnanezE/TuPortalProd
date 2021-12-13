using ApiCertificadoTributario.Data;
using ApiCertificadoTributario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ApiCertificadoTributario.Services
{
    public class ServiceCertificado : IServiceCertificado
    {
        public IRepository Repo => new Repository();
        public async Task<ResultadoConsulta> ObtenerCertificadosPendientes()
        {
            try
            {
                var consulta =  await Repo.RegistrarEnvioRmtContTodos();
                //var consulta = await Repo.ObtenerCertificadosPendientes();
                return new ResultadoConsulta
                {
                    Success = true,
                    Result = consulta
                };
            }
            catch (Exception ex)
            {
                Repo.RegLog("ObtenerCertificadosPendientes: ServiceCertificado", ex);
                return new ResultadoConsulta
                {
                    Success = false,
                    Message = "Error al obtener certificados pendientes"
                };
            }
        }
        public async Task ActualizarEnvio(EnvioCertificadosTributarios envio)
        {
            await Repo.ActualizarEnvio(envio);
        }
        public async Task ActualizarProceso(int idProceso, bool estado)
        {
            await Repo.ActualizarProceso(idProceso, estado);
        }
        public void RegLog(object infoLog, Exception ex)
        {
            Repo.RegLog(infoLog, ex);
        }
    }
}