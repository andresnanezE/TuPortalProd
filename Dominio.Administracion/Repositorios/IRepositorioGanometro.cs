using Dominio.Administracion.Entidades.ModeloPortal;
using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioGanometro
    {
        List<EME_REGISTRO_VENTAS> ObtenerVentas(int id_director, int? localización, int? Anio, int? Periodo, int? Dia);

        List<SPEM_CONSULTARVENTASXPERIODOXSEMANAXDIA> ObtenerVentasXperiodoXsemanaXdia(int id_director, int anio, int periodo, int idciudad);

        void InsertarVenta(EME_REGISTRO_VENTAS venta);

        PERIODO_VENTA ObtenerPeriodo(DateTime fecha);

        List<EME_METAS_VENTAS> ObtenerMetas(PERIODO_VENTA p, int idCiudad);
    }
}