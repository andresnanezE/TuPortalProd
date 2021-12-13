using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioSesionEnVezDe : IRepositorioSesionEnVezDe
    {
        public string CargaTerminosCondiciones()
        {
            string terminos = null;
            using (var modelo = new ContextoProcesos())
            {
                terminos = modelo.ConsultaSql<Terminos>("EMSP_CONSULTA_TERMINOS_CONDICIONES").ToList()[0].TERMINOS;
            }
            return terminos;
        }

        public IEnumerable<SesionEnVezDe> ValidarDocumento(string documento, string usuario)
        {
            IEnumerable<SesionEnVezDe> listaDatos = null;
            using (var modelo = new ContextoProcesos())
            {
                try
                {
                    listaDatos = modelo.ConsultaSql<SesionEnVezDe>("EMSP_VALIDAR_DOCUMENTO_SESION @DOCUMENTO, @USUARIO",
                        new SqlParameter("@DOCUMENTO", documento),
                        new SqlParameter("@USUARIO", usuario.ToString())
                        ).ToList();
                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                }
            }
            var RESULTADO = listaDatos;
            return listaDatos;
        }
    }
}