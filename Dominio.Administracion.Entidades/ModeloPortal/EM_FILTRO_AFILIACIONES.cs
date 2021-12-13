﻿using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public class EM_FILTRO_AFILIACIONES
    {
        [Key]
        public int ID_FILTRO { get; set; }

        public string NOMBRE { get; set; }
        public string ID_CODIGO { get; set; }
        public string ACTIVO { get; set; }
    }
}