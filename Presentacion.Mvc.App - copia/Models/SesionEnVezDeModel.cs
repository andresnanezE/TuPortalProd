// ----------------------------------------------------------------------------------------------
// <copyright file="UsuarioModel.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Aplicacion.Administracion.Dto.DtoKheiron;
using Aplicacion.Administracion.Dto.DtoProcesos;
using PagedList;

namespace Presentacion.Mvc.App.Models
{
    public class SesionEnVezDeModel
    {
        #region Instance Properties

        public bool Acepto { get; set; }

        public string Terminos { get; set; }

         
        #endregion
    }
}
