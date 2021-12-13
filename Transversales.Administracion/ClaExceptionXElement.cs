using System;
using System.Collections;
using System.Linq;
using System.Xml.Linq;

namespace Transversales.Administracion
{
    public class ClaExceptionXElement : XElement
    {
        /// <summary>Crea una instancia de ExceptionXElement.</summary>
        /// <param name="exception">La excepcion a serializar.</param>
        public ClaExceptionXElement(Exception exception)
            : this(exception, false)
        { }

        /// <summary>Crea una instancia de ExceptionXElement.</summary>
        /// <param name="exception">La excepcion a serializar..</param>
        /// <param name="omitStackTrace">
        /// Serializa una Excepcion, StackTrace puede o no ser nulo.
        /// </param>
        public ClaExceptionXElement(Exception exception, bool omitStackTrace)
            : base(new Func<XElement>(() =>
            {
                // Valida los argumentos

                if (exception == null)
                {
                    throw new ArgumentNullException("exception");
                }

                // la Raiz del elemento especifica el tipo del objeto.

                var vT = exception.GetType();
                string strVType = vT.ToString();

                XElement root = new XElement("System.ExceptionMod");

                try
                {
                    if (!string.IsNullOrEmpty(strVType))
                        strVType = Utils.RemoverCaracteresEspeciales(strVType);

                    root = new XElement(strVType);
                }
                catch
                {
                    root = new XElement("System.ExceptionMod");
                }

                if (exception.Message != null)
                {
                    root.Add(new XElement("Message", exception.Message));
                }

                // StackTrace puede ser nulo, ej:
                // new ExceptionAsXml(new Exception())

                if (!omitStackTrace && exception.StackTrace != null)
                {
                    root.Add
                    (
                        new XElement("StackTrace",
                            from frame in exception.StackTrace.Split('\n')
                            let prettierFrame = frame.Substring(6).Trim()
                            select new XElement("StackTraceDetail", prettierFrame))
                    );
                }

                // Data no sera nula.

                if (exception.Data.Count > 0)
                {
                    root.Add
                    (
                        new XElement("Data",
                            from entry in
                                exception.Data.Cast<DictionaryEntry>()
                            let key = entry.Key.ToString()
                            let value = (entry.Value == null) ?
                                "null" : entry.Value.ToString()
                            select new XElement(key, value))
                    );
                }

                // Adiciona una InnerException si existe.

                if (exception.InnerException != null)
                {
                    root.Add
                    (
                        new ClaExceptionXElement
                            (exception.InnerException, omitStackTrace)
                    );
                }

                return root;
            })())
        { }
    }
}