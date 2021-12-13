using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentacion.Mvc.App.Utilidades
{
    public static class Extensiones
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
    }
}