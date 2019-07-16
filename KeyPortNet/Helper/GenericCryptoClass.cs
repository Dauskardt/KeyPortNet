using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CryptoSerializer 
{
    /// <summary>
    /// Mithilfe dieser Generischen Klasse ist es Möglich, ein Object auf einen Datenträger verschlüsselt zu Serialisieren und 
    /// natürlich auch wieder entschlüsselt zu Deserialisieren.
    /// </summary>
    /// <example>
    /// <code lang="C#">
    ///     ToolSet.Crypto.GenericCryptoClass gcc = new ToolSet.Crypto.GenericCryptoClass();
    ///
    ///     gcc.CryptoSerialize<DataSet>(@"d:\ttest.bin", (DataSet)dataGridView1.DataSource);
    ///     dataGridView2.DataSource = gcc.CryptoDeSerialize<DataSet>(@"d:\ttest.bin");
    ///     dataGridView2.DataMember = "table";
    /// </code>
    /// </example>
    public class GenericCryptoClass {

        /// <summary>
        /// Ein Default Key, dieser wird immer benutzt falls keiner angegeben wird.
        /// </summary>
        private const String DEFAULTKEY = "o[cc_IQETy";

        /// <summary>
        /// Ein Default initialisierungsvektor,dieser wird immer benutzt falls keiner angegeben wird.
        /// </summary>
        private const String DEFAULTIV = @"jOU\9_JpWt4";

        /// <summary>
        /// Initialisiert eine neue GenericCryptoClass Klasse
        /// </summary>
        public GenericCryptoClass() { }

        /// <summary>
        /// Erzeugt eine verschluesselts, Serialisiertes Abbild der jeweiligen Klassen auf einem Datenträger
        /// </summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte abbild erezugt werden soll</param>
        /// <param name="toSerialize">Das Object, das Serialisiert werden soll</param>
        public void CryptoSerialize<T>(String path, T toSerialize) {
            CryptoSerialize(path, toSerialize, DEFAULTKEY);
        }

        /// <summary>
        /// Erzeugt eine verschluesselts, Serialisiertes Abbild der jeweiligen Klassen auf einem Datenträger
        /// </summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte abbild erezugt werden soll</param>
        /// <param name="toSerialize">Das Object, das Serialisiert werden soll</param>
        /// <param name="key">Der Schlüssel, mit dem das Abbild verschlüsselt werden soll</param>
        public void CryptoSerialize<T>(String path, T toSerialize, string key) {
            CryptoSerialize(path, toSerialize, key, DEFAULTIV);
        }

        /// <summary>
        /// Erzeugt eine verschluesselts, Serialisiertes Abbild der jeweiligen Klassen auf einem Datenträger
        /// </summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte abbild erezugt werden soll</param>
        /// <param name="toSerialize">Das Object, das Serialisiert werden soll</param>
        /// <param name="key">Der Schlüssel, mit dem das Abbild verschlüsselt werden soll</param>
        /// <param name="iv">Der Initialisierungsverktor, für die Initialisierung der Verschlüsselung</param>
        public void CryptoSerialize<T>(String path, T toSerialize, string key, string iv) {
            XmlSerializer xmlSerializer = null;
            CryptoStream cryptoStream = null;
            FileStream fileStream = null;

            try {
                fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                cryptoStream = new CryptoStream(fileStream, CryptoTransformer(key, iv).CreateEncryptor(), CryptoStreamMode.Write);

                xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(cryptoStream, toSerialize);

                fileStream.Flush();
                cryptoStream.Flush();
            } finally {
                cryptoStream.Close();
                fileStream.Close();
            }
        }

        /// <summary>
        /// Deserialisiert ein verschlüsseltes, serialisiertes Abbild einer Klasse von einem Datenträger
        /// </summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte abbild erezugt werden soll</param>
        /// <returns>Das Deserialisiert Object</returns>
        public T CryptoDeSerialize<T>(String path) { return CryptoDeSerialize<T>(path, DEFAULTKEY); }

        /// <summary>
        /// Deserialisiert ein verschlüsseltes, serialisiertes Abbild einer Klasse von einem Datenträger
        /// </summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte abbild erezugt werden soll</param>
        /// <param name="key">Der Schlüssel, mit dem das Abbild verschlüsselt werden soll</param>
        /// <returns>Das Deserialisiert Object</returns>
        public T CryptoDeSerialize<T>(String path, string key) { return CryptoDeSerialize<T>(path, key, DEFAULTIV); }

        /// <summary>
        /// Deserialisiert ein verschlüsseltes, serialisiertes Abbild einer Klasse von einem Datenträger
        /// </summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte abbild erezugt werden soll</param>
        /// <param name="key">Der Schlüssel, mit dem das Abbild verschlüsselt werden soll</param>
        /// <param name="iv">Der Initialisierungsverktor, für die Initialisierung der Verschlüsselung</param>
        /// <returns>Das Deserialisiert Object</returns>
        public T CryptoDeSerialize<T>(String path, string key, string iv) {
            XmlSerializer xmlSerializer = null;
            FileStream fileStream = null;
            CryptoStream cryptoStream = null;

            try {
                if(!File.Exists(path)) {
                    throw new FileNotFoundException(String.Format("Die Datei, {0} konnte nicht geöffnet werden", path));
                }

                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                cryptoStream = new CryptoStream(fileStream, CryptoTransformer(key, iv).CreateDecryptor(), CryptoStreamMode.Read);
                xmlSerializer = new XmlSerializer(typeof(T));

                return (T)xmlSerializer.Deserialize(cryptoStream);
            } finally {
                fileStream.Close();
            }
        }

        /// <summary>
        /// Erzeugt einen einfachen Rijndael CryptoTransformer
        /// </summary>
        /// <param name="key">Der Schlüssel, mit dem das Abbild verschlüsselt werden soll</param>
        /// <param name="iv">Der Initialisierungsverktor, für die Initialisierung der Verschlüsselung</param>
        /// <returns>Ein Rijndael CryptoTransformer</returns>
        private Rijndael CryptoTransformer(string key, string iv) {
            Rijndael cryptoTransformer = Rijndael.Create();
            cryptoTransformer.Key = ASCIIEncoding.ASCII.GetBytes(GetPaddedString(key));
            cryptoTransformer.IV = ASCIIEncoding.ASCII.GetBytes(GetPaddedString(iv));
            cryptoTransformer.Padding = PaddingMode.Zeros;

            return cryptoTransformer;
        }

        /// <summary>
        /// Diese Methode, verstärkt noch ein wenig die Verschlüsselung.
        /// </summary>
        /// <param name="val">ein Wert der gepaddet werden soll</param>
        /// <returns>ein gepaddeter String</returns>
        private string GetPaddedString(string val) {
            //je nach länge wird entweder vorn oder hinten und mit unterschiedlichen Zeichen gepaddet

            if(val.Length % 2 == 0)
                return val.PadLeft(16, (char)(val.Length + 64));
            else
                return val.PadRight(16, (char)(90 - val.Length));
        }
    }
}
