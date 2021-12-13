using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioSolicitudesInternas
    {
        void AgregarNota(SolicitudesInternasNotas notas);

        IEnumerable<SolicitudesInternasNotas> ObtenerNotas(int IdTicket);

        void AgregarSolicitudInterna(SolicitudesInternas solicitud);

        IEnumerable<SolicitudesInternas> ObtenerSolicitudesInternas(decimal cod_ases);
    }
}