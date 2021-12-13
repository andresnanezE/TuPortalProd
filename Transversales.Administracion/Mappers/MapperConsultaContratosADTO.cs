//using Aplicacion.Administracion.Dto.DtoSitioWeb;
using Dominio.Administracion.Entidades.MapperDto.DtoSitioWeb;
using Dominio.Administracion.Entidades.ModeloSitioWeb;
using System.Collections.Generic;

namespace Transversales.Administracion.Mappers
{
    public class MapperConsultaContratosADTO
    {
        public static IEnumerable<SPEM_CONSULTACONTRATOSDto> AdaptarMenu(IEnumerable<SPEM_CONSULTACONTRATOS> origen)
        {
            List<SPEM_CONSULTACONTRATOSDto> vRetorno = new List<SPEM_CONSULTACONTRATOSDto>();
            if (origen != null)
            {
                foreach (var index in origen)
                {
                    vRetorno.Add(EntidadDTOAConsultaContratos(index));
                }
            }
            return vRetorno;
        }

        public static SPEM_CONSULTACONTRATOSDto EntidadDTOAConsultaContratos(SPEM_CONSULTACONTRATOS item)
        {
            SPEM_CONSULTACONTRATOSDto vRetorno = new SPEM_CONSULTACONTRATOSDto()
            {
                Cartera = item.Cartera,
                CiudadAsesor = item.CiudadAsesor,
                cod_ases = item.cod_ases,
                cod_dire = item.cod_dire,
                CuotaMensual = item.CuotaMensual,
                diasFaltantes = item.diasFaltantes,
                diasProrrateo = item.diasProrrateo,
                Direccion = item.Direccion,
                Email = item.Email,
                Estado = item.Estado,
                EstadoAse = item.EstadoAse,
                EstadoAsesor = item.EstadoAsesor,
                fCorte = item.fCorte,
                FechaI = item.FechaI,
                FechaV = item.FechaV,
                FormaPago = item.FormaPago,
                Identificacion = item.Identificacion,
                ModoPago = item.ModoPago,
                MostrarBoton = item.MostrarBoton,
                Nombre = item.Nombre,
                NombreDirector = item.NombreDirector,
                nom_comp = item.nom_comp,
                Num_cont = item.Num_cont,
                num_pers = item.num_pers,
                PrefijoCont = item.PrefijoCont,
                RmtCont = item.RmtCont,
                SubCanal = item.SubCanal,
                TarifaConIva = item.TarifaConIva,
                TarifaSinIva = item.TarifaSinIva,
                TelAsesor = item.TelAsesor,
                TelDirector = item.TelDirector,
                Telefono = item.Telefono,
                TelefonoAse = item.TelefonoAse,
                Tipo = item.Tipo,
                TipoContrato = item.TipoContrato,
                ValorContrato = item.ValorContrato,
                Ver_Detalle = item.Ver_Detalle,
                vIva = item.vIva
            };
            return vRetorno;
        }
    }
}