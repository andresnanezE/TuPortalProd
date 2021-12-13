using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionCotizadorTarifas : IServicioAplicacionCotizadorTarifas
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioCotizadorTarifas _repositorioCotizadorTarifas;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionCotizadorTarifas(IRepositorioCotizadorTarifas repositorioCotizadorTarifas, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioCotizadorTarifas = repositorioCotizadorTarifas;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion C'tors

        public UsuarioCiudad getUsuarioCiudad(String sUsuario)
        {
            return _repositorioCotizadorTarifas.getUsuarioCiudad(sUsuario);
        }

        public IEnumerable<TipoTarifa> getTiposTarifas()
        {
            return _repositorioCotizadorTarifas.getTiposTarifas();
        }

        public IEnumerable<Usp_ObtenerDetalleValorFactor> getDetalleFactores(int idFactor)
        {
            return _repositorioCotizadorTarifas.getDetalleFactores(idFactor);
        }

        public IEnumerable<Campana> getCampanas(String sCodigoTarifa)
        {
            return _repositorioCotizadorTarifas.getCampanas(sCodigoTarifa);
        }

        public IEnumerable<TarifaBase> getTarifasBase(String sCiudad, int iPersonas, String sCodigoTarifa, String sCampana, String sUsuario)
        {
            return _repositorioCotizadorTarifas.getTarifasBase(sCiudad, iPersonas, sCodigoTarifa, sCampana, sUsuario);
        }

        public bool updateDetalleFactor(Dominio.Administracion.Entidades.MapperDto.ModificarFactorDto modificarFactorDto)
        {
            return _repositorioCotizadorTarifas.updateDetalleFactor(modificarFactorDto);
        }

        public bool updateValoresFactor(int idFactor, int idTipoFactor, decimal valorConstante, decimal valorExponente )
        {
            return _repositorioCotizadorTarifas.updateValoresFactor(idFactor, idTipoFactor,valorConstante, valorExponente );
        }

        public Detalle_Factor getValorDetalle_Factor(int idFactor)
        {
            return _repositorioCotizadorTarifas.getValorDetalle_Factor(idFactor);
        }
    }
}