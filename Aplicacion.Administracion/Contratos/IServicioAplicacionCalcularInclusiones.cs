using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using System;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    /// <summary>
    /// JohnNelsonRodriguex
    /// </summary>
    public interface IServicioAplicacionCalcularInclusiones
    {
        IEnumerable<CalcularInclusionesDto> CalcularInclusiones(int rmt, int nPersonas, DateTime fechaInclusion, decimal tarifa);

        IEnumerable<decimal> TarifasContrato(int rmt);

        IEnumerable<TarifasInclusiones> TarifasInclusiones(int rmt);
    }
}