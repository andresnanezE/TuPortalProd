



namespace Dominio.Administracion.Entidades.ModeloProcesos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class EMA_LOG
    {

        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LOGID { get; set; }
        public string LLAVE { get; set; }
        public string DESCRIPCION { get; set; }
        public string EXCEPCION { get; set; }
        public string PARAMETROXML { get; set; }
    }
}