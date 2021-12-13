using System;

namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class ReclutamientoRegistro
    {
        public Int64 NumeroDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int PaisId { get; set; }
        public int Estrato { get; set; }
        public string Direccion { get; set; }
        public string Barrio { get; set; }
        public int EpsId { get; set; }
        public int PensionId { get; set; }
        public string EstadoCivil { get; set; }
        public int NivelEducativoId { get; set; }
        public bool FuenteIngresos { get; set; }
        public bool TerminosCondiciones { get; set; }
        public int ProfesionId { get; set; }
        public string Recursos { get; set; }
        public bool PrSlft1 { get; set; }
        public bool PrSlft2 { get; set; }
        public bool PrSlft3 { get; set; }
        public bool PrSlft4 { get; set; }
        public bool PrSlft5 { get; set; }
        public bool PrSlft6 { get; set; }
        public string Operaciones { get; set; }
        public int Anios { get; set; }
        public string PrimerSector { get; set; }
        public string SegundoSector { get; set; }

        public bool Pregunta1 { get; set; }
        
        public bool Pregunta2 { get; set; }
        
        public bool Pregunta3 { get; set; }
        
        public bool Pregunta4 { get; set; }
        
        public bool Pregunta5 { get; set; }
        
        public bool Pregunta6 { get; set; }
        
        public bool Pregunta7 { get; set; }
        
        public bool Pregunta8 { get; set; }
        
        public bool Pregunta9 { get; set; }
        
        public bool Pregunta10 { get; set; }
        
        public bool Pregunta11 { get; set; }
        
        public bool Pregunta12 { get; set; }
        
        public bool Pregunta13 { get; set; }
        
        public bool Pregunta14 { get; set; }

        public int Hijo1 { get; set; }
        
        public int Hijo2 { get; set; }
        
        public int Hijo3 { get; set; }
    }
}
