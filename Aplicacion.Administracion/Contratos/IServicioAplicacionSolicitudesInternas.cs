using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

//using Dominio.Administracion.Entidades.MapperDto;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionSolicitudesInternas
    {
        void AgregarNota(SolicitudesInternasNotasDto notas);

        IEnumerable<SolicitudesInternasNotasDto> ObtenerNotas(int IdTicket);

        void AgregarSolicitusInterna(SolicitudesInternasDto solicitud);

        IEnumerable<SolicitudesInternasDto> ObtenerSolicitudesInternas(decimal codAses);
    }
}