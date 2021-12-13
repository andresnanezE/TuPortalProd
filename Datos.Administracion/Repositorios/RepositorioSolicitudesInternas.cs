using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioSolicitudesInternas : IRepositorioSolicitudesInternas
    {
        public void AgregarNota(SolicitudesInternasNotas notas)
        {
            using (var modelo = new ContextoPortal())
            {
                var l = modelo.ConsultaSql<SolicitudesInternasNotas>(@"EMESP_INSERT_SOLICITUD_INTERNA_NOTAS
                                                                        @ID_NOTA ,
	                                                                    @NOTA  ,
	                                                                    @ID_SOLICITUD ,
	                                                                    @FECHA ,
	                                                                    @URL_ARCHIVO ,
	                                                                    @NOMBRE_ARCHIVO",
                    new SqlParameter("@ID_NOTA", notas.IdNota),
                    new SqlParameter("@NOTA", notas.Nota),
                    //new SqlParameter("@ARCHIVO", notas.archivo),
                    new SqlParameter("@ID_SOLICITUD", notas.IdTicket),
                    new SqlParameter("@FECHA", notas.Fecha),
                    new SqlParameter("@URL_ARCHIVO", notas.url_archivo),
                    new SqlParameter("@NOMBRE_ARCHIVO", notas.nombre_archivo)
                    ).ToList();
            }
        }

        public IEnumerable<SolicitudesInternasNotas> ObtenerNotas(int IdTicket)
        {
            IEnumerable<SolicitudesInternasNotas> listaDatos = null;
            using (var modelo = new ContextoProcesos())
            {
                listaDatos = modelo.ConsultaSql<SolicitudesInternasNotas>("EMESP_SELECT_SOLICITUD_INTERNA_NOTAS @IdTicket",
                    new SqlParameter("@IdTicket", IdTicket)
                    ).ToList();
            }

            return listaDatos;
        }

        public void AgregarSolicitudInterna(SolicitudesInternas solicitud)
        {
            using (var modelo = new ContextoPortal())
            {
                var l = modelo.ConsultaSql<SolicitudesInternasNotas>("EMESP_INSERT_SOLICITUD_INTERNA  " +
                                                                     @"@ID_SOLICITUD
	                                                                ,@COD_ASES
	                                                                ,@ASUNTO
	                                                                ,@AREA
	                                                                ,@TIPO_REQUERIMIENTO
	                                                                ,@CIUDAD
	                                                                ,@FECHA_INI
                                                                    ,@DESCRIPCION
	                                                                ,@URL_ARCHIVO
                                                                    ,@NOMBRE_ARCHIVO",
                    new SqlParameter("@ID_SOLICITUD", solicitud.id_solicitud),
                    new SqlParameter("@COD_ASES", solicitud.cod_ases),
                    new SqlParameter("@ASUNTO", solicitud.asunto),
                    new SqlParameter("@AREA", solicitud.area),
                    new SqlParameter("@TIPO_REQUERIMIENTO", solicitud.tipo_requerimiento),
                    new SqlParameter("@CIUDAD", solicitud.ciudad),
                    new SqlParameter("@FECHA_INI", solicitud.fecha_ini),
                    //new SqlParameter("@ARCHIVO", solicitud.archivo),
                    new SqlParameter("@DESCRIPCION", solicitud.Descripcion),
                    new SqlParameter("@URL_ARCHIVO", solicitud.url_archivo),
                    new SqlParameter("@NOMBRE_ARCHIVO", solicitud.nombre_archivo)
                    ).ToList();
            }
        }

        public IEnumerable<SolicitudesInternas> ObtenerSolicitudesInternas(decimal cod_ases)
        {
            IEnumerable<SolicitudesInternas> listaDatos = null;
            using (var modelo = new ContextoProcesos())
            {
                listaDatos = modelo.ConsultaSql<SolicitudesInternas>("EMESP_SELECT_SOLICITUDES_INTERNAS  @COD_ASES",
                    new SqlParameter("@COD_ASES", cod_ases)
                    ).ToList();
            }

            return listaDatos;
        }
    }
}