//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;

namespace Transversales.Administracion.Mappers
{
    public class MapperConsultaBeneficiarioADTO
    {
        public static IEnumerable<ConsultaBeneficiarioDto> AdaptarMenu(IEnumerable<ConsultaBeneficiario> origen)
        {
            List<ConsultaBeneficiarioDto> vRetorno = new List<ConsultaBeneficiarioDto>();
            if (origen != null)
            {
                foreach (var index in origen)
                {
                    vRetorno.Add(EntidadConsultaContratosADTO(index));
                }
            }
            return vRetorno;
        }

        public static ConsultaBeneficiarioDto EntidadConsultaContratosADTO(ConsultaBeneficiario item)
        {
            ConsultaBeneficiarioDto vRetorno = new ConsultaBeneficiarioDto()
            {
                Ape_bene = item.Ape_bene,
                Fec_naci = item.Fec_naci,
                Nom_bene = item.Nom_bene,
                Num_iden = item.Num_iden,
                Tel_bene = item.Tel_bene
            };
            return vRetorno;
        }
    }
}