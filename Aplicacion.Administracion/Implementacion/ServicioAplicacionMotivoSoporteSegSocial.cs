using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Transversales.Administracion;

//
// j0HNn3LS0N r0DRIGU3Z
// 2016-10-25

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionMotivoSoporteSegSocial : IServicioAplicacionMotivoSegSocial
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioMotivoSoporteSegSocial _repositorioMotivosSegSocial;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionMotivoSoporteSegSocial(IRepositorioMotivoSoporteSegSocial repositorioMotivoSoporteSegSocial, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioMotivosSegSocial = repositorioMotivoSoporteSegSocial;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion C'tors

        #region Instance Methods

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        public void AgregarLog(MotivoSoporteSegSocialDto mss)
        {
            try
            {
                var _mss = new MotivoSoporteSegSocial();

                _mss.ARCHIVOSTXT = mss.ARCHIVOSTXT;
                _mss.USUARIO = mss.USUARIO;
                _mss.EMAIL = mss.EMAIL;
                _mss.OBSERVACION = mss.OBSERVACION;
                _mss.ID_MOTIVO = mss.ID_MOTIVO;

                _repositorioMotivosSegSocial.AgregarLog(_mss);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo lista de motivos");
            }
        }

        #endregion Instance Methods

        public System.Collections.Generic.IEnumerable<MotivoSoporteSegSocialDto> MotivosSoporteSegSocial()
        {
            try
            {
                var lista = _repositorioMotivosSegSocial.MotivosSoporteSegSocial();
                var listaDto = new List<MotivoSoporteSegSocialDto>();

                foreach (var item in lista)
                {
                    var dto = new MotivoSoporteSegSocialDto()
                    {
                        EMAIL = item.EMAIL,
                        ARCHIVOSTXT = item.ARCHIVOSTXT,
                        ID = item.ID,
                        ID_MOTIVO = item.ID_MOTIVO,
                        NOMBRE_MOTIVO = item.NOMBRE_MOTIVO,
                        OBSERVACION = item.OBSERVACION,
                        USUARIO = item.USUARIO
                    };

                    listaDto.Add(dto);
                }

                return listaDto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo lista de motivos");
            }
        }
    }
}