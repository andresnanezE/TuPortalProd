namespace Transversales.Administracion.Adaptadores
{
    using AutoMapper;

    public class AdaptadorDeObjetos : IAdaptadorDeObjetos
    {
        public AdaptadorDeObjetos()
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

        #endregion
    }
}