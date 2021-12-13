namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class ReclutamientoCapacitador
    {
        public int Id { get; set; }
        public int IdUsr { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public bool Activo { get; set; }
    }
}
