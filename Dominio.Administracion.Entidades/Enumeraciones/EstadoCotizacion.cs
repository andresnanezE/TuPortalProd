
using System.ComponentModel;

namespace Dominio.Administracion.Entidades.Enumeraciones
{
    public enum EstadoCotizacion
    {
        [Description("Sin Cotizar")]
        SinCotizar = 1,
        [Description("Cotizado")]
        Cotizado = 2,
        [Description("Reconsiderado")]
        Reconsiderado = 3,
        [Description("Pendiente Reconsideración")]
        PendienteReconsideracion = 4,
        [Description("No Reconsiderado")]
        NoReconsiderado = 5
    }
}
