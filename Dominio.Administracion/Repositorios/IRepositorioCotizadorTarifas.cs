using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioCotizadorTarifas
    {
        UsuarioCiudad getUsuarioCiudad(String sUsuario);

        IEnumerable<TipoTarifa> getTiposTarifas();

        IEnumerable<Usp_ObtenerDetalleValorFactor> getDetalleFactores(int idFactor);

        IEnumerable<Campana> getCampanas(String sCodigoTarifa);

        IEnumerable<TarifaBase> getTarifasBase(String sCiudad, int iPersonas, String sCodigoTarifa, String sCampana, String sUsuario);

        bool updateDetalleFactor(ModificarFactorDto modificarFactorDto);

        bool updateValoresFactor(int idFactor, int idTipoFactor, decimal valorConstante, decimal valorExponente);

        Detalle_Factor getValorDetalle_Factor(int idFactor);
    }
}