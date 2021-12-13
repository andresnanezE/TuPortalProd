using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionGanometro : IServicioAplicacionGanometro
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioGanometro _repositorioGanometro;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionGanometro(IRepositorioGanometro repositorioGanometro, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioGanometro = repositorioGanometro;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion C'tors

        public List<VentasGanometroDto> ObtenerVentas(int id_director, int? localización, int? Anio, int? Periodo, int? Dia)
        {
            var t = _repositorioGanometro.ObtenerVentas(id_director, null, null, null, null);

            var lista2 = _adaptadorDeObjetos.Adaptar<IEnumerable<VentasGanometroDto>>(t);
            return lista2.ToList();
        }

        public List<VentasDirectorPeriodoDto> ObtenerVentasXperiodoXsemanaXdia(int id_director, int anio, int periodo, int idCiudad)
        {
            var t = _repositorioGanometro.ObtenerVentasXperiodoXsemanaXdia(id_director, anio, periodo, idCiudad);

            var lst = _adaptadorDeObjetos.Adaptar<IEnumerable<VentasDirectorPeriodoDto>>(t);
            return lst.ToList();
        }

        public void InsertarVenta(VentasGanometroDto v)
        {
            var venta = _adaptadorDeObjetos.Adaptar<EME_REGISTRO_VENTAS>(v);
            _repositorioGanometro.InsertarVenta(venta);
        }

        public PeriodoVentaDto ObtenerPeriodo(DateTime fecha)
        {
            var v = _repositorioGanometro.ObtenerPeriodo(fecha);
            var t = _adaptadorDeObjetos.Adaptar<PeriodoVentaDto>(v);

            return t;
        }

        public List<MetasGanometroDto> ObtenerMetas(PeriodoVentaDto periodo, int idCiudad)
        {
            var p = _adaptadorDeObjetos.Adaptar<PERIODO_VENTA>(periodo);
            var res = _repositorioGanometro.ObtenerMetas(p, idCiudad);
            return _adaptadorDeObjetos.Adaptar<List<MetasGanometroDto>>(res);
        }
    }
}