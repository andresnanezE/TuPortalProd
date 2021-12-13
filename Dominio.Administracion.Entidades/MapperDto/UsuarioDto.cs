using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class UsuarioDto
    {
        public UsuarioDto()
        {
            Roles = new List<int>();
        }

        public int Id { get; set; }
        public string logIn { get; set; }
        public string Nombre { get; set; }
        public string tipoDoc { get; set; }
        public string documento { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public DateTime fechaUltimaSesion { get; set; }
        public DateTime fechaExpiraClave { get; set; }
        public DateTime fechaRegistro { get; set; }
        public bool activo { get; set; }
        public List<int> Roles { get; set; }
    }
}