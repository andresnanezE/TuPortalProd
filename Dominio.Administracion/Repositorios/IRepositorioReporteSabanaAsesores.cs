using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioReporteSabanaAsesores
    {
        IEnumerable<AfiliacionesPeriodo> Obtener_Periodos();

        //IEnumerable<resultadosConsultaAfiliacionResumen> Consultar_Detalle_Afiliacion_Resumen(DatosConsultaAfiliacion _datos);
        ConsultaDoble Consultar_Detalle_Afiliacion_Resumen(DatosConsultaAfiliacion _datos);

        IEnumerable<ResultadosConsultaAfiliacionResumenTabla> Consultar_Detalle_Afiliacion_ResumenTabla(DatosConsultaAfiliacion _datos);

        IEnumerable<AfiliacionesFiltro> ObtenerFiltro_x_Rol(int _rol);

        IEnumerable<NovedadesHomologadas> Obtener_Novedades_Homologadas(List<string> comisiona);

        IEnumerable<Ciudades> Obtener_Ciudades_Homologadas(string user);

        IEnumerable<Canales> Obtener_Canales(string user);
    }
}