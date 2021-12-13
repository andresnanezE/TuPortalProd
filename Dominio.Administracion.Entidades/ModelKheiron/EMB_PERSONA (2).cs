using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModelKheiron
{
    using System;
    using System.Collections.Generic;

    public partial class EMB_PERSONA
    {
        [Key]
        public string NUMERO_IDENTIFICACION { get; set; }
        public string ID_TIPO_IDENTIFICACION { get; set; }
        public string ID_GENERO { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string ES_ACTIVO { get; set; }
        public Nullable<System.DateTime> FECHA_NACIMIENTO { get; set; }
        public string USUARIO { get; set; }
        public string ID_CIUDAD { get; set; }
        public string COD_FUNC_HOMOLOG { get; set; }
        public Nullable<int> ID_PERFIL { get; set; }

        public virtual EMB_TIPO_IDENTIFICACION EMB_TIPO_IDENTIFICACION { get; set; }
    }
}