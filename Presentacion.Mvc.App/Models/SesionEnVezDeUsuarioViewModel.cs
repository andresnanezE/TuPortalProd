using Dominio.Administracion.Entidades.MapperDto;

//using Dominio.Administracion.Entidades.MapperDto;
using PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Models
{
    public class SesionEnVezDeUsuarioViewModel
    {
        private List<CiudadUsuario> _ciudad = new List<Models.CiudadUsuario>();
        private List<CanalUsuario> _canal = new List<Models.CanalUsuario>();
        private List<SegmentoUsuario> _segmento = new List<Models.SegmentoUsuario>();
        private List<PerfilUsuario> _perfil = new List<Models.PerfilUsuario>();

        public IPagedList<Usuario> ListaPageUsuarios { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dto"></param>
        public SesionEnVezDeUsuarioViewModel(IEnumerable<SesionEnVezDeParametrosCrearUsuarioDto> dto)
        {
            var error = dto.Where(c => c.tipo == "Error");

            if (error.Any())
            {
                Error = dto.Where(c => c.tipo == "Error").Single().valor;
                //return;
            }

            var canales = dto.Where(c => c.tipo == "Canal");
            var perfiles = dto.Where(c => c.tipo == "Perfil");
            var segmentos = dto.Where(c => c.tipo == "Segmento");
            var ciudades = dto.Where(c => c.tipo == "Ciudad");
            var usuario = dto.Where(c => c.tipo == "Usuario").Single();

            var _id = dto.FirstOrDefault(c => c.tipo == "Modificado");
            if (_id != null)
            {
                Id = int.Parse(_id.valor);
            }

            foreach (var c in canales)
            {
                var canal = new CanalUsuario()
                {
                    cod_canal = c.valor,
                    canal = c.valor
                };

                _canal.Add(canal);
            }
            foreach (var p in perfiles)
            {
                var perfil = new PerfilUsuario()
                {
                    cod_perfil = p.valor,
                    perfil = p.valor
                };

                _perfil.Add(perfil);
            }
            foreach (var s in segmentos)
            {
                var segmento = new SegmentoUsuario()
                {
                    cod_segmento = s.valor,
                    segmento = s.valor
                };

                _segmento.Add(segmento);
            }
            foreach (var c in ciudades)
            {
                var ciudad = new CiudadUsuario()
                {
                    cod_ciudad = c.valor,
                    ciudad = c.valor
                };

                _ciudad.Add(ciudad);
            }

            Nombre = usuario.valor;
            //Codusuario  = usuario.id.ToString();
            //Error = null;
        }

        public SesionEnVezDeUsuarioViewModel()
        {
            //CiudadId = null;

            ListaUsuarios = new List<Usuario>();
        }

        /// <summary>
        /// Comstructor sin parámetros.
        /// </summary>
        //public SesionEnVezDeUsuarioViewModel(int? cod)
        //{
        //    _canal.Add(new CanalUsuario()
        //    { cod_canal = cod, canal = "" });

        //    _perfil.Add(new PerfilUsuario()
        //    { cod_perfil = cod, perfil = "" });

        //    _ciudad.Add(new CiudadUsuario()
        //    { cod_ciudad = cod, ciudad = "" });
        //}

        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "Ciudad obligatorio")]
        public string Ciudad { get; set; }

        public IEnumerable<SelectListItem> CiudadUsuario
        {
            get { return new SelectList(_ciudad, "cod_ciudad", "ciudad"); }
        }

        [Display(Name = "Canal")]
        [Required(ErrorMessage = "Canal es obligatorio")]
        public string Canal { get; set; }

        public IEnumerable<SelectListItem> CanalUsuario
        {
            get { return new SelectList(_canal, "cod_canal", "canal"); }
        }

        [Display(Name = "Segmento")]
        public string Segmento { get; set; }

        public IEnumerable<SelectListItem> SegmentoUsuario
        {
            get { return new SelectList(_segmento, "cod_segmento", "segmento"); }
        }

        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "perfil es obligatorio")]
        public string Perfil { get; set; }

        public IEnumerable<SelectListItem> PerfilUsuario
        {
            get { return new SelectList(_perfil, "cod_perfil", "perfil"); }
        }

        [Required(ErrorMessage = "Documento es obligatorio")]
        [Display(Name = "Nro. Documento")]
        public string Codusuario { get; set; }

        public string Nombre { get; set; }

        [Display(Name = "Id Usuario")]
        public int Id { get; set; }

        public int SegmentoId { get; set; }
        public int PerfilId { get; set; }
        public int CanalId { get; set; }
        public int CiudadId { get; set; }
        public string Estado { get; set; }
        public string Mensaje { get; set; }
        public string Error { get; set; }

        public List<Usuario> ListaUsuarios { get; set; }

        public string Modificar { get; set; }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public string Segmento { get; set; }
        public string Perfil { get; set; }
        public string Canal { get; set; }
        public string Estado { get; set; }
    }
}