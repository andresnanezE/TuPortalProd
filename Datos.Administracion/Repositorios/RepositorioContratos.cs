// ----------------------------------------------------------------------------------------------
// <copyright file="RepositorioNovedades.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

//using Aplicacion.Administracion.Dto.DtoSitioWeb;
using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloSitioWeb;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioContratos : IRepositorioContratos
    {
        #region Readonly & Static Fields

#pragma warning disable CS0169 // The field 'RepositorioContratos._manejadorLogs' is never used
        private readonly IRepositorioLogs _manejadorLogs;
#pragma warning restore CS0169 // The field 'RepositorioContratos._manejadorLogs' is never used

        #endregion Readonly & Static Fields

        #region RepositorioBeneficiarios Members

        /// <summary>
        /// obtiene el listado de contratos, contratantes o ascesores deacuerdo al numero espesificado
        /// </summary>
        /// <param name="codigo">criterio de busqueda. Id usuario contrato o beneficiario</param>
        /// <param name="usuario">usuario ejecutor de la consulta</param>
        /// <param name="rol">rol del usuario</param>
        /// <returns>istado de elementos que coinsidan con el criterio de busquda</returns>
        public IEnumerable<SPEM_CONSULTACONTRATOS> ObtenerConsultaContratos(decimal codigo, decimal usuario, int rol)
        {
            try
            {
                var listaProcesos = new List<SPEM_CONSULTACONTRATOS>();

                using (var modelProcesos = new ContextoProcesos())
                {
                    if (codigo > 0)
                    {
                        listaProcesos =
                            modelProcesos.Database.SqlQuery<SPEM_CONSULTACONTRATOS>(
                                "SPEM_CONSULTACONTRATOS_LIGERA_V2 @criterio, @asec, @rol",
                                new SqlParameter("criterio", codigo),
                                new SqlParameter("asec", usuario),
                                new SqlParameter("rol", rol)).ToList();
                    }
                }
                return listaProcesos.ToList();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new List<SPEM_CONSULTACONTRATOS>();
            }
        }

        /// <summary>
        /// obtiene el listado de contratos, contratantes o ascesores deacuerdo al nombre de la persona dado espesificado
        /// </summary>
        /// <param name="nombre">criterio de busqueda. Id usuario contrato o beneficiario</param>
        /// <param name="usuario">usuario ejecutor de la consulta</param>
        /// <param name="rol">rol del usuario</param>
        /// <returns>istado de elementos que coinsidan con el criterio de busquda</returns>
        public IEnumerable<SPEM_CONSULTACONTRATOS> ObtenerConsultaContratosPorNombre(string nombre, decimal usuario, int rol)
        {
            try
            {
                var listaProcesos = new List<SPEM_CONSULTACONTRATOS>();

                using (var modelProcesos = new ContextoProcesos())
                {
                    if (!string.IsNullOrWhiteSpace(nombre))
                    {
                        listaProcesos =
                            modelProcesos.Database.SqlQuery<SPEM_CONSULTACONTRATOS>(
                                "SPEM_CONSULTACONTRATOS_POR_NOMBRE @criterio, @asec, @rol",
                                    new SqlParameter("criterio", nombre),
                                    new SqlParameter("asec", usuario),
                                    new SqlParameter("rol", rol)
                                ).ToList();
                    }
                }
                return listaProcesos.ToList();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new List<SPEM_CONSULTACONTRATOS>();
            }
        }

        public IEnumerable<SPEM_CONSULTACONTRATOS> ObtenerContratos(decimal codigo)
        {
            try
            {
                var rmtProc = (int)codigo;
                decimal contratante = 0;
                decimal beneficiario = 0;

                var listaProcesos = new List<SPEM_CONSULTACONTRATOS>();

                using (var modelProcesos = new ContextoProcesos())
                {
                    if (codigo > 0)
                    {
                        listaProcesos =
                            modelProcesos.Database.SqlQuery<SPEM_CONSULTACONTRATOS>(
                                "SPEM_CONSULTACONTRATOS @rmt, @terc, @cc, @asec",
                                new SqlParameter("rmt", rmtProc),
                                new SqlParameter("terc", contratante),
                                new SqlParameter("cc", beneficiario),
                                new SqlParameter("asec", rmtProc)).ToList();
                    }
                }

                return listaProcesos.ToList();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new List<SPEM_CONSULTACONTRATOS>();
            }
        }

        public IEnumerable<SPEM_CONSULTACONTRATOS> ObtenerContratosContratante(decimal codigo, int rmt)
        {
            try
            {
                decimal contratante = 0;
                decimal beneficiario = 0;

                var listaProcesos = new List<SPEM_CONSULTACONTRATOS>();

                using (var modelProcesos = new ContextoProcesos())
                {
                    if (listaProcesos.Count == 0)
                    {
                        contratante = codigo;
                        var rmtProc = rmt;
                        beneficiario = 0;
                        contratante = codigo;
                        listaProcesos =
                            modelProcesos.Database.SqlQuery<SPEM_CONSULTACONTRATOS>(
                                "SPEM_CONSULTACONTRATOS_CONTRATANTE @rmt,@terc,@cc,@asec",
                                new SqlParameter("rmt", rmtProc),
                                new SqlParameter("terc", contratante),
                                new SqlParameter("cc", beneficiario),
                                new SqlParameter("asec", rmtProc)).ToList();
                    }
                }

                return listaProcesos.ToList();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new List<SPEM_CONSULTACONTRATOS>();
            }
        }

        public IEnumerable<SPEM_CONSULTACONTRATOS> ObtenerContratosBeneficiario(decimal codigo, int rmt,
            decimal CCcontratante, string contratante)
        {
            try
            {
                var listaProcesos = new List<SPEM_CONSULTACONTRATOS>();

                using (var modelProcesos = new ContextoProcesos())
                {
                    if (codigo > 0)
                    {
                        listaProcesos =
                            modelProcesos.Database.SqlQuery<SPEM_CONSULTACONTRATOS>(
                                "SPEM_CONSULTACONTRATOS_BENEFICIARIO @rmt, @terc, @cc, @contratante",
                                new SqlParameter("rmt", rmt),
                                new SqlParameter("terc", CCcontratante),
                                new SqlParameter("cc", codigo),
                                new SqlParameter("contratante", contratante)).ToList();
                    }
                }

                return listaProcesos.ToList();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new List<SPEM_CONSULTACONTRATOS>();
            }
        }

        public IEnumerable<ConsultaBeneficiario> ObtenerBeneficiario(string benef)
        {
            try
            {
                var listaProcesos = new List<ConsultaBeneficiario>();

                using (var modelProcesos = new ContextoProcesos())
                {
                    if (listaProcesos.Count == 0)
                    {
                        listaProcesos =
                            modelProcesos.Database.SqlQuery<ConsultaBeneficiario>("SPEM_CONSULTABENEF @bene",
                                new SqlParameter("bene", benef)).ToList();
                    }
                }

                return listaProcesos.ToList();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new List<ConsultaBeneficiario>();
            }
        }

        public IEnumerable<ConsultaFactura> ObtenerFacturas(string doc)
        {
            try
            {
                var listaProcesos = new List<ConsultaFactura>();

                using (var modelProcesos = new ContextoProcesos())
                {
                    if (listaProcesos.Count == 0)
                    {
                        listaProcesos =
                            modelProcesos.Database.SqlQuery<ConsultaFactura>("EMSP_CONSULTA_CONTRATOS_USUARIOS_FACTURAS @NumeroDocumento",
                                new SqlParameter("NumeroDocumento", doc)).ToList();
                    }
                }

                return listaProcesos.ToList();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new List<ConsultaFactura>();
            }
        }

        /// <summary>
        /// Obtiene contratante / beneficiario con observación que indica si permite inclusión o no
        /// </summary>
        /// <param name="rmtCont">RMT_CONT de stone</param>
        /// <param name="codTerc">Identificación de contratante</param>
        /// <param name="codBene">Identificación de beneficiarios</param>
        /// <returns>istado de elementos que coinsidan con el criterio de busquda</returns>
        public IEnumerable<SPCCBENEFINCLUSIONTUPORTAL> ValidacionBeneficiarioInclusion(int rmtCont, string codTerc, string codBene)
        {
            var resultado = new List<SPCCBENEFINCLUSIONTUPORTAL>();
            try
            {
                using (var modelStone = new ContextoStone())
                {
                    resultado = modelStone.Database.SqlQuery<SPCCBENEFINCLUSIONTUPORTAL>(
                            "SPCCBENEFINCLUSIONTUPORTAL @RMT_CONT, @COD_TERC, @COD_BENE",
                            new SqlParameter("@RMT_CONT", rmtCont),
                            new SqlParameter("@COD_TERC", codTerc ?? ""),
                            new SqlParameter("@COD_BENE", codBene ?? "")).ToList();
                }

                return resultado;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return resultado;
            }
        }
    }

    #endregion RepositorioBeneficiarios Members
}