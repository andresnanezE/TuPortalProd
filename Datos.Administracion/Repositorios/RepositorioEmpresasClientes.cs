using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioEmpresasClientes : IRepositorioEmpresasClientes
    {
        public string ObtenerNombreClienteXNIT(decimal auxNIT)
        {
            using (var bdCotizacion = new ContextoPortal())
            {
                //var lstCotiz = (from u in bdCotizacion.Cotizaciones select u).ToList();

                var nombreCliente = bdCotizacion.Database.SqlQuery<string>("EMSP_OBTENERNOMBRECLIENTE @NIT",
                    new SqlParameter("@NIT", auxNIT)).FirstOrDefault();
                //var nombreCliente = (from a in lstCotiz
                //                     where a.NIT == auxNIT
                //                     select a.nombreEmpresa).First();
                if (nombreCliente != null)
                    return nombreCliente;

                return null;
            }
        }

        public EMSP_EmpresasCliente ObtenerNombreXNIT(decimal NIT)
        {
            int DV = 0;
            bool existe = false;
            var strNit = NIT.ToString();
            using (var bdStone = new ContextoStone())
            {
                var decNit = NIT;
                var lstClients = (from u in bdStone.GN_TERCE where u.COD_TERC == decNit || u.NUM_IDEN == strNit select u).ToList();
                //var client = lstClients.Find(s => s.COD_TERC == decNit || s.NUM_IDEN.Equals(NIT.ToString()));

                var client = (lstClients != null && lstClients.Count > 0 ? lstClients.First() : null);
                if (client != null)
                {
                    existe = true;
                    return new EMSP_EmpresasCliente()
                    {
                        DIG_VERI = Convert.ToInt32(client.DIG_VERI),
                        NOM_TERC = client.NOM_COMP
                    };
                }
                if (existe == false)
                {
                    DV = calcularDigitoValidacion(NIT);
                    if (DV != -1)
                    {
                        return new EMSP_EmpresasCliente()
                        {
                            DIG_VERI = DV,
                            NOM_TERC = null
                        };
                    }
                    else
                    {
                        return new EMSP_EmpresasCliente()
                        {
                            DIG_VERI = -1,
                            NOM_TERC = null
                        };
                    }
                }
            }

            return null;
        }

        public int calcularDigitoValidacion(decimal NIT)
        {
            var vpri = new int[16];
            var x = 0;
            string y = null;
            int z = 0;
            int modu = 0;
            string auxNIT = Convert.ToString(NIT);
            if (auxNIT.Length <= 9)
            {
                int tama = auxNIT.Length;

                vpri[1] = 3;
                vpri[2] = 7;
                vpri[3] = 13;
                vpri[4] = 17;
                vpri[5] = 19;
                vpri[6] = 23;
                vpri[7] = 29;
                vpri[8] = 37;
                vpri[9] = 41;
                vpri[10] = 43;
                vpri[11] = 47;
                vpri[12] = 53;
                vpri[13] = 59;
                vpri[14] = 67;
                vpri[15] = 71;

                for (int i = 0; i < tama; i++)
                {
                    y = (auxNIT.Substring(i, 1));
                    z = Convert.ToInt32(y);
                    x += (z * vpri[tama - i]);
                }
                modu = x % 11;
                return (modu > 1) ? 11 - modu : modu;
            }

            return -1;
        }
    }
}