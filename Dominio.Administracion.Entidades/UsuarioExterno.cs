// ----------------------------------------------------------------------------------------------
// <copyright file="UsuarioExterno.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

namespace Dominio.Administracion.Entidades
{
    public class UsuarioExterno
    {
        public string Usuario { get; set; }
        public string TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Contrasena { get; set; }
        public string NombreUsuario { get; set; }
        public int Rol { get; set; }
        public string Email { get; set; }
    }
}