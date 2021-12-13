using ApiCertificadoTributario.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ApiCertificadoTributario.Data
{
    public class Repository : IRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void RegLog(object infoLog, Exception ex)
        {
            log.Error(infoLog, ex);
        }

        public async Task<List<EnvioCertificadosTributarios>> RegistrarEnvioRmtContTodos()
        {
            using (var bdCertificado = new DatabaseCertificado())
            {
                var pendienteRmtContTodos = await bdCertificado.Database.SqlQuery<PendientesRmtContTodos>
                    ("EXEC SP_CONSULTA_PENDIENTES_RMT_CONT_TODOS").ToListAsync();
                foreach (var reg in pendienteRmtContTodos)
                {                    
                    var parametros = new SqlParameter[3];
                    parametros[0] = new SqlParameter("@Año", reg.Año);
                    parametros[1] = new SqlParameter("@TipoContrato", Convert.ToInt32(reg.TipoContrato));
                    parametros[2] = new SqlParameter("@ProcesoId", reg.ProcesoId);
                    
                    await bdCertificado.Database.ExecuteSqlCommandAsync
                        ("EXEC SP_INSERTAR_ENVIO_CERTIFICADOS_TRIBUTARIOS_TODOS @Año, @TipoContrato, @ProcesoId", parametros);
                    return await ObtenerCertificadosPendientes(reg.ProcesoId);
                }
                return null;
            }
        }
        public async Task<List<EnvioCertificadosTributarios>> ObtenerCertificadosPendientes(int procesoId)
        {
            using (var bd = new DatabaseCertificado())
            {
                var parametros = new SqlParameter[1];
                parametros[0] = new SqlParameter("@ProcesoId", procesoId);
                var consulta = await bd.Database.SqlQuery<EnvioCertificadosTributarios>
                    ("EXEC SP_ENVIO_CERTIFICADOS_TRIBUTARIOS  @ProcesoId", parametros).ToListAsync();
                return consulta;
            }
        }
        public async Task ActualizarEnvio(EnvioCertificadosTributarios envio)
        {
            using (var bd = new DatabaseCertificado())
            {
                var regEnvio = bd.EnvioCertificadoTributario.FirstOrDefault(e => e.Id == envio.IdEnvio);
                regEnvio.EstadoEnvio = envio.EstadoEnvio;
                regEnvio.EstadoError = envio.EstadoError;
                if(!envio.EstadoError)
                    regEnvio.FechaEnvio = envio.FechaEnvio;
                regEnvio.Mensaje = envio.Mensaje;
                await bd.SaveChangesAsync();
            }
        }

        public async Task ActualizarProceso(int idProceso, bool estado)
        {
            using (var bd = new DatabaseCertificado())
            {
                var regProceso = bd.ProcesoEnvioCertificadoTributario.FirstOrDefault
                    (e => e.Id == idProceso);
                regProceso.Estado = estado;
                regProceso.Mensaje = "Finalizado";
                await bd.SaveChangesAsync();
            }
        }
    }
}