
using System.ComponentModel;

namespace Dominio.Administracion.Entidades.Enumeraciones
{
    public enum RolPortal
    {
        [Description("Asesor")]
        Asesor = 8,
        [Description("Director")]
        Director = 7,
        [Description("Pricing")]
        Pricing = 28,
        [Description("Gerente Regional")]
        GerenteRegional = 29,
        [Description("Gerente Nacional")]
        GerenteNacional = 30
    }
}
