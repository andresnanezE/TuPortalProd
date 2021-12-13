namespace Dominio.Administracion.Entidades
{
    public class SesionEnVezDeNuevoUsuario
    {
        public decimal cod_usuario { get; set; }
        public int? cod_ciudad { get; set; }
        public int cod_canal { get; set; }
        public int cod_segmento { get; set; }
        public int cod_perfil { get; set; }
        public char estado { get; set; }
        public int registrosMod { get; set; }

        public int Id { get; set; }
        public string CodUsuario { get; set; }
        public string Ciudad { get; set; }
        public string Perfil { get; set; }
        public string Segmento { get; set; }
        public string Canal { get; set; }
    }
}