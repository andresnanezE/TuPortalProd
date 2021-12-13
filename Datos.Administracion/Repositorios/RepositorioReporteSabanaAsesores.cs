using AutoMapper;
using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioReporteSabanaAsesores : IRepositorioReporteSabanaAsesores
    {
        public IEnumerable<AfiliacionesPeriodo> Obtener_Periodos()
        {
            /*
             *   PROCEDURE [dbo].[EMSP_Obtener_Periodos]
             * */
            IEnumerable<AfiliacionesPeriodo> lista = null;
            using (var modelo = new ContextoProcesos())
            {
                lista = modelo.ConsultaSql<AfiliacionesPeriodo>("EMSP_Obtener_Periodos").ToList();
            }
            return lista;
        }

        public IEnumerable<ResultadosConsultaAfiliacionResumenTabla> Consultar_Detalle_Afiliacion_ResumenTabla(DatosConsultaAfiliacion _datos)
        {
            /*
             * PROCEDURE [dbo].[EMSP_DETALLE_AFILIACIONES]
                @DOCUMENTO NCHAR(20),
                @ROL INT,
                @TIPO char(1),
                @FECH_PERIODO DATETIME,
                @COMISIONA NCHAR(100),
                @TIP_CONTR NCHAR(100),
                @TIP_NOVEDAD NCHAR(100),
                @EST_BENEF NCHAR(100)
             *
             */
            IEnumerable<ResultadosConsultaAfiliacionResumenTabla> listaDatos = null;
            using (var modelo = new ContextoProcesos())
            {
                listaDatos = modelo.ConsultaSql<ResultadosConsultaAfiliacionResumenTabla>("EMSP_RTE_AFILIACIONES @DOCUMENTO,@ROL,@TIPO,@FECH_PERIODO,@FECH_PERIODO2,@COMISIONA,@TIP_CONTR,@TIP_NOVEDAD,@EST_BENEF",
                    new SqlParameter("@DOCUMENTO", _datos.DOCUMENTO),
                    new SqlParameter("@ROL", _datos.ROL),
                    new SqlParameter("@TIPO", _datos.TIPO),
                        new SqlParameter("@FECH_PERIODO", _datos.FECH_PERIODO.ToString("s")),
                    new SqlParameter("@FECH_PERIODO2", _datos.FECH_PERIODO2.ToString("s")),
                    new SqlParameter("@COMISIONA", _datos.COMISIONA),
                    new SqlParameter("@TIP_CONTR", _datos.TIP_CONTR),
                    new SqlParameter("@TIP_NOVEDAD", _datos.TIP_NOVEDAD),
                    new SqlParameter("@EST_BENEF", _datos.EST_BENEF)
                    ).ToList();
            }
            return listaDatos;
        }

        public ConsultaDoble Consultar_Detalle_Afiliacion_Resumen(DatosConsultaAfiliacion _datos)
        {
            DataSet listaDatos = null;
            var res = new ConsultaDoble();
            using (var modelo = new ContextoProcesos())
            {
                listaDatos = modelo.ConsultaSqlDataSet("EMSP_DETALLE_SABANA_ASESORES",
                    new SqlParameter("@DOCUMENTO", _datos.DOCUMENTO),
                    new SqlParameter("@ROL", _datos.ROL),
                    new SqlParameter("@TIPO", _datos.TIPO),
                    new SqlParameter("@FECH_PERIODO", _datos.FECH_PERIODO.ToString("s")),
                    new SqlParameter("@FECH_PERIODO2", _datos.FECH_PERIODO2.ToString("s")),
                    new SqlParameter("@COMISIONA", _datos.COMISIONA),
                    new SqlParameter("@TIP_CONTR", _datos.TIP_CONTR),
                    new SqlParameter("@TIP_NOVEDAD", _datos.TIP_NOVEDAD),
                    new SqlParameter("@EST_BENEF", _datos.EST_BENEF)
                    );
            }

            Mapper.CreateMap<IDataReader, resultadosConsultaAfiliacionResumen>();

            res.listaResumenAfiliaciones = Mapper.DynamicMap<List<resultadosConsultaAfiliacionResumen>>(listaDatos.Tables[0].CreateDataReader());

            Mapper.CreateMap<IDataReader, resultadosConsultaAfiliacionEstatus>();

            res.listaResumenAfiliacionesEstatus = Mapper.DynamicMap<List<resultadosConsultaAfiliacionEstatus>>(listaDatos.Tables[1].CreateDataReader());
            return res;
        }

        public IEnumerable<AfiliacionesFiltro> ObtenerFiltro_x_Rol(int _rol)
        {
            List<AfiliacionesFiltro> lista = null;
            using (var modelo = new ContextoProcesos())
            {
                lista =
                    modelo.EM_FILTRO_AFILIACIONES.Join(modelo.EM_FILTROXROL_AFILIACIONES, x => x.ID_FILTRO,
                        y => y.ID_FILTRO, (x, y) => new { x, y })
                        .Where(m => m.y.ID_ROL == _rol)
                        .Select(n => new AfiliacionesFiltro()
                        {
                            DatosFiltro = new DatosGeneralesFiltroAfiliaciones()
                            {
                                ID_FILTRO = n.x.ID_FILTRO,
                                NOMBRE = n.x.NOMBRE,
                                ACTIVO = n.x.ACTIVO,
                                ID_CODIGO = n.x.ID_CODIGO.Trim()
                            },
                            Mostar = n.y.MOSTRAR,
                            FiltroDefault = n.y.OPCION_DEFAULT.Trim(),
                            ListaOpciones = modelo.EM_FILTRO_OPCION_AFILIACIONES.Where(y => y.ID_FILTRO == n.y.ID_FILTRO).ToList()
                        }).ToList();

                //foreach (var filtro in listaFiltros)
                //{
                //    var listaOpciones =
                //        modelo.EM_FILTRO_OPCION_AFILIACIONES.Where(x => x.ID_FILTRO == filtro.ID_FILTRO).ToList();

                //    lista.Add(new AfiliacionesFiltro(){Filtro=filtro,ListaOpciones = listaOpciones});
                //}
            }
            return lista;
        }

        public IEnumerable<NovedadesHomologadas> Obtener_Novedades_Homologadas(List<string> comisiona)
        {
            List<NovedadesHomologadas> listaNovedades = null;
            using (var modelo = new ContextoProcesos())
            {
                var listaWhere = modelo.EM_NOVEDAD_HOMOLOGADA.Where(x => comisiona.Contains(x.APLI_COMISION)).ToList();
                listaNovedades =
                    modelo.EM_NOVEDAD_HOMOLOGADA.Where(x => comisiona.Contains(x.APLI_COMISION)).GroupBy(x => new { x.NOMB_HOMOLOG })
                        .Select(x => new NovedadesHomologadas()
                        {
                            Nombre = x.Key.NOMB_HOMOLOG
                        }).ToList();
            }
            return listaNovedades;
        }

        public IEnumerable<Ciudades> Obtener_Ciudades_Homologadas(string user)
        {
            IEnumerable<Ciudades> lista = null;
            using (var modelo = new ContextoProcesos())
            {
                lista = modelo.ConsultaSql<Ciudades>("EMSP_Ciudades @DOCUMENTO", new SqlParameter("@DOCUMENTO", user)).ToList();
            }
            return lista;
        }

        public IEnumerable<Canales> Obtener_Canales(string user)
        {
            IEnumerable<Canales> lista = null;
            using (var modelo = new ContextoProcesos())
            {
                lista = modelo.ConsultaSql<Canales>("EMSP_Canales @DOCUMENTO", new SqlParameter("@DOCUMENTO", user)).ToList();
            }
            return lista;
        }

        public IEnumerable<resultadosConsultaAfiliacionResumen> Consultar_Detalle_Netos_Resumen(DatosConsultaAfiliacion _datos)
        {
            /*
             * PROCEDURE [dbo].[EMSP_DETALLE_AFILIACIONES]
	            @DOCUMENTO NCHAR(20),
	            @ROL INT,
	            @TIPO char(1),
	            @FECH_PERIODO DATETIME,
	            @COMISIONA NCHAR(100),
	            @TIP_CONTR NCHAR(100),
	            @TIP_NOVEDAD NCHAR(100),
	            @EST_BENEF NCHAR(100)
             *
             */
            IEnumerable<resultadosConsultaAfiliacionResumen> listaDatos = null;
            using (var modelo = new ContextoProcesos())
            {
                listaDatos = modelo.ConsultaSql<resultadosConsultaAfiliacionResumen>("EMSP_RTE_AFILIACIONES @DOCUMENTO,@ROL,@TIP_CONTR",
                    new SqlParameter("@DOCUMENTO", _datos.DOCUMENTO),
                    new SqlParameter("@ROL", _datos.ROL),
                    new SqlParameter("@TIP_CONTR", _datos.TIP_CONTR)

                    ).ToList();
            }
            return listaDatos;
        }
    }
}