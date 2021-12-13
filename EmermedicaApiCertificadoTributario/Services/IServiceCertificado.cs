using ApiCertificadoTributario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCertificadoTributario.Services
{
    public interface IServiceCertificado
    {
        Task<ResultadoConsulta> ObtenerCertificadosPendientes();
        Task ActualizarEnvio(EnvioCertificadosTributarios envio);
        Task ActualizarProceso(int idProceso, bool estado);
        void RegLog(object infoLog, Exception ex);
    }
}
