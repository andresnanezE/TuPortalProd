
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloMedioPago
{
    public class MedioPagoAfiliado
    {
        [Key]
        public int Id { get; set; }
        public int Id_Contrato { get; set; }
        public string Descripcion { get; set; }
        public string Ciudad { get; set; }
        public string Mediopago { get; set; }
        public string NumeroCuenta { get; set; }
        public string Banco { get; set; }
        public string FormaPago { get; set; }
        public string DescripcionTarjeta { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }
    }
}
