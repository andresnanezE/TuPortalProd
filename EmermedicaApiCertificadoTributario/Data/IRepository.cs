using ApiCertificadoTributario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCertificadoTributario.Data
{
    public interface IRepository
    {
        void RegLog(object infoLog, Exception ex);
        Task<List<EnvioCertificadosTributarios>> RegistrarEnvioRmtContTodos();
        //Task<List<EnvioCertificadosTributarios>> ObtenerCertificadosPendientes();
        Task ActualizarProceso(int idProceso,bool estado);
        Task ActualizarEnvio(EnvioCertificadosTributarios envio);

    }
}
