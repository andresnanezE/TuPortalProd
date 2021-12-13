using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioEsCadenaSupervision : IRepositorioEsCadenaSupervision
    {
        public IEnumerable<CadenaSupervision> EsCadenaSupervision(string cc)
        {
            try
            {
                IEnumerable<CadenaSupervision> listaDatos = null;
                using (var modelo = new ContextoPortal())
                {
                    listaDatos = modelo.ConsultaSql<CadenaSupervision>("EMSP_ESCADENA_DESUPERVISION @CC_ASESOR",
                        new SqlParameter("@CC_ASESOR", cc)
                        ).ToList();
                }
                return listaDatos;
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                throw;
            }
        }
    }
}