using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace KeyPortNet.Helper
{
    class IOHelper
    {

        public static void SaveDB(string path,string key, Model.DataBase db)
        {
            try
            {
                db.Attribute.LastWrite = DateTime.Now;
                db.Attribute.User = Environment.UserName;
                db.Attribute.Computer = Environment.MachineName;
                db.Attribute.LastSavePath = path;

                Crc32 crc = new Crc32();
                db.Attribute.Crc32 = BitConverter.ToString(crc.ComputeHash(ObjectToByteArray(db))).Replace("-", string.Empty).ToUpper();

                CryptoSerializer.GenericCryptoClass cc = new CryptoSerializer.GenericCryptoClass();
                cc.CryptoSerialize<Model.DataBase>(path, db, key);

                GenSerialize.XmlSerialisierung<Model.DataBase>.Serialisieren(db, path + "_");
                KeyPortNet.Properties.Settings.Default.DBPfad = path;
                KeyPortNet.Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static Model.DataBase LoadDB(string path,string key)
        {
            CryptoSerializer.GenericCryptoClass cc = new CryptoSerializer.GenericCryptoClass();
            return cc.CryptoDeSerialize<Model.DataBase>(path,key);

        }

        // Convert an object to a byte array
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
