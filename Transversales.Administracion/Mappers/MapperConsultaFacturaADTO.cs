//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

namespace Transversales.Administracion.Mappers
{
    public class MapperConsultaFacturaADTO
    {
        public static IEnumerable<ConsultaFacturaDto> AdaptarMenu(IEnumerable<ConsultaFactura> origen)
        {
            List<ConsultaFacturaDto> vRetorno = new List<ConsultaFacturaDto>();
            if (origen != null)
            {
                foreach (var index in origen)
                {
                    vRetorno.Add(EntidadConsultaFacturaADTO(index));
                }
            }
            return vRetorno;
        }

        public static ConsultaFacturaDto EntidadConsultaFacturaADTO(ConsultaFactura item)
        {
            ConsultaFacturaDto vRetorno = new ConsultaFacturaDto()
            {
                ABR_DOCU = item.ABR_DOCU,
                CLA_CONT = item.CLA_CONT,
                COD_ASES = item.COD_ASES,
                COD_CCOS = item.COD_CCOS,
                COD_DETE = item.COD_DETE,
                COD_DOCU = item.COD_DOCU,
                COD_EMPR = item.COD_EMPR,
                COD_MDUR = item.COD_MDUR,
                COD_MONE = item.COD_MONE,
                COD_PLAN = item.COD_PLAN,
                COD_PVTA = item.COD_PVTA,
                COD_REFE = item.COD_REFE,
                COD_TERC = item.COD_TERC,
                COD_TIPC = item.COD_TIPC,
                DES_CONT = item.DES_CONT,
                DES_TIPC = item.DES_TIPC,
                DIA_FACT = item.DIA_FACT,
                DIR_COBR = item.DIR_COBR,
                DOC_ADMI = item.DOC_ADMI,
                FECHA_FINAL = item.FECHA_FINAL,
                FECHA_INICIAL = item.FECHA_INICIAL,
                FEC_INIC = item.FEC_INIC,
                FEC_TASA = item.FEC_TASA,
                FEC_VENC = item.FEC_VENC,
                MOD_PAGO = item.MOD_PAGO,
                NOM_TIPC = item.NOM_TIPC,
                NUM_CONT = item.NUM_CONT,
                NUM_DOCU = item.NUM_DOCU,
                NUM_FACT = item.NUM_FACT,
                NUM_PERS = item.NUM_PERS,
                PERIODO_FACTURA = item.PERIODO_FACTURA,
                RMT_CAFA = item.RMT_CAFA,
                RMT_CONT = item.RMT_CONT,
                TER_EMER = item.TER_EMER,
                VAL_MDUR = item.VAL_MDUR,
                VAL_SALD = item.VAL_SALD,
                VAL_TASA = item.VAL_TASA,
                VAL_TOTA = item.VAL_TOTA,
                Consecutivo = item.Consecutivo,
                Prefijo = item.Prefijo,
                TipoDocumento = item.TipoDocumento 

            };
            return vRetorno;
        }
    }
}