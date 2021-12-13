using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionReporteVentasAfiliaciones
    {
        IEnumerable<AfiliacionesPeriodoDto> Obtener_Periodos();

        IEnumerable<resultadosConsultaAfiliacionResumenDto> Consultar_Detalle_Afiliacion_Resumen(
            DatosAfiliacionDto datos);

        IEnumerable<ResultadosConsultaAfiliacionResumenTablaDto> Consultar_Detalle_Afiliacion_ResumenTabla(
            DatosConsultaAfiliacionDto datos);

        IEnumerable<AfiliacionesFiltroDto> ObtenerFiltro_x_Rol(int _rol);

        IEnumerable<AfiliacionesFiltroDto> ObtenerFiltroRoles(string[] roles);

        IEnumerable<NovedadesHomologadasDto> Obtener_Novedades_Homologadas(List<string> comisiona);

        IEnumerable<CiudadesDto> Obtener_Ciudades_Homologadas(string user);

        IEnumerable<CanalesDto> Obtener_Canales(string user);

        string MensajeCantidadRegistrosNetos(DatosConsultaAfiliacionDto datos);

        List<CentrosCosto> ObtenerCentrosCosto();

        List<Escalera> ObtenerEscaleras();
    }
}