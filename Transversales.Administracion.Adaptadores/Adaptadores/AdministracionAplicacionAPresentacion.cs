using System.Collections.Generic;
using Aplicacion.Administracion.Dto;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Aplicacion.Administracion.Dto.DtoKheiron;
using AutoMapper;

namespace Transversales.Administracion.Adaptadores
{
    public class AdministracionAplicacionAPresentacion : Profile
    {
        #region Instance Properties

        public override string ProfileName
        {
            get { return "AdministracionAplicacionAPresentacion"; }
        }

        #endregion

        #region Instance Methods

        protected override void Configure()
        {
            Mapper.CreateMap<EmbRolDto, LoginMoldel>();

        }

        #endregion
    }
}