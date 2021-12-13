using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Presentacion.Mvc.App.Utilidades
{
    public class Utils
    {
       

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Especifica el tipo de dato que se requiere retorn ar</typeparam>
        /// <param name="keyName">nombre de la llave solicitada</param>
        /// <returns></returns>
        public static T getKey<T>(string keyName)
        {
            return ConfigurationManager.AppSettings[keyName].ConvertirA<T>();
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
                #endregion
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
                string Aplicacion = getKey<string>("AppName");
                Int32 idApp = getKey<int>("AppID");
                string fuente = getKey<string>("LogSource");

                var mensaje = "Excepcion en: " + clase_metodo
                    + Environment.NewLine + "Detalle:" + (excepcion.Message == null ? "" : excepcion.Message);

                if (excepcion.InnerException != null)
                {
                    mensaje += Environment.NewLine
                        + "Excepcion interna: " + excepcion.InnerException.Message;
                }
                if (excepcion.InnerException != null && excepcion.InnerException.InnerException != null)
                {
                    mensaje += Environment.NewLine
                        + "Excepcion interna2: " + excepcion.InnerException.InnerException.Message;
                }

                using (EventLog eventLog = new EventLog(Aplicacion))// la variable Aplicacion tiene el nombre que va a quedar en el nodo del arbol del event viewer
                {
                    eventLog.Source = fuente;
                    eventLog.WriteEntry(mensaje, EventLogEntryType.Error, idApp, 1);
                }
            }
            catch (Exception e)
            {
                clase_metodo = RemoverCaracteresEspeciales(clase_metodo);
                GuardarLogXML(excepcion, clase_metodo, ObtenerRutaLog(), DateTime.Now);
            }
        }

        public static void RegistrarLogInfoWindows(string mensaje)
        {
            try
            {
                string Aplicacion = getKey<string>("AppName");
                Int32 idApp = getKey<int>("AppID");
                string fuente = getKey<string>("LogSource");
                bool registrarLogsInformativos = bool.Parse(getKey<string>("registrarLogsInformativos"));
                if (registrarLogsInformativos)
                {
                    using (EventLog eventLog = new EventLog(Aplicacion))// la variable Aplicacion tiene el nombre que va a quedar en el nodo del arbol del event viewer
                    {
                        eventLog.Source = fuente;
                        eventLog.WriteEntry(mensaje, EventLogEntryType.Information, idApp, 1);
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
    }
}