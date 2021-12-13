
namespace Dominio.Administracion.Entidades.MapperDto.DtoStone.DtoSitioWeb
{
    public class DatosActualizacionMedioPagoDto
    {
        public string IdentificationNumber { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public int RMTCont { get; set; }
        public string TipDeau { get; set; }

        public int? codBanc { get; set; }

        public string CueDeau { get; set; }

    }
}
