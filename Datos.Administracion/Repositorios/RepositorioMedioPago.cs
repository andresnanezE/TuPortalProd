using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoStone.DtoSitioWeb;
using Dominio.Administracion.Entidades.ModeloMedioPago;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioMedioPago : IRepositorioMedioPago
    {
        public List<MedioPagoAfiliado> ObtenerMedioPagoActualPorUsuario(string numDocu)
        {
            try
            {
                var contexto = new ContextoTuEmermedica();
                var resultado = contexto.ConsultaSql<MedioPagoAfiliado>("SCISP_OBTENER_LISTADO_MEDIO_PAGO @COD_TERC",
                    new SqlParameter("@COD_TERC", numDocu)).ToList();
                return resultado;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ResponseMedioPago> UpdateMedioPagoAsync(DatosActualizacionMedioPagoDto model)
        {
            SqlParameter[] @params;
            ResponseMedioPago respuesta;
            var contexto = new ContextoTuEmermedica();

            if (model.TipDeau == "C")
            {
                @params = new[]
                                {
                                   new SqlParameter("Num_Docu", model.IdentificationNumber),
                                   new SqlParameter("Email", model.Email),
                                   new SqlParameter("Telefono", model.Telefono),
                                   new SqlParameter("RMT_Cont", model.RMTCont),
                                   new SqlParameter("Tip_Deau", model.TipDeau)
                                 };

                respuesta = await contexto.Database.SqlQuery<ResponseMedioPago>("SCISP_InsertActualizacionMedioPagoAsesor @Num_Docu,@Email,@Telefono,@RMT_Cont,@Tip_Deau", @params).FirstOrDefaultAsync();                                

            }
            else
            {
                @params = new[]
                                {
                                  new SqlParameter("Num_Docu", model.IdentificationNumber),
                                  new SqlParameter("Email", model.Email),
                                  new SqlParameter("Telefono", model.Telefono),
                                  new SqlParameter("RMT_Cont", model.RMTCont),
                                  new SqlParameter("Tip_Deau", model.TipDeau),
                                  new SqlParameter("Cod_Banc", model.codBanc),
                                  new SqlParameter("Cue_Deau", model.CueDeau)
                                };

                respuesta = await contexto.Database.SqlQuery<ResponseMedioPago>("SCISP_InsertActualizacionMedioPagoAsesor @Num_Docu,@Email,@Telefono,@RMT_Cont,@Tip_Deau,@Cod_Banc,@Cue_Deau", @params).FirstOrDefaultAsync();
            }

            return respuesta;
        }

        public async Task UpdateMedioPagoIvrAsync(DatosActualizacionMedioPagoIvrDto model)
        {
            SqlParameter[] @params;
            var contexto = new ContextoTuEmermedica();
            
            @params = new[]
                            {
                                new SqlParameter("Id_CC_ACTFP", model.Id_CC_ACTFP),
                                new SqlParameter("EST_PASARELA", model.EST_PASARELA),
                                new SqlParameter("SESION_IVR", model.SESION_IVR)
                            };

            await contexto.Database.ExecuteSqlCommandAsync("SCISP_ActualizacionMedioPago @Id_CC_ACTFP,@EST_PASARELA,@SESION_IVR", @params);            
        }

        public async Task<IEnumerable<BancosDto>> GetBancosAsyc()
        {
            var contexto = new ContextoTuEmermedica();
            return await contexto.Database.SqlQuery<BancosDto>("SCISP_ObtenerBancos").ToListAsync();
        }
    }
}
