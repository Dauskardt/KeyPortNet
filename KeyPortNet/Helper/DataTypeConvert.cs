using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konvertierung
{
    class DataTypeConvert
    {
        public static byte[] HexToBytes(string input)
        {
            byte[] result = new byte[input.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(input.Substring(2 * i, 2), 16);
            }
            return result;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        /// <summary>
        /// String in ByteArray umwandeln.
        /// </summary>
        /// <param name="str">Text</param>
        /// <returns>byte[] aus Text</returns>
        public static byte[] StrToByteArray(string str)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }

        /// <summary>
        /// Exclusive OR (XOR)
        /// </summary>
        /// <param name="key">Erstes byte[]</param>
        /// <param name="PAN">Zweites byte[]</param>
        /// <returns>XOR-Ergebnis</returns>
        public static byte[] exclusiveOR(byte[] key, byte[] PAN)
        {
            if (key.Length == PAN.Length)
            {
                byte[] result = new byte[key.Length];
                for (int i = 0; i < key.Length; i++)
                {
                    result[i] = (byte)(key[i] ^ PAN[i]);
                }
                string hex = BitConverter.ToString(result).Replace("-", "");

                return StrToByteArray(hex);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
