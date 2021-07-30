using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GetIrnConsoleApplication.AppClasses
{
    public static class EncryptionUtils
    {

        public static string DecryptBySymmerticKey(string encryptedText, byte[] key)
        {
            //Emcrypting SEK
            try
            {
                byte[] dataToDecrypt = Convert.FromBase64String(encryptedText);
                var keyBytes = key;
                AesManaged tdes = new AesManaged();
                tdes.KeySize = 256;
                tdes.BlockSize = 128;
                tdes.Key = keyBytes;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform decrypt__1 = tdes.CreateDecryptor();
                byte[] deCipher = decrypt__1.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
                tdes.Clear();

                string EK_result = Convert.ToBase64String(deCipher);
                //var EK = Convert.FromBase64String(EK_result);

                // return EK;
                return EK_result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string EncryptBySymmerticKey(string text, string sek)
        {
            //Emcrypting SEK
            try
            {
                byte[] dataToEncrypt = Convert.FromBase64String(text);
                var keyBytes = Convert.FromBase64String(sek);
                AesManaged tdes = new AesManaged();
                tdes.KeySize = 256;
                tdes.BlockSize = 128;
                tdes.Key = keyBytes;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform encrypt__1 = tdes.CreateEncryptor();
                byte[] deCipher = encrypt__1.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
                tdes.Clear();

                string EK_result = Convert.ToBase64String(deCipher);
                //var EK = Convert.FromBase64String(EK_result);
                // return EK;
                return EK_result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static byte[] generateSecureKey()
        {
            Aes KEYGEN = Aes.Create();
            byte[] secretKey = KEYGEN.Key;
            //return Convert.ToBase64String(secretKey);

            return secretKey;
        }

        public static string Encrypt(string data, string key)
        {

            byte[] keyBytes =
           Convert.FromBase64String(key); // your key here

            AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
            RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;


            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned();
            rsaParameters.Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParameters);

            byte[] plaintext = Encoding.UTF8.GetBytes(data);
            byte[] ciphertext = rsa.Encrypt(plaintext, false);
            string cipherresult = Convert.ToBase64String(ciphertext);

            return cipherresult;
        }


        public static string Encrypt(byte[] data, string key)
        {

            byte[] keyBytes =
           Convert.FromBase64String(key); // your key here

            AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
            RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;


            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned();
            rsaParameters.Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParameters);

            byte[] plaintext = data;
            byte[] ciphertext = rsa.Encrypt(plaintext, false);
            string cipherresult = Convert.ToBase64String(ciphertext);

            return cipherresult;
        }

    }

}
