using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades
{
    public class EM_NITEMPRESACLIENTE
    {
        [Key]
        public int nitEmpresaClienteID { get; set; }

        public int NIT { get; set; }
        public string razonSocial { get; set; }
    }
}