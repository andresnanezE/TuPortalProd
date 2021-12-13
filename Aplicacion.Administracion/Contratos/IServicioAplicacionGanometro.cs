using Dominio.Administracion.Entidades.MapperDto;
using System;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionGanometro
    {
        List<VentasGanometroDto> ObtenerVentas(int id_director, int? localización, int? Anio, int? Periodo, int? Dia);

        List<VentasDirectorPeriodoDto> ObtenerVentasXperiodoXsemanaXdia(int id_director, int anio, int periodo, int idCiudad);

        void InsertarVenta(VentasGanometroDto venta);

        PeriodoVentaDto ObtenerPeriodo(DateTime fecha);

        List<MetasGanometroDto> ObtenerMetas(PeriodoVentaDto p, int idCiudad);
    }
}