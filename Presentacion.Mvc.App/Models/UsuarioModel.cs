// ----------------------------------------------------------------------------------------------
// <copyright file="UsuarioModel.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos.DtoKheiron;

//using Dominio.Administracion.Entidades.MapperDto.DtoKheiron;
//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Mvc.App.Models
{
    public class UsuarioModel
    {
        #region Instance Properties

        public bool Activo { get; set; }

        public string Contrasena { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el correo.")]
        [EmailAddress(ErrorMessage = "Por favor ingrese un correo valido.")]
        public string Correo { get; set; }

        public string CorreoFiltro { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el documento.")]
        public string Documento { get; set; }

        public string DocumentoFiltro { get; set; }

        public DateTime FechaModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaUltimaSesion { get; set; }
        public IPagedList<EMA_USUARIODto> ListaUsuarios { get; set; }
        public IEnumerable<RolModel> ListadoRoles { get; set; }
        public string Mensaje { get; set; }

        [Required(ErrorMessage = "Por favor realizar la búsqueda del usuario.")]
        public string NombreUsuario { get; set; }

        public string NombreUsuarioFiltro { get; set; }

        public IEnumerable<EMA_ROLDto> Roles { get; set; }

        [Required(ErrorMessage = "Por favor seleccione un rol.")]
        public int[] RolesIds { get; set; }

        public string TipoDocumento { get; set; }
        public IEnumerable<EMB_TIPO_IDENTIFICACIONDto> TipoDocumentos { get; set; }
        public string Usuario { get; set; }
        public string UsuarioFiltro { get; set; }
        public int UsuarioId { get; set; }

        #endregion Instance Properties
    }
}