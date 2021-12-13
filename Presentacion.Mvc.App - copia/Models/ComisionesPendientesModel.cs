using Aplicacion.Administracion.Dto;
using Dominio.Administracion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentacion.Mvc.App.Models
{
    public class ComisionesPendientesModel
    {
        public IEnumerable<AfiliacionesPeriodoDto> ListaPeriodos { get; set; }
        public IEnumerable<CentrosCosto> CentrosCosto { get; set; }
        public IEnumerable<Escalera> Escaleras { get; set; }
        public int CodSucursal { get; set; }
        public string CodAses { get; set; }
        public int CodEsca { get; set; }
        public string Periodo { get; set; }
        public string Periodo2 { get; set; }
    }
}