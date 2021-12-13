﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentacion.Mvc.App.Models
{
    public enum TipoLog
    {
        NuevaNoticia = 74,
        EditarNoticia = 75, 
        EliminarNoticia = 76,
        EditarDestacado = 77,
        EliminarDestacado = 78,
        NuevoBanner = 79,
        EditarBanner = 80,
        EliminarBanner = 81
    }

    public enum Menus
    {
        SinMenu = 0,
        Administracion = 1,
        Menu = 2,
        Usuarios = 3,
        Reportes,
        Detalle_de_afiliaciones = 5,
        Extracto_de_comisiones = 10,
        Consultas,
        Contratos = 12,
        Reporte_bajas = 14,
        FOSYGA,
        Perimetros,
        Bogota = 17,
        Chia = 18,
        Soacha = 19,
        Medellin = 20,
        Cali = 21,
        Neiva = 22,
        Villavicencio = 23,
        Bucaramanga = 24,
        Floridablanca,
        Piedecuesta,

        Tarjetas_transmilenio = 30,
        Afiliados_acumulados ,
        Seguimiento_asesores,
        Transacciones,
        Soportes_seguridad_social = 34,
        Apoyo_transporte,
        Caracterizacion_Campañas = 36,
        Cotizador,
        Juridico_interno = 41,
        Juridico_externo,
        Historico_comisiones,
        Sesion_en_vez_de = 46,
        Ayuda,
        Manual_usuario = 48,
        Reporte_reactivaciones = 49,
        Solicitudes_Internas = 57,
        Enviar_Solicitud,
        Consulta_de_solicitudes = 55,
        Administracion_sesion_en_vez_de = 58,
        Cargue_Tarifas_Campana = 59,
        Cargue_Tarifas_Plenas = 60,
        Familiares = 62,
        Destacados = 64,
        Noticias = 65,
        Reporte_Sesion_en_vez_de = 66,
        ReportesAfiliadosAcumulados = 31,
        ReporteApoyoRodamiento = 35,
        GeoZonificador = 36
    }
}