using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioMotivoSoporteSegSocial : IRepositorioMotivoSoporteSegSocial
    {
        public IEnumerable<MotivoSoporteSegSocial> MotivosSoporteSegSocial()
        {
            using (var modelo = new ContextoPortal())
            {
                return modelo.ConsultaSql<MotivoSoporteSegSocial>("EMESP_MOTIVOS_SOPORTE_SEGSOCIAL").ToList();
            }
        }

        public void AgregarLog(MotivoSoporteSegSocial mss)
        {
            using (var modelo = new ContextoPortal())
            {
                modelo.ConsultaSql<MotivoSoporteSegSocial>("INSERT_LOG_SOPORTE_SEGSOCIAL @EMAIL,@ID_MOTIVO,@ARCHIVOSTXT,@USUARIO,@OBSERVACION",
                    new SqlParameter("@EMAIL", mss.EMAIL),
                    new SqlParameter("@ID_MOTIVO", mss.ID_MOTIVO.ToString()),
                    new SqlParameter("@ARCHIVOSTXT", mss.ARCHIVOSTXT),
                    new SqlParameter("@USUARIO", mss.USUARIO),
                    new SqlParameter("@OBSERVACION", mss.OBSERVACION.ToString())
                    );
            }
        }
    }
}