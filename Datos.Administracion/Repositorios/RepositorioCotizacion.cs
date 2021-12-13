using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.Enumeraciones;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Entidades.ReportesDto;
using Dominio.Administracion.Repositorios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioCotizacion : IRepositorioCotizacion
    {
        public string ObtenerProductosCliente(int CotizacionID)
        {
            List<int> idProdus = new List<int>();
            string nomProdus = null;
            using (var bd = new ContextoPortal())
            {
                var lstProd = (from c in bd.Cotizacion_Producto where CotizacionID == c.id_cotizacion select c).ToList();
                var lstnomProd = (from a in bd.Productos select a).ToList();

                //foreach (var p in lstProd)
                //{
                //    if (p.id_cotizacion == CotizacionID)
                //    {
                //        idProdus.Add(p.id_producto);
                //    }
                //}

                if (lstProd != null)
                {
                    foreach (var d in lstProd)
                    {
                        foreach (var a in lstnomProd)
                        {
                            if (d.id_producto == a.COD_SERV)
                            {
                                nomProdus = a.NombreProducto;
                            }
                        }
                    }

                    return nomProdus;
                }
            }
            return null;
        }

        public List<EMSP_ValidarRegistroCotizacionPorProducto> ValidarPermisoRegistroPorProducto(string NIT, int userId, int[] productosIds, string ciudad)
        {
            using (var bd = new ContextoPortal())
            {
                int idPro = 0;
                List<EMSP_ValidarRegistroCotizacionPorProducto> produs = new List<EMSP_ValidarRegistroCotizacionPorProducto>();

                int empresaNIT = Convert.ToInt32(NIT);

                if (productosIds != null)
                {
                    for (int p = 0; p < productosIds.Length; p++)
                    {
                        if (productosIds.Length > 0)
                        {
                            idPro = productosIds[p];
                        }
                    }

                    var Validacion = bd.Database.SqlQuery<EMSP_ValidarRegistroCotizacionPorProducto>("EMSP_VALIDARPERMISOREGISTROCOTIZACIONXPRODUCTO @NIT, @USERID, @CIUDAD, @PRODUCTOID",
                        new SqlParameter("@NIT", empresaNIT),
                        new SqlParameter("@USERID", userId),
                        new SqlParameter("@CIUDAD", ciudad),
                        new SqlParameter("@PRODUCTOID", idPro)).ToList();

                    if (Validacion.Count >= 0 || Validacion != null)
                    {
                        foreach (var co in Validacion)
                        {
                            var bloq = new EMSP_ValidarRegistroCotizacionPorProducto()
                            {
                                CotizacionID = co.CotizacionID,
                                ProductoID = co.ProductoID,
                                ciudadCotiz = co.ciudadCotiz,
                                nombreProducto = co.nombreProducto
                            };
                            produs.Add(bloq);
                        }
                    }
                }
                if (produs.Count <= 0 || produs == null)
                    produs = new List<EMSP_ValidarRegistroCotizacionPorProducto>();

                return produs;
            }
        }

        public List<EMSP_ValidarRegistroCotizacion> ValidarPermisoRegistroPorNIT(string NIT, int userID)
        {
            using (var bd = new ContextoPortal())
            {
                List<EMSP_ValidarRegistroCotizacion> cotizNIT = new List<EMSP_ValidarRegistroCotizacion>();

                decimal empresaNIT = Convert.ToDecimal(NIT);

                var lstAse = bd.Database.SqlQuery<EMSP_ValidarRegistroCotizacion>("EMSP_BLOQUEOSNIT @NIT",
                    new SqlParameter("@NIT", empresaNIT)).ToList();

                foreach (var cot in lstAse)
                {
                    if (cot != null)
                    {
                        if (cot.asesorID == userID)
                        {
                            var NitXCoti = new EMSP_ValidarRegistroCotizacion() { asesorID = userID, cotizacionID = cot.cotizacionID, fechaExpiracion = cot.fechaExpiracion.ToString(), ciudad = cot.ciudad, nombreAsesor = cot.nombreAsesor, nombreDirector = cot.nombreDirector, PRODUCTOID = cot.PRODUCTOID, NombreProducto = cot.NombreProducto };
                            cotizNIT.Add(NitXCoti);
                        }
                        else
                        {
                            var NitXCoti = new EMSP_ValidarRegistroCotizacion() { asesorID = cot.asesorID, cotizacionID = cot.cotizacionID, fechaExpiracion = cot.fechaExpiracion.ToString(), ciudad = cot.ciudad, nombreAsesor = cot.nombreAsesor, nombreDirector = cot.nombreDirector, PRODUCTOID = cot.PRODUCTOID, NombreProducto = cot.NombreProducto };
                            cotizNIT.Add(NitXCoti);
                        }
                    }
                }

                if (cotizNIT == null)
                    cotizNIT = new List<EMSP_ValidarRegistroCotizacion>();
                return cotizNIT;
            }
        }

        private Cotizaciones ObtenerInfoCotizacion(int NIT)
        {
            Cotizaciones cotiza = null;
            using (var modelo = new ContextoPortal())
            {
                var coso = from coti in modelo.Cotizaciones
                           where coti.NIT == NIT
                           select coti;

                if (coso != null && coso.ToList().Count > 0)
                    cotiza = coso.ToList().First();
            }
            return cotiza;
        }

        public bool RegistrarCotizacion(Cotizaciones regiCotizacion, int[] productosID, int userID)
        {
            bool registro = false;
            bool registroCotiAseo = false;
            bool registroCotiProd = false;

            if (registro == false)
            {
                //Guardar en la tabla Cotizaciones
                regiCotizacion.id_cotizacion = regiCotizaciones(regiCotizacion, userID);

                if (regiCotizacion.id_cotizacion >= 0)
                {
                    registroCotiAseo = regiCotizacionAsesor(regiCotizacion.id_cotizacion, userID);
                    registroCotiProd = regiCotizacionProducto(productosID, regiCotizacion.id_cotizacion);
                    if (registroCotiAseo == true && registroCotiProd == true)
                    {
                        return registro = true;
                    }
                    else
                    {
                        return registro = false;
                    }
                }
            }

            return registro = false;
        }

        /// <summary>
        /// Metodo auxiliar para la obtnecion del nombre del sector seleccionado por el usuario
        /// </summary>
        /// <param name="sectorId"></param>
        /// <returns></returns>
        private string ObtenerNombreSector(int? sectorId)
        {
            using (var secto = new ContextoPortal())
            {
                if (sectorId != null)
                {
                    var IdSector = (from a in secto.SECTORES
                                    where a.Id_sector == sectorId
                                    select a.Id_sector).First();

                    var auxSector = IdSector.ToString();

                    return auxSector;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Metodo auxiliar para la obtencion del area protegida seleccionado por el usuario
        /// </summary>
        /// <param name="areasProtegID"></param>
        /// <returns></returns>
        private string ObtenerTipoAreasProtegidas(int? areasProtegID)
        {
            using (var area = new ContextoPortal())
            {
                if (areasProtegID != null)
                {
                    var Idarea = (from a in area.Tipos_AP
                                  where a.Id_area == areasProtegID
                                  select a.Id_area).First();

                    var auxIdArea = Idarea.ToString();
                    return auxIdArea;
                }
                else
                {
                    return null;
                }
            }
        }

        private bool ObtenerValidacion(int[] validacionId)
        {
            bool esValido = false;
            for (int i = 0; i < validacionId.Length; i++)
            {
                if (validacionId[i] == 1)
                {
                    esValido = true;
                }
                else
                {
                    esValido = false;
                }
            }
            return esValido;
        }

        /// <summary>
        /// Metodo auxiliar de registro de cotizacion
        /// </summary>
        /// <param name="regiCotizacion"></param>
        /// <returns></returns>
        private int regiCotizaciones(Cotizaciones regiCotizacion, int usuarioId)
        {
            int respuesta = 0;
            string motivo = null;
            string estadoReserva = null;
            string estadoPricing = null;
            using (var modelo = new ContextoPortal())
            {
                var lstEstado = (from u in modelo.EstadosCotizacion select u).ToList();
                var lstEstadoCoti = (from u in modelo.EstadosPricing select u).ToList();

                foreach (var est in lstEstado)
                {
                    if (est.Id_Estado == 4)
                    {
                        estadoReserva = est.nombreEstado;
                    }
                }

                foreach (var est in lstEstadoCoti)
                {
                    if (est.Id_Estado == 5)
                    {
                        estadoPricing = est.Descripcion;
                    }
                }

                if (regiCotizacion.motivoVisita != null)
                {
                    motivo = regiCotizacion.motivoVisita;
                }
                else
                {
                    motivo = "Registro de la visita";
                }

                var poli = modelo.PoliticaNITS.Find(1);
                Cotizaciones cotizacion = new Cotizaciones()
                {
                    NIT = regiCotizacion.NIT,
                    DV = regiCotizacion.DV,
                    nombreEmpresa = regiCotizacion.nombreEmpresa,
                    contacto = regiCotizacion.contacto,
                    telefono = regiCotizacion.telefono,
                    cargo = regiCotizacion.cargo,
                    celular = regiCotizacion.celular,
                    correoElectronico = regiCotizacion.correoElectronico,
                    fechaVisita = DateTime.Now,
                    motivoVisita = motivo,
                    bloqueo = true,
                    fechaExpiracion = DateTime.Now.AddDays(poli.numero_dias_validez),
                    numeroRenovaciones = 0,
                    estado = estadoReserva,
                    ciudad = regiCotizacion.ciudad,
                    canal = regiCotizacion.canal,
                    EstadoPricing = estadoPricing,
                    TelefonoExt = regiCotizacion.TelefonoExt,
                    ObservacionReserva = regiCotizacion.ObservacionReserva,
                    EstadoCotizacion = regiCotizacion.EstadoCotizacion
                };
                modelo.Cotizaciones.Add(cotizacion);

                modelo.SaveChanges();
                respuesta = (from u in modelo.Cotizaciones select u).Where(x => x.id_cotizacion == cotizacion.id_cotizacion).FirstOrDefault().id_cotizacion;
                GuardarNotaTraza(respuesta.ToString(), usuarioId, regiCotizacion.ObservacionReserva);
            }
            return respuesta;
        }

        public bool Cotizar(string IdCotizacion, string MotivoVisita, string numeroCapacitaciones, string NumeroExpuestos, string numeroEventos, string numeroSedes, int? sectorID, int? areasProtegID, int[] validacionIds, int userId)
        {
            string motivo = null;

            var ObtenerSector = ObtenerNombreSector(sectorID);
            var ObtenerTipoArea = ObtenerTipoAreasProtegidas(areasProtegID);
            var esValida = ObtenerValidacion(validacionIds);

            using (var modelo = new ContextoPortal())
            {
                if (esValida == true)
                {
                    var inforReserv = (from u in modelo.Cotizaciones where u.id_cotizacion.ToString() == IdCotizacion select u).FirstOrDefault();

                    if (inforReserv != null)
                    {
                        if (MotivoVisita != null)
                        {
                            motivo = MotivoVisita;
                        }
                        else
                        {
                            motivo = "Registro de la visita";
                        }
                        if (inforReserv.NumeroCapacitaciones == null || inforReserv.SectorEconomico == null || inforReserv.VeracidadInformacion == null || inforReserv.Total.ToString() == null)
                        {
                            inforReserv.motivoVisita = motivo;
                            inforReserv.SectorEconomico = ObtenerSector;
                            inforReserv.TipoAP = ObtenerTipoArea;
                            inforReserv.VeracidadInformacion = esValida;
                            inforReserv.NumeroCapacitaciones = numeroCapacitaciones;
                            inforReserv.NumeroExpuestos = NumeroExpuestos;
                            inforReserv.NumeroEventos = numeroEventos;
                            inforReserv.NumeroSedes = numeroSedes;

                            modelo.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Metodo auxiliar de registro de cotizaciones por asesor
        /// </summary>
        /// <param name="cotizacionID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private bool regiCotizacionAsesor(int cotizacionID, int userID)
        {
            bool respCotiAseso = false;
            if (respCotiAseso == false)
            {
                using (var modelo = new ContextoPortal())
                {
                    Cotizacion_Asesor cotiXAsesor = new Cotizacion_Asesor()
                    {
                        id_cotizacion = cotizacionID,
                        id_asesor = userID
                    };
                    modelo.Cotizacion_Asesor.Add(cotiXAsesor);
                    modelo.SaveChanges();
                    return respCotiAseso = true;
                }
            }
            return respCotiAseso = false;
        }

        /// <summary>
        /// Metodo auxiliar de cotizacion por producto
        /// </summary>
        /// <param name="productosID"></param>
        /// <param name="idCoti"></param>
        /// <returns></returns>
        private bool regiCotizacionProducto(int[] productosID, int idCoti)
        {
            bool respCotiProd = false;
            if (respCotiProd == false)
            {
                using (var cotaprod = new ContextoPortal())
                {
                    for (int i = 0; i < productosID.Length; i++)
                    {
                        Cotizacion_Producto cotiXProducto = new Cotizacion_Producto()
                        {
                            id_cotizacion = idCoti,
                            id_producto = productosID[i]
                        };
                        cotaprod.Cotizacion_Producto.Add(cotiXProducto);
                        cotaprod.SaveChanges();
                    }
                    return respCotiProd = true;
                }
            }
            return respCotiProd = false;
        }

        /*
         * Metodo que obtiene las ciudad de origen de un asesor o director
         */

        public string ObtenerCiudadAsesor(int userId)
        {
            string ciudadAses = null;

            using (var usua = new ContextoUsuarios())
            {
                using (var sto = new ContextoStone())
                {
                    using (var areaETL = new ContextEMERMEDICA_AreaETL())
                    {
                        var lstusua = (from u in usua.usrCentral select u).ToList();
                        var lstETL = (from u in areaETL.EME_CIUDAD_HOMOLOG select u).ToList();
                        var lstDirec = (from u in sto.CC_DIREC select u).ToList();

                        var docuAseso = (from a in lstusua
                                         where userId == a.id_usr
                                         select a.num_doc).FirstOrDefault();

                        decimal auxDocuAseso = Convert.ToDecimal(docuAseso);
                        if (docuAseso != null)
                        {
                            var ciudAseso = (from a in lstDirec
                                             join d in sto.RE_ESCAL on a.COD_ESCA equals d.COD_ESCA
                                             join b in lstETL on a.COD_CCOS equals b.COD_CCOS
                                             join c in sto.CC_ASESO on a.COD_DIRE equals c.COD_DIRE
                                             where (a.EST_DIRE != "I" || c.EST_ASES != "I") && (c.COD_ASES == auxDocuAseso ||
                                             a.COD_DIRE == auxDocuAseso || a.COD_GERE == auxDocuAseso)
                                             select b.CIUDAD_HOMOLOG).FirstOrDefault();

                            ciudadAses = ciudAseso;

                            return ciudadAses;
                        }
                    }
                }
            }
            return null;
        }

        public string ObtenerCiudadAsesorXDocumento(decimal numDoc)
        {
            string ciudadAses = null;

            using (var usua = new ContextoUsuarios())
            {
                using (var sto = new ContextoStone())
                {
                    using (var areaETL = new ContextEMERMEDICA_AreaETL())
                    {
                        var lstusua = (from u in usua.usrCentral select u).ToList();
                        var lstETL = (from u in areaETL.EME_CIUDAD_HOMOLOG select u).ToList();
                        var lstDirec = (from u in sto.CC_DIREC select u).ToList();

                        //var docuAseso = (from a in lstusua
                        //                 where userId == a.id_usr
                        //                 select a.num_doc).FirstOrDefault();

                        //decimal auxDocuAseso = Convert.ToDecimal(numDoc);
                        if (numDoc > 0)
                        {
                            var ciudAseso = (from a in lstDirec
                                             join d in sto.RE_ESCAL on a.COD_ESCA equals d.COD_ESCA
                                             join b in lstETL on a.COD_CCOS equals b.COD_CCOS
                                             join c in sto.CC_ASESO on a.COD_DIRE equals c.COD_DIRE
                                             where (a.EST_DIRE != "I" || c.EST_ASES != "I") && (c.COD_ASES == numDoc ||
                                             a.COD_DIRE == numDoc || a.COD_GERE == numDoc)
                                             select b.CIUDAD_HOMOLOG).FirstOrDefault();

                            ciudadAses = ciudAseso;

                            return ciudadAses;
                        }
                    }
                }
            }
            return null;
        }

        /*
         * Metodo para obtener el canal de origen de un asesor o director
         */

        public string ObtenerCanalAsesor(int userId)
        {
            string canalAses = null;

            using (var usua = new ContextoUsuarios())
            {
                using (var sto = new ContextoStone())
                {
                    using (var areaETL = new ContextEMERMEDICA_AreaETL())
                    {
                        var lstusua = (from u in usua.usrCentral select u).ToList();
                        var lstETL = (from u in areaETL.EME_CIUDAD_HOMOLOG select u).ToList();
                        var lstDirec = (from u in sto.CC_DIREC select u).ToList();

                        var docuAseso = (from a in lstusua
                                         where userId == a.id_usr
                                         select a.num_doc).FirstOrDefault();
                        decimal auxDocuAseso = Convert.ToDecimal(docuAseso);
                        if (docuAseso != null)
                        {
                            var ciudAseso = (from a in lstDirec
                                             join d in sto.RE_ESCAL on a.COD_ESCA equals d.COD_ESCA
                                             join b in lstETL on a.COD_CCOS equals b.COD_CCOS
                                             join c in sto.CC_ASESO on a.COD_DIRE equals c.COD_DIRE
                                             where (a.EST_DIRE != "I" || c.EST_ASES != "I") && (c.COD_ASES == auxDocuAseso ||
                                             a.COD_DIRE == auxDocuAseso)
                                             select d.NOM_ESCA).FirstOrDefault();

                            if (ciudAseso != null)
                            {
                                if (ciudAseso.Contains("COLPATRIA"))
                                {
                                    canalAses = "COLPATRIA";
                                }
                                else
                                {
                                    canalAses = "EMERMEDICA";
                                }
                            }

                            return canalAses;
                        }
                    }
                }
            }
            return null;
        }

        //Metodo para traer de base de datos la información del correo de princing
        public string ObtenerInfoPricing()
        {
            string retorno = null;
            using (var bd = new ContextoPortal())
            {
                //var lstPric = (from u in bd.Datos_Pricing select u).ToList();

                var correo = (from a in bd.Datos_Pricing
                              where a.Id_funcionario == 1
                              select a.Correo).First();

                if (correo != null)
                {
                    retorno = correo;
                }
                else
                {
                    retorno = null;
                }
            }
            return retorno;
        }

        public string ObtenerPassPricing()
        {
            string retorno = null;
            using (var bd = new ContextoPortal())
            {
                //var lstPric = (from u in bd.Datos_Pricing select u).ToList();

                var contras = (from a in bd.Datos_Pricing
                               where a.Id_funcionario == 1
                               select a.Contrasena).First();

                if (contras != null)
                {
                    retorno = contras;
                }
                else
                {
                    retorno = null;
                }
            }
            return retorno;
        }

        /*
         * Metodo para traer la lista de productos posibles de base de datos
         */

        public List<Productos> ObtenerProductos()
        {
            var retorno = new List<Productos>();
            using (var bd = new ContextoPortal())
            {
                var vGet = bd.Productos;
                if (vGet != null)
                    retorno = vGet.ToList();
            }

            return retorno;
        }


        /*
         * Metodo para check de validacion
         */

        public List<Validacion> ObtenerValidacion()
        {
            var retorno = new List<Validacion>();

            var validacion = new Validacion()
            {
                COD_Validacion = 1,
                veracidadInformacion = "Certifico que la información proporcionada es veraz y exacta."
            };

            retorno.Add(validacion);

            return retorno;
        }

        /*
         * Metodo check para copia de correo con cotizacion para el director
         */

        public List<ValidacionCopiaDirector> ObtenerInfoCopiaDirector()
        {
            var retorno = new List<ValidacionCopiaDirector>();

            var copiaDirector = new ValidacionCopiaDirector()
            {
                COD_Validacion = 1,
                copiaDirector = "Copia de correo para director."
            };

            retorno.Add(copiaDirector);

            return retorno;
        }

        /*
         * Metodo para traer la lista de niveles de interes de base de datos
         */

        public List<NivelesInteres> ObtenerNivelesInteres()
        {
            var retorno = new List<NivelesInteres>();
            using (var bd = new ContextoPortal())
            {
                //var vGet = bd.NivelesInteres;
                //if (vGet != null)
                //    retorno = vGet.ToList();
            }

            return retorno;
        }

        public List<EstadosCotizacions> ObtenerEstados()
        {
            var retorno = new List<EstadosCotizacions>();
            using (var bd = new ContextoPortal())
            {
                var vGet = bd.EstadosCotizacion;
                if (vGet != null)
                    retorno = vGet.ToList();
            }
            return retorno;
        }

        public List<EstadosCotizacion> ObtenerEstadosCotizacion()
        {
            var retorno = new List<EstadosCotizacion>();
            using (var bd = new ContextoPortal())
            {
                var vGet = bd.EstadosCotizacionSCI;
                if (vGet != null)
                    retorno = vGet.ToList();
            }
            return retorno;
        }

        public int ObtenerIDAsesor(int cotID)
        {
            int retornoID = -1;

            using (var bd = new ContextoPortal())
            {
                //var lstCotizAseso = (from u in bd.Cotizacion_Asesor select u).ToList();

                retornoID = (from a in bd.Cotizacion_Asesor
                             where a.id_cotizacion == cotID
                             select a.id_asesor).First();
            }
            return retornoID;
        }

        public string ObtenerNombreAsesor(int asesorID)
        {
            string nombreAseso = null;

            using (var bd = new ContextoUsuarios())
            {
                //var lstAsesos = (from u in bd.usrCentral select u).ToList();

                nombreAseso = (from a in bd.usrCentral
                               where a.id_usr == asesorID
                               select a.nom_usr).First();
            }
            return nombreAseso;
        }

        public List<ESTADOS_COTI_PRIC> ObtenerEstadosPricing()
        {
            var retorno = new List<ESTADOS_COTI_PRIC>();
            using (var bd = new ContextoPortal())
            {
                var vGet = bd.EstadosPricing;
                if (vGet != null)
                    retorno = vGet.ToList();
            }
            return retorno;
        }

        public List<SECTOR> ObtenerSectores()
        {
            var retorno = new List<SECTOR>();
            using (var bd = new ContextoPortal())
            {
                var vGet = bd.SECTORES;
                retorno = vGet.Where(s => s.Estado == true).ToList();
            }
            return retorno;
        }

        /// <summary>
        /// Obtener Listado de Factores para Calcular la tarifa de la cotización.
        /// </summary>
        /// <returns></returns>
        public List<Factores> ObtenerFactores(int? tipoFactor = 0)
        {
            var retorno = new List<Factores>();
            using (var bd = new ContextoPortal())
            {
                var vGet = bd.Factores;
                if (vGet != null)
                {
                    switch (tipoFactor)
                    {
                        //Factores Fijo
                        case 1:
                            retorno = vGet.Where(t => t.Estado == true && t.Id_TipoFactor == 1).ToList();
                            break;

                        //Factores Dinámico
                        case 2:
                            retorno = vGet.Where(t => t.Estado == true && t.Id_TipoFactor == 2).ToList();
                            break;

                        default:
                            retorno = vGet.Where(t => t.Estado == true).ToList();
                            break;
                    }
                }
            }
            return retorno;
        }

        public List<Tipos_AP> ObtenerTipoAreasProtegidas()
        {
            var retorno = new List<Tipos_AP>();
            using (var bd = new ContextoPortal())
            {
                var vGet = bd.Tipos_AP;
                if (vGet != null)
                    retorno = vGet.Where(ta => ta.Estado == true).ToList();
            }
            return retorno;
        }

        public List<CiudadesFactor> ObtenerCiudadesFactor()
        {
            var retorno = new List<CiudadesFactor>();
            using (var bd = new ContextoPortal())
            {
                var vGet = bd.CiudadesFactor;
                if (vGet != null)
                    retorno = vGet.Where(c => c.Estado == true).ToList();
            }
            return retorno;
        }

        public List<TipoRiesgo> ObtenerTipoRiesgo()
        {
            var retorno = new List<TipoRiesgo>();
            using (var bd = new ContextoPortal())
            {
                var vGet = bd.TipoRiesgo;
                if (vGet != null)
                    retorno = vGet.Where(tr => tr.Estado == true).ToList();
            }
            return retorno;
        }

        public bool validarPermisoContrato(int NIT, int userID, int productoId, string ciudad)
        {
            try
            {
                bool contrat = false;
                List<string> produCoti = new List<string>();
                var lstusua = (new RepositorioUsuarios()).ObtenerUsuarioId(userID);
                string ciudadNITEmermedica = null;

                using (var modelo = new ContextoStone())
                {
                    using (var usua = new ContextoUsuarios())
                    {
                        using (var cot = new ContextoPortal())
                        {
                            //var lstCont = (from u in modelo.CC_CONTR select u).ToList();
                            //var lstusuar = (from u in usua.usrCentral select u).ToList();
                            //var lstAseso = (from u in modelo.CC_ASESO select u).ToList();
                            //var lstProd = (from u in cot.Productos select u).ToList();

                            var contr = (from a in modelo.CC_CONTR
                                         join b in modelo.CC_ASESO on a.COD_ASES equals b.COD_ASES
                                         join c in modelo.CC_TIPCO on a.COD_TIPC equals c.COD_TIPC
                                         join d in modelo.GN_TERCE on a.COD_ASES.ToString() equals d.NUM_IDEN
                                         //join e in lstusua on d.NUM_IDEN equals e.num_doc
                                         where a.EST_CONT == "A" && a.COD_TERC == NIT
                                         select a).FirstOrDefault();

                            if (contr != null)
                            {
                                if (contr.COD_ASES == 800126785)
                                {
                                    ciudadNITEmermedica = "BOGOTA";
                                }
                                //director del usuario dueño del contrato
                                var dire = buscarDirector(contr.COD_ASES);
                                //asesor o director dueño del contrato
                                var asesoA = (from a in modelo.CC_CONTR
                                              join b in modelo.CC_ASESO on a.COD_ASES equals b.COD_ASES
                                              where (b.COD_ASES == contr.COD_ASES || contr.COD_ASES == b.COD_DIRE) && (b.EST_ASES == "A" || b.EST_ASES == "I")
                                              select b).First();
                                //id en usrCentral del asesor o director dueño del contrato
                                var idAseso = (from a in usua.usrCentral
                                               where asesoA.COD_ASES.ToString() == a.num_doc || asesoA.COD_DIRE.ToString() == a.num_doc
                                               select a.id_usr).FirstOrDefault();

                                //ciudad del asesor o director dueño del contrato
                                var ciudAseso = ObtenerCiudadAsesorXDocumento(asesoA.COD_ASES);
                                //si el asesor no esta en tu portal se debe buscar el director del asesor dueñor del contrato
                                if (ciudAseso == null)
                                {
                                    //var idDireContr = (from a in usua.usrCentral
                                    //                   where dire.ToString() == a.num_doc
                                    //                   select a.id_usr).FirstOrDefault();
                                    var ciudDirect = ObtenerCiudadAsesorXDocumento(dire);

                                    if (ciudDirect != null)
                                    {
                                        ciudAseso = ciudDirect;
                                    }
                                    else
                                    {
                                        var gerent = buscarGerente(dire);

                                        //var idGerentContr = (from a in usua.usrCentral
                                        //                     where gerent.ToString() == a.num_doc
                                        //                     select a.id_usr).FirstOrDefault();
                                        var ciudGerent = ObtenerCiudadAsesorXDocumento(gerent);
                                        if (ciudGerent != null)
                                        {
                                            ciudAseso = ciudGerent;
                                        }
                                    }
                                }
                                //producto que aparece en el contrato
                                var prod = (from a in modelo.CC_CONTR
                                            join c in modelo.CC_TIPCO on a.COD_TIPC equals c.COD_TIPC
                                            join d in modelo.GN_TERCE on a.COD_ASES.ToString() equals d.NUM_IDEN
                                            //join e in lstusua on d.NUM_IDEN equals e.num_doc
                                            where a.RMT_CONT == contr.RMT_CONT
                                            select c.NOM_TIPC).FirstOrDefault();
                                produCoti.Add(prod);

                                if (asesoA.EST_ASES == "I")
                                {
                                    //director del usuario que esta tratando de bloquear el nit
                                    var aseDire = buscarDirector(Convert.ToDecimal(lstusua.num_doc));

                                    if (aseDire != dire)
                                    {
                                        //var nomProdSelect = (from a in cot.Productos
                                        //                     join b in productosIds on a.COD_SERV equals b
                                        //                     select a.NombreProducto).First();

                                        var nomProdSelect = (from a in cot.Productos where a.COD_SERV == productoId select a).FirstOrDefault();

                                        if (nomProdSelect.NombreProducto == "PPE")
                                        {
                                            foreach (var i in produCoti)
                                            {
                                                if (i.Contains("PREFERENCIAL") && (ciudAseso == ciudad || ciudadNITEmermedica == ciudad))
                                                {
                                                    contrat = true;
                                                    return contrat;
                                                }
                                            }
                                        }
                                        else if (nomProdSelect.NombreProducto == "Área Protegida")
                                        {
                                            foreach (var i in produCoti)
                                            {
                                                if (i.Contains("AREA") && (ciudAseso == ciudad || ciudadNITEmermedica == ciudad))
                                                {
                                                    contrat = true;
                                                    return contrat;
                                                }
                                            }
                                        }
                                        else if (nomProdSelect.NombreProducto == "Auto Protegido")
                                        {
                                            foreach (var i in produCoti)
                                            {
                                                if (i.Contains("AUTO") && (ciudAseso == ciudad || ciudadNITEmermedica == ciudad))
                                                {
                                                    contrat = true;
                                                    return contrat;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return contrat = false;
                                    }
                                }
                                if (asesoA.EST_ASES == "A")
                                {
                                    //var nomProdSelect = (from a in cot.Productos
                                    //                     join b in productosIds on a.COD_SERV equals b
                                    //                     select a.NombreProducto).First();

                                    var nomProdSelect = (from a in cot.Productos where a.COD_SERV == productoId select a).FirstOrDefault();

                                    if (nomProdSelect.NombreProducto == "PPE")
                                    {
                                        foreach (var i in produCoti)
                                        {
                                            if (i.Contains("PREFERENCIAL") && (ciudAseso == ciudad || ciudadNITEmermedica == ciudad))
                                            {
                                                contrat = true;
                                                return contrat;
                                            }
                                        }
                                    }
                                    else if (nomProdSelect.NombreProducto == "Área Protegida")
                                    {
                                        foreach (var i in produCoti)
                                        {
                                            if (i.Contains("AREA") && (ciudAseso == ciudad || ciudadNITEmermedica == ciudad))
                                            {
                                                contrat = true;
                                                return contrat;
                                            }
                                        }
                                    }
                                    else if (nomProdSelect.NombreProducto == "Auto Protegido")
                                    {
                                        foreach (var i in produCoti)
                                        {
                                            if (i.Contains("AUTO") && (ciudAseso == ciudad || ciudadNITEmermedica == ciudad))
                                            {
                                                contrat = true;
                                                return contrat;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                return contrat = false;
                            }
                        }
                    }
                }
                return contrat = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public EMSP_INFOCONTRATOS validarPermiContrato(decimal auxNIT, int userId)
        {
            try
            {
                EMSP_INFOCONTRATOS vRet = new EMSP_INFOCONTRATOS();

                List<string> produCoti = new List<string>();
                var lstusua = (new RepositorioUsuarios()).ObtenerUsuarioId(userId);
                string ciudadNITEmermedica = null;

                using (var modelo = new ContextoStone())
                {
                    using (var usua = new ContextoUsuarios())
                    {

                        var contr = (from a in modelo.CC_CONTR
                                     join b in modelo.CC_ASESO on a.COD_ASES equals b.COD_ASES
                                     join c in modelo.CC_TIPCO on a.COD_TIPC equals c.COD_TIPC
                                     join d in modelo.GN_TERCE on a.COD_ASES.ToString() equals d.NUM_IDEN
                                     //join e in lstusua on d.NUM_IDEN equals e.num_doc
                                     where a.EST_CONT == "A" && a.COD_TERC == auxNIT
                                     select a).FirstOrDefault();
                        if (contr != null)
                        {
                            if (contr.COD_ASES == 800126785)
                            {
                                ciudadNITEmermedica = "BOGOTA";
                            }
                            var dire = buscarDirector(contr.COD_ASES);
                            var asesoA = (from a in modelo.CC_CONTR
                                          join b in modelo.CC_ASESO on a.COD_ASES equals b.COD_ASES

                                          where (b.COD_ASES == contr.COD_ASES || contr.COD_ASES == b.COD_DIRE) && (b.EST_ASES == "A" || b.EST_ASES == "I")
                                          select b).First();

                            var idAseso = (from a in usua.usrCentral
                                           where asesoA.COD_ASES.ToString() == a.num_doc || asesoA.COD_DIRE.ToString() == a.num_doc
                                           select a.id_usr).FirstOrDefault();

                            var ciudAseso = ObtenerCiudadAsesor(idAseso);
                            if (ciudAseso == null && ciudadNITEmermedica != null)
                            {
                                ciudAseso = ciudadNITEmermedica;
                            }
                            var canalAseso = ObtenerCanalAsesor(idAseso);
                            if (canalAseso == null && contr.COD_ASES == 800126785)
                            {
                                canalAseso = "EMERMEDICA";
                            }
                            var prod = (from a in modelo.CC_CONTR
                                        join c in modelo.CC_TIPCO on a.COD_TIPC equals c.COD_TIPC
                                        join d in modelo.GN_TERCE on a.COD_ASES.ToString() equals d.NUM_IDEN
                                        //join e in lstusua on d.NUM_IDEN equals e.num_doc
                                        where a.RMT_CONT == contr.RMT_CONT
                                        select c.NOM_TIPC).FirstOrDefault();
                            produCoti.Add(prod);

                            if (asesoA.EST_ASES == "I")
                            {
                                var aseDire = buscarDirector(Convert.ToDecimal(lstusua.num_doc));
                                if (aseDire != dire)
                                {
                                    var nombreAseso = (from d in modelo.GN_TERCE
                                                       where contr.COD_ASES.ToString().Equals(d.NUM_IDEN)
                                                       select d.NOM_COMP).FirstOrDefault();
                                    var nombreDire = (from u in modelo.GN_TERCE
                                                      where u.NUM_IDEN.Equals(dire.ToString())
                                                      select u.NOM_COMP).FirstOrDefault();
                                    return new EMSP_INFOCONTRATOS()
                                    {
                                        idContrato = contr.RMT_CONT.ToString(),
                                        fechaExpiracion = contr.FEC_VENC,
                                        nombreAsesor = nombreAseso,
                                        nombreDirector = nombreDire,
                                        productos = produCoti,
                                        ciudadAsesor = ciudAseso,
                                        canalAsesor = canalAseso
                                    };
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            if (asesoA.EST_ASES == "A")
                            {
                                var nombreAseso = (from d in modelo.GN_TERCE
                                                   where contr.COD_ASES.ToString().Equals(d.NUM_IDEN)
                                                   select d.NOM_COMP).FirstOrDefault();
                                var nombreDire = (from u in modelo.GN_TERCE
                                                  where u.NUM_IDEN.Equals(dire.ToString())
                                                  select u.NOM_COMP).FirstOrDefault();

                                return new EMSP_INFOCONTRATOS()
                                {
                                    idContrato = contr.RMT_CONT.ToString(),
                                    fechaExpiracion = contr.FEC_VENC,
                                    nombreAsesor = nombreAseso,
                                    nombreDirector = nombreDire,
                                    productos = produCoti,
                                    ciudadAsesor = ciudAseso,
                                    canalAsesor = canalAseso
                                };
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public decimal buscarDirector(decimal asesorID)
        {
            using (var modelo = new ContextoStone())
            {
                //var lstAseso = (from u in modelo.CC_ASESO select u).ToList();
                var dire = (from u in modelo.CC_ASESO
                            where u.COD_ASES == asesorID
                            select u.COD_DIRE).FirstOrDefault();
                return dire;
            }
        }

        public decimal ObtenerNoDocumentoDirector(int asesorID)
        {
            using (var modeloUsuarios = new ContextoUsuarios())
            {
                var userDocumento = (from u in modeloUsuarios.usrCentral where u.id_usr == asesorID select u.num_doc).FirstOrDefault();
                var noDocumentoDirector = Convert.ToDecimal(userDocumento);

                using (var modeloStone = new ContextoStone())
                {
                    var retorno = (from u in modeloStone.CC_ASESO
                                   where u.COD_ASES == noDocumentoDirector
                                   select u.COD_DIRE).FirstOrDefault();
                    return retorno;
                }
            }
        }

        private decimal buscarGerente(decimal documentoDirect)
        {
            using (var modelo = new ContextoStone())
            {
                //var lstDirec = (from u in modelo.CC_DIREC select u).ToList();
                var gerent = (from u in modelo.CC_DIREC
                              where u.COD_DIRE == documentoDirect
                              select u.COD_GERE).FirstOrDefault();
                return gerent;
            }
        }

        public IEnumerable<EMA_Cotizacion> ObtenerMisCotizaciones(int idUser, string estadoCotizacion)
        {
            List<EMA_Cotizacion> vLista = new List<EMA_Cotizacion>();

            if (string.IsNullOrEmpty(estadoCotizacion)) estadoCotizacion = "Vigente";

            using (var usua = new ContextoUsuarios())
            {
                using (var modelo = new ContextoPortal())
                {
                    var Cot = modelo.Database.SqlQuery<EMA_Cotizacion>("EMSP_MISRESERVAS @IDUSUARIO, @ESTADOCOTIZACION",
                            new SqlParameter("@IDUSUARIO", idUser),
                            new SqlParameter("@ESTADOCOTIZACION", estadoCotizacion)
                        ).ToList();

                    vLista = Cot;
                }
            }

            if (vLista == null)
                vLista = new List<EMA_Cotizacion>();

            return vLista;
        }

        public IEnumerable<EMA_Cotizacion> ObtenerMisReservasACotizar(int idUser)
        {
            List<EMA_Cotizacion> vLista = new List<EMA_Cotizacion>();

            using (var usua = new ContextoUsuarios())
            {
                using (var modelo = new ContextoPortal())
                {
                    List<string> nombreProducto = new List<string>();
                    var v1 = modelo.Database.SqlQuery<EMA_Cotizacion>(
                            "EMSP_RESERVASACOTIZAR @USERID",
                            new SqlParameter("@USERID", idUser)).ToList();

                    if (v1 != null && v1.ToList().Count > 0)
                        vLista = v1.ToList();
                }
            }

            if (vLista == null)
                vLista = new List<EMA_Cotizacion>();

            return vLista;
        }

        /// <summary>
        /// Metodo para obtener la jerarquía, según rol.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Usp_ObtenerCotizaciones> ObtenerJerarquiaCotizaciones(string noDocumento, string estadoCotizacion, string producto, string estadoReserva, string fechaInicio, string fechaFin, string rol)
        {
            producto = string.IsNullOrEmpty(producto) ? string.Empty : producto;
            estadoReserva = string.IsNullOrEmpty(estadoReserva) ? string.Empty : estadoReserva;

            List<Usp_ObtenerCotizaciones> vLista = new List<Usp_ObtenerCotizaciones>();
            //if (string.IsNullOrEmpty(estadoCotizacion)) estadoCotizacion = "Vigente";

            using (var modelo = new ContextoPortal())
            {
                var lstCotizaciones = modelo.Database.SqlQuery<Usp_ObtenerCotizaciones>("Usp_ObtenerCotizaciones @NoDocumento, @EstadoCotizacion, @Producto, @EstadoReserva, @FechaInicio, @FechaFin, @Rol",
                        new SqlParameter("@NoDocumento", noDocumento),
                        new SqlParameter("@EstadoCotizacion", estadoCotizacion),
                        new SqlParameter("@Producto", producto),
                        new SqlParameter("@EstadoReserva", estadoReserva),
                        new SqlParameter("@FechaInicio", fechaInicio),
                        new SqlParameter("@FechaFin", fechaFin),
                        new SqlParameter("@Rol", rol)
                    ).ToList();

                vLista = lstCotizaciones;
            }

            if (vLista == null)
                vLista = new List<Usp_ObtenerCotizaciones>();

            return vLista;
        }

        /// <summary>
        /// Metodo para obtener las reservas activas en tu portal que se le van a mostrar a pricing
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EMA_Cotizacion> ObtenerReservasActivas()
        {
            List<EMA_Cotizacion> vLista = new List<EMA_Cotizacion>();

            using (var usua = new ContextoUsuarios())
            {
                using (var reserv = new ContextoPortal())
                {
                    var v1 = reserv.Database.SqlQuery<EMA_Cotizacion>("EMSP_RESERVASACOTIZARPRICING").ToList();

                    if (v1 != null && v1.ToList().Count > 0)
                        vLista = v1.ToList();
                }
            }
            if (vLista == null)
                vLista = new List<EMA_Cotizacion>();

            return vLista;
        }

        /// <summary>
        /// Metodo para obtener los asesores de un director
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<EMA_CotizacionXAsesor> ObtenerMisAsesores(int userId)
        {
            List<EMA_CotizacionXAsesor> misAsesores = new List<EMA_CotizacionXAsesor>();
            using (var stone = new ContextoStone())
            {
                using (var usua = new ContextoUsuarios())
                {
                    using (var modelo = new ContextoPortal())
                    {
                        var lstCotiAseso = (from u in modelo.Cotizacion_Asesor select u).ToList();

                        var docuDire = (from u in usua.usrCentral
                                        where userId == u.id_usr
                                        select u.num_doc).FirstOrDefault();

                        var lstAsesoDirect = modelo.Database.SqlQuery<EMA_CotizacionXAsesor>("EMSP_COTIZACIONESXDIRECTOR @DIRECTORDOCU",
                            new SqlParameter("@DIRECTORDOCU", docuDire)).ToList();

                        if (lstAsesoDirect != null && lstAsesoDirect.Count() > 0)
                        {
                            foreach (var co in lstAsesoDirect)
                            {
                                var v1 = new EMA_CotizacionXAsesor()
                                {
                                    id_asesor = co.id_asesor,
                                    estadoAsesor = co.estadoAsesor,
                                    nombreAsesor = co.nombreAsesor,
                                    numeroDocumento = co.numeroDocumento,
                                    numeroVisitas = co.numeroVisitas
                                };
                                misAsesores.Add(v1);
                            }
                        }
                    }
                }
            }
            if (misAsesores == null)
                misAsesores = new List<EMA_CotizacionXAsesor>();

            return misAsesores;
        }

        /// <summary>
        /// Metodo para obtener las reservas de un asesor filtradas
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="descripcion"></param>
        /// <param name="estadoCotID"></param>
        /// <param name="cotiNIT"></param>
        /// <param name="nombreCliente"></param>
        /// <param name="aseID"></param>
        /// <returns></returns>
        public IEnumerable<EMA_Cotizacion> ObtenerCotizacionFiltros(int idProducto, int idEstadoReserva, int idEstadoCotizacion, int aseID, string nitNombre, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var listadoCotizaciones = new List<EMA_Cotizacion>();
            String nombreEstadoReserva = "";
            String nombreEstadoCotizacion = "";
            String nombreProducto = "";

            using (var usua = new ContextoUsuarios())
            {
                using (var modelo = new ContextoPortal())
                {
                    var lstEstadoReserva = (from u in modelo.EstadosCotizacion select u).ToList();
                    var lstEstadoCotizacion = (from u in modelo.EstadosCotizacionSCI select u).ToList();

                    //Nombre estado reserva
                    foreach (var est in lstEstadoReserva)
                    {
                        if (est.Id_Estado == idEstadoReserva)
                        {
                            nombreEstadoReserva = est.nombreEstado;
                        }
                    }

                    //Nombre estado cotización
                    foreach (var est in lstEstadoCotizacion)
                    {
                        if (est.Id == idEstadoCotizacion)
                        {
                            nombreEstadoCotizacion = est.Nombre;
                        }
                    }

                    var lstCotiAseso = (from u in modelo.Cotizacion_Asesor join c in modelo.Cotizaciones on u.id_cotizacion equals c.id_cotizacion where u.id_asesor == aseID && c.estado == nombreEstadoReserva select u).ToList();
                    var lstProds = (from u in modelo.Productos select u).ToList();

                    //Nombre producto
                    foreach (var est in lstProds)
                    {
                        if (est.COD_SERV == idProducto)
                        {
                            nombreProducto = est.NombreProducto;
                        }
                    }

                    var v1 = (from a in lstCotiAseso
                              join b in usua.usrCentral on a.id_asesor equals b.id_usr
                              join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                              where (
                                           //Combinaciones nitNombre
                                           (nitNombre != null && nombreEstadoCotizacion == "" && (fechaInicio == null && fechaFin == null)) ?
                                                c.nombreEmpresa.ToLower().Contains(nitNombre.ToLower().Trim()) :

                                            (nitNombre != null && nombreEstadoCotizacion != "" && (fechaInicio == null && fechaFin == null)) ?
                                                c.nombreEmpresa.ToLower().Contains(nitNombre.ToLower().Trim()) &&
                                                c.EstadoCotizacion == nombreEstadoCotizacion :

                                            (nitNombre != null && (fechaInicio != null && fechaFin != null) && nombreEstadoCotizacion == "") ?
                                                c.nombreEmpresa.ToLower().Contains(nitNombre.ToLower().Trim()) &&
                                                (c.fechaExpiracion >= fechaInicio && c.fechaExpiracion <= fechaFin) :

                                           //Combinaciones nombreEstadoCotizacion
                                           (nombreEstadoCotizacion != "" && nitNombre == null && (fechaInicio == null && fechaFin == null)) ?
                                                c.EstadoCotizacion == nombreEstadoCotizacion :

                                           (nombreEstadoCotizacion != "" && nitNombre != null && (fechaInicio == null && fechaFin == null)) ?
                                                c.EstadoCotizacion == nombreEstadoCotizacion &&
                                                c.nombreEmpresa.ToLower().Contains(nitNombre.ToLower().Trim()) :

                                           (nombreEstadoCotizacion != "" && (fechaInicio != null && fechaFin != null)) && nitNombre == null ?
                                                c.EstadoCotizacion == nombreEstadoCotizacion &&
                                                (c.fechaExpiracion >= fechaInicio && c.fechaExpiracion <= fechaFin) :

                                           //Todas
                                           (nombreEstadoCotizacion != "" && (fechaInicio != null && fechaFin != null)) && nitNombre != null ?
                                                c.EstadoCotizacion == nombreEstadoCotizacion &&
                                                (c.fechaExpiracion >= fechaInicio && c.fechaExpiracion <= fechaFin) &&
                                                c.nombreEmpresa.ToLower().Contains(nitNombre.ToLower().Trim())

                                               : a.id_asesor == aseID && c.estado == nombreEstadoReserva
                                         )
                              select new EMA_Cotizacion()
                              {
                                  id_cotizacion = a.id_cotizacion,
                                  nombreAsesor = b.nom_usr,
                                  estado = c.estado,
                                  fechaVencimiento = c.fechaExpiracion,
                                  NIT = c.NIT.ToString(),
                                  nombreEmpresa = c.nombreEmpresa
                              });

                    if (v1 != null && v1.ToList().Count > 0)
                        listadoCotizaciones = v1.ToList();

                    //Asignar Producto
                    foreach (EMA_Cotizacion item in listadoCotizaciones)
                    {
                        var vfiltro = (from a in modelo.Cotizacion_Producto
                                       join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                       where a.id_cotizacion == item.id_cotizacion
                                       select b.Descripcion
                                       ).FirstOrDefault();

                        //var estCoti = (from a in modelo.Cotizaciones
                        //               join b in modelo.EstadosPricing on a.EstadoPricing equals b.Descripcion
                        //               where item.id_cotizacion == a.id_cotizacion
                        //               select b.Descripcion).FirstOrDefault();

                        var estCoti = (from a in modelo.Cotizaciones
                                       join b in modelo.EstadosCotizacionSCI on a.EstadoCotizacion equals b.Nombre
                                       where item.id_cotizacion == a.id_cotizacion
                                       select b.Nombre).FirstOrDefault();

                        if (estCoti != null)
                            item.estadoCotizacion = estCoti;

                        if (vfiltro != null)
                            item.productos = vfiltro;
                    }
                }
            }

            if (listadoCotizaciones == null)
                listadoCotizaciones = new List<EMA_Cotizacion>();

            return listadoCotizaciones;
        }

        /// <summary>
        /// Metodo de filtrado de cotizaciones para el pricing
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="descripcion"></param>
        /// <param name="estadoPricID"></param>
        /// <param name="cotiNIT"></param>
        /// <param name="nombreCliente"></param>
        /// <param name="pricID"></param>
        /// <returns></returns>
        public IEnumerable<EMA_Cotizacion> ObtenerCotizacionPricingFiltros(int menuId, string descripcion, int? estadoPricID, string cotiNIT, string nombreCliente, int pricID)
        {
            var listadoCotizaciones = new List<EMA_Cotizacion>();
            string nombEstado = null;
            using (var usua = new ContextoUsuarios())
            {
                using (var modelo = new ContextoPortal())
                {
                    //REVISAR ESTADO DE COTIZACION PRICING
                    //var lstCotiz = (from u in modelo.Cotizaciones select u).ToList();
                    var lstEstados = (from u in modelo.EstadosPricing select u).ToList();
                    //var lstCotiProd = (from u in modelo.Cotizacion_Producto select u).ToList();
                    var lstCotiAseso = (from u in modelo.Cotizacion_Asesor select u).ToList();
                    //var lstProds = (from u in modelo.Productos select u).ToList();
                    //var lstUsua = (from u in usua.usrCentral select u).ToList();

                    foreach (var est in lstEstados)
                    {
                        if (est.Id_Estado == estadoPricID)
                        {
                            nombEstado = est.Descripcion;
                        }
                    }

                    if (nombEstado != null || cotiNIT != null || nombreCliente != null)
                    {
                        if (cotiNIT != null && nombEstado == null && nombreCliente == null)
                        {
                            var v1 = (from a in lstCotiAseso
                                      join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                      join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                      where c.NIT == Convert.ToInt32(cotiNIT) && c.estado == "Vigente" && c.VeracidadInformacion == true
                                      select new EMA_Cotizacion()
                                      {
                                          id_cotizacion = a.id_cotizacion,
                                          nombreAsesor = b.nom_usr,
                                          //estado = c.estado,
                                          fechaVencimiento = c.fechaExpiracion,
                                          NIT = c.NIT.ToString(),
                                          nombreEmpresa = c.nombreEmpresa,
                                          FechaVisita = c.fechaVisita
                                      });

                            if (v1 != null && v1.ToList().Count > 0)
                                listadoCotizaciones = v1.ToList();

                            foreach (EMA_Cotizacion item in listadoCotizaciones)
                            {
                                //var vfiltro = (from a in modelo.Cotizacion_Producto
                                //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                //               where a.id_cotizacion == item.id_cotizacion
                                //               select b
                                //               ).ToList();
                                var vfiltro = (from a in modelo.Cotizacion_Producto
                                               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                               where a.id_cotizacion == item.id_cotizacion
                                               select b.Descripcion
                                              ).FirstOrDefault();
                                var estCoti = (from a in modelo.Cotizaciones
                                               join b in modelo.EstadosPricing on a.EstadoPricing equals b.Descripcion
                                               where item.id_cotizacion == a.id_cotizacion
                                               select b.Descripcion).FirstOrDefault();

                                if (estCoti != null)
                                    item.estadoCotizacion = estCoti;

                                if (vfiltro != null)
                                    item.productos = vfiltro;
                            }
                        }
                        if (nombEstado != null && cotiNIT != null && nombreCliente != null)
                        {
                            var v1 = (from a in lstCotiAseso
                                      join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                      join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                      where c.nombreEmpresa.Contains(nombreCliente) && c.EstadoPricing == nombEstado && c.NIT == Convert.ToInt32(cotiNIT) && c.estado == "Vigente" && c.VeracidadInformacion == true
                                      select new EMA_Cotizacion()
                                      {
                                          id_cotizacion = a.id_cotizacion,
                                          nombreAsesor = b.nom_usr,
                                          //estado = c.estado,
                                          fechaVencimiento = c.fechaExpiracion,
                                          NIT = c.NIT.ToString(),
                                          nombreEmpresa = c.nombreEmpresa,
                                          estadoCotizacion = c.EstadoPricing,
                                          FechaVisita = c.fechaVisita
                                      });

                            if (v1 != null && v1.ToList().Count > 0)
                                listadoCotizaciones = v1.ToList();

                            foreach (EMA_Cotizacion item in listadoCotizaciones)
                            {
                                //var vfiltro = (from a in modelo.Cotizacion_Producto
                                //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                //               where a.id_cotizacion == item.id_cotizacion
                                //               select b
                                //               ).ToList();
                                var vfiltro = (from a in modelo.Cotizacion_Producto
                                               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                               where a.id_cotizacion == item.id_cotizacion
                                               select b.Descripcion
                                               ).FirstOrDefault();

                                if (vfiltro != null)
                                    item.productos = vfiltro;
                            }
                        }
                        if (nombEstado == null && cotiNIT != null && nombreCliente != null)
                        {
                            var v1 = (from a in lstCotiAseso
                                      join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                      join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                      where c.nombreEmpresa.Contains(nombreCliente) && c.estado == "Vigente" && c.VeracidadInformacion == true
                                      select new EMA_Cotizacion()
                                      {
                                          id_cotizacion = a.id_cotizacion,
                                          nombreAsesor = b.nom_usr,
                                          //estado = c.estado,
                                          fechaVencimiento = c.fechaExpiracion,
                                          NIT = c.NIT.ToString(),
                                          nombreEmpresa = c.nombreEmpresa,
                                          FechaVisita = c.fechaVisita
                                      });

                            if (v1 != null && v1.ToList().Count > 0)
                                listadoCotizaciones = v1.ToList();

                            foreach (EMA_Cotizacion item in listadoCotizaciones)
                            {
                                //var vfiltro = (from a in modelo.Cotizacion_Producto
                                //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                //               where a.id_cotizacion == item.id_cotizacion
                                //               select b
                                //               ).ToList();
                                var vfiltro = (from a in modelo.Cotizacion_Producto
                                               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                               where a.id_cotizacion == item.id_cotizacion
                                               select b.Descripcion
                                               ).FirstOrDefault();
                                if (vfiltro != null)
                                    item.productos = vfiltro;

                                var estCoti = (from a in modelo.Cotizaciones
                                               join b in lstEstados on a.EstadoPricing equals b.Descripcion
                                               where item.id_cotizacion == a.id_cotizacion
                                               select b.Descripcion).FirstOrDefault();

                                if (estCoti != null)
                                    item.estadoCotizacion = estCoti;
                            }
                        }
                        if (nombEstado != null && cotiNIT != null && nombreCliente == null)
                        {
                            var v1 = (from a in lstCotiAseso
                                      join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                      join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                      where c.EstadoPricing == nombEstado && c.NIT == Convert.ToInt32(cotiNIT) && c.estado == "Vigente" && c.VeracidadInformacion == true
                                      select new EMA_Cotizacion()
                                      {
                                          id_cotizacion = a.id_cotizacion,
                                          nombreAsesor = b.nom_usr,
                                          //estado = c.estado,
                                          fechaVencimiento = c.fechaExpiracion,
                                          NIT = c.NIT.ToString(),
                                          nombreEmpresa = c.nombreEmpresa,
                                          estadoCotizacion = c.EstadoPricing,
                                          FechaVisita = c.fechaVisita
                                      });

                            if (v1 != null && v1.ToList().Count > 0)
                                listadoCotizaciones = v1.ToList();

                            foreach (EMA_Cotizacion item in listadoCotizaciones)
                            {
                                //var vfiltro = (from a in modelo.Cotizacion_Producto
                                //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                //               where a.id_cotizacion == item.id_cotizacion
                                //               select b
                                //               ).ToList();
                                var vfiltro = (from a in modelo.Cotizacion_Producto
                                               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                               where a.id_cotizacion == item.id_cotizacion
                                               select b.Descripcion
                                               ).FirstOrDefault();
                                if (vfiltro != null)
                                    item.productos = vfiltro;
                            }
                        }
                        if (cotiNIT == null)
                        {
                            if (nombreCliente == null)
                            {
                                var v1 = (from a in lstCotiAseso
                                          join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                          join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                          where c.EstadoPricing == nombEstado && c.estado == "Vigente" && c.VeracidadInformacion == true
                                          select new EMA_Cotizacion()
                                          {
                                              id_cotizacion = a.id_cotizacion,
                                              nombreAsesor = b.nom_usr,
                                              estadoCotizacion = c.EstadoPricing,
                                              fechaVencimiento = c.fechaExpiracion,
                                              NIT = c.NIT.ToString(),
                                              nombreEmpresa = c.nombreEmpresa,
                                              FechaVisita = c.fechaVisita
                                          });

                                if (v1 != null && v1.ToList().Count > 0)
                                    listadoCotizaciones = v1.ToList();

                                foreach (EMA_Cotizacion item in listadoCotizaciones)
                                {
                                    //var vfiltro = (from a in modelo.Cotizacion_Producto
                                    //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                    //               where a.id_cotizacion == item.id_cotizacion
                                    //               select b
                                    //               ).ToList();
                                    var vfiltro = (from a in modelo.Cotizacion_Producto
                                                   join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                                   where a.id_cotizacion == item.id_cotizacion
                                                   select b.Descripcion
                                                   ).FirstOrDefault();
                                    if (vfiltro != null)
                                        item.productos = vfiltro;
                                }
                            }
                            if (nombreCliente != null)
                            {
                                if (nombEstado != null)
                                {
                                    var v1 = (from a in lstCotiAseso
                                              join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                              join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                              where c.nombreEmpresa.Contains(nombreCliente) && c.EstadoPricing == nombEstado && c.estado == "Vigente" && c.VeracidadInformacion == true
                                              select new EMA_Cotizacion()
                                              {
                                                  id_cotizacion = a.id_cotizacion,
                                                  nombreAsesor = b.nom_usr,
                                                  estadoCotizacion = c.EstadoPricing,
                                                  fechaVencimiento = c.fechaExpiracion,
                                                  NIT = c.NIT.ToString(),
                                                  nombreEmpresa = c.nombreEmpresa,
                                                  FechaVisita = c.fechaVisita
                                              });

                                    if (v1 != null && v1.ToList().Count > 0)
                                        listadoCotizaciones = v1.ToList();

                                    foreach (EMA_Cotizacion item in listadoCotizaciones)
                                    {
                                        //var vfiltro = (from a in modelo.Cotizacion_Producto
                                        //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                        //               where a.id_cotizacion == item.id_cotizacion
                                        //               select b
                                        //               ).ToList();
                                        var vfiltro = (from a in modelo.Cotizacion_Producto
                                                       join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                                       where a.id_cotizacion == item.id_cotizacion
                                                       select b.Descripcion
                                                       ).FirstOrDefault();
                                        if (vfiltro != null)
                                            item.productos = vfiltro;
                                    }
                                }
                                if (nombEstado == null)
                                {
                                    var v1 = (from a in lstCotiAseso
                                              join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                              join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                              where c.nombreEmpresa.Contains(nombreCliente) && c.estado == "Vigente" && c.VeracidadInformacion == true
                                              select new EMA_Cotizacion()
                                              {
                                                  id_cotizacion = a.id_cotizacion,
                                                  nombreAsesor = b.nom_usr,
                                                  //estado = c.estado,
                                                  fechaVencimiento = c.fechaExpiracion,
                                                  NIT = c.NIT.ToString(),
                                                  nombreEmpresa = c.nombreEmpresa,
                                                  FechaVisita = c.fechaVisita
                                              });

                                    if (v1 != null && v1.ToList().Count > 0)
                                        listadoCotizaciones = v1.ToList();

                                    foreach (EMA_Cotizacion item in listadoCotizaciones)
                                    {
                                        //var vfiltro = (from a in modelo.Cotizacion_Producto
                                        //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                        //               where a.id_cotizacion == item.id_cotizacion
                                        //               select b
                                        //               ).ToList();
                                        var vfiltro = (from a in modelo.Cotizacion_Producto
                                                       join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                                       where a.id_cotizacion == item.id_cotizacion
                                                       select b.Descripcion
                                                       ).FirstOrDefault();
                                        var estCoti = (from a in modelo.Cotizaciones
                                                       join b in modelo.EstadosPricing on a.EstadoPricing equals b.Descripcion
                                                       where item.id_cotizacion == a.id_cotizacion
                                                       select b.Descripcion).FirstOrDefault();

                                        if (estCoti != null)
                                            item.estadoCotizacion = estCoti;

                                        if (vfiltro != null)
                                            item.productos = vfiltro;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (listadoCotizaciones == null)
                listadoCotizaciones = new List<EMA_Cotizacion>();

            return listadoCotizaciones;
        }

        public IEnumerable<EMA_Cotizacion> FiltroReservasCotizaciones(int menuId, string descripcion, int? estadoPricID, string cotiNIT, string nombreCliente, int pricID)
        {
            var listadoCotizaciones = new List<EMA_Cotizacion>();
            string nombEstado = null;
            using (var usua = new ContextoUsuarios())
            {
                using (var modelo = new ContextoPortal())
                {
                    //REVISAR ESTADO DE COTIZACION PRICING
                    //var lstCotiz = (from u in modelo.Cotizaciones select u).ToList();
                    var lstEstados = (from u in modelo.EstadosPricing where u.Id_Estado == estadoPricID select u).ToList();
                    //var lstCotiProd = (from u in modelo.Cotizacion_Producto select u).ToList();
                    var lstCotiAseso = (from u in modelo.Cotizacion_Asesor where u.id_asesor == pricID select u).ToList();
                    //var lstProds = (from u in modelo.Productos select u).ToList();
                    //var lstUsua = (from u in usua.usrCentral select u).ToList();

                    foreach (var est in lstEstados)
                    {
                        if (est.Id_Estado == estadoPricID)
                        {
                            nombEstado = est.Descripcion;
                        }
                    }

                    if (nombEstado != null || cotiNIT != null || nombreCliente != null)
                    {
                        if (cotiNIT != null && nombEstado == null && nombreCliente == null)
                        {
                            var v1 = (from a in lstCotiAseso
                                      join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                      join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                      where c.NIT == Convert.ToInt32(cotiNIT) && a.id_asesor == pricID && c.estado == "Vigente"
                                      select new EMA_Cotizacion()
                                      {
                                          id_cotizacion = a.id_cotizacion,
                                          nombreAsesor = b.nom_usr,
                                          estado = c.estado,
                                          fechaVencimiento = c.fechaExpiracion,
                                          NIT = c.NIT.ToString(),
                                          nombreEmpresa = c.nombreEmpresa
                                      });

                            if (v1 != null && v1.ToList().Count > 0)
                            {
                                listadoCotizaciones = v1.ToList();

                                foreach (EMA_Cotizacion item in listadoCotizaciones)
                                {
                                    //var vfiltro = (from a in modelo.Cotizacion_Producto
                                    //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                    //               where a.id_cotizacion == item.id_cotizacion
                                    //               select b
                                    //               ).ToList();
                                    var vfiltro = (from a in modelo.Cotizacion_Producto
                                                   join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                                   where a.id_cotizacion == item.id_cotizacion
                                                   select b.Descripcion
                                                   ).FirstOrDefault();
                                    var estCoti = (from a in modelo.Cotizaciones
                                                   join b in modelo.EstadosPricing on a.EstadoPricing equals b.Descripcion
                                                   where item.id_cotizacion == a.id_cotizacion
                                                   select b.Descripcion).FirstOrDefault();

                                    if (estCoti != null)
                                        item.estadoCotizacion = estCoti;

                                    if (vfiltro != null)
                                        item.productos = vfiltro;
                                }
                            }
                        }
                        if (nombEstado != null && cotiNIT != null && nombreCliente != null)
                        {
                            var v1 = (from a in lstCotiAseso
                                      join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                      join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                      where c.nombreEmpresa.Contains(nombreCliente) && c.EstadoPricing == nombEstado && c.NIT == Convert.ToInt32(cotiNIT) && a.id_asesor == pricID && c.estado == "Vigente"
                                      select new EMA_Cotizacion()
                                      {
                                          id_cotizacion = a.id_cotizacion,
                                          nombreAsesor = b.nom_usr,
                                          estado = c.estado,
                                          fechaVencimiento = c.fechaExpiracion,
                                          NIT = c.NIT.ToString(),
                                          nombreEmpresa = c.nombreEmpresa,
                                          estadoCotizacion = c.EstadoPricing
                                      });

                            if (v1 != null && v1.ToList().Count > 0)
                            {
                                listadoCotizaciones = v1.ToList();

                                foreach (EMA_Cotizacion item in listadoCotizaciones)
                                {
                                    //var vfiltro = (from a in modelo.Cotizacion_Producto
                                    //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                    //               where a.id_cotizacion == item.id_cotizacion
                                    //               select b
                                    //               ).ToList();
                                    var vfiltro = (from a in modelo.Cotizacion_Producto
                                                   join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                                   where a.id_cotizacion == item.id_cotizacion
                                                   select b.Descripcion
                                                   ).FirstOrDefault();
                                    if (vfiltro != null)
                                        item.productos = vfiltro;
                                }
                            }
                        }
                        if (nombEstado == null && cotiNIT != null && nombreCliente != null)
                        {
                            var v1 = (from a in lstCotiAseso
                                      join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                      join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                      where c.nombreEmpresa.Contains(nombreCliente) && a.id_asesor == pricID && c.estado == "Vigente"
                                      select new EMA_Cotizacion()
                                      {
                                          id_cotizacion = a.id_cotizacion,
                                          nombreAsesor = b.nom_usr,
                                          estado = c.estado,
                                          fechaVencimiento = c.fechaExpiracion,
                                          NIT = c.NIT.ToString(),
                                          nombreEmpresa = c.nombreEmpresa
                                      });

                            if (v1 != null && v1.ToList().Count > 0)
                            {
                                listadoCotizaciones = v1.ToList();

                                foreach (EMA_Cotizacion item in listadoCotizaciones)
                                {
                                    //var vfiltro = (from a in modelo.Cotizacion_Producto
                                    //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                    //               where a.id_cotizacion == item.id_cotizacion
                                    //               select b
                                    //               ).ToList();
                                    var vfiltro = (from a in modelo.Cotizacion_Producto
                                                   join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                                   where a.id_cotizacion == item.id_cotizacion
                                                   select b.Descripcion
                                                   ).FirstOrDefault();
                                    if (vfiltro != null)
                                        item.productos = vfiltro;

                                    var estCoti = (from a in modelo.Cotizaciones
                                                   join b in modelo.EstadosPricing on a.EstadoPricing equals b.Descripcion
                                                   where item.id_cotizacion == a.id_cotizacion
                                                   select b.Descripcion).FirstOrDefault();

                                    if (estCoti != null)
                                        item.estadoCotizacion = estCoti;
                                }
                            }
                        }
                        if (nombEstado != null && cotiNIT != null && nombreCliente == null)
                        {
                            var v1 = (from a in lstCotiAseso
                                      join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                      join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                      where c.EstadoPricing == nombEstado && c.NIT == Convert.ToInt32(cotiNIT) && a.id_asesor == pricID && c.estado == "Vigente"
                                      select new EMA_Cotizacion()
                                      {
                                          id_cotizacion = a.id_cotizacion,
                                          nombreAsesor = b.nom_usr,
                                          estado = c.estado,
                                          fechaVencimiento = c.fechaExpiracion,
                                          NIT = c.NIT.ToString(),
                                          nombreEmpresa = c.nombreEmpresa,
                                          estadoCotizacion = c.EstadoPricing
                                      });

                            if (v1 != null && v1.ToList().Count > 0)
                            {
                                listadoCotizaciones = v1.ToList();

                                foreach (EMA_Cotizacion item in listadoCotizaciones)
                                {
                                    //var vfiltro = (from a in modelo.Cotizacion_Producto
                                    //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                    //               where a.id_cotizacion == item.id_cotizacion
                                    //               select b
                                    //               ).ToList();
                                    var vfiltro = (from a in modelo.Cotizacion_Producto
                                                   join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                                   where a.id_cotizacion == item.id_cotizacion
                                                   select b.Descripcion
                                                   ).FirstOrDefault();
                                    if (vfiltro != null)
                                        item.productos = vfiltro;
                                }
                            }
                        }
                        if (cotiNIT == null)
                        {
                            if (nombreCliente == null)
                            {
                                var v1 = (from a in lstCotiAseso
                                          join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                          join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                          where c.EstadoPricing == nombEstado && a.id_asesor == pricID && c.estado == "Vigente"
                                          select new EMA_Cotizacion()
                                          {
                                              id_cotizacion = a.id_cotizacion,
                                              nombreAsesor = b.nom_usr,
                                              estado = c.estado,
                                              estadoCotizacion = c.EstadoPricing,
                                              fechaVencimiento = c.fechaExpiracion,
                                              NIT = c.NIT.ToString(),
                                              nombreEmpresa = c.nombreEmpresa
                                          });

                                if (v1 != null && v1.ToList().Count > 0)
                                {
                                    listadoCotizaciones = v1.ToList();

                                    foreach (EMA_Cotizacion item in listadoCotizaciones)
                                    {
                                        //var vfiltro = (from a in modelo.Cotizacion_Producto
                                        //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                        //               where a.id_cotizacion == item.id_cotizacion
                                        //               select b
                                        //               ).ToList();
                                        var vfiltro = (from a in modelo.Cotizacion_Producto
                                                       join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                                       where a.id_cotizacion == item.id_cotizacion
                                                       select b.Descripcion
                                                      ).FirstOrDefault();
                                        if (vfiltro != null)
                                            item.productos = vfiltro;
                                    }
                                }
                            }
                            if (nombreCliente != null)
                            {
                                if (nombEstado != null)
                                {
                                    var v1 = (from a in lstCotiAseso
                                              join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                              join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                              where c.nombreEmpresa.Contains(nombreCliente) && c.EstadoPricing == nombEstado && a.id_asesor == pricID && c.estado == "Vigente"
                                              select new EMA_Cotizacion()
                                              {
                                                  id_cotizacion = a.id_cotizacion,
                                                  nombreAsesor = b.nom_usr,
                                                  estado = c.estado,
                                                  estadoCotizacion = c.EstadoPricing,
                                                  fechaVencimiento = c.fechaExpiracion,
                                                  NIT = c.NIT.ToString(),
                                                  nombreEmpresa = c.nombreEmpresa
                                              });

                                    if (v1 != null && v1.ToList().Count > 0)
                                    {
                                        listadoCotizaciones = v1.ToList();

                                        foreach (EMA_Cotizacion item in listadoCotizaciones)
                                        {
                                            //var vfiltro = (from a in modelo.Cotizacion_Producto
                                            //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                            //               where a.id_cotizacion == item.id_cotizacion
                                            //               select b
                                            //               ).ToList();
                                            var vfiltro = (from a in modelo.Cotizacion_Producto
                                                           join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                                           where a.id_cotizacion == item.id_cotizacion
                                                           select b.Descripcion
                                                           ).FirstOrDefault();
                                            if (vfiltro != null)
                                                item.productos = vfiltro;
                                        }
                                    }
                                }
                                if (nombEstado == null)
                                {
                                    var v1 = (from a in lstCotiAseso
                                              join b in usua.usrCentral on a.id_asesor equals b.id_usr
                                              join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                                              where c.nombreEmpresa.Contains(nombreCliente) && a.id_asesor == pricID && c.estado == "Vigente"
                                              select new EMA_Cotizacion()
                                              {
                                                  id_cotizacion = a.id_cotizacion,
                                                  nombreAsesor = b.nom_usr,
                                                  estado = c.estado,
                                                  fechaVencimiento = c.fechaExpiracion,
                                                  NIT = c.NIT.ToString(),
                                                  nombreEmpresa = c.nombreEmpresa
                                              });

                                    if (v1 != null && v1.ToList().Count > 0)
                                    {
                                        listadoCotizaciones = v1.ToList();

                                        foreach (EMA_Cotizacion item in listadoCotizaciones)
                                        {
                                            //var vfiltro = (from a in modelo.Cotizacion_Producto
                                            //               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                            //               where a.id_cotizacion == item.id_cotizacion
                                            //               select b
                                            //               ).ToList();
                                            var vfiltro = (from a in modelo.Cotizacion_Producto
                                                           join b in modelo.Productos on a.id_producto equals b.COD_SERV
                                                           where a.id_cotizacion == item.id_cotizacion
                                                           select b.Descripcion
                                                           ).FirstOrDefault();
                                            var estCoti = (from a in modelo.Cotizaciones
                                                           join b in modelo.EstadosPricing on a.EstadoPricing equals b.Descripcion
                                                           where item.id_cotizacion == a.id_cotizacion
                                                           select b.Descripcion).FirstOrDefault();

                                            if (estCoti != null)
                                                item.estadoCotizacion = estCoti;

                                            if (vfiltro != null)
                                                item.productos = vfiltro;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (listadoCotizaciones == null)
                listadoCotizaciones = new List<EMA_Cotizacion>();

            return listadoCotizaciones;
        }

        public IEnumerable<EMA_CotizacionXAsesor> ObtenerAsesorFiltro(int cotiasesoId, string descripcion, string numeroDocumento, string nombreAsesor, int directID)
        {
            var listadoAsesores = new List<EMA_CotizacionXAsesor>();
            using (var contAseso = new ContextoStone())
            {
                using (var usua = new ContextoUsuarios())
                {
                    using (var cotiza = new ContextoPortal())
                    {
                        if (numeroDocumento != null)
                        {
                            var documentoDirect = (from u in usua.usrCentral
                                                   where directID == u.id_usr
                                                   select u.num_doc).FirstOrDefault();
                            var esMiAsesor = (from a in contAseso.CC_ASESO
                                              join d in contAseso.GN_TERCE on a.COD_ASES equals d.COD_TERC
                                              where a.COD_DIRE.ToString() == documentoDirect && a.COD_ASES.ToString() == numeroDocumento
                                              select a.COD_ASES).FirstOrDefault();
                            if (esMiAsesor > 0)
                            {
                                var nomAsesor = (from d in contAseso.GN_TERCE
                                                 where numeroDocumento.Equals(d.NUM_IDEN)
                                                 select d.NOM_COMP).FirstOrDefault();

                                var idAsesor = (from u in usua.usrCentral
                                                where numeroDocumento == u.num_doc
                                                select u.id_usr).FirstOrDefault();

                                var numeroCoti = (from u in cotiza.Cotizacion_Asesor
                                                  where idAsesor == u.id_asesor
                                                  select u).ToList();
                                var estAseso = (from d in contAseso.CC_ASESO
                                                where numeroDocumento == d.COD_ASES.ToString()
                                                select d.EST_ASES).FirstOrDefault();
                                var listAseso = new EMA_CotizacionXAsesor()
                                {
                                    id_asesor = idAsesor,
                                    nombreAsesor = nomAsesor,
                                    numeroDocumento = numeroDocumento,
                                    numeroVisitas = numeroCoti.Count(),
                                    estadoAsesor = estAseso
                                };
                                if (listAseso != null)
                                    listadoAsesores.Add(listAseso);
                            }
                            else
                            {
                                listadoAsesores = new List<EMA_CotizacionXAsesor>();
                            }
                        }

                        if (numeroDocumento == null)
                        {
                            var lstCotiAseso = (from u in cotiza.Cotizacion_Asesor select u).ToList();
                            var lstAseso = (from u in contAseso.CC_ASESO select u).ToList();
                            var lstTerce = (from u in contAseso.GN_TERCE where u.NOM_COMP.Contains(nombreAsesor) select u).ToList();
                            var lstUsua = (from u in usua.usrCentral select u).ToList();

                            var docuDire = (from u in lstUsua
                                            where directID == u.id_usr
                                            select u.num_doc).FirstOrDefault();

                            var asesores = (from u in lstTerce
                                            join d in lstAseso on u.COD_TERC equals d.COD_ASES
                                            join c in lstUsua on u.COD_TERC.ToString() equals c.num_doc
                                            where u.NOM_COMP.Contains(nombreAsesor) && d.COD_DIRE.ToString() == docuDire
                                            select new EMA_CotizacionXAsesor()
                                            {
                                                id_asesor = c.id_usr,
                                                nombreAsesor = u.NOM_COMP,
                                                numeroDocumento = d.COD_ASES.ToString(),
                                                estadoAsesor = d.EST_ASES
                                            }).GroupBy(o => o.id_asesor).Select(g => g.First());

                            if (asesores != null && asesores.ToList().Count > 0)
                                listadoAsesores = asesores.ToList();

                            foreach (EMA_CotizacionXAsesor cot in listadoAsesores)
                            {
                                var cotiz = (from a in lstCotiAseso
                                             join b in lstUsua on a.id_asesor equals b.id_usr
                                             where cot.numeroDocumento == b.num_doc
                                             select a).ToList();

                                if (cotiz != null)
                                    cot.numeroVisitas = cotiz.Count();
                            }
                        }
                    }
                }
            }
            if (listadoAsesores == null)
                listadoAsesores = new List<EMA_CotizacionXAsesor>();

            return listadoAsesores;
        }

        public IEnumerable<Cotizaciones> EditarCotizacion(int IdCotizacion, int userId)
        {
            List<Cotizaciones> vLista = new List<Cotizaciones>();
#pragma warning disable CS0219 // The variable 'tipoArea' is assigned but its value is never used
            string tipoArea = null;
#pragma warning restore CS0219 // The variable 'tipoArea' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'observacionesPri' is assigned but its value is never used
            string observacionesPri = null;
#pragma warning restore CS0219 // The variable 'observacionesPri' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'valorCotizacion' is assigned but its value is never used
            decimal? valorCotizacion = 0;
#pragma warning restore CS0219 // The variable 'valorCotizacion' is assigned but its value is never used

            using (var modelo = new ContextoPortal())
            {
                using (var usu = new ContextoUsuarios())
                {
                    var lstEditCotiz = modelo.Database.SqlQuery<Cotizaciones>("EMSP_EDITARRESERVA @IDCOTIZACION",
                        new SqlParameter("@IDCOTIZACION", IdCotizacion)).ToList();

                    if (lstEditCotiz != null && lstEditCotiz.Count() > 0)
                    {
                        foreach (var edit in lstEditCotiz)
                        {
                            if (edit.TipoAP != "")
                            {
                                var v1 = new Cotizaciones()
                                {
                                    NIT = edit.NIT,
                                    nombreEmpresa = edit.nombreEmpresa,
                                    contacto = edit.contacto,
                                    telefono = edit.telefono,
                                    cargo = edit.cargo,
                                    celular = edit.celular,
                                    correoElectronico = edit.correoElectronico,
                                    TipoAP = edit.TipoAP,
                                    fechaVisita = edit.fechaVisita,
                                    ciudad = edit.ciudad,
                                    canal = edit.canal,
                                    estado = edit.estado,
                                    Total = edit.Total,
                                    ObservacionesPricing = edit.ObservacionesPricing
                                };

                                if (v1 != null)
                                    vLista.Add(v1);
                            }
                            else
                            {
                                var v1 = new Cotizaciones()
                                {
                                    NIT = edit.NIT,
                                    nombreEmpresa = edit.nombreEmpresa,
                                    contacto = edit.contacto,
                                    telefono = edit.telefono,
                                    cargo = edit.cargo,
                                    celular = edit.celular,
                                    correoElectronico = edit.correoElectronico,
                                    TipoAP = "No aplica",
                                    fechaVisita = edit.fechaVisita,
                                    ciudad = edit.ciudad,
                                    canal = edit.canal,
                                    estado = edit.estado,
                                    Total = edit.Total,
                                    ObservacionesPricing = "No aplica"
                                };

                                if (v1 != null)
                                    vLista.Add(v1);
                            }
                        }
                    }
                }
            }
            if (vLista == null)
                vLista = new List<Cotizaciones>();

            return vLista;
        }

        public IEnumerable<Cotizaciones> ObtenerDatosReserva(int cotID, int userId)
        {
            List<Cotizaciones> vLista = new List<Cotizaciones>();
            using (var modelo = new ContextoPortal())
            {
                using (var usu = new ContextoUsuarios())
                {
                    var v1 = modelo.Database.SqlQuery<Cotizaciones>("EMSP_DATOSPARASOLICITARCOTIZACION @IDCOTIZACION",
                        new SqlParameter("@IDCOTIZACION", cotID)).ToList();

                    if (v1 != null && v1.ToList().Count > 0)
                        vLista = v1.ToList();
                }
            }
            if (vLista == null)
                vLista = new List<Cotizaciones>();

            return vLista;
        }

        public IEnumerable<SedesCotizacionDto> ObtenerInfSedesCotizacion(int idCotizacion)
        {
            List<SedesCotizacionDto> vLista = new List<SedesCotizacionDto>();
            using (var modelo = new ContextoPortal())
            {
                var vfiltro = (from a in modelo.Cotizacion_Sedes
                               join cot in modelo.Cotizaciones on a.Id_Cotizacion equals cot.id_cotizacion
                               where a.Id_Cotizacion == idCotizacion
                               select new SedesCotizacionDto
                               {
                                   Id = a.Id,
                                   Id_Cotizacion = cot.id_cotizacion,
                                   FechaCreacion = a.FechaCreacion,
                                   FechaModificacion = a.FechaModificacion,
                                   Id_Ciudad = a.Id_Ciudad,
                                   Id_TipoRiesgo = a.Id_TipoRiesgo,
                                   NombreCiudad = a.NombreCiudad,
                                   NombreSede = a.NombreSede,
                                   NombreTipoRiesgo = a.NombreTipoRiesgo,
                                   NoPersonalPermanente = a.NoPersonalPermanente,
                                   NoPersonalVisitantes = a.NoPersonalVisitantes,
                                   Valor = a.Valor,
                                   ValorReconsideracion = a.ValorReconsideracion,
                                   SectorEconomico = cot.SectorEconomico,
                                   TipoAP = cot.TipoAP
                               }
                               ).ToList();

                if (vfiltro != null)
                    vLista = vfiltro;
            }

            if (vLista == null)
                vLista = new List<SedesCotizacionDto>();

            return vLista;
        }

        public IEnumerable<Cotizaciones> EditarCotizacionPricing(int cotID, int userId)
        {
            List<Cotizaciones> vLista = new List<Cotizaciones>();
#pragma warning disable CS0219 // The variable 'tipoArea' is assigned but its value is never used
            string tipoArea = null;
#pragma warning restore CS0219 // The variable 'tipoArea' is assigned but its value is never used
            using (var modelo = new ContextoPortal())
            {
                using (var usu = new ContextoUsuarios())
                {
                    var v1 = modelo.Database.SqlQuery<Cotizaciones>("EMSP_EDITARCOTIZACIONPRICING @IDCOTIZACION",
                        new SqlParameter("@IDCOTIZACION", cotID)).ToList();

                    foreach (var re in v1)
                    {
                        if (re.TipoAP != "")
                        {
                            if (v1 != null && v1.ToList().Count > 0)
                                vLista = v1.ToList();
                        }
                        else
                        {
                            re.TipoAP = "No aplica";

                            if (v1 != null && v1.ToList().Count > 0)
                                vLista = v1.ToList();
                        }
                    }
                }
            }

            if (vLista == null)
                vLista = new List<Cotizaciones>();

            return vLista;
        }

        public IEnumerable<Productos> productosCotizacion(int IdCotizacion)
        {
            List<Productos> vLista = new List<Productos>();
            //List<Productos> productos = new List<Productos>();

            using (var modelo = new ContextoPortal())
            {
                //var lstCotiProd = (from u in modelo.Cotizacion_Producto select u).ToList();
                //var lstProds = (from u in modelo.Productos select u).ToList();

                var vfiltro = (from a in modelo.Cotizacion_Producto
                               join b in modelo.Productos on a.id_producto equals b.COD_SERV
                               where a.id_cotizacion == IdCotizacion
                               select b
                               ).ToList();

                if (vfiltro != null)
                    vLista = vfiltro;
            }

            if (vLista == null)
                vLista = new List<Productos>();

            return vLista;
        }

        private void GuardarNotaTraza(string IdCotizacion, int usuarioId, string notaCotizacion)
        {
            try
            {
                if (string.IsNullOrEmpty(notaCotizacion))
                    return;

                using (var context = new ContextoPortal())
                {
                    var nota = new Cotizacion_NotaTracking()
                    {
                        Fecha = DateTime.Now,
                        Id_Cotizacion = int.Parse(IdCotizacion),
                        Id_Usuario = usuarioId,
                        Nota = notaCotizacion
                    };

                    context.Cotizaciones_Notas.Add(nota);

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //log ex
            }
        }

        public bool liberarReserva(string IdCotizacion, string motivo, int estadoCotiID, int IdUsuario)
        {
            using (var modelo = new ContextoPortal())
            {
                var poli = modelo.PoliticaNITS.Find(1);
                int idCoti = Convert.ToInt32(IdCotizacion);
                var bloque = (from u in modelo.Cotizaciones where u.id_cotizacion == idCoti select u).FirstOrDefault();

                if (!bloque.estado.Equals("Anulada"))
                {
                    if (DateTime.Now <= bloque.fechaExpiracion)
                    {
                        bloque.bloqueo = false;
                        bloque.motivoVisita = motivo;
                        bloque.estado = "Anulada";
                        //bloque.fechaExpiracion = null;
                        modelo.SaveChanges();
                        GuardarNotaTraza(IdCotizacion, IdUsuario, motivo);
                        return true;
                    }
                    else
                    {
                        bloque.estado = "Vencida";
                        bloque.numeroRenovaciones = 0;
                        bloque.bloqueo = false;
                        //bloque.fechaExpiracion = null;
                        modelo.SaveChanges();
                        GuardarNotaTraza(IdCotizacion, IdUsuario, motivo);
                        return false;
                    }
                }
                else
                {
                    if (bloque.estado.Equals("Anulada"))
                    {
                        bloque.estado = "Anulada";
                        bloque.numeroRenovaciones = 0;
                        bloque.bloqueo = false;
                        //bloque.fechaExpiracion = null;
                        modelo.SaveChanges();
                        GuardarNotaTraza(IdCotizacion, IdUsuario, motivo);
                        return false;
                    }
                }

                return false;
            }
        }

        public bool renovarReserva(string IdCotizacion, string MotivoVisita, int estadoCotiID, int IdUsuario)
        {
            using (var modelo = new ContextoPortal())
            {
                var poli = modelo.PoliticaNITS.Find(1);
                int idCoti = Convert.ToInt32(IdCotizacion);
                var renov = (from u in modelo.Cotizaciones where u.id_cotizacion == idCoti select u).FirstOrDefault();


                if (renov.numeroRenovaciones < poli.numero_maximo_renovacion)
                {
                    if (!renov.estado.Equals("Vencida"))
                    {
                        if (!renov.estado.Equals("Anulada"))
                        {
                            if (DateTime.Now <= renov.fechaExpiracion)
                            {
                                renov.motivoVisita = MotivoVisita;
                                // var nuevoEstado = (from u in modelo.EstadosCotizacion where u.Id_Estado == estadoCotiID select u).FirstOrDefault();
                                //if (nuevoEstado != null)
                                //    renov.estado = "Vigente";
                                //if (nuevoEstado == null)
                                //    renov.estado = "Vigente";
                                renov.estado = "Vigente";
                                renov.numeroRenovaciones += 1;
                                //renov.fechaExpiracion = Convert.ToDateTime(renov.fechaExpiracion).AddDays(poli.numero_dias_validez);
                                renov.fechaExpiracion = DateTime.Now.AddDays(poli.numero_dias_validez);
                                modelo.SaveChanges();
                                GuardarNotaTraza(IdCotizacion, IdUsuario, MotivoVisita);
                                return true;
                            }
                            else
                            {
                                renov.estado = "Vencida";
                                renov.numeroRenovaciones = 0;
                                renov.bloqueo = false;
                                renov.fechaExpiracion = null;
                                modelo.SaveChanges();
                                GuardarNotaTraza(IdCotizacion, IdUsuario, MotivoVisita);
                                return false;
                            }
                        }
                        else
                        {
                            if (renov.estado.Equals("Anulada"))
                            {
                                renov.estado = "Anulada";
                                renov.numeroRenovaciones = 0;
                                renov.bloqueo = false;
                                renov.fechaExpiracion = null;
                                modelo.SaveChanges();
                                GuardarNotaTraza(IdCotizacion, IdUsuario, MotivoVisita);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (renov.estado.Equals("Vencida"))
                        {
                            renov.estado = "Vencida";
                            renov.numeroRenovaciones = 0;
                            renov.bloqueo = false;
                            //renov.fechaExpiracion = null;
                            modelo.SaveChanges();
                            GuardarNotaTraza(IdCotizacion, IdUsuario, MotivoVisita);
                            return false;
                        }
                    }
                }

                return false;
            }
        }

        public bool GuardarCotizacion(string IdCotizacion, string ObservacionesPricing, string total, string nombreArchivo)
        {
            using (var coti = new ContextoPortal())
            {
                string estadPric = null;
                int idCoti = Convert.ToInt32(IdCotizacion);
                var guardCoti = (from u in coti.Cotizaciones where u.id_cotizacion == idCoti select u).FirstOrDefault();

                foreach (var est in coti.EstadosPricing)
                {
                    if (est.Id_Estado == 4)
                    {
                        estadPric = est.Descripcion;
                    }
                }

                if (guardCoti != null)
                {
                    guardCoti.EstadoPricing = estadPric;
                    guardCoti.ObservacionesPricing = ObservacionesPricing;
                    guardCoti.Total = Convert.ToDecimal(total);
                    guardCoti.ArchivoCotizacion = nombreArchivo;
                    coti.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool GuardarCotizacionSCI(GuardarCotizacion objData)
        {
            using (var context = new ContextoPortal())
            {
                //Actualizar Los Campos de la Cotización.
                int id_Cotizacion = Convert.ToInt32(objData.Id_Cotizacion);
                var guardarCotizacion = (from u in context.Cotizaciones where u.id_cotizacion == id_Cotizacion select u).FirstOrDefault();

                if (guardarCotizacion != null)
                {
                    guardarCotizacion.SectorEconomico = objData.SectorEconomico;
                    guardarCotizacion.TipoAP = objData.TipoAP;
                    guardarCotizacion.EstadoCotizacion = "Cotizado";
                    guardarCotizacion.Total = objData.ValorTarifa;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool GuardarCotizacionSedesSCI(GuardarCotizacion objData)
        {
            using (var context = new ContextoPortal())
            {
                //Insertar las sedes en la tabla Cotizacion_Sedes
                if (objData.ListadoSedes != null)
                {
                    foreach (var item in objData.ListadoSedes)
                    {
                        item.FechaCreacion = DateTime.Now;
                        item.FechaModificacion = DateTime.Now;
                        item.NombreCiudad = (from a in context.CiudadesFactor where a.Id == item.Id_Ciudad select a.Nombre).FirstOrDefault();
                        item.NombreTipoRiesgo = (from a in context.TipoRiesgo where a.Id == item.Id_TipoRiesgo select a.Nombre).FirstOrDefault();
                    }

                    context.Cotizacion_Sedes.AddRange(objData.ListadoSedes);
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public List<decimal> CalcularTarifaSedeSCI(CalcularTarifaSede objData)
        {
            List<decimal> valorTarifaSede = new List<decimal>();
            using (var context = new ContextoPortal())
            {
                //Calcular la tarifa por Sede
                if (objData != null)
                {
                    var valorTarifa = context.Database.SqlQuery<decimal>(
                        "Usp_ObtenerTarifaCotizacion @Id_Ciudad, @Id_Sector, @Id_TipoRiesgo, @Id_TipoAP, @NumeroEmpleados, @NumeroVisitantes",
                                new SqlParameter("Id_Ciudad", Convert.ToInt32(objData.Id_Ciudad)),
                                new SqlParameter("Id_Sector", Convert.ToInt32(objData.Id_Sector)),
                                new SqlParameter("Id_TipoRiesgo", Convert.ToInt32(objData.Id_TipoRiesgo)),
                                new SqlParameter("Id_TipoAP", Convert.ToInt32(objData.Id_TipoAP)),
                                new SqlParameter("NumeroEmpleados", Convert.ToInt32(objData.NumeroEmpleados)),
                                new SqlParameter("NumeroVisitantes", Convert.ToInt32(objData.NumeroVisitantes))
                        ).ToList();

                    if (valorTarifa != null && valorTarifa.ToList().Count > 0)
                        valorTarifaSede = valorTarifa.ToList();
                }
            }
            return valorTarifaSede;
        }

        /// <summary>
        /// PASAR A SP ESTE METODO
        /// </summary>
        /// <param name="IdCotizacion"></param>
        /// <returns></returns>
        public string ObtenerCorreoAsesor(string IdCotizacion)
        {
            string correo = null;

            using (var modelo = new ContextoPortal())
            {
                using (var usua = new ContextoUsuarios())
                {
                    //var lstCotiz = (from u in modelo.Cotizaciones select u).ToList();
                    //var lstEstados = (from u in modelo.EstadosCotizacion select u).ToList();
                    //var lstCotiProd = (from u in modelo.Cotizacion_Producto select u).ToList();
                    var lstCotiAseso = (from u in modelo.Cotizacion_Asesor select u).ToList();
                    //var lstProds = (from u in modelo.Productos select u).ToList();
                    //var lstSectores = (from u in modelo.SECTORES select u).ToList();
                    var lstUsua = (from u in usua.usrCentral select u).ToList();

                    var v1 = (from a in lstCotiAseso
                              join b in lstUsua on a.id_asesor equals b.id_usr
                              join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                              join d in modelo.SECTORES on c.SectorEconomico equals d.Id_sector.ToString()
                              where c.id_cotizacion.ToString() == IdCotizacion
                              select b.correo).First();

                    if (v1 != null)
                    {
                        correo = v1;
                    }
                    else
                    {
                        correo = null;
                    }
                }
            }
            return correo;
        }

        public string ObtenerDocumentoAsesor(string IdCotizacion)
        {
            string documentoAseso = null;

            using (var modelo = new ContextoPortal())
            {
                using (var usua = new ContextoUsuarios())
                {
                    var v1 = modelo.Database.SqlQuery<string>("EMSP_ObtenerDocumentoAsesor @IDCOTIZACION",
                        new SqlParameter("@IDCOTIZACION", IdCotizacion)).FirstOrDefault();

                    if (v1 != null)
                    {
                        documentoAseso = v1;
                    }
                    else
                    {
                        documentoAseso = null;
                    }
                }
            }
            return documentoAseso;
        }

        public string ObtenerCorreoDirector(string Document)
        {
            string correoDirector = null;

            using (var modelo = new ContextoStone())
            {
                using (var usua = new ContextoUsuarios())
                {
                    //var lstUsua = (from u in usua.usrCentral select u).ToList();
                    //var lstAseDire = (from u in modelo.CC_ASESO select u).ToList();

                    var v1 = (from a in modelo.CC_ASESO
                              where a.COD_ASES.ToString() == Document
                              select a.COD_DIRE.ToString()).FirstOrDefault();

                    if (v1 != null)
                    {
                        var correoD = (from b in usua.usrCentral
                                       where b.num_doc == v1
                                       select b.correo).FirstOrDefault();

                        if (correoD != null)
                        {
                            correoDirector = correoD;
                            return correoDirector;
                        }
                    }
                    return null;
                }
            }
        }

        public string ObtenerNombreDirector(string userAseso)
        {
            string nombreDirector = null;

            using (var modelo = new ContextoStone())
            {
                using (var usua = new ContextoUsuarios())
                {
                    //var lstAseDire = (from u in modelo.CC_ASESO select u).ToList();
                    //var lstGnTerc = (from u in modelo.GN_TERCE select u).ToList();

                    var v1 = (from a in modelo.CC_ASESO
                              where a.COD_ASES.ToString() == userAseso
                              select a.COD_DIRE.ToString()).FirstOrDefault();

                    if (v1 != null)
                    {
                        var nomDire = (from b in modelo.GN_TERCE
                                       where b.COD_TERC.ToString() == v1
                                       select b.NOM_COMP).First();

                        if (nomDire != null)
                        {
                            nombreDirector = nomDire;
                            return nombreDirector;
                        }
                    }
                    return null;
                }
            }
        }

        public string ObtenerEmpresaCliente(string IdCotizacion)
        {
            string empreClient = null;

            using (var modelo = new ContextoPortal())
            {
                using (var usua = new ContextoUsuarios())
                {
                    //var lstCotiz = (from u in modelo.Cotizaciones select u).ToList();
                    //var lstEstados = (from u in modelo.EstadosCotizacion select u).ToList();
                    //var lstCotiProd = (from u in modelo.Cotizacion_Producto select u).ToList();
                    var lstCotiAseso = (from u in modelo.Cotizacion_Asesor select u).ToList();
                    //var lstProds = (from u in modelo.Productos select u).ToList();
                    //var lstSectores = (from u in modelo.SECTORES select u).ToList();
                    var lstUsua = (from u in usua.usrCentral select u).ToList();

                    var v1 = (from a in lstCotiAseso
                              join b in lstUsua on a.id_asesor equals b.id_usr
                              join c in modelo.Cotizaciones on a.id_cotizacion equals c.id_cotizacion
                              join d in modelo.SECTORES on c.SectorEconomico equals d.Id_sector.ToString()
                              where c.id_cotizacion.ToString() == IdCotizacion
                              select c.nombreEmpresa).First();

                    if (v1 != null)
                    {
                        empreClient = v1;
                    }
                    else
                    {
                        empreClient = null;
                    }
                }
            }
            return empreClient;
        }

        public List<ReporteCotizacion> ObtenerInfReporteCotizacion(int idCotizacion)
        {
            List<ReporteCotizacion> lstReporteCotizacion = new List<ReporteCotizacion>();
            using (var context = new ContextoPortal())
            {
                if (idCotizacion != 0)
                {
                    var dataReporteCotizacion = context.Database.SqlQuery<ReporteCotizacion>(
                        "Usp_DataReporteCotizacion @Id_Cotizacion",
                                new SqlParameter("Id_Cotizacion", Convert.ToInt32(idCotizacion))
                        ).ToList();

                    if (dataReporteCotizacion != null && dataReporteCotizacion.ToList().Count > 0)
                        lstReporteCotizacion = dataReporteCotizacion.ToList();
                }
            }
            return lstReporteCotizacion;
        }

        public List<GlobalReporte> ObtenerReporteGlobal()
        {
            List<GlobalReporte> lstReporteGlobal = new List<GlobalReporte>();
            using (var context = new ContextoPortal())

                try
                {
                    var dataGridReporte = context.Database.SqlQuery<GlobalReporte>(
                        "Usp_ReporteCotizacionExport").ToList();

                    if (dataGridReporte != null && dataGridReporte.ToList().Count > 0)
                        lstReporteGlobal = dataGridReporte.ToList();
                }
                catch (Exception e)
                {

                    throw e;
                }



            return lstReporteGlobal;
        }

        public bool EnviarCotizacionAReconsideracion(ReconsideracionCotizacionDto reconsideracion)
        {
            if (reconsideracion.RolId.Count() == 1 && reconsideracion.RolId.Contains((int)RolPortal.Asesor))
                return false;

            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
                {
                    using (var context = new ContextoPortal())
                    {
                        var cotizacion = (from u in context.Cotizaciones where u.id_cotizacion == reconsideracion.CotizacionId select u).FirstOrDefault();

                        if (cotizacion == null || cotizacion.EstadoCotizacion == GetEnumDescription(EstadoCotizacion.Reconsiderado))
                            return false;

                        if (cotizacion.NumeroReconsideraciones == null) cotizacion.NumeroReconsideraciones = 0;

                        if (!reconsideracion.RolId.Contains((int)RolPortal.Pricing))
                        {
                            cotizacion.NumeroReconsideraciones += 1;
                            cotizacion.EstadoCotizacion = GetEnumDescription(EstadoCotizacion.PendienteReconsideracion);
                        }

                        cotizacion.valorReconsideracion = reconsideracion.Sedes.Sum(s => s.ValorReconsideracion.Value);

                        var sedesCotizacion = (from u in context.Cotizacion_Sedes where u.Id_Cotizacion == reconsideracion.CotizacionId select u).ToList();

                        foreach (var sede in sedesCotizacion)
                        {
                            var sedeReconsideracion = reconsideracion.Sedes.FirstOrDefault(s => s.SedeId == sede.Id);

                            if (sedeReconsideracion != null &&
                                sedeReconsideracion.ValorReconsideracion.Value != sede.Valor &&
                                sedeReconsideracion.ValorReconsideracion != sede.ValorReconsideracion)
                            {
                                sede.ValorReconsideracion = sedeReconsideracion.ValorReconsideracion.Value;
                            }
                        }

                        var nota = new Cotizacion_NotaTracking()
                        {
                            DetalleReconsideracion = reconsideracion.DetalleReconsideracion,
                            Fecha = DateTime.Now,
                            Id_Cotizacion = reconsideracion.CotizacionId,
                            Id_Usuario = reconsideracion.UserId,
                            Nota = reconsideracion.NotaReconsideracion
                        };

                        context.Cotizaciones_Notas.Add(nota);

                        context.SaveChanges();
                        scope.Complete();

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool AprobarReconsideracion(ReconsideracionCotizacionDto reconsideracion)
        {
            try
            {
                if (!reconsideracion.RolId.Contains((int)RolPortal.Pricing))
                    return false;

                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
                {
                    using (var context = new ContextoPortal())
                    {
                        var cotizacion = (from u in context.Cotizaciones where u.id_cotizacion == reconsideracion.CotizacionId select u).FirstOrDefault();

                        if (cotizacion == null || cotizacion.EstadoCotizacion == GetEnumDescription(EstadoCotizacion.Reconsiderado))
                            return false;

                        cotizacion.EstadoCotizacion = GetEnumDescription(EstadoCotizacion.Reconsiderado);

                        cotizacion.Total = cotizacion.valorReconsideracion;

                        var sedesCotizacion = (from u in context.Cotizacion_Sedes where u.Id_Cotizacion == reconsideracion.CotizacionId select u).ToList();

                        foreach (var sede in sedesCotizacion)
                        {
                            if (sede.ValorReconsideracion != null)
                                sede.Valor = sede.ValorReconsideracion.Value;
                        }

                        var nota = new Cotizacion_NotaTracking()
                        {
                            Fecha = DateTime.Now,
                            Id_Cotizacion = reconsideracion.CotizacionId,
                            Id_Usuario = reconsideracion.UserId,
                            Nota = reconsideracion.NotaReconsideracion
                        };

                        context.Cotizaciones_Notas.Add(nota);

                        context.SaveChanges();
                        scope.Complete();

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool RechazarReconsideracion(ReconsideracionCotizacionDto reconsideracion)
        {
            try
            {
                if (!reconsideracion.RolId.Contains((int)RolPortal.Pricing))
                    return false;

                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
                {
                    using (var context = new ContextoPortal())
                    {
                        var cotizacion = (from u in context.Cotizaciones where u.id_cotizacion == reconsideracion.CotizacionId select u).FirstOrDefault();

                        if (cotizacion == null ||
                            cotizacion.EstadoCotizacion == GetEnumDescription(EstadoCotizacion.Reconsiderado) ||
                            cotizacion.EstadoCotizacion == GetEnumDescription(EstadoCotizacion.NoReconsiderado))
                            return false;

                        cotizacion.EstadoCotizacion = GetEnumDescription(EstadoCotizacion.NoReconsiderado);

                        var nota = new Cotizacion_NotaTracking()
                        {
                            Fecha = DateTime.Now,
                            Id_Cotizacion = reconsideracion.CotizacionId,
                            Id_Usuario = reconsideracion.UserId,
                            Nota = reconsideracion.NotaReconsideracion
                        };

                        context.Cotizaciones_Notas.Add(nota);

                        context.SaveChanges();
                        scope.Complete();

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<NotaCotizacionDto> ObtenerNotasCotizacion(int cotizacionId)
        {
            using (var usuarios = new ContextoUsuarios())
            {
                using (var modelo = new ContextoPortal())
                {
                    var notas = (from N in modelo.Cotizaciones_Notas where N.Id_Cotizacion == cotizacionId select N).ToList();

                    var notasCotizacion = (from N in notas
                                           join U in usuarios.usrCentral on N.Id_Usuario equals U.id_usr
                                           join UR in usuarios.UsuarioXappXrol on U.id_usr equals UR.id_usr
                                           join R in usuarios.Roles on UR.id_rol equals R.id_rol
                                           where N.Id_Cotizacion == cotizacionId
                                           orderby N.Fecha descending
                                           select new NotaCotizacionDto()
                                           {
                                               Id = N.Id,
                                               Fecha = N.Fecha,
                                               Rol = R.nom_rol,
                                               NombreUsuario = U.nom_usr,
                                               Nota = N.Nota,
                                               OrderId = GetOrderRol(UR.id_rol)
                                           })
                                           .GroupBy(n => n.Id).Select(n => new NotaCotizacionDto()
                                           {
                                               Fecha = n.Select(nota => nota.Fecha).FirstOrDefault(),
                                               NombreUsuario = n.Select(nota => nota.NombreUsuario).FirstOrDefault(),
                                               Nota = n.Select(nota => nota.Nota).FirstOrDefault(),
                                               Rol = n.OrderByDescending(nota => nota.OrderId).Select(nota => nota.Rol).FirstOrDefault()
                                           })
                                           .ToList();

                    return notasCotizacion;
                }
            }


        }

        private static int GetOrderRol(int RolId)
        {
            // Jerarquía para reconsideración
            switch (RolId)
            {
                case 8:
                    return 1;
                case 7:
                case 29:
                case 30:
                    return 2;
                case 28:
                    return 3;
                default:
                    return 1;
            }
        }

        private static string GetEnumDescription(Enum value)
        {
            System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());

            System.ComponentModel.DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false) as System.ComponentModel.DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        public List<GlobalReporte> ObtenerDataReporte(FiltrosReporte filtros, string noDocumento, string rol)
        {
            List<GlobalReporte> ListDataReporte = new List<GlobalReporte>();
            using (var context = new ContextoPortal())

                try
                {
                    var dataGridReporte = context.Database.SqlQuery<GlobalReporte>(
                    "Usp_ObtenerReporteCotizaciones @Idproducto, @IdCiudad, @IdCanal, @IdEstadoReserva, @IdEstadoCotizacion, @IdSectorEconomico, @startDate, @endDate, @NoDocumento, @Rol,@IdAsesores, @IdDirectores",
                            new SqlParameter("IdProducto", filtros.ProductosId ?? System.Data.SqlTypes.SqlString.Null),
                            new SqlParameter("IdCiudad", filtros.CiudadesId ?? System.Data.SqlTypes.SqlString.Null),
                            new SqlParameter("IdCanal", filtros.CanalesId ?? System.Data.SqlTypes.SqlString.Null),
                            new SqlParameter("IdEstadoReserva", filtros.EstadosId ?? System.Data.SqlTypes.SqlString.Null),
                            new SqlParameter("IdEstadoCotizacion", filtros.EstadosCotizacionId ?? System.Data.SqlTypes.SqlString.Null),
                            new SqlParameter("IdSectorEconomico", filtros.SectoresId ?? System.Data.SqlTypes.SqlString.Null),
                            new SqlParameter("StartDate", filtros.FechaInicio.ToShortDateString()),
                            new SqlParameter("EndDate", filtros.FechaFin.ToShortDateString()),
                            new SqlParameter("@NoDocumento", noDocumento),
                            new SqlParameter("@Rol", filtros.DescripcionRol),
                            new SqlParameter("@IdAsesores", filtros.AsesoresId ?? System.Data.SqlTypes.SqlString.Null),
                            new SqlParameter("@IdDirectores", filtros.DirectoresId ?? System.Data.SqlTypes.SqlString.Null)


                    ).ToList();

                    if (dataGridReporte != null && dataGridReporte.ToList().Count > 0)
                        ListDataReporte = dataGridReporte.ToList();
                }
                catch (Exception e)
                {

                    throw e;
                }



            return ListDataReporte;
        }

        public List<ItemSelect> ObtenerCiudades()
        {
            var retorno = new List<ItemSelect>();
            try
            {
                using (var areaETL = new ContextEMERMEDICA_AreaETL())
                {
                    var vGet = areaETL.EME_CIUDAD_HOMOLOG;
                    if (vGet != null)
                        retorno = vGet.GroupBy(x => x.CIUDAD_HOMOLOG)
                          .Select(y => new ItemSelect
                          {
                              Id = y.Max(x => x.COD_CCOS),
                              Descripcion = y.Key
                          }).ToList();
                }

            }
            catch (Exception e)
            {

                throw e;
            }


            return retorno;
        }


        public List<EMB_Canal> ObtenerCanales()
        {
            var retorno = new List<EMB_Canal>();

            using (var areaETL = new ContextEMERMEDICA_AreaETL())
            {
                var vGet = areaETL.EME_Canal;
                if (vGet != null)
                    retorno = vGet.ToList();
            }
            return retorno;
        }

        public List<SECTOR> ObtenerSector()
        {
            var retorno = new List<SECTOR>();

            using (var areaETL = new ContextoPortal())
            {
                var vGet = areaETL.SECTORES;
                if (vGet != null)
                    retorno = vGet.ToList();
            }
            return retorno;
        }

        public List<Director> ObtenerDirector(string ciudad = null)
        {

            using (var modelo = new ContextoStone())
            {
                var retorno = new List<Director>();
                var parametro = string.IsNullOrEmpty(ciudad) ? string.Empty : $"'{ciudad}'";
                retorno = modelo.Database.SqlQuery<Director>(
                    $"SCI_OBTENER_LISTADO_DIRECTORES {parametro}").OrderBy(d => d.nom_usr).ToList();
                return retorno;
            }
        }

        public List<Productos> ObtenerProductosDisponiblesParaNit(decimal nit, string ciudadUsuario)
        {
            var retorno = new List<Productos>();
            using (var dbContext = new ContextoPortal())
            {
                var productosReservados = (from p in dbContext.Productos
                                           join cp in dbContext.Cotizacion_Producto on p.COD_SERV equals cp.id_producto
                                           join c in dbContext.Cotizaciones on cp.id_cotizacion equals c.id_cotizacion
                                           where c.NIT == nit && c.ciudad == ciudadUsuario && c.bloqueo == true
                                           select p.id_producto).ToList();

                var vGet = dbContext.Productos.Where(p => !productosReservados.Contains(p.id_producto)).ToList();
                if (vGet != null)
                    retorno = vGet.ToList();
            }

            return retorno;
        }

        public int ObtenerMaximoRenovaciones()
        {
            using (var dbContext = new ContextoPortal())
            {
                var poli = dbContext.PoliticaNITS.Find(1);
                if (poli != null)
                    return poli.numero_maximo_renovacion;
                else
                    return 3;
            }
        }

        public List<ItemUsuario> ObtenerAsesoresXDirectores(List<decimal> directorIds)
        {
            using (var modelo = new ContextoPortal())
            {
                var retorno = new List<ItemUsuario>();
                retorno = modelo.Database.SqlQuery<ItemUsuario>($"Usp_ObtenerAsesoresXDirector {string.Join(",", directorIds)}").ToList();
                return retorno;
            }
        }


        public Respuesta RegistrarSolicitudDigitalizacion(InfoSolicitud info, int userId)
        {
            try
            {
                var link = string.IsNullOrEmpty(info.Link) ? string.Empty : info.Link;
                using (var table = new DataTable())
                {
                    table.Columns.Add("Id", typeof(int));
                    table.Columns.Add("NombreOriginal", typeof(string));
                    table.Columns.Add("NombreArchivo", typeof(string));
                    table.Columns.Add("ContentType", typeof(string));
                    table.Columns.Add("RutaArchivo", typeof(string));

                    foreach (var item in info.FileInfo)
                    {
                        table.Rows.Add(0, item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo);
                    }

                    using (var modelo = new ContextoPortal())
                    {
                        var infoList = new SqlParameter("@FileInfo", SqlDbType.Structured)
                        {
                            TypeName = "dbo.ListFileInto",
                            Value = table
                        };

                        var exitosoParameter = new SqlParameter
                        {
                            ParameterName = "@State",
                            Direction = ParameterDirection.Output,
                            SqlDbType = SqlDbType.Bit,
                            Size = 1
                        };
                        var mensajeParameter = new SqlParameter
                        {
                            ParameterName = "@MensajeError",
                            Direction = ParameterDirection.Output,
                            SqlDbType = SqlDbType.NVarChar,
                            Size = -1
                        };

                        var result = modelo.Database.ExecuteSqlCommand("SCISP_RegistrarSolicitudDigitalizacion @IdUsr, " +
                            "@NumeroFormulario, @CedulaAsesor, @CedulaContratante, @NumeroAfiliados," +
                            "@Link, @TipoArchivo, @FileInfo, @State OUTPUT, @MensajeError OUTPUT,@TipoContrato,@NumeroContrato",
                                new SqlParameter("@IdUsr", userId),
                                new SqlParameter("@NumeroFormulario", info.NumeroFormulario),
                                new SqlParameter("@CedulaAsesor", info.CedulaAsesor),
                                new SqlParameter("@CedulaContratante", info.CedulaContratante),
                                new SqlParameter("@NumeroAfiliados", info.NumeroAfiliados),
                                new SqlParameter("@Link", link),
                                new SqlParameter("@TipoArchivo", info.TipoArchivo),
                                infoList,
                                exitosoParameter,
                                mensajeParameter,
                                new SqlParameter("@TipoContrato", info.TipoContrato),
                                new SqlParameter("@NumeroContrato", string.IsNullOrEmpty(info.NumeroContrato) ? DBNull.Value.ToString() : info.NumeroContrato)
                            );

                        return new Respuesta
                        {
                            Exitoso = (bool)exitosoParameter.Value,
                            Mensaje = mensajeParameter.Value.ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool EditarSolicitudDigitalizacion(InfoSolicitud info, int userId)
        {
            try
            {

                var link = string.IsNullOrEmpty(info.Link) ? string.Empty : info.Link;
                var tableArchivos = new DataTable();
                tableArchivos.Columns.Add("Id", typeof(int));
                tableArchivos.Columns.Add("NombreOriginal", typeof(string));
                tableArchivos.Columns.Add("NombreArchivo", typeof(string));
                tableArchivos.Columns.Add("ContentType", typeof(string));
                tableArchivos.Columns.Add("RutaArchivo", typeof(string));

                var tableEliminarArchivos = new DataTable();
                tableEliminarArchivos.Columns.Add("Id", typeof(int));
                tableEliminarArchivos.Columns.Add("NombreOriginal", typeof(string));
                tableEliminarArchivos.Columns.Add("NombreArchivo", typeof(string));
                tableEliminarArchivos.Columns.Add("ContentType", typeof(string));
                tableEliminarArchivos.Columns.Add("RutaArchivo", typeof(string));

                var tableCausales = new DataTable();
                tableCausales.Columns.Add("CausalId", typeof(string));
                tableCausales.Columns.Add("Causal", typeof(string));

                if (info.FileInfo != null)
                {
                    foreach (var item in info.FileInfo)
                    {
                        tableArchivos.Rows.Add(item.Id, item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo);
                    }
                }

                if (info.FileInfoEliminar != null)
                {
                    foreach (var item in info.FileInfoEliminar)
                    {
                        tableEliminarArchivos.Rows.Add(item.Id, item.NombreOriginal, item.NombreArchivo, item.ContentType, item.RutaArchivo);
                    }
                }

                if (info.Causales != null)
                {
                    foreach (var item in info.Causales)
                    {
                        tableCausales.Rows.Add(item.CausalId, item.Causal);
                    }
                }

                using (var modelo = new ContextoPortal())
                {
                    var infoListArchivos = new SqlParameter("@FileInfo", SqlDbType.Structured)
                    {
                        TypeName = "dbo.ListFileInto",
                        Value = tableArchivos
                    };

                    var infoListEliminarArchivos = new SqlParameter("@FileInfoEliminar", SqlDbType.Structured)
                    {
                        TypeName = "dbo.ListFileInto",
                        Value = tableEliminarArchivos
                    };

                    var infoListCausales = new SqlParameter("@Causales", SqlDbType.Structured)
                    {
                        TypeName = "dbo.ListCausales",
                        Value = tableCausales
                    };

                    if (tableArchivos.Rows.Count > 0 && tableEliminarArchivos.Rows.Count > 0 && tableCausales.Rows.Count > 0)
                    {

                    }
                    var identity = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity);
                    var documentoGestionador = identity.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                    var rolGestionador = identity.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;
                    var result = modelo.Database.ExecuteSqlCommand("SCISP_ActualizarSolicitudDigitalizacion @Id, @IdUsr, @NumeroFormulario," +
                        " @CedulaAsesor, @CedulaContratante, @NumeroAfiliados, @Link, @TipoArchivo,@FileInfoEliminar, @FileInfo,@Causales," +
                        "@IdEstado, @Observaciones, @State, @MedioPago, @TipoContrato, @NumeroContrato, @DocumentoGestionador," +
                        "@RolGestionador",
                            new SqlParameter("@Id", info.Id),
                            new SqlParameter("@IdUsr", userId),
                            new SqlParameter("@NumeroFormulario", info.NumeroFormulario),
                            new SqlParameter("@CedulaAsesor", info.CedulaAsesor),
                            new SqlParameter("@CedulaContratante", info.CedulaContratante),
                            new SqlParameter("@NumeroAfiliados", info.NumeroAfiliados),
                            new SqlParameter("@Link", link),
                            new SqlParameter("@TipoArchivo", info.TipoArchivo),
                            tableEliminarArchivos.Rows.Count > 0 ? infoListEliminarArchivos : new SqlParameter("@FileInfoEliminar", SqlDbType.Structured)
                            {
                                TypeName = "dbo.ListFileInto",
                                Value = null
                            },
                            tableArchivos.Rows.Count > 0 ? infoListArchivos : new SqlParameter("@FileInfo", SqlDbType.Structured)
                            {
                                TypeName = "dbo.ListFileInto",
                                Value = null
                            },
                            tableCausales.Rows.Count > 0 ? infoListCausales : new SqlParameter("@Causales", SqlDbType.Structured)
                            {
                                TypeName = "dbo.ListCausales",
                                Value = null
                            },
                            new SqlParameter("@IdEstado", info.EstadoId),
                            new SqlParameter("@Observaciones", info.Observaciones ?? ""),
                            new SqlParameter("@State", ParameterDirection.Output),
                            new SqlParameter("@MedioPago", string.IsNullOrEmpty(info.MedioPago) ? DBNull.Value.ToString() : info.MedioPago),
                            new SqlParameter("@TipoContrato", string.IsNullOrEmpty(info.TipoContrato) ? DBNull.Value.ToString() : info.TipoContrato),
                            new SqlParameter("@NumeroContrato", string.IsNullOrEmpty(info.NumeroContrato) ? DBNull.Value.ToString() : info.NumeroContrato),
                            new SqlParameter("@DocumentoGestionador", documentoGestionador),
                            new SqlParameter("@RolGestionador", rolGestionador)
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

        public string ObtenerCedulaAsesor(int userId)
        {
            try
            {
                var cedulaAsesor = string.Empty;

                using (var bd = new ContextoUsuarios())
                {
                    cedulaAsesor = (from a in bd.usrCentral
                                    where a.id_usr == userId
                                    select a.num_doc).First();
                }
                return cedulaAsesor;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<SolicitudDigital> ObtenerSolicitudesDigitalizacion(string documento, string userId, string fechaInicio, string fechaFin, bool esDirector)
        {
            var obj = new List<SolicitudDigital>();
            var objArchivo = new List<ArchivosSolicitud>();

            userId = string.IsNullOrEmpty(userId) ? string.Empty : userId;

            using (var modelo = new ContextoPortal())
            {
                var lstSolicitudes = modelo.Database.SqlQuery<SolicitudDigitalizacion>("SCISP_ObtenerSolicitudDigitalizacion @Documento, @IdUsr, @FechaInicio, @FechaFin, @EsDirector",
                        new SqlParameter("@Documento", documento)
                        , new SqlParameter("@IdUsr", userId)
                        , new SqlParameter("@FechaInicio", fechaInicio)
                        , new SqlParameter("@FechaFin", fechaFin)
                        , new SqlParameter("@EsDirector", esDirector ? "1" : "0")
                    ).ToList();

                var count = 0;
                foreach (var item in lstSolicitudes)
                {
                    var result = lstSolicitudes.Where(x => x.Id == item.Id).Count();

                    if (result == 1)
                    {
                        objArchivo = new List<ArchivosSolicitud>();

                        objArchivo.Add(new ArchivosSolicitud
                        {
                            Id = item.ArchivoId,
                            NombreOriginal = item.NombreOriginal,
                            RutaArchivo = item.RutaArchivo
                        });

                        obj.Add(new SolicitudDigital
                        {
                            Id = item.Id,
                            IdUsr = item.IdUsr,
                            NumeroFormulario = item.NumeroFormulario.ToString(),
                            CedulaAsesor = item.CedulaAsesor,
                            NombreAsesor = item.NombreAsesor,
                            Director = item.Director,
                            CedulaContratante = item.CedulaContratante,
                            NumeroAfiliados = item.NumeroAfiliados,
                            Link = item.Link,
                            TipoArchivo = item.TipoArchivo,
                            FechaRegistro = item.FechaRegistro,
                            FechaCargue = item.FechaCargue,
                            FechaUltimoCargue = item.FechaUltimoCargue,
                            ClaveTipoContrato = item.ClaveTipoContrato,
                            TipoContrato = item.TipoContrato,
                            MedioPago = item.MedioPago,
                            NumeroContrato_Inclusion = item.NumeroContrato_Inclusion,
                            Archivos = objArchivo,
                            IdEstado = item.IdEstado,
                            Estado = item.Estado,
                            Observaciones = item.Observaciones
                        });
                    }

                    if (result > 1)
                    {
                        count++;

                        if (count == 1)
                            objArchivo = new List<ArchivosSolicitud>();

                        objArchivo.Add(new ArchivosSolicitud
                        {
                            Id = item.ArchivoId,
                            NombreOriginal = item.NombreOriginal,
                            RutaArchivo = item.RutaArchivo
                        });

                        if (count == result)
                        {
                            obj.Add(new SolicitudDigital
                            {
                                Id = item.Id,
                                IdUsr = item.IdUsr,
                                NumeroFormulario = item.NumeroFormulario.ToString(),
                                CedulaAsesor = item.CedulaAsesor,
                                NombreAsesor = item.NombreAsesor,
                                Director = item.Director,
                                CedulaContratante = item.CedulaContratante,
                                NumeroAfiliados = item.NumeroAfiliados,
                                Link = item.Link,
                                TipoArchivo = item.TipoArchivo,
                                FechaRegistro = item.FechaRegistro,
                                FechaCargue = item.FechaCargue,
                                FechaUltimoCargue = item.FechaUltimoCargue,
                                ClaveTipoContrato = item.ClaveTipoContrato,
                                TipoContrato = item.TipoContrato,
                                MedioPago = item.MedioPago,
                                NumeroContrato_Inclusion = item.NumeroContrato_Inclusion,
                                Archivos = objArchivo,
                                IdEstado = item.IdEstado,
                                Estado = item.Estado,
                                Observaciones = item.Observaciones
                            });

                            count = 0;
                        }
                    }

                }
            }

            return obj;
        }

        public List<CausalesDigitalizacion> ObtenerCausales(int idEstado)
        {
            using (var model = new ContextoPortal())
            {
                var prm = new SqlParameter[1];
                prm[0] = new SqlParameter("@ESTADO", idEstado);
                var consulta = model.Database.SqlQuery<CausalesDigitalizacion>("SCISP_CONSULTAR_CAUSALES_DIGITALIZACION @ESTADO",
                    prm).ToList();
                return consulta;
            }
        }


        public List<CausalesSolicitud> GetCausalesSolicitud(int idSolicitud)
        {
            using (var model = new ContextoPortal())
            {
                var consulta = model.Database.SqlQuery<CausalesSolicitud>("SCISP_ConsultarCausalesSolicitud @IdSolicitud",
                    new SqlParameter("@IdSolicitud", idSolicitud)).ToList();
                return consulta;
            }
        }
        public List<HistoricoSolicitud> GetHistoricoSolicitud(int idSolicitud)
        {
            using (var model = new ContextoPortal())
            {
                var historicos = model.Database.SqlQuery<HistoricoSolicitud>("SCISP_ConsultarHistoricoSolicitud @IdSolicitud",
                    new SqlParameter("@IdSolicitud", idSolicitud)).ToList();
                foreach (var reg in historicos)
                {
                    if (reg.IdHistorico == null)
                        reg.Causales = model.Database.SqlQuery<CausalesSolicitud>("SCISP_ConsultarCausalesSolicitud @IdSolicitud",
                            new SqlParameter("@IdSolicitud", idSolicitud)).ToList();
                    else
                        reg.Causales = model.Database.SqlQuery<CausalesSolicitud>("SCISP_ConsultarCausalesHistoricoSolicitud @IdHistorico",
                                new SqlParameter("@IdHistorico", reg.IdHistorico == null ? 0 : reg.IdHistorico)).ToList();
                }
                return historicos;
            }
        }
        public bool VerificarContrato(string numeroContrato)
        {
            using (var model = new ContextoPortal())
            {

                var contratos = model.Database.SqlQuery<object>("SPCONSULTACONTRATOPORTALCOMERCIAL @NUMEROCONTRATO",
                    new SqlParameter("@NUMEROCONTRATO", numeroContrato)).ToList();
                return contratos.Count > 0;
            }
        }
        public async Task<TokenReferenciaPago> GetTokenRefPago()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var urlToken = ConfigurationManager.AppSettings["UrlPasarela"].Replace("{0}", "token");
            var username_api = ConfigurationManager.AppSettings["ApiPasarelaUsername"];
            var password_api = ConfigurationManager.AppSettings["ApiPasarelaPassword"];


            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, urlToken);
            var keys = new List<KeyValuePair<string, string>>();
            keys.Add(new KeyValuePair<string, string>("grant_type", "password"));
            keys.Add(new KeyValuePair<string, string>("username", username_api));
            keys.Add(new KeyValuePair<string, string>("password", password_api));
            request.Content = new FormUrlEncodedContent(keys);
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage responseMessage_token = await httpClient.SendAsync(request);
            return JsonConvert.DeserializeObject<TokenReferenciaPago>(await responseMessage_token.Content.ReadAsStringAsync());
        }
        public async Task<ResultadoReferenciaDto> GetReferenciaPago(GenerarReferenciaDto model)
        {
            System.Net.ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var metodoPasarela = model.isInclusion ? "pasarela/reference-payment-inc" : "pasarela/reference-payment";
            var urlGeneracionReferenciaPago = ConfigurationManager.AppSettings["UrlPasarela"].Replace("{0}", metodoPasarela);

            var objetoResultado = new ResultadoReferenciaDto();
            try
            {
                objetoResultado.IsInclusion = model.isInclusion;
                if (model.isInclusion)
                {
                    objetoResultado.ContratoValido = VerificarContrato(model.NumeroContratoInclusion);
                    if (!objetoResultado.ContratoValido)
                    {
                        objetoResultado.Exitoso = true;
                        return objetoResultado;
                    }
                }

                var objToken = await GetTokenRefPago();

                var httpClient = new HttpClient();
                HttpRequestMessage requestMessage_RefPago = new HttpRequestMessage(HttpMethod.Post, urlGeneracionReferenciaPago);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", objToken.access_token);

                string body = "";
                if (model.isInclusion)
                {
                    body = JsonConvert.SerializeObject(new
                    {
                        data = new
                        {
                            ContractNumber = model.NumeroContratoInclusion,
                            ContractorDocument = model.CedulaContratante,
                            BenficiaryDocument = model.CedulaBeneficiario
                        }
                    });
                }
                else
                {
                    body = JsonConvert.SerializeObject(new
                    {
                        data = new
                        {
                            IdentificationNumber = model.Cedula
                        }
                    });
                }

                requestMessage_RefPago.Content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage_RefPago = await httpClient.SendAsync(requestMessage_RefPago);

                if (responseMessage_RefPago.StatusCode == HttpStatusCode.OK)
                {
                    objetoResultado.Exitoso = true;
                    objetoResultado.ReferenciaPago = await responseMessage_RefPago.Content.ReadAsStringAsync();
                }
                else
                {
                    objetoResultado.Exitoso = false;
                }
            }
            catch (Exception ex)
            {
                objetoResultado.Mensaje = ex.Message;
                objetoResultado.Exitoso = false;
            }
            return objetoResultado;
        }
        public async Task<ResultadoPaymentDataDto> GetPaymentData(string refPago)
        {
            System.Net.ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var metodoPasarela = "pasarela/reference-payment-return";
            var urlPaymentData = ConfigurationManager.AppSettings["UrlPasarela"].Replace("{0}", metodoPasarela);

            var objetoResultado = new ResultadoPaymentDataDto();
            try
            {
                var objToken = await GetTokenRefPago();

                var httpClient = new HttpClient();
                HttpRequestMessage requestMessage_PaymentData = new HttpRequestMessage(HttpMethod.Post, urlPaymentData);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", objToken.access_token);

                string body = JsonConvert.SerializeObject(new { referenceNumber = refPago });

                requestMessage_PaymentData.Content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage_PaymentData = await httpClient.SendAsync(requestMessage_PaymentData);

                if (responseMessage_PaymentData.StatusCode == HttpStatusCode.OK)
                {
                    objetoResultado.Exitoso = true;
                    objetoResultado.Resultado = await responseMessage_PaymentData.Content.ReadAsStringAsync();
                }
                else
                {
                    objetoResultado.Exitoso = false;
                }
            }
            catch (Exception ex)
            {
                objetoResultado.Mensaje = ex.Message;
                objetoResultado.Exitoso = false;
            }
            return objetoResultado;
        }
    }
}