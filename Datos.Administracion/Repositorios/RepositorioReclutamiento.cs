using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dominio.Administracion.Repositorios;
using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades.Reclutamiento;
using Dominio.Administracion.Entidades.ModeloCotizacion;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioReclutamiento : IRepositorioReclutamiento
    {
        public bool CargueReclutamiento(List<FileReclutamiento> infoFile, int numeroDocumento, int estadoCargue)
        {
            try
            {
                using (var table = new DataTable())
                {
                    table.Columns.Add("NombreOriginal", typeof(string));
                    table.Columns.Add("NombreArchivo", typeof(string));
                    table.Columns.Add("ContentType", typeof(string));
                    table.Columns.Add("RutaArchivo", typeof(string));
                    table.Columns.Add("Input", typeof(string));

                    int count = infoFile.Count;

                    foreach (var item in infoFile)
                    {
                        if(estadoCargue == 8)
                        {
                            table.Rows.Add(item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo, item.Input);
                        }
                        else
                        {
                            if (count < 7)
                            {
                                if (item.Input == "file1" || item.Input == "file2" || item.Input == "file3" || item.Input == "file4")
                                    table.Rows.Add(item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo, item.Input);
                                
                                if (item.Input == "file5")
                                {
                                    if(count == 7)
                                        table.Rows.Add(item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo, item.Input);
                                    else
                                    {
                                        if(estadoCargue != 0)
                                            table.Rows.Add(item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo, item.Input);
                                        else
                                        {
                                            table.Rows.Add(item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo, item.Input);
                                            table.Rows.Add("ND", "ND", "ND", "ND", "file6");
                                        }
                                    }
                                }

                                if (item.Input == "file6")
                                {
                                    if (count == 7)
                                        table.Rows.Add(item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo, item.Input);
                                    else
                                    {
                                        if (estadoCargue != 0)
                                            table.Rows.Add(item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo, item.Input);
                                        else
                                        {
                                            table.Rows.Add("ND", "ND", "ND", "ND", "file5");
                                            table.Rows.Add(item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo, item.Input);
                                        }
                                    }
                                }

                                if (item.Input == "file7")
                                {
                                    if(count == 5 && estadoCargue == 0)
                                    {
                                        table.Rows.Add("ND", "ND", "ND", "ND", "file5");
                                        table.Rows.Add("ND", "ND", "ND", "ND", "file6");
                                    }

                                    table.Rows.Add(item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo, item.Input);
                                }
                            }

                            if (count == 7)
                                table.Rows.Add(item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo, item.Input);
                        }
                    }

                    using (var modelo = new ContextoPortal())
                    {
                        var infoList = new SqlParameter("@FileInfo", SqlDbType.Structured);
                        infoList.TypeName = "dbo.FileReclutamiento";
                        infoList.Value = table;

                        var result = modelo.Database.ExecuteSqlCommand("SCISP_CargueArchivoReclutamiento @EstadoCargue, @NumeroDocumento, @FileInfo, @State",
                            new SqlParameter("@EstadoCargue", estadoCargue),
                            new SqlParameter("@NumeroDocumento", numeroDocumento),
                                infoList,
                                new SqlParameter("@State", ParameterDirection.Output)
                            );

                        if (result >= 1)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarCertificacion(Int64 numeroDocumento, string nombreOriginal, string nombreArchivo, string rutaArchivo)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<int>("SCISP_ActualizarCertificadoReclutamiento @NumeroDocumento, @NombreOriginal, @NombreArchivo, @RutaArchivo",
                        new SqlParameter("@NumeroDocumento", numeroDocumento),
                        new SqlParameter("@NombreOriginal", nombreOriginal),
                        new SqlParameter("@NombreArchivo", nombreArchivo),
                        new SqlParameter("@RutaArchivo", rutaArchivo)).First();

                    if (result >= 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<NotaReclutamiento> ObtenerNotaReclutamiento(Int64 numeroDocumento)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var lstNotaReclutamiento = modelo.Database.SqlQuery<NotaReclutamiento>("SCISP_ObtenerNotasReclutamiento @NumeroDocumento",
                            new SqlParameter("@NumeroDocumento", numeroDocumento)).ToList();

                    return lstNotaReclutamiento;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<EstadoArchivoReclutamiento> ObtenerEstadArchivoReclutamiento(int numeroDocumento)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var lstEstadoArchivo = modelo.Database.SqlQuery<EstadoArchivoReclutamiento>("SCISP_ObtenerEstadoArchivoReclutamiento @NumeroDocumento",
                            new SqlParameter("@NumeroDocumento", numeroDocumento)).ToList();

                    return lstEstadoArchivo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoCiudad> ObtenerCiudadReclutamiento()
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var lstEstadoCiudad = modelo.Database.SqlQuery<ReclutamientoCiudad>("SCISP_ObtenerCiudadesReclutamiento").ToList();
                    return lstEstadoCiudad;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoReferido> ObtenerReferidosPorCiudad(int ciudadId)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var lstReferidos = modelo.Database.SqlQuery<ReclutamientoReferido>("SCISP_ObtenerReferidosPorCiudad @CiudadId",
                            new SqlParameter("@CiudadId", ciudadId)).ToList();
                    return lstReferidos;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<SolicitudProspecto> ObtenerSolicitudesProspecto(int userId, bool first, string proceso, string estado, string director, DateTime? fechaInicio, DateTime? fechaFin, string numeroDocumento)
        {
            var obj = new List<SolicitudProspecto>();
            var objArchivo = new List<ArchivosProspecto>();

            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var lstSolicitudes = modelo.Database.SqlQuery<SolicitudesProspecto>("SCISP_ObtenerSolicitudesReclutamiento @IdUsr, @First, @Proceso, @Estado, @Director, @FechaInicio, @FechaFin, @NumeroDocumento",
                            new SqlParameter("@IdUsr", userId)
                            , new SqlParameter("@First", first)
                            , new SqlParameter("@Proceso", proceso)
                            , new SqlParameter("@Estado", estado)
                            , new SqlParameter("@Director", director)
                            , new SqlParameter("@FechaInicio", fechaInicio)
                            , new SqlParameter("@FechaFin", fechaFin)
                            , new SqlParameter("@NumeroDocumento", numeroDocumento)
                            ).ToList();

                    var count = 0;
                    foreach (var item in lstSolicitudes)
                    {
                        var result = lstSolicitudes.Where(x => x.Id == item.Id).Count();

                        if (result == 1)
                        {
                            objArchivo = new List<ArchivosProspecto>();

                            objArchivo.Add(new ArchivosProspecto
                            {
                                NombreOriginal = item.NombreOriginal,
                                RutaArchivo = item.RutaArchivo,
                                EstadoArchivo = item.EstadoArchivo
                            });

                            obj.Add(new SolicitudProspecto
                            {
                                Id = item.Id
                                ,TipoDocumento = item.TipoDocumento
                                ,NumeroDocumento = item.NumeroDocumento
                                ,NombresApellidos = item.NombresApellidos
                                ,Estado = item.Estado
                                ,Proceso = item.Proceso
                                ,CorreoElectronico = item.CorreoElectronico
                                ,Telefono = item.Telefono
                                ,CiudadVinculacion = item.CiudadVinculacion
                                ,TipoReclutador = item.TipoReclutador
                                ,Gestionado = item.Gestionado
                                ,IdUsrDirector = item.IdUsrDirector
                                ,LogUsr = item.LogUsr
                                ,NomUsr = item.NomUsr
                                ,FechaRegistro = item.FechaRegistro
                                ,Archivos = objArchivo
                            });
                        }

                        if (result > 1)
                        {
                            count++;

                            if (count == 1)
                                objArchivo = new List<ArchivosProspecto>();

                            objArchivo.Add(new ArchivosProspecto
                            {
                                NombreOriginal = item.NombreOriginal,
                                RutaArchivo = item.RutaArchivo,
                                EstadoArchivo = item.EstadoArchivo
                            });

                            if (count == result)
                            {
                                obj.Add(new SolicitudProspecto
                                {
                                    Id = item.Id
                                    ,TipoDocumento = item.TipoDocumento
                                    ,NumeroDocumento = item.NumeroDocumento
                                    ,NombresApellidos = item.NombresApellidos
                                    ,Estado = item.Estado
                                    ,Proceso = item.Proceso
                                    ,CorreoElectronico = item.CorreoElectronico
                                    ,Telefono = item.Telefono
                                    ,CiudadVinculacion = item.CiudadVinculacion
                                    ,TipoReclutador = item.TipoReclutador
                                    ,Gestionado = item.Gestionado
                                    ,IdUsrDirector = item.IdUsrDirector
                                    ,LogUsr = item.LogUsr
                                    ,NomUsr = item.NomUsr
                                    ,FechaRegistro = item.FechaRegistro
                                    ,Archivos = objArchivo
                                });

                                count = 0;
                            }
                        }

                    }
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<GestionContrato> ObtenerGestionContratoProspecto(int userId)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var lstContrato = modelo.Database.SqlQuery<GestionContrato>("SCISP_ObtenerGestionContratoProspecto @IdUsr",
                            new SqlParameter("@IdUsr", userId)).ToList();
                    return lstContrato;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarSolicitudProspecto(GestionSolicitud gestion)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.ExecuteSqlCommand("SCISP_ActualizarSolicitudProspecto @Id, @File1, @File2, @File3, @File4, @File5, @File6, @File7, @Estado, @Observaciones, @State",
                            new SqlParameter("@Id", gestion.IdProspecto),
                            new SqlParameter("@File1", gestion.File1),
                            new SqlParameter("@File2", gestion.File2),
                            new SqlParameter("@File3", gestion.File3),
                            new SqlParameter("@File4", gestion.File4),
                            new SqlParameter("@File5", gestion.File5),
                            new SqlParameter("@File6", gestion.File6),
                            new SqlParameter("@File7", gestion.File7),
                            new SqlParameter("@Estado", gestion.Estado),
                            new SqlParameter("@Observaciones", gestion.Observaciones),
                            new SqlParameter("@State", ParameterDirection.Output)
                            );

                        if (result >= 1)
                            return true;
                        else
                            return false;
                    }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarSolicitudContrato(GestionSolicitudContrato gestion)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.ExecuteSqlCommand("SCISP_ActualizarSolicitudContratoProspecto @Id, @InformacionPersonal, @Sarlaft, @ExperienciaComercial, @Contrato, @CertificacionTributaria, @Estado, @Observaciones, @State",
                            new SqlParameter("@Id", gestion.IdProspecto),
                            new SqlParameter("@InformacionPersonal", gestion.InformacionPersonal),
                            new SqlParameter("@Sarlaft", gestion.Sarlaft),
                            new SqlParameter("@ExperienciaComercial", gestion.ExperienciaComercial),
                            new SqlParameter("@Contrato", gestion.Contrato),
                            new SqlParameter("@CertificacionTributaria", gestion.CertificacionTributaria),
                            new SqlParameter("@Estado", gestion.Estado),
                            new SqlParameter("@Observaciones", gestion.Observaciones),
                            new SqlParameter("@State", ParameterDirection.Output)
                            );

                    if (result >= 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarSolicitudCapacitacion(GestionCapacitacion gestion)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {

                    if (gestion.FechaInicio == null)
                        gestion.FechaInicio = "";

                    if (gestion.FechaFin == null)
                        gestion.FechaFin = "";

                    var result = modelo.Database.ExecuteSqlCommand("SCISP_ActualizarSolicitudCapacitacion @IdProspecto, @Estado, @CapacitadorId, @FechaInicial, @FechaFinal, @Observaciones, @State",
                            new SqlParameter("@IdProspecto", gestion.IdProspecto),
                            new SqlParameter("@Estado", gestion.Estado),
                            new SqlParameter("@CapacitadorId", gestion.CapacitadorId),
                            new SqlParameter("@FechaInicial", gestion.FechaInicio),
                            new SqlParameter("@FechaFinal", gestion.FechaFin),
                            new SqlParameter("@Observaciones", gestion.Observaciones),
                            new SqlParameter("@State", ParameterDirection.Output)
                            );

                    if (result >= 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<SolicitudesCapacitacion> ObtenerSolicitudesCapacitacion(int userId, string estado, bool first)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {

                    var lstSolicitudes = modelo.Database.SqlQuery<SolicitudesCapacitacion>("SCISP_ObtenerSolicitudesCapacitacion @IdUsr, @Estado, @First",
                        new SqlParameter("@IdUsr", userId)
                        , new SqlParameter("@Estado", estado)
                        , new SqlParameter("@First", first)).ToList();

                    return lstSolicitudes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoCapacitador> ObtenerCapacitadores()
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {

                    var lstSolicitudes = modelo.Database.SqlQuery<ReclutamientoCapacitador>("SCISP_ObtenerCapacitadores").ToList();

                    return lstSolicitudes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<NotaCapacitacion> ObtenerNotasCapacitacion(int numeroDocumento)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var lstNotaCapacitacion = modelo.Database.SqlQuery<NotaCapacitacion>("SCISP_ObtenerNotasCapacitacion @NumeroDocumento",
                            new SqlParameter("@NumeroDocumento", numeroDocumento)).ToList();

                    return lstNotaCapacitacion;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool IngresoProspecto(Int64 numeroDocumento, string pass)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<int>("SCISP_IngresoProspecto @NumeroDocumento, @Password",
                            new SqlParameter("@NumeroDocumento", numeroDocumento),
                            new SqlParameter("@Password", pass)).First();

                    if (result == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ValidarIngresoProspecto(Int64 numeroDocumento, bool validate)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<int>("SCISP_ValidarIngresoProspecto @NumeroDocumento, @Validate",
                            new SqlParameter("@NumeroDocumento", numeroDocumento),
                            new SqlParameter("@Validate", validate)).First();

                    if (result == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoPais> ObtenerPaisesReclutamiento()
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<ReclutamientoPais>("SCISP_ObtenerPaisesReclutamiento").ToList();
                    
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoEps> ObtenerEpsReclutamiento()
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<ReclutamientoEps>("SCISP_ObtenerEpsReclutamiento").ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoPensiones> ObtenerPensionesReclutamiento()
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<ReclutamientoPensiones>("SCISP_ObtenerPensionesReclutamiento").ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoNivelEducativo> ObtenerNivelEducativoReclutamiento()
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<ReclutamientoNivelEducativo>("SCISP_ObtenerNivelEducativoReclutamiento").ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoProfesiones> ObtenerProfesionesReclutamiento()
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<ReclutamientoProfesiones>("SCISP_ObtenerProfesionesReclutamiento").ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool CompletarRegistro(ReclutamientoRegistro registro)
        {
            try
            {
                var operaciones = registro.Operaciones ?? "";

                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<int>("SCISP_CompletarRegistroReclutamiento @FechaNacimiento,@PaisId,@Estrato,@Direccion,@Barrio,@EpsId,@PensionId,@EstadoCivil,@NivelEducativoId,@FuenteIngresos,@TerminosCondiciones,@ProfesionId,@Recursos,@PrSlft1,@PrSlft2,@PrSlft3,@PrSlft4,@PrSlft5,@PrSlft6,@Operaciones,@Anios,@PrimerSector,@SegundoSector,@Pregunta1,@Pregunta2,@Pregunta3,@Pregunta4,@Pregunta5,@Pregunta6,@Pregunta7,@Pregunta8,@Pregunta9,@Pregunta10,@Pregunta11,@Pregunta12,@Pregunta13,@Pregunta14,@Hijo1,@Hijo2,@Hijo3,@NumeroDocumento"
                            , new SqlParameter("@FechaNacimiento", registro.FechaNacimiento)
                            , new SqlParameter("@PaIsId", registro.PaisId)
                            , new SqlParameter("@Estrato", registro.Estrato)
                            , new SqlParameter("@Direccion", registro.Direccion)
                            , new SqlParameter("@Barrio", registro.Barrio)
                            , new SqlParameter("@EpsId", registro.EpsId)
                            , new SqlParameter("@PensionId", registro.PensionId)
                            , new SqlParameter("@EstadoCivil", registro.EstadoCivil)
                            , new SqlParameter("@NivelEducativoId", registro.NivelEducativoId)
                            , new SqlParameter("@FuenteIngresos", registro.FuenteIngresos)
                            , new SqlParameter("@TerminosCondiciones", registro.TerminosCondiciones)
                            , new SqlParameter("@ProfesionId", registro.ProfesionId)
                            , new SqlParameter("@Recursos", registro.Recursos)
                            , new SqlParameter("@PrSlft1", registro.PrSlft1)
                            , new SqlParameter("@PrSlft2", registro.PrSlft2)
                            , new SqlParameter("@PrSlft3", registro.PrSlft3)
                            , new SqlParameter("@PrSlft4", registro.PrSlft4)
                            , new SqlParameter("@PrSlft5", registro.PrSlft5)
                            , new SqlParameter("@PrSlft6", registro.PrSlft6)
                            , new SqlParameter("@Operaciones", operaciones)
                            , new SqlParameter("@Anios", registro.Anios)
                            , new SqlParameter("@PrimerSector", registro.PrimerSector)
                            , new SqlParameter("@SegundoSector", registro.SegundoSector)
                            , new SqlParameter("@Pregunta1", registro.Pregunta1)
                            , new SqlParameter("@Pregunta2", registro.Pregunta2)
                            , new SqlParameter("@Pregunta3", registro.Pregunta3)
                            , new SqlParameter("@Pregunta4", registro.Pregunta4)
                            , new SqlParameter("@Pregunta5", registro.Pregunta5)
                            , new SqlParameter("@Pregunta6", registro.Pregunta6)
                            , new SqlParameter("@Pregunta7", registro.Pregunta7)
                            , new SqlParameter("@Pregunta8", registro.Pregunta8)
                            , new SqlParameter("@Pregunta9", registro.Pregunta9)
                            , new SqlParameter("@Pregunta10", registro.Pregunta10)
                            , new SqlParameter("@Pregunta11", registro.Pregunta11)
                            , new SqlParameter("@Pregunta12", registro.Pregunta12)
                            , new SqlParameter("@Pregunta13", registro.Pregunta13)
                            , new SqlParameter("@Pregunta14", registro.Pregunta14)
                            , new SqlParameter("@Hijo1", registro.Hijo1)
                            , new SqlParameter("@Hijo2", registro.Hijo2)
                            , new SqlParameter("@Hijo3", registro.Hijo3)
                            , new SqlParameter("@NumeroDocumento", registro.NumeroDocumento)).FirstOrDefault();

                    if (result >= 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<SolicitudContratacion> ObtenerProcesoContrataciones(int userId, string estado, bool first)
        {
            var obj = new List<SolicitudContratacion>();
            var objArchivo = new List<ArchivosContratacion>();

            using (var modelo = new ContextoPortal())
            {
                var lstContratacion = modelo.Database.SqlQuery<SolicitudesContratacion>("SCISP_ObtenerContratacionesReclutamiento @IdUsr, @Estado, @First",
                        new SqlParameter("@IdUsr", userId)
                        , new SqlParameter("@Estado", estado)
                        , new SqlParameter("@First", first)).ToList();

                var count = 0;
                foreach (var item in lstContratacion)
                {
                    var result = lstContratacion.Where(x => x.Id == item.Id).Count();

                    if (result == 1)
                    {
                        objArchivo = new List<ArchivosContratacion>();

                        objArchivo.Add(new ArchivosContratacion
                        {
                            NombreOriginal = item.NombreOriginal,
                            RutaArchivo = item.RutaArchivo,
                            EstadoArchivo = item.EstadoArchivo,
                            NombreOriginalContratacion = item.NombreOriginalContratacion,
                            RutaArchivoContratacion = item.RutaArchivoContratacion
                        });

                        obj.Add(new SolicitudContratacion
                        {
                            Id = item.Id,
                            NombresApellidos = item.NombresApellidos,
                            TipoIdentificacion = item.TipoIdentificacion,
                            NumeroDocumento = item.NumeroDocumento,
                            Direccion = item.Direccion,
                            Telefono = item.Telefono,
                            Proceso = item.Proceso,
                            Estado = item.Estado,
                            CorreoElectronico = item.CorreoElectronico,
                            Ciudad = item.Ciudad,
                            TipoReclutador = item.TipoReclutador,
                            NombresApellidosReclutamiento = item.NombresApellidosReclutamiento,
                            FechaRegistro = item.FechaRegistro,
                            InformacionPersonal = item.InformacionPersonal,
                            Sarlaft = item.Sarlaft,
                            ExperienciaComercial = item.ExperienciaComercial,
                            CertificacionTributaria = item.CertificacionTributaria,
                            Contrato = item.Contrato,
                            Archivos = objArchivo
                        });
                    }

                    if (result > 1)
                    {
                        count++;

                        if (count == 1)
                            objArchivo = new List<ArchivosContratacion>();

                        objArchivo.Add(new ArchivosContratacion
                        {
                            NombreOriginal = item.NombreOriginal,
                            RutaArchivo = item.RutaArchivo,
                            EstadoArchivo = item.EstadoArchivo,
                            NombreOriginalContratacion = item.NombreOriginalContratacion,
                            RutaArchivoContratacion = item.RutaArchivoContratacion
                        });

                        if (count == result)
                        {
                            obj.Add(new SolicitudContratacion
                            {
                                Id = item.Id,
                                NombresApellidos = item.NombresApellidos,
                                TipoIdentificacion = item.TipoIdentificacion,
                                NumeroDocumento = item.NumeroDocumento,
                                Direccion = item.Direccion,
                                Telefono = item.Telefono,
                                Proceso = item.Proceso,
                                Estado = item.Estado,
                                CorreoElectronico = item.CorreoElectronico,
                                Ciudad = item.Ciudad,
                                TipoReclutador = item.TipoReclutador,
                                NombresApellidosReclutamiento = item.NombresApellidosReclutamiento,
                                FechaRegistro = item.FechaRegistro,
                                InformacionPersonal = item.InformacionPersonal,
                                Sarlaft = item.Sarlaft,
                                ExperienciaComercial = item.ExperienciaComercial,
                                CertificacionTributaria = item.CertificacionTributaria,
                                Contrato = item.Contrato,
                                Archivos = objArchivo
                            });

                            count = 0;
                        }
                    }

                }
            }

            return obj;
        }

        public bool ActualizarSolicitudContratacion(GestionContratacion gestion)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.ExecuteSqlCommand("SCISP_ActualizarSolicitudContratacion @IdProspecto, @Estado, @File1, @File2, @File3, @File4, @File5, @File6, @File7, @InformacionPersonal, @Sarlaft, @ExperienciaComercial, @Contrato, @CertificacionTributaria, @Observaciones, @State",
                            new SqlParameter("@IdProspecto", gestion.IdProspecto),
                            new SqlParameter("@Estado", gestion.Estado),
                            new SqlParameter("@File1", gestion.File1),
                            new SqlParameter("@File2", gestion.File2),
                            new SqlParameter("@File3", gestion.File3),
                            new SqlParameter("@File4", gestion.File4),
                            new SqlParameter("@File5", gestion.File5),
                            new SqlParameter("@File6", gestion.File6),
                            new SqlParameter("@File7", gestion.File7),
                            new SqlParameter("@InformacionPersonal", gestion.InformacionPersonal),
                            new SqlParameter("@Sarlaft", gestion.Sarlaft),
                            new SqlParameter("@ExperienciaComercial", gestion.ExperienciaComercial),
                            new SqlParameter("@Contrato", gestion.Contrato),
                            new SqlParameter("@CertificacionTributaria", gestion.CertificacionTributaria),
                            new SqlParameter("@Observaciones", gestion.Observaciones),
                            new SqlParameter("@State", ParameterDirection.Output)
                            );

                    if (result >= 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoInfo> ObtenerInfoReclutamiento(Int64 numeroDocumento, bool recovery)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<ReclutamientoInfo>("SCISP_ObtenerInfoReclutamiento @NumeroDocumento, @Recovery"
                                , new SqlParameter("@NumeroDocumento", numeroDocumento)
                                , new SqlParameter("@Recovery", recovery)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ValidarRecovery(Int64 numeroDocumento, string recovery)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<int>("SCISP_ValidarRecoveryReclutamiento @NumeroDocumento, @UrlRecovery",
                            new SqlParameter("@NumeroDocumento", numeroDocumento),
                            new SqlParameter("@UrlRecovery", recovery)).FirstOrDefault();

                    if (result == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarContrasenia(Int64 numeroDocumento, string passwordI, string rcvr)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<int>("SCISP_ActualizarContraseniaReclutamiento @NumeroDocumento, @Password, @Recovery",
                            new SqlParameter("@NumeroDocumento", numeroDocumento)
                            ,new SqlParameter("@Password", passwordI)
                            ,new SqlParameter("@Recovery", rcvr)).FirstOrDefault();

                    if (result == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoTipoIdentificacion> ObtenerTipoIdentificacion()
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var lstTipoIdentificacion = modelo.Database.SqlQuery<ReclutamientoTipoIdentificacion>("SCISP_ObtenerTipoIdentificacionReclutamiento").ToList();
                    return lstTipoIdentificacion;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public ReclutamientoListaRestrictiva ValidarListaRestrictiva(string numeroDocumento)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var listaRestrictiva = modelo.Database.SqlQuery<ReclutamientoListaRestrictiva>("SP_LISTA_RESTRICTIVA @TERCERO",
                                                new SqlParameter("@TERCERO", numeroDocumento)).First();
                    return listaRestrictiva;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool GuardarTrazaContrato(Int64 numeroDocumento, string ip)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.ExecuteSqlCommand("SCISP_RegistrarTrazaContrato @NumeroDocumento, @Ip, @State",
                                new SqlParameter("@NumeroDocumento", numeroDocumento)
                                ,new SqlParameter("@Ip", ip)
                                ,new SqlParameter("@State", ParameterDirection.Output));

                    if (result == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool GestionarSucursal(int? id, string nombre, bool activo, string tipo)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.ExecuteSqlCommand("SCISP_GestionarCiudadReclutamiento @Id, @Nombre, @Activo, @Tipo",
                                new SqlParameter("@Id", id)
                                , new SqlParameter("@Nombre", nombre)
                                , new SqlParameter("@Activo", activo)
                                , new SqlParameter("@Tipo", tipo));

                    if (result == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoCiudad> ObtenerSucursales(int? id)
        {
            try
            {
                var lstSucursales = new List<ReclutamientoCiudad>();

                using (var modelo = new ContextoPortal())
                {
                    if(id == null)
                    {
                        lstSucursales = modelo.Database.SqlQuery<ReclutamientoCiudad>("SCISP_GestionarCiudadReclutamiento @Id, @Nombre, @Activo, @Tipo"
                                                        , new SqlParameter("@Id", DBNull.Value)
                                                        , new SqlParameter("@Nombre", string.Empty)
                                                        , new SqlParameter("@Activo", true)
                                                        , new SqlParameter("@Tipo", "Read")).ToList();
                    }
                    if (id != null)
                    {
                        lstSucursales = modelo.Database.SqlQuery<ReclutamientoCiudad>("SCISP_GestionarCiudadReclutamiento @Id, @Nombre, @Activo, @Tipo"
                                                        , new SqlParameter("@Id", id)
                                                        , new SqlParameter("@Nombre", string.Empty)
                                                        , new SqlParameter("@Activo", true)
                                                        , new SqlParameter("@Tipo", "Read")).ToList();
                    }
                    
                    return lstSucursales;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool GestionarCapacitador(int? id, int? idUsr, string nombres, string apellidos, bool activo, string tipo)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.ExecuteSqlCommand("SCISP_GestionarCapacitadorReclutamiento @Id, @IdUsr, @Nombres, @Apellidos, @Activo, @Tipo",
                                new SqlParameter("@Id", id)
                                , new SqlParameter("@IdUsr", idUsr)
                                , new SqlParameter("@Nombres", nombres)
                                , new SqlParameter("@Apellidos", apellidos)
                                , new SqlParameter("@Activo", activo)
                                , new SqlParameter("@Tipo", tipo));

                    if (result == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoCapacitador> ObtenerCapacitadores(int? id)
        {
            try
            {
                var lstCapacitadores = new List<ReclutamientoCapacitador>();

                using (var modelo = new ContextoPortal())
                {
                    if (id == null)
                    {
                        lstCapacitadores = modelo.Database.SqlQuery<ReclutamientoCapacitador>("SCISP_GestionarCapacitadorReclutamiento @Id, @IdUsr, @Nombres, @Apellidos, @Activo, @Tipo",
                                                        new SqlParameter("@Id", DBNull.Value)
                                                        , new SqlParameter("@IdUsr", DBNull.Value)
                                                        , new SqlParameter("@Nombres", string.Empty)
                                                        , new SqlParameter("@Apellidos", string.Empty)
                                                        , new SqlParameter("@Activo", true)
                                                        , new SqlParameter("@Tipo", "Read")).ToList();
                    }
                    if (id != null)
                    {
                        lstCapacitadores = modelo.Database.SqlQuery<ReclutamientoCapacitador>("SCISP_GestionarCapacitadorReclutamiento @Id, @IdUsr, @Nombres, @Apellidos, @Activo, @Tipo",
                                                        new SqlParameter("@Id", id)
                                                        , new SqlParameter("@IdUsr", DBNull.Value)
                                                        , new SqlParameter("@Nombres", string.Empty)
                                                        , new SqlParameter("@Apellidos", string.Empty)
                                                        , new SqlParameter("@Activo", true)
                                                        , new SqlParameter("@Tipo", "Read")).ToList();
                    }

                    return lstCapacitadores;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoInfoCompleta> ObtenerInformacionCompleta(Int64 numeroDocumento)
        {
            try
            {
                var lstInfoCompleta = new List<ReclutamientoInfoCompleta>();

                using (var modelo = new ContextoPortal())
                {
                    lstInfoCompleta = modelo.Database.SqlQuery<ReclutamientoInfoCompleta>("SCISP_ObtenerInfoCompletaReclutamiento @NumeroDocumento"
                                                    , new SqlParameter("@NumeroDocumento", numeroDocumento)).ToList();

                    return lstInfoCompleta;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarRegistro(ReclutamientoRegistro registro)
        {
            try
            {
                var operaciones = registro.Operaciones ?? "";

                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.SqlQuery<int>("SCISP_ActualizarRegistroReclutamiento @FechaNacimiento,@PaisId,@Estrato,@Direccion,@Barrio,@EpsId,@PensionId,@EstadoCivil,@NivelEducativoId,@FuenteIngresos,@TerminosCondiciones,@ProfesionId,@Recursos,@PrSlft1,@PrSlft2,@PrSlft3,@PrSlft4,@PrSlft5,@PrSlft6,@Operaciones,@Anios,@PrimerSector,@SegundoSector,@Pregunta1,@Pregunta2,@Pregunta3,@Pregunta4,@Pregunta5,@Pregunta6,@Pregunta7,@Pregunta8,@Pregunta9,@Pregunta10,@Pregunta11,@Pregunta12,@Pregunta13,@Pregunta14,@Hijo1,@Hijo2,@Hijo3,@NumeroDocumento"
                            , new SqlParameter("@FechaNacimiento", registro.FechaNacimiento)
                            , new SqlParameter("@PaIsId", registro.PaisId)
                            , new SqlParameter("@Estrato", registro.Estrato)
                            , new SqlParameter("@Direccion", registro.Direccion)
                            , new SqlParameter("@Barrio", registro.Barrio)
                            , new SqlParameter("@EpsId", registro.EpsId)
                            , new SqlParameter("@PensionId", registro.PensionId)
                            , new SqlParameter("@EstadoCivil", registro.EstadoCivil)
                            , new SqlParameter("@NivelEducativoId", registro.NivelEducativoId)
                            , new SqlParameter("@FuenteIngresos", registro.FuenteIngresos)
                            , new SqlParameter("@TerminosCondiciones", registro.TerminosCondiciones)
                            , new SqlParameter("@ProfesionId", registro.ProfesionId)
                            , new SqlParameter("@Recursos", registro.Recursos)
                            , new SqlParameter("@PrSlft1", registro.PrSlft1)
                            , new SqlParameter("@PrSlft2", registro.PrSlft2)
                            , new SqlParameter("@PrSlft3", registro.PrSlft3)
                            , new SqlParameter("@PrSlft4", registro.PrSlft4)
                            , new SqlParameter("@PrSlft5", registro.PrSlft5)
                            , new SqlParameter("@PrSlft6", registro.PrSlft6)
                            , new SqlParameter("@Operaciones", operaciones)
                            , new SqlParameter("@Anios", registro.Anios)
                            , new SqlParameter("@PrimerSector", registro.PrimerSector)
                            , new SqlParameter("@SegundoSector", registro.SegundoSector)
                            , new SqlParameter("@Pregunta1", registro.Pregunta1)
                            , new SqlParameter("@Pregunta2", registro.Pregunta2)
                            , new SqlParameter("@Pregunta3", registro.Pregunta3)
                            , new SqlParameter("@Pregunta4", registro.Pregunta4)
                            , new SqlParameter("@Pregunta5", registro.Pregunta5)
                            , new SqlParameter("@Pregunta6", registro.Pregunta6)
                            , new SqlParameter("@Pregunta7", registro.Pregunta7)
                            , new SqlParameter("@Pregunta8", registro.Pregunta8)
                            , new SqlParameter("@Pregunta9", registro.Pregunta9)
                            , new SqlParameter("@Pregunta10", registro.Pregunta10)
                            , new SqlParameter("@Pregunta11", registro.Pregunta11)
                            , new SqlParameter("@Pregunta12", registro.Pregunta12)
                            , new SqlParameter("@Pregunta13", registro.Pregunta13)
                            , new SqlParameter("@Pregunta14", registro.Pregunta14)
                            , new SqlParameter("@Hijo1", registro.Hijo1)
                            , new SqlParameter("@Hijo2", registro.Hijo2)
                            , new SqlParameter("@Hijo3", registro.Hijo3)
                            , new SqlParameter("@NumeroDocumento", registro.NumeroDocumento)).FirstOrDefault();

                    if (result >= 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoProceso> ObtenerProcesoNumeroIdentificacion(Int64 numeroDocumento)
        {
            try
            {
                var lstProceso = new List<ReclutamientoProceso>();

                using (var modelo = new ContextoPortal())
                {
                    lstProceso = modelo.Database.SqlQuery<ReclutamientoProceso>("SCISP_ObtenerReclutamientoProceso @NumeroDocumento"
                                                    , new SqlParameter("@NumeroDocumento", numeroDocumento)).ToList();

                    return lstProceso;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoDirector> ObtenerDirectoresPorReferido(int referidoId, string tipo)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var lstReferidos = modelo.Database.SqlQuery<ReclutamientoDirector>("SCISP_ObtenerDirectoresPorReferido @ReferidoId, @Tipo",
                            new SqlParameter("@ReferidoId", referidoId)
                            ,new SqlParameter("@Tipo", tipo)).ToList();
                    return lstReferidos;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoReclutadores> ObtenerReclutadores(int? id)
        {
            try
            {
                var lstReclutadores = new List<ReclutamientoReclutadores>();

                using (var modelo = new ContextoPortal())
                {
                    if (id == null)
                    {
                        lstReclutadores = modelo.Database.SqlQuery<ReclutamientoReclutadores>("SCISP_ObtenerReclutadoresReclutamiento @IdReclutador, @First",
                                                        new SqlParameter("@IdReclutador", DBNull.Value)
                                                        , new SqlParameter("@First", false)).ToList();
                    }
                    if (id != null)
                    {
                        lstReclutadores = modelo.Database.SqlQuery<ReclutamientoReclutadores>("SCISP_GestionarCapacitadorReclutamiento @Id, @IdUsr, @Nombres, @Apellidos, @Activo, @Tipo",
                                                        new SqlParameter("@Id", id)
                                                        , new SqlParameter("@IdUsr", DBNull.Value)
                                                        , new SqlParameter("@Nombres", string.Empty)
                                                        , new SqlParameter("@Apellidos", string.Empty)
                                                        , new SqlParameter("@Activo", true)
                                                        , new SqlParameter("@Tipo", "Read")).ToList();
                    }

                    return lstReclutadores;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool GestionarReclutadores(int id, int? idUsr, string nombres, string apellidos, int? ciudadId, string[] directores, string tipo)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var _idUsr = idUsr == null ? 0 : idUsr;
                    var _idCiudad = ciudadId == null ? 0 : ciudadId;
                    var _directores = directores[0].ToString();
                    var result = modelo.Database.ExecuteSqlCommand("SCISP_GestionarReclutadoresReclutamiento @Id, @IdUsr, @Nombres, @Apellidos, @IdCiudad, @Directores, @Tipo",
                                new SqlParameter("@Id", id)
                                , new SqlParameter("@IdUsr", _idUsr)
                                , new SqlParameter("@Nombres", nombres)
                                , new SqlParameter("@Apellidos", apellidos)
                                , new SqlParameter("@IdCiudad", _idCiudad)
                                , new SqlParameter("@Directores", _directores)
                                , new SqlParameter("@Tipo", tipo));

                    if (result >= 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoDirectores> ObtenerDirectores()
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var lstDirectores = modelo.Database.SqlQuery<ReclutamientoDirectores>("SCISP_ObtenerDirectores").ToList();
                    return lstDirectores;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<ReclutamientoReclutadores> ObtenerReclutadorPorId(int id)
        {
            try
            {
                var lstReclutadores = new List<ReclutamientoReclutadores>();

                using (var modelo = new ContextoPortal())
                {
                    lstReclutadores = modelo.Database.SqlQuery<ReclutamientoReclutadores>("SCISP_GestionarReclutadoresReclutamiento @Id, @IdUsr, @Nombres, @Apellidos, @IdCiudad, @Directores, @Tipo",
                                new SqlParameter("@Id", id)
                                , new SqlParameter("@IdUsr", DBNull.Value)
                                , new SqlParameter("@Nombres", "")
                                , new SqlParameter("@Apellidos", "")
                                , new SqlParameter("@IdCiudad", DBNull.Value)
                                , new SqlParameter("@Directores", "")
                                , new SqlParameter("@Tipo", "Read")).ToList();

                    return lstReclutadores;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public ReclutamientoRepresentante ObtenerRepresentanteLegal()
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var representante = modelo.Database.SqlQuery<ReclutamientoRepresentante>("SCISP_ObtenerRepresentanteLegal").First();
                    return representante;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ActualizarCapacitacionReclutador(CapacitacionReclutador gestion)
        {
            try
            {
                using (var modelo = new ContextoPortal())
                {
                    var result = modelo.Database.ExecuteSqlCommand("SCISP_GestionarCapacitacionReclutador @IdProspecto, @Estado, @Observaciones",
                                new SqlParameter("@IdProspecto", gestion.IdProspecto)
                                , new SqlParameter("@Estado", gestion.Estado)
                                , new SqlParameter("@Observaciones", gestion.Observaciones));

                    if (result >= 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}