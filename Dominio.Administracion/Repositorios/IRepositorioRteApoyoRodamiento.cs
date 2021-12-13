using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioRteApoyoRodamiento
    {
        IEnumerable<ApoyoRodamientoPeriodo> PeriodosNoDefinitivos();
    }
}