using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    /// <summary>
    /// JohnNelsonRodriguex
    /// </summary>
    public class RepositorioCalcularInclusiones : IRepositorioCalcularInclusiones
    {
        public IEnumerable<CalcularInclusiones> CalcularInclusiones(int rmt, int nPersonas, DateTime fechaInclusion, decimal tarifa)
        {
            try
            {
                using (var modelo = new ContextoStone())
                {
                    return modelo.ConsultaSql<CalcularInclusiones>("EMESP_CALCULAR_VALOR_PRORRATEO "
                        + rmt.ToString() + ",'" + nPersonas.ToString() + "'" + ",'"
                        + string.Format("{0:yyy-MM-dd}", fechaInclusion) + "'," + tarifa);
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                throw;
            }
        }

        public IEnumerable<decimal> TarifasContrato(int rmt)
        {
            try
            {
                using (var modelo = new ContextoStone())
                {
                    List<decimal> lista = new List<decimal>();

                    var tarifas = modelo.ConsultaSql<CalcularInclusiones>("EMESP_TARIFAS_COMTRATO " + rmt.ToString());

                    if (tarifas.Any())
                    {
                        lista = new List<decimal>();
                        tarifas.ForEach(t => lista.Add(t.tarifas));
                    }

                    return lista.ToList();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                throw;
            }
        }

        public IEnumerable<TarifasInclusiones> TarifasInclusiones(int rmt)
        {
            try
            {
                using (var modelo = new ContextoStone())
                {
                    List<decimal> lista = new List<decimal>();

                    var tarifas = modelo.ConsultaSql<TarifasInclusiones>("EMSP_TARIFAS_INCLUSIONES @RMT_CONT",
                        new SqlParameter("@RMT_CONT", rmt));

                    return tarifas;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                throw;
            }
        }
    }
}