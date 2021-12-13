using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioSesionEnVezDe
    {
        string CargaTerminosCondiciones();

        IEnumerable<SesionEnVezDe> ValidarDocumento(string documento, string usuario);
    }
}