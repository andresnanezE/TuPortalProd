using Dominio.Administracion.Entidades.ModeloCotizacion;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Administracion.Entidades.MapperDto.DtoProcesos
{
    [NotMapped]
    public class ProductosSelector : Productos
    {
        public int? seleccionado { get; set; }
    }
}