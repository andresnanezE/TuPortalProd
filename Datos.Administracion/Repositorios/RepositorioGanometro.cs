using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioGanometro : IRepositorioGanometro
    {
        //TODO: nsarmiento: reemplazar este metodo de prueba
        public List<EME_REGISTRO_VENTAS> ObtenerVentas(int id_director, int? localización, int? Anio, int? Periodo, int? Dia)
        {
            var res = new List<EME_REGISTRO_VENTAS>();
            using (var bd = new ContextoPortal())
            {
                var t = (from a in bd.EME_REGISTRO_VENTAS select a).ToList();
                res = t;
                var entidades = bd.ConsultaSql<SPEM_CONSULTARVENTASXPERIODOXSEMANAXDIA>(
                        "SPEM_CONSULTARVENTASXPERIODOXSEMANAXDIA @ANIO, @PERIODO, @ID_DIRECTOR"
                        , new SqlParameter("@ANIO", 2018)
                        , new SqlParameter("@PERIODO", 7)
                        , new SqlParameter("@ID_DIRECTOR", 17788)
                    ).ToList();

                //
                //Traer por datatable
                //
                //  var z = bd.ConsultaSqlDataTable("SPEM_CONSULTARVENTASXPERIODOXSEMANAXDIA"
                //        , new SqlParameter("@ANIO", 2018)
                //        , new SqlParameter("@PERIODO", 7)
                //        , new SqlParameter("@ID_DIRECTOR", 17788));
            }
            return res;
        }

        public List<SPEM_CONSULTARVENTASXPERIODOXSEMANAXDIA> ObtenerVentasXperiodoXsemanaXdia(int id_director, int anio, int periodo, int idciudad)
        {
            var res = new List<SPEM_CONSULTARVENTASXPERIODOXSEMANAXDIA>();
            using (var bd = new ContextoPortal())
            {
                res = bd.ConsultaSql<SPEM_CONSULTARVENTASXPERIODOXSEMANAXDIA>(
                        "SPEM_CONSULTARVENTASXPERIODOXSEMANAXDIA @ANIO, @PERIODO, @IDCIUDAD, @ID_DIRECTOR"
                        , new SqlParameter("@ANIO", anio)
                        , new SqlParameter("@PERIODO", periodo)
                        , new SqlParameter("@IDCIUDAD", idciudad)
                        , new SqlParameter("@ID_DIRECTOR", id_director)
                    ).ToList();

                //
                //Traer por datatable
                //
                //  var z = bd.ConsultaSqlDataTable("SPEM_CONSULTARVENTASXPERIODOXSEMANAXDIA"
                //        , new SqlParameter("@ANIO", 2018)
                //        , new SqlParameter("@PERIODO", 7)
                //        , new SqlParameter("@ID_DIRECTOR", 17788));
            }
            return res;
        }

        public void InsertarVenta(EME_REGISTRO_VENTAS venta)
        {
            using (var bd = new ContextoPortal())
            {
                var t = (from a in bd.EME_REGISTRO_VENTAS
                         where
                            a.FECHA == venta.FECHA &&
                            a.CIUD_HOMOL == venta.CIUD_HOMOL &&
                            a.ID_DIRECTOR == venta.ID_DIRECTOR
                         select a).ToList();
                if (t.Any())
                {
                    var v = t.FirstOrDefault();
                    v.CANT_VENTAS = venta.CANT_VENTAS;
                }
                else
                {
                    bd.EME_REGISTRO_VENTAS.Add(venta);
                }
            }
        }

        /// <summary>
        /// Obtiene el periodo al cual pertenece la fecha idicada
        /// </summary>
        /// <param name="fecha">Fecha a examinar</param>
        /// <returns>Periodo de la fecha</returns>
        public PERIODO_VENTA ObtenerPeriodo(DateTime fecha)
        {
            var res = new PERIODO_VENTA();
            using (var bd = new ContextoPortal())
            {
                var t = (from a in bd.EME_METAS_VENTAS where a.FECHA == fecha.Date select a).ToList();

                if (t.Any())
                {
                    var v = t.FirstOrDefault();
                    res.ANIO = v.ANIO;
                    res.PERIODO = v.PERIODO;
                    res.SEMANA = v.SEMANA;
                    res.DIA = v.DIA;
                    res.FECHA = v.FECHA;
                }
                else
                {
                    res = null;
                }
            }
            return res;
        }

        public List<EME_METAS_VENTAS> ObtenerMetas(PERIODO_VENTA p, int idCiudad)
        {
            var res = new List<EME_METAS_VENTAS>();
            using (var bd = new ContextoPortal())
            {
                res = (from a in bd.EME_METAS_VENTAS
                       where
                            a.ANIO == p.ANIO &&
                            a.PERIODO == p.PERIODO &&
                            a.CIUD_HOMOL == idCiudad
                       select a).ToList();
            }
            return res;
        }
    }
}