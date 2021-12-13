// ----------------------------------------------------------------------------------------------
// <copyright file="RolModel.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using Aplicacion.Administracion.Dto.DtoProcesos;
using PagedList;

namespace Presentacion.Mvc.App.Models
{
    public class RolModel
    {
        #region Instance Properties

        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        
        public IPagedList<EMA_ROLDto> ListadoRoles { get; set; }

        [Required(ErrorMessage = "Por favor digite un rol.")]
        public string Rol { get; set; }
        public string RolFiltro { get; set; }
        public int RolId { get; set; }
        public bool Seleccionado { get; set; }

        public string Mensaje { get; set; }

        #endregion
    }
}
