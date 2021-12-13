using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloProcesos
{
    public partial class EMA_ROLXUSUARIOold
    {
        [Key]
        public int ROLXUSUARIOID { get; set; }

        public int ROLID { get; set; }
        public int USUARIOID { get; set; }

        //public virtual EMA_ROL EMA_ROL { get; set; }
        //public virtual EMA_USUARIO EMA_USUARIO { get; set; }
    }
}