using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace Kryptografie
{
    public class AESx
    {
        #region encrypt..

        public static byte[] AES_encrypt_block(byte[] plainText, byte[] Key)
        {
            return AES_encrypt_block(plainText, Key, CipherMode.ECB, PaddingMode.None);
        }
        
        public static byte[] AES_encrypt_block(byte[] plainText, byte[] Key,CipherMode CM)
        {
            return AES_encrypt_block(plainText, Key, CM, PaddingMode.None);
        }
        
        public static byte[] AES_encrypt_block(byte[] plainText, byte[] Key, CipherMode CM, PaddingMode PM)
        {
            byte[] output_buffer = new byte[plainText.Length];

            using (AesManaged aesAlg = new AesManaged())
            {
                //If CBC, must initialize IV = O_{128} 
                //aesAlg.Mode = CipherMode.CBC; 
                //aesAlg.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; 
                aesAlg.Mode = CM;

                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 128;
                aesAlg.Padding = PM;
                aesAlg.Key = Key;

                // Create a decrytor to perform the stream transform. 
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                encryptor.TransformBlock(plainText, 0, plainText.Length, output_buffer, 0);
            }

            return output_buffer;
        }

        #endregion

        #region decrypt..

        public static byte[] AES_Decrypt_block(byte[] cipherText, byte[] Key)
        {
            return AES_Decrypt_block(cipherText,Key, CipherMode.ECB, PaddingMode.None);
        }

        public static byte[] AES_Decrypt_block(byte[] cipherText, byte[] Key, CipherMode CM)
        {
            return AES_Decrypt_block(cipherText, Key, CM, PaddingMode.None);
        }

        public static byte[] AES_Decrypt_block(byte[] cipherText, byte[] Key, CipherMode CM, PaddingMode PM)
        {
            // Declare the string used to hold the decrypted text.  
            byte[] output_buffer = new byte[cipherText.Length];

            using (AesManaged aesAlg = new AesManaged())
            {
                //If CBC, must initialize IV = O_{128} 
                //aesAlg.Mode = CipherMode.CBC; 
                //aesAlg.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; 
                aesAlg.Mode = CM;

                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 128;
                aesAlg.Padding = PM;
                aesAlg.Key = Key;

                // Create a decrytor to perform the stream transform. 
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                decryptor.TransformBlock(cipherText, 0, cipherText.Length, output_buffer, 0);
            }

            return output_buffer;
        }

        #endregion

        public static byte[] ConvertHexPasswordToByte(string KeyHexStrg)
        {
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < KeyHexStrg.Length; i += 2)
            {
                // den Hex-Wert aus dem langen String holen..
                string sub = KeyHexStrg.Substring(i, 2);
                // Parse-Methode von byte aufrufen
                // Parameter1: Der Hex-String
                // Parameter2: Eine Angabe das es sich dabei um Hex handelt..
                bytes.Add(byte.Parse(sub, System.Globalization.NumberStyles.HexNumber));
            }

            return bytes.ToArray();
        }

    }
}