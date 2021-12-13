// ----------------------------------------------------------------------------------------------
// <copyright file="CotizadorTarifasModel.cs" company="Apptic">
//     Copyright (c) Apptic 2016. Todos los derechos reservados.
// </copyright>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Presentacion.Mvc.App.Models
{
    public class CotizadorTarifasModel
    {
        #region Instance Properties

        public UsuarioCiudad UsuarioCiudad { get; set; }
        public IEnumerable<TipoTarifa> TiposTarifas { get; set; }
        public IEnumerable<TarifaBase> TarifasBase { get; set; }
        public JavaScriptSerializer Serializador { get; internal set; }

        #endregion Instance Properties
    }
}