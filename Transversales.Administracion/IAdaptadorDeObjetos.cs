namespace Transversales.Administracion
{
    public interface IAdaptadorDeObjetos
    {
        T Adaptar<T>(object origen) where T : class;

        void Iniciar();
    }
}