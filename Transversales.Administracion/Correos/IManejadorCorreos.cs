// ***********************************************************************
// Ensamblado         		: Emermedica Sitio Empresarial - Infraestructura.Transversales.Administracion - IManejadorCorreo.cs
// Autor					: Alex Mauricio Palacios Caicedo
// Creado          			: 16/02/2015
//
// Ultima Modificación por 	: Alex Mauricio Palacios Caicedo
// Ultima Modificación en 	: 16/02/2015
// ***********************************************************************
// <copyright file="IManejadorCorreo.cs" Compañia="SCI Software">
//     Copyright (c) SCI Software. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;

namespace Transversales.Administracion.Correos
{
    public interface IManejadorCorreos
    {
        #region Instance Properties

        IEnumerable<string> Adjuntos { get; set; }
        string Asunto { get; set; }
        IEnumerable<string> Destinatarios { get; set; }
        string Mensaje { get; set; }

        #endregion Instance Properties

        #region Instance Methods

        void Enviar();

        #endregion Instance Methods
    }
}