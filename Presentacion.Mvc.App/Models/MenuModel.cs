// ----------------------------------------------------------------------------------------------
// <copyright file="MenuModel.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Mvc.App.Models
{
    public class MenuModel
    {
        #region Instance Properties

        [Required(ErrorMessage = "Por favor digite una Acción.")]
        public string Action { get; set; }

        [Required(ErrorMessage = "Por favor digite un controlador.")]
        public string Controller { get; set; }

        [Required(ErrorMessage = "Por favor digite una descripción.")]
        public string Descripcion { get; set; }

        public string DescripcionFiltro { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaModificacion { get; set; }
        public IEnumerable<EMA_ROLDto> ListadoComboRoles { get; set; }
        public IPagedList<MenuAplicacionDto> ListadoMenu { get; set; }
        public IEnumerable<EMA_MENUDto> ListadoMenuPadres { get; set; }
        public IEnumerable<RolModel> ListadoRoles { get; set; }
        public string Mensaje { get; set; }
        public int MenuId { get; set; }
        public int? NodoPadreId { get; set; }
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Por favor digite un rol.")]
        public List<int> RolesIds { get; set; }

        [Required(ErrorMessage = "Por favor digite la url.")]
        public string Url { get; set; }

        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

        #endregion Instance Properties
    }
}