// ----------------------------------------------------------------------------------------------
// <copyright file="ErrorServicio.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

namespace Dominio.Administracion.Entidades
{
    public class ErrorServicio
    {
        #region C'tors

        public ErrorServicio(string detalle)
        {
            Detalle = detalle;
        }

        #endregion C'tors

        #region Instance Properties

        public string Detalle { get; set; }

        #endregion Instance Properties
    }
}