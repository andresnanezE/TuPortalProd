using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioRteTarjetasTransmilenio
    {
        IEnumerable<TarjetasTransmilenioPeriodo> PeriodosNoDefinitivos();
    }
}