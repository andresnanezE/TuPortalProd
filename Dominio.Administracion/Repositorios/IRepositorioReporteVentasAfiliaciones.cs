using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioReporteVentasAfiliaciones
    {
        IEnumerable<AfiliacionesPeriodo> Obtener_Periodos();

        IEnumerable<resultadosConsultaAfiliacionResumen> Consultar_Detalle_Afiliacion_Resumen(DatosAfiliacion _datos);

        IEnumerable<ResultadosConsultaAfiliacionResumenTabla> Consultar_Detalle_Afiliacion_ResumenTabla(DatosConsultaAfiliacion _datos);

        IEnumerable<AfiliacionesFiltro> ObtenerFiltro_x_Rol(int _rol);

        IEnumerable<AfiliacionesFiltro> ObtenerFiltroRoles(string[] roles);

        IEnumerable<NovedadesHomologadas> Obtener_Novedades_Homologadas(List<string> comisiona);

        IEnumerable<Ciudades> Obtener_Ciudades_Homologadas(string user);

        IEnumerable<Canales> Obtener_Canales(string user);

        string MensajeCantidadRegistrosNetos(DatosConsultaAfiliacion _datos);

        List<CentrosCosto> ObtenerCentrosCosto();

        List<Escalera> ObtenerEscaleras();
    }
}