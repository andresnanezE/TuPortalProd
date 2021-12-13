using AutoMapper;

namespace Transversales.Administracion.IoC.Adaptadores
{
    public class AdaptadorDeObjetosAplicacion : IAdaptadorDeObjetos
    {
        public AdaptadorDeObjetosAplicacion()
        {
            this.Iniciar();
        }

        #region IAdaptadorDeObjetos Members

        public T Adaptar<T>(object origen) where T : class
        {
            return Mapper.Map<T>(origen);
        }

        public void Iniciar()
        {
            Mapper.Initialize(ap =>
            {
                ap.AddProfile<AdministracionAplicacionADominio>();
                ap.AddProfile<AdministracionDominioAAplicacion>();
            });
        }

        #endregion IAdaptadorDeObjetos Members
    }
}