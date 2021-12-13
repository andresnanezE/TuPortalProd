using AutoMapper;
using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioCotizadorTarifas : IRepositorioCotizadorTarifas
    {
        ////llamado a los metodos del web services Emermedicaws

        //WebServiceEmermedicaCotizador ws;
        //AuthHeader auth;

        private RepositorioUsuarios rep = null;

        public RepositorioCotizadorTarifas()
        {
            rep = new RepositorioUsuarios();
            //ws = new WebServiceEmermedicaCotizador();
            //auth = new AuthHeader();
            //auth.Usuario = "";
            //auth.Password = "";
            //ws.AuthHeaderValue = auth;
        }

        public UsuarioCiudad getUsuarioCiudad(String sUsuario)
        {
            CTSP_CONSULTA_USUARIO_CIUDAD usuario = ConsultaUsuario(sUsuario);

            Mapper.CreateMap<CTSP_CONSULTA_USUARIO_CIUDAD, UsuarioCiudad>().
                ForMember(ev => ev.USUARIO, m => m.MapFrom(a => a.USUARIO)).
                ForMember(ev => ev.CONTRASENA, m => m.MapFrom(a => a.CONTRASENA)).
                ForMember(ev => ev.CIUDAD, m => m.MapFrom(a => a.CIUDAD)).
                ForMember(ev => ev.ESTADO, m => m.MapFrom(a => a.ESTADO));

            UsuarioCiudad usuarioCiudad = Mapper.Map<CTSP_CONSULTA_USUARIO_CIUDAD, UsuarioCiudad>(usuario);

            return usuarioCiudad;
        }

        public IEnumerable<TipoTarifa> getTiposTarifas()
        {
            try
            {
                List<CTSP_CONSULTA_TIPO_TARIFA> lst = new List<CTSP_CONSULTA_TIPO_TARIFA>();

                using (var contexto = new ContextoPortal())
                {
                    lst = contexto.Database.SqlQuery<CTSP_CONSULTA_TIPO_TARIFA>(
                        "CTSP_CONSULTA_TIPO_TARIFA"
                        ).ToList();
                }

                Mapper.CreateMap<CTSP_CONSULTA_TIPO_TARIFA, TipoTarifa>().
                   ForMember(ev => ev.CODIGO_TARIFA, m => m.MapFrom(a => a.CODIGO_TARIFA)).
                   ForMember(ev => ev.TIPO_TARIFA, m => m.MapFrom(a => a.TIPO_TARIFA));

                IEnumerable<TipoTarifa> iETiposTarifas = Mapper.Map<List<CTSP_CONSULTA_TIPO_TARIFA>, IEnumerable<TipoTarifa>>(lst);
                return iETiposTarifas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Campana> getCampanas(String sCodigoTarifa)
        {
            try
            {
                List<CTSP_CONSULTA_CAMPANA> lst = new List<CTSP_CONSULTA_CAMPANA>();

                using (var contexto = new ContextoPortal())
                {
                    lst = contexto.Database.SqlQuery<CTSP_CONSULTA_CAMPANA>(
                            "CTSP_CONSULTA_CAMPANA @CODIGO_TARIFA",
                            new SqlParameter("CODIGO_TARIFA", sCodigoTarifa)
                        ).ToList();
                }

                Mapper.CreateMap<CTSP_CONSULTA_CAMPANA, Campana>().
                   ForMember(ev => ev.CAMPANA_TARIFA, m => m.MapFrom(a => a.CAMPANA_TARIFA)).
                   ForMember(ev => ev.RUTA_IMAGEN, m => m.MapFrom(a => a.RUTA_IMAGEN)).
                   ForMember(ev => ev.CARACTERIZACION, m => m.MapFrom(a => a.CARACTERIZACION)).
                   ForMember(ev => ev.CODIGO_TARIFA, m => m.MapFrom(a => a.CODIGO_TARIFA)).
                   ForMember(ev => ev.FECHA_VENCIMIENTO_TARIFA, m => m.MapFrom(a => a.FECHA_VENCIMIENTO_TARIFA));

                IEnumerable<Campana> iECampanas = Mapper.Map<List<CTSP_CONSULTA_CAMPANA>, IEnumerable<Campana>>(lst);

                return iECampanas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<TarifaBase> getTarifasBase(String sCiudad, int iPersonas, String sCodigoTarifa, String sCampana, String sUsuario)
        {
            try
            {
                var lst = new List<CTSP_CONSULTA_TARIFAS_BASE>();

                using (var contexto = new ContextoPortal())
                {
                    lst = contexto.Database.SqlQuery<CTSP_CONSULTA_TARIFAS_BASE>(
                        "CTSP_CONSULTA_TARIFAS_BASE @i_ciudad, @i_numpersonas, @i_tipo_Tarifa, @i_campana, @i_usuario",
                                new SqlParameter("i_ciudad", sCiudad),
                                new SqlParameter("i_numpersonas", iPersonas),
                                new SqlParameter("i_tipo_Tarifa", sCodigoTarifa),
                                new SqlParameter("i_campana", sCampana),
                                new SqlParameter("i_usuario", sUsuario)
                        ).ToList();
                }

                Mapper.CreateMap<CTSP_CONSULTA_TARIFAS_BASE, TarifaBase>().
                   ForMember(ev => ev.TIPO_TARIFA, m => m.MapFrom(a => a.TIPO_TARIFA)).
                   ForMember(ev => ev.CAMPANA_TARIFA, m => m.MapFrom(a => a.CAMPANA_TARIFA)).
                   ForMember(ev => ev.CIUDAD, m => m.MapFrom(a => a.CIUDAD)).
                   ForMember(ev => ev.RANGO_INICIAL_PERSONA, m => m.MapFrom(a => a.RANGO_INICIAL_PERSONA)).
                   ForMember(ev => ev.RANGO_FINAL_PERSONA, m => m.MapFrom(a => a.RANGO_FINAL_PERSONA)).
                   ForMember(ev => ev.MODALIDAD_PAGO, m => m.MapFrom(a => a.MODALIDAD_PAGO)).
                   ForMember(ev => ev.FORMA_PAGO, m => m.MapFrom(a => a.FORMA_PAGO)).
                   ForMember(ev => ev.VALOR_TARIFA, m => m.MapFrom(a => a.VALOR_TARIFA)).
                   ForMember(ev => ev.VALOR_IVA_TARIFA, m => m.MapFrom(a => a.VALOR_IVA_TARIFA)).
                   ForMember(ev => ev.FECHA_VENCIMIENTO_TARIFA, m => m.MapFrom(a => a.FECHA_VENCIMIENTO_TARIFA)).
                   ForMember(ev => ev.VALOR_AHORRO, m => m.MapFrom(a => a.VALOR_AHORRO)).
                   ForMember(ev => ev.VALOR_comision, m => m.MapFrom(a => a.VALOR_COMISION)).
                   ForMember(ev => ev.PORCENTAJE_AHORRO, m => m.MapFrom(a => a.PORCENTAJE_AHORRO)).
                   ForMember(ev => ev.VALOR_CUOTA_PERIODICA, m => m.MapFrom(a => a.VALOR_CUOTA_PERIODICA)).
                   ForMember(ev => ev.VALOR_AHORRO_LIQUIDACION, m => m.MapFrom(a => a.VALOR_AHORRO_LIQUIDACION)).
                   ForMember(ev => ev.TOTAL_TARIFA_MES, m => m.MapFrom(a => a.TOTAL_TARIFA_MES)).
                   ForMember(ev => ev.ID_ESTADO, m => m.MapFrom(a => a.ID_ESTADO));

                IEnumerable<TarifaBase> iETarifasBase = Mapper.Map<List<CTSP_CONSULTA_TARIFAS_BASE>, IEnumerable<TarifaBase>>(lst);

                return iETarifasBase;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Usp_ObtenerDetalleValorFactor> getDetalleFactores(int idFactor)
        {
            try
            {
                var lst = new List<Usp_ObtenerDetalleValorFactor>();

                using (var contexto = new ContextoPortal())
                {
                    lst = contexto.Database.SqlQuery<Usp_ObtenerDetalleValorFactor>(
                        "Usp_ObtenerDetalleValorFactor @IdFactor",
                                new SqlParameter("IdFactor", idFactor)
                        ).ToList();
                }

                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool updateDetalleFactor(ModificarFactorDto modificar)
        {
            try
            {
                using (var contexto = new ContextoPortal())
                {
                    switch (modificar.IdFactor)
                    {
                        //Tipo Área Protegida
                        case 1:
                            var valueSelectTipoAP = (from u in contexto.Tipos_AP where u.Id_area == modificar.IdDetalleFactor select u).FirstOrDefault();
                            valueSelectTipoAP.Tipo_area = modificar.NombreDetalleFactor;
                            valueSelectTipoAP.Valor = modificar.ValorDetalleFactor;
                            valueSelectTipoAP.Estado = modificar.Estado;
                            valueSelectTipoAP.Valor_Constante = modificar.ValorDetalleConstante;
                            valueSelectTipoAP.Valor_Exponencial = modificar.ValorDetalleExponencial;
                            contexto.SaveChanges();
                            break;

                        //Sector Económico
                        case 2:
                            var valueSelectSectorEc = (from u in contexto.SECTORES where u.Id_sector == modificar.IdDetalleFactor select u).FirstOrDefault();
                            valueSelectSectorEc.Descripcion = modificar.NombreDetalleFactor;
                            valueSelectSectorEc.Valor = modificar.ValorDetalleFactor;
                            valueSelectSectorEc.Estado = modificar.Estado;
                            valueSelectSectorEc.Valor_Constante = modificar.ValorDetalleConstante;
                            valueSelectSectorEc.Valor_Exponencial = modificar.ValorDetalleExponencial;
                            valueSelectSectorEc.Factor_Ajuste = modificar.FactorAjuste;
                            contexto.SaveChanges();
                            break;

                        //Ciudad
                        case 3:
                            var ciudadFactor = (from u in contexto.CiudadesFactor where u.Id == modificar.IdDetalleFactor select u).FirstOrDefault();
                            ciudadFactor.Nombre = modificar.NombreDetalleFactor;
                            ciudadFactor.Valor = modificar.ValorDetalleFactor;
                            ciudadFactor.Estado = modificar.Estado;
                            ciudadFactor.Valor_Constante = modificar.ValorDetalleConstante;
                            ciudadFactor.Valor_Exponencial = modificar.ValorDetalleExponencial;
                            ciudadFactor.ValorIndicadorFuga = modificar.ValorIndicadorFuga;
                            ciudadFactor.GastosAdministrativos = modificar.GastosAdministrativos;
                            ciudadFactor.GastosComerciales = modificar.GastosComerciales;
                            ciudadFactor.FactorUtilidad = modificar.FactorUtilidad;
                            ciudadFactor.ValorMinimoCotizacion = modificar.ValorMinimoCotizacion;
                            contexto.SaveChanges();
                            break;

                        //Tipo Riesgo
                        case 4:
                            var valueSelectTipoRiesgo = (from u in contexto.TipoRiesgo where u.Id == modificar.IdDetalleFactor select u).FirstOrDefault();
                            valueSelectTipoRiesgo.Nombre = modificar.NombreDetalleFactor;
                            valueSelectTipoRiesgo.Valor = modificar.ValorDetalleFactor;
                            valueSelectTipoRiesgo.Estado = modificar.Estado;
                            valueSelectTipoRiesgo.Valor_Constante = modificar.ValorDetalleConstante;
                            valueSelectTipoRiesgo.Valor_Exponencial = modificar.ValorDetalleExponencial;
                            contexto.SaveChanges();
                            break;

                        //Prima Pura
                        case 12:
                            var valueSelectPrimaPura = (from u in contexto.Prima where u.Id == modificar.IdDetalleFactor select u).FirstOrDefault();
                            valueSelectPrimaPura.Nombre = modificar.NombreDetalleFactor;
                            valueSelectPrimaPura.Valor = modificar.ValorDetalleFactor;
                            valueSelectPrimaPura.Estado = modificar.Estado;
                            contexto.SaveChanges();
                            break;

                        //Factor Adicional Uno
                        case 15:
                            var valueSelectFactorAdicional1 = (from u in contexto.FactorAdicional1 where u.Id == modificar.IdDetalleFactor select u).FirstOrDefault();
                            valueSelectFactorAdicional1.Nombre = modificar.NombreDetalleFactor;
                            valueSelectFactorAdicional1.Valor = modificar.ValorDetalleFactor;
                            valueSelectFactorAdicional1.Estado = modificar.Estado;
                            valueSelectFactorAdicional1.Valor_Constante = modificar.ValorDetalleConstante;
                            valueSelectFactorAdicional1.Valor_Exponencial = modificar.ValorDetalleExponencial;
                            contexto.SaveChanges();
                            break;

                        //Factor Adicional Dos
                        case 16:
                            var valueSelectFactorAdicional2 = (from u in contexto.FactorAdicional2 where u.Id == modificar.IdDetalleFactor select u).FirstOrDefault();
                            valueSelectFactorAdicional2.Nombre = modificar.NombreDetalleFactor;
                            valueSelectFactorAdicional2.Valor = modificar.ValorDetalleFactor;
                            valueSelectFactorAdicional2.Estado = modificar.Estado;
                            valueSelectFactorAdicional2.Valor_Constante = modificar.ValorDetalleConstante;
                            valueSelectFactorAdicional2.Valor_Exponencial = modificar.ValorDetalleExponencial;
                            contexto.SaveChanges();
                            break;

                        //Factor Adicional Tres
                        case 17:
                            var valueSelectFactorAdicional3 = (from u in contexto.FactorAdicional3 where u.Id == modificar.IdDetalleFactor select u).FirstOrDefault();
                            valueSelectFactorAdicional3.Nombre = modificar.NombreDetalleFactor;
                            valueSelectFactorAdicional3.Valor = modificar.ValorDetalleFactor;
                            valueSelectFactorAdicional3.Estado = modificar.Estado;
                            valueSelectFactorAdicional3.Valor_Constante = modificar.ValorDetalleConstante;
                            valueSelectFactorAdicional3.Valor_Exponencial = modificar.ValorDetalleExponencial;
                            contexto.SaveChanges();
                            break;

                        //Factor Adicional Cuatro
                        case 18:
                            var valueSelectFactorAdicional4 = (from u in contexto.FactorAdicional4 where u.Id == modificar.IdDetalleFactor select u).FirstOrDefault();
                            valueSelectFactorAdicional4.Nombre = modificar.NombreDetalleFactor;
                            valueSelectFactorAdicional4.Valor = modificar.ValorDetalleFactor;
                            valueSelectFactorAdicional4.Estado = modificar.Estado;
                            valueSelectFactorAdicional4.Valor_Constante = modificar.ValorDetalleConstante;
                            valueSelectFactorAdicional4.Valor_Exponencial = modificar.ValorDetalleExponencial;
                            contexto.SaveChanges();
                            break;

                        //Factor Adicional Cinco
                        case 19:
                            var valueSelectFactorAdicional5 = (from u in contexto.FactorAdicional5 where u.Id == modificar.IdDetalleFactor select u).FirstOrDefault();
                            valueSelectFactorAdicional5.Nombre = modificar.NombreDetalleFactor;
                            valueSelectFactorAdicional5.Valor = modificar.ValorDetalleFactor;
                            valueSelectFactorAdicional5.Estado = modificar.Estado;
                            valueSelectFactorAdicional5.Valor_Constante = modificar.ValorDetalleConstante;
                            valueSelectFactorAdicional5.Valor_Exponencial = modificar.ValorDetalleExponencial;
                            contexto.SaveChanges();
                            break;

                        default:
                            return false;
                    }
                }

                return true;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        }

        /// <summary>
        /// Aplica solo para los factores de tipo fijos.
        /// </summary>
        public bool updateValoresFactor(int idFactor, int idTipoFactor, decimal valorConstante, decimal valorExponente)
        {
            try
            {
                using (var contexto = new ContextoPortal())
                {
                    switch (idTipoFactor)
                    {
                        //Tipo Factor Dinamicos
                        case 2:
                            var valueSelect = (from u in contexto.Detalle_Factor where u.Id_Factor == idFactor select u).FirstOrDefault();                            
                            valueSelect.ValorConstante = valorConstante;
                            valueSelect.ValorExponente = valorExponente;
                            
                            contexto.SaveChanges();
                            break;

                        default:
                            return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Detalle_Factor getValorDetalle_Factor(int idFactor)
        {
            var retorno = new Detalle_Factor();
            using (var bd = new ContextoPortal())
            {
                var vGet = (from u in bd.Detalle_Factor where u.Id_Factor == idFactor select u);
                if (vGet != null)
                    retorno = vGet.FirstOrDefault();
            }
            return retorno;
        }

        #region IReporsitorioUsuarios HerramientasComercial

        public CTSP_CONSULTA_USUARIO_CIUDAD ConsultaUsuario(string sUsuario)
        {
            try
            {
                List<CTSP_CONSULTA_USUARIO_CIUDAD> lstUsr = new List<CTSP_CONSULTA_USUARIO_CIUDAD>();

                using (var contexto = new ContextoPortal())
                {
                    lstUsr = contexto.Database.SqlQuery<CTSP_CONSULTA_USUARIO_CIUDAD>(
                        "CTSP_CONSULTA_USUARIO_CIUDAD @DOCUMENTO",
                                new SqlParameter("DOCUMENTO", sUsuario)
                        ).ToList();
                }
                return lstUsr.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion IReporsitorioUsuarios HerramientasComercial
    }
}