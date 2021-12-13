// ----------------------------------------------------------------------------------------------
// <copyright file="IRepositorioNovedades.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloSitioWeb;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioContratos
    {
        IEnumerable<SPEM_CONSULTACONTRATOS> ObtenerContratos(decimal codigo);

        IEnumerable<SPEM_CONSULTACONTRATOS> ObtenerContratosContratante(decimal codigo, int rmt);

        IEnumerable<SPEM_CONSULTACONTRATOS> ObtenerConsultaContratosPorNombre(string nombre, decimal usuario, int rol);

        IEnumerable<SPEM_CONSULTACONTRATOS> ObtenerContratosBeneficiario(decimal codigo, int rmt, decimal CCcontratante,
            string contratante);

        IEnumerable<ConsultaBeneficiario> ObtenerBeneficiario(string benef);

        IEnumerable<ConsultaFactura> ObtenerFacturas(string doc);

        IEnumerable<SPEM_CONSULTACONTRATOS> ObtenerConsultaContratos(decimal codigo, decimal usuario, int rol);

        IEnumerable<SPCCBENEFINCLUSIONTUPORTAL> ValidacionBeneficiarioInclusion(int rmtCont, string codTerc, string codBene);
    }
}