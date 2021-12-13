using System;

namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class ReclutamientoInfoCompleta
    {
        public int Id { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int IdPais { get; set; }
        public string Pais { get; set; }
        public int Estrato { get; set; }
        public string Direccion { get; set; }
        public string Barrio { get; set; }
        public int IdEps { get; set; }
        public int IdReclutamientoPensiones { get; set; }
        public string Eps { get; set; }
        public string EstadoCivil { get; set; }
        public int IdNivelEducativo { get; set; }
        public bool FuentesIngresos { get; set; }
        public bool TerminosCondicionesI { get; set; }
        public int IdNivelEducativoSarlaft { get; set; }
        public string DescripcionRecursos { get; set; }
        public bool ManejaRecursosPublicos { get; set; }
        public bool EsServidorPublico { get; set; }
        public bool OstentaAlgunGradoDePoderPublico { get; set; }
        public bool TieneCondicionPersonaExpuestaPoliticamente { get; set; }
        public bool RealizaOperacionesInternacionales { get; set; }
        public bool GozaReconocimientoPublico { get; set; }
        public string Ooperaciones { get; set; }
        public string AniosExperiencia { get; set; }
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
        public bool PreguntaD1 { get; set; }
        public bool PreguntaD2 { get; set; }
        public bool PreguntaD3 { get; set; }
        public bool PreguntaD4 { get; set; }
        public int NumeroHijos1 { get; set; }
        public int NumeroHijos2 { get; set; }
        public int NumeroHijos3 { get; set; }
        public bool InformacionPersonal { get; set; }
        public bool Sarlaft { get; set; }
        public bool ExperienciaComercial { get; set; }
        public bool Contrato { get; set; }
        public bool CertificacionTributaria { get; set; }
    }
}