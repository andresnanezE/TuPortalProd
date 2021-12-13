// ----------------------------------------------------------------------------------------------
// <copyright file="Utility.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Transversales.Administracion
{
    public class Utility
    {
        public string Encriptar(string unHashed)
        {
            try
            {
                var x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                var data =
                System.Text.Encoding.ASCII.GetBytes(unHashed);
                data = x.ComputeHash(data);
                return data.Aggregate("", (current, item) => current + item.ToString("x2").ToLower());
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "";
            }
        }
    }

    public static class Utils
    {
        /// <summary>
        /// Retorna la conversion del objeto convertida al formato T solicitado.
        /// </summary>
        /// <typeparam name="T">Tipo de Dato a retornar</typeparam>
        /// <param name="value">valor del objeto a convertir</param>
        /// <returns></returns>
        public static T ConvertirA<T>(this object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T));
            Type t = typeof(T);

            // Get the type that was made nullable.
            Type valueType = Nullable.GetUnderlyingType(typeof(T));

            if (valueType != null)
            {
                // Nullable type.

                if (value == null)
                {
                    // you may want to do something different here.
                    return default(T);
                }
                else
                {
                    // Convert to the value type.
                    object result = Convert.ChangeType(value, valueType);

                    // Cast the value type to the nullable type.
                    return (T)result;
                }
            }
            else
            {
                // Not nullable.
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T">Especifica el tipo de dato que se requiere retorn ar</typeparam>
        /// <param name="keyName">nombre de la llave solicitada</param>
        /// <param name="throwException">Lanza la excepcion si se genera cuando se envia TRUE</param>
        /// <returns></returns>
        public static T getKey<T>(string keyName, bool throwException = false)
        {
            try
            {
                return ConfigurationManager.AppSettings[keyName].ConvertirA<T>();
            }
            catch (Exception ex)
            {
                if (!throwException)
                    return default(T);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// crea un log
        /// </summary>
        /// <param name="fuente">Método o función que genera el error</param>
        /// <param name="Aplicacion">Aplicación que genera el error</param>
        /// <param name="evento">el evento que genero el mensaje</param>
        public static void RegistrarLogWindowsOld(string fuente, string Aplicacion, Exception excepcion, EventLogEntryType tipoEvento)
        {
            try
            {
                #region log

                var mensaje = "Excepcion: " + excepcion.Message == null ? "" : excepcion.Message
                            + Environment.NewLine
                            + "Excepcion interna: " + (excepcion.InnerException == null ? "" : excepcion.InnerException.Message);

                if (!EventLog.SourceExists(Aplicacion))
                    EventLog.CreateEventSource(Aplicacion, fuente);

                Int32 idApp = getKey<int>("AppID");
                string strSource = string.Format("{0}{3}{1}{3}{2}", Aplicacion, fuente, mensaje, Environment.NewLine);

                EventLog.WriteEntry(Aplicacion, strSource, tipoEvento, idApp, 1);

                #endregion log
            }
            catch (Exception)
            {
                fuente = RemoverCaracteresEspeciales(fuente);
                GuardarLogXML(excepcion, fuente, ObtenerRutaLog(), DateTime.Now);
            }
        }

        /// <summary>
        /// crea un log en el eventViewer de windows
        /// </summary>
        public static void RegistrarLogWindows(string clase_metodo, Exception excepcion)
        {
            try
            {
                if (excepcion == null)
                    RegistrarLogInfoWindows(clase_metodo);

                string Aplicacion = getKey<string>("AppName");
                Int32 idApp = getKey<int>("AppID");
                string fuente = getKey<string>("LogSource");

                string strNewLine = Environment.NewLine;

                StringBuilder mensaje = new StringBuilder();

                mensaje.AppendFormat("<Excepcion>{0}<Detalle>{1}</Detalle>{2}", clase_metodo
                    , (excepcion.Message == null ? "" : excepcion.Message), strNewLine);
                mensaje.AppendLine();

                if (excepcion.InnerException != null)
                {
                    mensaje.AppendFormat("<Excepcion_interna>{0}</Excepcion_interna>{1}", excepcion.InnerException.Message, strNewLine);
                    mensaje.AppendLine();
                }

                if (excepcion.InnerException != null && excepcion.InnerException.InnerException != null)
                {
                    mensaje.AppendFormat("<Excepcion_interna2>{0}</Excepcion_interna2>{1}", excepcion.InnerException.InnerException.Message, strNewLine);
                    mensaje.AppendLine();
                }

                mensaje.Append("</Excepcion>");

                try
                {
                    using (EventLog eventLog = new EventLog(Aplicacion))// la variable Aplicacion tiene el nombre que va a quedar en el nodo del arbol del event viewer
                    {
                        eventLog.Source = fuente;
                        eventLog.WriteEntry(mensaje.ToString(), EventLogEntryType.Error, idApp, 1);
                    }
                }
                catch (Exception ex)
                {
                    if (getKey<bool>("registrarLogsInformativos"))
                        GuardarLogXML(ex, clase_metodo + "_exepcion0", ObtenerRutaLog(), DateTime.Now);

                    string defaultAppNameWindowsLog = getKey<string>("VisorEventosDefaultAppNameWindows");
                    if (string.IsNullOrEmpty(defaultAppNameWindowsLog))
                        defaultAppNameWindowsLog = "Application";

                    using (EventLog eventLog = new EventLog(defaultAppNameWindowsLog))// la variable Aplicacion tiene el nombre que va a quedar en el nodo del arbol del event viewer
                    {
                        eventLog.Source = fuente;
                        eventLog.WriteEntry(mensaje.ToString(), EventLogEntryType.Error, idApp, 1);
                    }
                }
            }
            catch (Exception ex2)
            {
                clase_metodo = RemoverCaracteresEspeciales(clase_metodo);
                if (getKey<bool>("registrarLogsInformativos"))
                    GuardarLogXML(ex2, clase_metodo + "_exepcion2", ObtenerRutaLog(), DateTime.Now);

                GuardarLogXML(excepcion, clase_metodo, ObtenerRutaLog(), DateTime.Now);
            }
        }

        public static void RegistrarLogInfoWindows(string mensaje, EnumTipoLog tipoLog = EnumTipoLog.informativo)
        {
            try
            {
                string Aplicacion = getKey<string>("AppName");
                Int32 idApp = getKey<int>("AppID");
                string fuente = getKey<string>("LogSource");
                bool registrarLogsInformativos = bool.Parse(getKey<string>("registrarLogsInformativos"));
                bool registrarLogsDebug = bool.Parse(getKey<string>("registrarLogsDebug"));

                try
                {
                    using (EventLog eventLog = new EventLog(Aplicacion))// la variable Aplicacion tiene el nombre que va a quedar en el nodo del arbol del event viewer
                    {
                        eventLog.Source = fuente;
                        //eventLog.WriteEntry(mensaje, EventLogEntryType.Information, idApp, 1);
                        switch (tipoLog)
                        {
                            case EnumTipoLog.informativo:
                                if (registrarLogsInformativos)
                                    eventLog.WriteEntry(mensaje, EventLogEntryType.Information, idApp, 1);
                                break;

                            case EnumTipoLog.error:
                                eventLog.WriteEntry(mensaje, EventLogEntryType.Error, idApp, 1);
                                break;

                            case EnumTipoLog.debug:
                                if (registrarLogsDebug)
                                    eventLog.WriteEntry(mensaje, EventLogEntryType.Warning, idApp, 1);
                                break;

                            case EnumTipoLog.informativoYDebug:
                                if (registrarLogsDebug || registrarLogsInformativos)
                                    eventLog.WriteEntry(mensaje, EventLogEntryType.Warning, idApp, 1);
                                break;

                            default:
                                if (registrarLogsInformativos)
                                    eventLog.WriteEntry(mensaje, EventLogEntryType.Information, idApp, 1);
                                break;
                        }
                    }
                }
                catch
                {
                    string defaultAppNameWindowsLog = getKey<string>("VisorEventosDefaultAppNameWindows");
                    if (string.IsNullOrEmpty(defaultAppNameWindowsLog))
                        defaultAppNameWindowsLog = "Application";

                    using (EventLog eventLog = new EventLog(defaultAppNameWindowsLog))// la variable Aplicacion tiene el nombre que va a quedar en el nodo del arbol del event viewer
                    {
                        eventLog.Source = fuente;
                        //eventLog.WriteEntry(mensaje, EventLogEntryType.Information, idApp, 1);
                        switch (tipoLog)
                        {
                            case EnumTipoLog.informativo:
                                if (registrarLogsInformativos)
                                    eventLog.WriteEntry(mensaje, EventLogEntryType.Information, idApp, 1);
                                break;

                            case EnumTipoLog.error:
                                eventLog.WriteEntry(mensaje, EventLogEntryType.Error, idApp, 1);
                                break;

                            case EnumTipoLog.debug:
                                if (registrarLogsDebug)
                                    eventLog.WriteEntry(mensaje, EventLogEntryType.Warning, idApp, 1);
                                break;

                            case EnumTipoLog.informativoYDebug:
                                if (registrarLogsDebug || registrarLogsInformativos)
                                    eventLog.WriteEntry(mensaje, EventLogEntryType.Warning, idApp, 1);
                                break;

                            default:
                                if (registrarLogsInformativos)
                                    eventLog.WriteEntry(mensaje, EventLogEntryType.Information, idApp, 1);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                GuardarLogXML(e, "RegistrarLogInfoWindows", ObtenerRutaLog(), DateTime.Now);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string ObtenerMetodoActualSimple(StackTrace st2 = null)
        {
            try
            {
                StackTrace st = (st2 == null ? new StackTrace() : st2);
                StackFrame sfH = st.GetFrame(1);
                StackFrame sfP = st.GetFrame(2);

                string strClaseP = ((System.Reflection.MemberInfo)(sfP.GetMethod())).DeclaringType.FullName;
                string strClaseH = ((System.Reflection.MemberInfo)(sfH.GetMethod())).DeclaringType.FullName;

                string strMetodoP = sfP.GetMethod().Name;
                string strMetodoH = sfH.GetMethod().Name;

                return strMetodoH + "-" + strClaseH;
            }
            catch (Exception)
            {
                return "MetodoNoEncontrado";
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static List<string> ObtenerMetodoActualSimpleLista(StackTrace st)
        {
            List<string> lRet = new List<string>();
            try
            {
                //StackTrace st = new StackTrace();
                StackFrame sf0 = st.GetFrame(0);
                StackFrame sfH = st.GetFrame(1);
                StackFrame sfP = st.GetFrame(2);

                lRet.Add(sf0.GetMethod().Name.ToString());

                if (st.FrameCount > 1)
                {
                    // Display the highest-level function call
                    // in the trace.
                    sf0 = st.GetFrame(st.FrameCount - 1);
                    lRet.Add("Original function call at top of call stack):");
                    lRet.Add(sf0.GetMethod().ToString());
                }

                string strClaseP = ((System.Reflection.MemberInfo)(sfP.GetMethod())).DeclaringType.FullName;
                string strClaseH = ((System.Reflection.MemberInfo)(sfH.GetMethod())).DeclaringType.FullName;

                string strMetodoP = sfP.GetMethod().Name;
                string strMetodoH = sfH.GetMethod().Name;

                return lRet;
            }
            catch (Exception)
            {
                return lRet;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string ObtenerMetodoActual(StackTrace st2 = null)
        {
            try
            {
                StackTrace st = (st2 == null ? new StackTrace() : st2);
                StackFrame sfH = st.GetFrame(1);
                StackFrame sfP = st.GetFrame(2);

                string strClaseP = ((System.Reflection.MemberInfo)(sfP.GetMethod())).DeclaringType.FullName;
                string strClaseH = ((System.Reflection.MemberInfo)(sfH.GetMethod())).DeclaringType.FullName;

                string strMetodoP = sfP.GetMethod().Name;
                string strMetodoH = sfH.GetMethod().Name;

                return "Error en el metodo: '" + strMetodoH + "' de la clase: '" + strClaseH + "' invocado por el metodo: '" + strMetodoP + "' de la clase: " + strClaseP + "'";
            }
            catch (Exception)
            {
                return "Error al evaluar el nombre del metodo";
            }
        }

        public static string ObtenerNombreAplicacion()
        {
            return getKey<string>("AppName");
        }

        public static string ObtenerRutaLog()
        {
            return getKey<string>("AppPathLog");
        }

        /// <summary>
        /// Guarda la excepcion en un archivo XML
        /// </summary>
        /// <param name="parException">Excepcion</param>
        /// <param name="parType">Tipo de Excepcion para generar archivo</param>
        /// <param name="parPath">Ruta donde se guardara el XML</param>
        /// <param name="datNow">Fecha Actual para guardar el LOG</param>
        public static void GuardarLogXML(Exception parException, string parType, string parPath, DateTime datNow)
        {
            try
            {
                if (string.IsNullOrEmpty(parPath) || parException == null || datNow == null)
                    return;

                ClaExceptionXElement varC = new ClaExceptionXElement(parException);
                string varpath = parPath;
                DateTime varNow = datNow;

                varpath = string.Format("{0}/{1}/{2}/{3}/", varpath, varNow.ToString("yyyy"), varNow.ToString("MM"), varNow.ToString("dd"));

                if (!System.IO.Directory.Exists(varpath))
                {
                    System.IO.Directory.CreateDirectory(varpath);
                }

                string dateFormatLog = "yyyyMMdd_HHmmss_fff";

                string filename = string.Empty;
                if (string.IsNullOrEmpty(parType))
                {
                    filename = varpath + varNow.ToString(dateFormatLog) + ".XML";
                }
                else
                {
                    filename = varpath + parType + "-" + varNow.ToString(dateFormatLog) + ".XML";
                }

                System.IO.File.WriteAllText(filename, varC.ToString());
            }
            catch { }
        }

        public static string RemoverCaracteresEspeciales(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            // Create  a string array and add the special characters you want to remove
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]" };
            //Iterate the number of times based on the String array length.

            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], "");
                }
            }

            return str;
        }

        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            else
                s = s.Trim();
            return (char.ToUpper(s[0])).ToString();
        }

        public static string PrimeraPalabra(string p, int v)
        {
            try
            {
                if (string.IsNullOrEmpty(p))
                {
                    return string.Empty;
                }
                else
                {
                    p = p.Trim();
                    string[] palabras = p.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    return palabras[v].ToUpper();
                }
            }
            catch (Exception)
            {
                return p;
            }
        }

        public static string CreacionNombreUsuario(string letra, string apellido, int opcion = 2)
        {
            if (string.IsNullOrWhiteSpace(letra) || string.IsNullOrWhiteSpace(apellido))
                return "";

            if (opcion == 1)
            {
                return UppercaseFirst(letra) + PrimeraPalabra(apellido, 0);
            }
            else
                return UppercaseFirst(letra) + UppercaseFirst(PrimeraPalabra(apellido, 1)) + PrimeraPalabra(apellido, 0);
        }

        /// <summary>
        /// Funcion encriptacion presente en aplicacion de mantenimiento
        /// </summary>
        /// <param name="unHashed"></param>
        /// <returns></returns>
        public static string EncriptarContrasenaMTO(string unHashed)
        {
            try
            {
                if (!Seguridad.EncriptacionAntigua())
                    return Seguridad.CalculateHashedPassword256(unHashed);

                System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] data =
                System.Text.Encoding.ASCII.GetBytes(unHashed);
                data = x.ComputeHash(data);
                string salida = "";
                foreach (byte item in data)
                {
                    salida += item.ToString("x2").ToLower();
                }
                return salida;
            }
            catch
            {
                //Alert.MessageShow("Error: " + ex.ToString());
                return "";
            }
        }

        public static bool ValidarExcepcionGlobal(Exception exG)
        {
            if (exG == null)
                return false;

            string strValuesValidate = getKey<string>("skipErrorMessagesGlobalAsax");
            if (!string.IsNullOrEmpty(strValuesValidate) && !string.IsNullOrEmpty(exG.Message))
            {
                List<string> vValidate = strValuesValidate.Split('|').ToList();
                var vG = vValidate.FindAll(s => exG.Message.ToUpperInvariant().Contains(s.ToUpperInvariant()));
                if (vG != null && vG.Count > 0)
                    return false;
            }
            return true;
        }

        public static bool ReseteoClaveVieja()
        {
            return getKey<bool>("ReseteoClaveVieja");
        }
    }
}