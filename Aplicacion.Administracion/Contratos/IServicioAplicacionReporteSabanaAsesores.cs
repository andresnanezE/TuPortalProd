using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionReporteSabanaAsesores
    {
        IEnumerable<AfiliacionesPeriodoDto> Obtener_Periodos();

        //IEnumerable<resultadosConsultaAfiliacionResumenDto> Consultar_Detalle_Afiliacion_Resumen(DatosConsultaAfiliacionDto datos);
        ConsultaDobleDto Consultar_Detalle_Afiliacion_Resumen(DatosConsultaAfiliacionDto datos);

        IEnumerable<ResultadosConsultaAfiliacionResumenTablaDto> Consultar_Detalle_Afiliacion_ResumenTabla(
            DatosConsultaAfiliacionDto datos);

        IEnumerable<AfiliacionesFiltroDto> ObtenerFiltro_x_Rol(int _rol);

        IEnumerable<NovedadesHomologadasDto> Obtener_Novedades_Homologadas(List<string> comisiona);

        IEnumerable<CiudadesDto> Obtener_Ciudades_Homologadas(string user);

        IEnumerable<CanalesDto> Obtener_Canales(string user);
    }
}