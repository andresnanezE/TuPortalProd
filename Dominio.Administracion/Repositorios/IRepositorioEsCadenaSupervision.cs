using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioEsCadenaSupervision
    {
        IEnumerable<CadenaSupervision> EsCadenaSupervision(string cc);
    }
}