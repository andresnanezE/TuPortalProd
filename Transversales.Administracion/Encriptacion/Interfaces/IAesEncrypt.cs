namespace Transversales.Administracion.Encriptacion.Interfaces
{
    public interface IAesEncrypt
    {
        string EncryptText(string input, string password);

        string DecryptText(string input, string password);
    }
}