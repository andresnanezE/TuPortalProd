using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Transversales.Administracion
{
    public class Seguridad
    {
        private static byte[] IV = Encoding.ASCII.GetBytes("Devjoker7.37hAES");

        private const string _addSalt = "AES256-ModVers";

        public static bool EncriptacionAntigua()
        {
            return Utils.getKey<bool>("EncriptacionAntigua");
        }

        #region encripcion asincrona

        /// <summary>
        /// Crea un hash a partir de un texto. este NO ES REVERSIBLE
        /// </summary>
        /// <param name="unHashed">Texto plano sin ser tratado</param>
        /// <returns>hash generado a partir del texto proporcionado</returns>
        public static string CreateHash(string unHashed)
        {
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] data = System.Text.Encoding.ASCII.GetBytes(unHashed);
                data = x.ComputeHash(data);
                //return System.Text.Encoding.ASCII.GetString(data);
                string salida = "";
                foreach (byte item in data)
                {
                    salida += item.ToString("x2").ToLower();
                }
                return salida;
            }
            catch (Exception ex)
            {
                //General.RegistrarError(General.GetCurrentMethod(), ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// valida si el tezto ingresado es igual al un hash proporcionado
        /// </summary>
        /// <param name="hashed">hash a comparar</param>
        /// <param name="unHashed">texto sin hash</param>
        /// <returns></returns>
        public static bool MatchHash(string hashed, string unHashed)
        {
            unHashed = CreateHash(unHashed);
            if (unHashed == hashed)
                return true;
            else
                return false;
        }

        public static string CalculateHashedPassword(string cadena)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(cadena));
                return Convert.ToBase64String(computedHash);
            }
        }

        public static string CalculateHashedPassword256Old(string cadena)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(cadena));
                return Convert.ToBase64String(computedHash);
            }
        }

        public static string CalculateHashedPassword256(string cadena)
        {
            return Convert.ToBase64String(getBytesSha256(cadena));
        }

        public static string CalculateHashedPassword256UTF8(string cadena)
        {
            return Convert.ToBase64String(getBytesSha256UTF8(cadena));
        }

        public static byte[] getBytesSha256(string cadena)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(cadena));
                return computedHash;
            }
        }

        public static byte[] getBytesSha256UTF8(string cadena)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.UTF8.GetBytes(cadena));
                return computedHash;
            }
        }

        public static string CalculateHashedPassword512(string cadena)
        {
            using (var sha = SHA512.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(cadena));
                return Convert.ToBase64String(computedHash);
            }
        }

        public static String sha512_hash_salt(string value)
        {
            #region Salt

            string extraData = Encriptar(HashPassword(value), value);

            #endregion Salt

            StringBuilder Sb = new StringBuilder();

            using (var hash = HMACSHA512.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(extraData));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static string GetCrypt_SHA512(string text)
        {
            string hash = "";
            SHA512 alg = SHA512.Create();
            byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(text));
            hash = Encoding.UTF8.GetString(result);
            return hash;
        }

        #endregion encripcion asincrona

        #region encripcion sincrona (reversible)

        /// <summary>
        /// Retorna un texto encriptado a partir de un texto dado y una llave predefinida
        /// </summary>
        /// <param name="Cadena">texto a encriptar</param>
        /// <param name="clave">clave a incluir</param>
        /// <param name="validaViejo">Validacion Anterior</param>
        /// <returns></returns>
        public static string Encriptar(string Cadena, string clave, bool validaViejo = false)
        {
            if (!validaViejo)
                return Encriptarv2(Cadena, clave);

            var key = Encoding.ASCII.GetBytes(clave);
            byte[] inputBytes = Encoding.ASCII.GetBytes(Cadena);
            byte[] encripted;
            RijndaelManaged cripto = new RijndaelManaged();

            using (MemoryStream ms = new MemoryStream(inputBytes.Length))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(key, IV), CryptoStreamMode.Write))
                {
                    objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
                    objCryptoStream.FlushFinalBlock();
                    objCryptoStream.Close();
                }
                encripted = ms.ToArray();
            }

            return Convert.ToBase64String(encripted);
        }

        /// <summary>
        /// Retorna un texto encriptado a partir de un texto dado y una llave predefinida. AES256
        /// </summary>
        /// <param name="Cadena">texto a encriptar</param>
        /// <param name="strClave">Clave de la cadena a Encriptar</param>
        /// <returns>String, Cadena Encriptada</returns>
        public static string Encriptarv2(string Cadena, string strClave)
        {
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(Cadena);

            StringBuilder sb = new StringBuilder();
            //sb.Append(strClave);
            //sb.Append(string.Format("{0}|{1}", strClave, _addSalt));

            //Genera Salt, Random Data basado en strClave
            StringBuilder _sbSalt = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                //_sbSalt.Append("," + sb.Length.ToString());
                _sbSalt.AppendFormat("{0},{1}", sb.Length.ToString(), sb.Length - i);
            }
            byte[] Salt = Encoding.ASCII.GetBytes(_sbSalt.ToString());

            //Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(sb.ToString(), Salt, 10000);
            Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(strClave, 10000);
            RijndaelManaged _RijndaelManaged = new RijndaelManaged();
            _RijndaelManaged.BlockSize = 256;

            byte[] key = pwdGen.GetBytes(_RijndaelManaged.KeySize / 8);   //256 bits key
            byte[] iv = pwdGen.GetBytes(_RijndaelManaged.BlockSize / 8);  //256 bits IV

            _RijndaelManaged.Key = key;
            _RijndaelManaged.IV = iv;

            //encriptar
            byte[] cipherText2 = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, _RijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(PlainText, 0, PlainText.Length);
                }
                cipherText2 = ms.ToArray();
            }
            return Convert.ToBase64String(cipherText2);
        }

        /// <summary>
        /// Desencripta la cadena dada a partir de una llaves
        /// </summary>
        /// <param name="Cadena">cadena desencriptada</param>
        /// <returns></returns>
        public static string Desencripta(string Cadena, string clave, bool validaViejo = false)
        {
            if (!validaViejo)
                return Desencriptav2(Cadena, clave);

            var _key = Encoding.ASCII.GetBytes(clave);
            var d = Convert.FromBase64String(Cadena);
            byte[] inputBytes = Convert.FromBase64String(Cadena);

            byte[] resultBytes = new byte[inputBytes.Length];
            string textoLimpio = String.Empty;
            RijndaelManaged cripto = new RijndaelManaged();

            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(_key, IV), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(objCryptoStream, true))
                    {
                        textoLimpio = sr.ReadToEnd();
                    }
                }
            }
            return textoLimpio;
        }

        /// <summary>
        /// Desencripta la cadena dada a partir de una llaves. AES256
        /// </summary>
        /// <param name="cadena">cadena a desencriptar</param>
        /// <param name="clave">Clave de la cadena a desencriptar</param>
        /// <returns>String, Cadena desencriptada</returns>
        public static string Desencriptav2(string cadena, string clave)
        {
            byte[] cipherText2 = Convert.FromBase64String(cadena);
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0}|{1}", clave, _addSalt));

            //Genera Salt, Random Data basado en strClave
            StringBuilder _sbSalt = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                _sbSalt.AppendFormat("{0},{1}", sb.Length.ToString(), sb.Length - i);
            }
            byte[] Salt = Encoding.ASCII.GetBytes(_sbSalt.ToString());

            //Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(sb.ToString(), Salt, 10000);
            Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(clave, 10000);

            RijndaelManaged _RijndaelManaged = new RijndaelManaged();
            _RijndaelManaged.BlockSize = 256; //Increase it to 256 bits- more secure

            byte[] key = pwdGen.GetBytes(_RijndaelManaged.KeySize / 8);   //256 bits key
            byte[] iv = pwdGen.GetBytes(_RijndaelManaged.BlockSize / 8);  //256 bits IV

            _RijndaelManaged.Key = key;
            _RijndaelManaged.IV = iv;

            //Now decrypt
            byte[] plainText2 = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, _RijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherText2, 0, cipherText2.Length);
                }
                plainText2 = ms.ToArray();
            }
            //Retornar desencriptado
            return System.Text.Encoding.Unicode.GetString(plainText2);
        }

        #endregion encripcion sincrona (reversible)

        #region salt methods

        public const int SaltByteSize = 24;
        public const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash
        public const int Pbkdf2Iterations = 1000;
        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;

        /// <summary>
        /// password-based key derivation functionality
        /// </summary>
        /// <param name="password">Clave</param>
        /// <param name="salt"></param>
        /// <param name="iterations">iteraciones</param>
        /// <param name="outputBytes">Bytes de salida</param>
        /// <returns></returns>
        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)

        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }

        /// <summary>
        /// Random Number Generator Salt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);
            return string.Format("{0}:{1}:{2}", Pbkdf2Iterations, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        private static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        #endregion salt methods
    }
}