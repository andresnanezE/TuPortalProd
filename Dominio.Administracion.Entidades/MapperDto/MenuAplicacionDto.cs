using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class MenuAplicacionDto
    {
        #region Instance Properties

        public string Action { get; set; }
        public string Controller { get; set; }
        public string Roles { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionPadre { get; set; }
        public IEnumerable<EMA_MENUDto> MenuHijos { get; set; }
        public int MenuId { get; set; }
        public Nullable<int> NodoPadreId { get; set; }
        public string Url { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string Icono { get; set; }

        #endregion Instance Properties
    }
}