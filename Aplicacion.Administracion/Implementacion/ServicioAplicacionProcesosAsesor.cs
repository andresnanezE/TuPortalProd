using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos.DtoKheiron;
using Dominio.Administracion.Entidades.ModelKheiron;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Transversales.Administracion;

//using Aplicacion.Administracion.Dto.DtoKheiron;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionProcesosAsesor : IServicioAplicacionProcesosAsesor
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioProcesosAsesor _repositorioProcesosAsesor;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionProcesosAsesor(IRepositorioProcesosAsesor repositorioProcesosAsesor, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioProcesosAsesor = repositorioProcesosAsesor;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        #endregion C'tors

        public IEnumerable<ProcesosAsesorDto> ProcesosAsesor(int mes, int anio, int ccAsesor)
        {
            try
            {
                var lst = new List<ProcesosAsesorDto>();

                var lista = _repositorioProcesosAsesor.ProcesosAsesor(mes, anio, ccAsesor);

                foreach (ProcesosAsesor item in lista)
                {
                    var procesosDto = new ProcesosAsesorDto
                    {
                        ID_PROC = item.ID_PROC,
                        FECHA_INI = item.FECHA_INI,
                        FECHA_FIN = item.FECHA_FIN,
                        ANIO = item.ANIO
                    };

                    lst.Add(procesosDto);
                }

                return lst;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo lista de procesos asesor");
            }
        }

        public IEnumerable<EMB_CIUDADDto> ObtenerCiudades()
        {
            try
            {
                var lst = new List<EMB_CIUDADDto>();
                var lista = _repositorioProcesosAsesor.ObtenerCiudades();

                foreach (EMB_CIUDAD item in lista)
                {
                    var ciudad = new EMB_CIUDADDto
                    {
                        ID_CIUDAD = item.ID_CIUDAD,
                        ID_AREA = item.ID_AREA,
                        CIUDAD = item.CIUDAD
                    };

                    lst.Add(ciudad);
                }
                return lst;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo lista de ciudades");
            }
        }
    }
}