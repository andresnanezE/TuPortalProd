using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Presentacion.Mvc.App.Models
{
    public class SolicitudesInternas
    {
        public int IdSolicitud { get; set; }

        [Display(Name = "Asunto")]
        [Required(ErrorMessage = "El campo es obligatorio")]
        public string Asunto { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "El campo es obligatorio")]
        public string Descripcion { get; set; }

        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "El campo es obligatorio")]
        public Ciudad Ciudad { get; set; }

        public IEnumerable<SelectListItem> CiudadesItems
        {
            get { return new SelectList(_Ciudades, "idCiudad", "nombreCiudad"); }
        }

        private List<Ciudad> _Ciudades;

        [Display(Name = "Area")]
        [Required(ErrorMessage = "El campo es obligatorio")]
        public Area Area { get; set; }

        public IEnumerable<SelectListItem> AreasItems
        {
            get { return new SelectList(_Areas, "idArea", "nombreArea"); }
        }

        private List<Area> _Areas;

        [Display(Name = "Tipo de Requerimiento")]
        [Required(ErrorMessage = "El campo es obligatorio")]
        public TipoRequerimiento TipoRequerimiento { get; set; }

        public IEnumerable<SelectListItem> TRequerimientosItems
        {
            //get { return new SelectList(_TRequerimientos, "idTRequerimiento", "nombreTRequerimiento"); }
            get
            {
                return _TRequerimientos.Select(r => new SelectListItem { Value = r.idTRequerimiento.ToString(), Text = r.nombreTRequerimiento });
            }
        }

        private List<TipoRequerimiento> _TRequerimientos;

        public IEnumerable<HttpPostedFileBase> Attach1 { get; set; }
        public IEnumerable<HttpPostedFileBase> Attach2 { get; set; }
        public IEnumerable<HttpPostedFileBase> Attach3 { get; set; }
        public IEnumerable<HttpPostedFileBase> Attach4 { get; set; }
        public IEnumerable<HttpPostedFileBase> Attach5 { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaFinalizacion { get; set; }

        public string Estado { get; set; }

        public SolicitudesInternas()
        {
            llenarCiudades();
            llenarAreas();
            llenarTRequerimientos();
        }

        private void llenarTRequerimientos()
        {
            string path = string.Format(@"{0}/TiposRequerimientos.xml", ConfiguracionesGlobales.SolicitudesIntrernasPathXmls);

            _TRequerimientos = new List<TipoRequerimiento>();

            XElement tipoRequerimientoE = XElement.Load(HostingEnvironment.MapPath(path));

            var tipoRequerimientos = from e in tipoRequerimientoE.Elements("requerimiento") select e;

            _TRequerimientos.Add(new TipoRequerimiento { idTRequerimiento = 0, nombreTRequerimiento = "..." });

            foreach (var requerimiento in tipoRequerimientos)
            {
                var r = new TipoRequerimiento { idTRequerimiento = (int)requerimiento.Element("idTRequerimiento"), nombreTRequerimiento = (string)requerimiento.Element("nombreTRequerimiento") };
                _TRequerimientos.Add(r);
            }
        }

        private void llenarAreas()
        {
            string path = string.Format(@"{0}/Areas.xml", ConfiguracionesGlobales.SolicitudesIntrernasPathXmls);

            _Areas = new List<Area>();

            XElement areaE = XElement.Load(HostingEnvironment.MapPath(path));

            var areas = from e in areaE.Elements("Area") select e;

            _Areas.Add(new Area { idArea = 0, nombreArea = "..." });

            foreach (var area in areas)
            {
                var a = new Area { idArea = (int)area.Element("idArea"), nombreArea = (string)area.Element("nombreArea") };
                _Areas.Add(a);
            }
        }

        private void llenarCiudades()
        {
            string path = string.Format(@"{0}/Ciudades.xml", ConfiguracionesGlobales.SolicitudesIntrernasPathXmls);

            _Ciudades = new List<Ciudad>();

            XElement ciudadesE = XElement.Load(HostingEnvironment.MapPath(path));

            var ciudades = from e in ciudadesE.Elements("Ciudad") select e;

            _Ciudades.Add(new Ciudad { idCiudad = 0, nombreCiudad = "..." });

            foreach (var ciudad in ciudades)
            {
                var c = new Ciudad { idCiudad = (int)ciudad.Element("idCiudad"), nombreCiudad = (string)ciudad.Element("nombreCiudad") };
                _Ciudades.Add(c);
            }
        }

        public string Mensaje { get; set; }
    }

    public class Ciudad
    {
        public int idCiudad { get; set; }

        public string nombreCiudad { get; set; }
    }

    public class Area
    {
        public int idArea { get; set; }

        public string nombreArea { get; set; }
    }

    public class TipoRequerimiento
    {
        public int idTRequerimiento { get; set; }

        public string nombreTRequerimiento { get; set; }
    }
}