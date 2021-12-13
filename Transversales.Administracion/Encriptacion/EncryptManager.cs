using Transversales.Administracion.Encriptacion.Metodos;

namespace Transversales.Administracion.Encriptacion
{
    public class EncryptManager
    {
        private static readonly string password = "Emerm3d1caTuP0rta1";
        private AesEncrypt _servicioAes = new AesEncrypt();

        public string AesEncriptarTexto(string input)
        {
            return _servicioAes.EncryptText(input, password);
        }

        public string AesDesncriptarTexto(string input)
        {
            return _servicioAes.DecryptText(input, password);
        }
    }
}