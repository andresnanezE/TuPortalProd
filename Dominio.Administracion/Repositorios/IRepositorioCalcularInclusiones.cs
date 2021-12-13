using Dominio.Administracion.Entidades;
using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    /// <summary>
    /// JohnNelsonRodriguex
    /// </summary>
    public interface IRepositorioCalcularInclusiones
    {
        IEnumerable<CalcularInclusiones> CalcularInclusiones(int rmt, int nPersonas, DateTime fechaInclusion, decimal tarifa);

        IEnumerable<decimal> TarifasContrato(int rmt);

        IEnumerable<TarifasInclusiones> TarifasInclusiones(int rmt);
    }
}