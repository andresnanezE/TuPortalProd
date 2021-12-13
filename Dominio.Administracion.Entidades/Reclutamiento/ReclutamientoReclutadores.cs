namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class ReclutamientoReclutadores
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Ciudad { get; set; }
        public string TipoReclutador { get; set; }
        public int? DirectoresAsignados { get; set; }
    }
}
