namespace Dominio.Administracion.Entidades.MapperDto.DtoProcesos.DtoKheiron
{
    using System;

    public partial class EMB_USUARIODto
    {
        public decimal USER_ID { get; set; }
        public decimal ID_DOMINIO { get; set; }
        public string USER_NAME { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public string DESCRIPCION { get; set; }
        public string CONTRASENA { get; set; }
        public string ACTIVO_SN { get; set; }
        public string EMAIL { get; set; }
        public string CAMBIAR_CONTRASENA_SN { get; set; }
        public string CONTRASENA_CADUCA_SN { get; set; }
        public Nullable<System.DateTime> FECHA_EXPIRA { get; set; }
        public System.DateTime FECHA_ULTIMA_CLAVE { get; set; }
        public decimal NUM_INTENTOS_FALLIDOS { get; set; }
        public Nullable<System.DateTime> FECHA_ULTIMO_INGRESO { get; set; }
        public string ADMIN_SN { get; set; }
        public string CONTRASENA_KII { get; set; }
    }
}