using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

namespace GenSerialize
{
        /// <summary>
        /// Generische Implementierung einer XML-Serialisation
        /// </summary>
        /// <typeparam name="T">
        /// generischer Typ
        /// </typeparam>
        public static class XmlSerialisierung<T> where T : class
        {
            /// <summary>
            /// Serialisiert das Objekt
            /// </summary>
            /// <param name="obj">
            /// zu serialisierendes Objekt
            /// </param>
            /// <param name="file">
            /// XML-Datei in die serialisiert wird
            /// </param>
            public static void Serialisieren(T obj, string file)
            {
                try
                {
                    XmlSerializer xmlSer = new XmlSerializer(typeof(T));

                    using (FileStream fs = new FileStream(file, FileMode.Create))
                    {
                        xmlSer.Serialize(fs, obj);
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine(ex.Message.ToString(),"IO-Fehler (Serialisierung)");
                }
                
            }

            /// <summary>
            /// Deserialisiert das Objekt
            /// </summary>
            /// <param name="file">
            /// XML-Datei
            /// </param>
            /// <returns>
            /// deserialisiertes Objekt
            /// </returns>
            public static T Deserialisieren(string file)
            {
                try
                {
                XmlSerializer xmlSer = new XmlSerializer(typeof(T));

                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    return xmlSer.Deserialize(fs) as T;
                }
                }
                catch (Exception ex)
                {
                    string IE = "";
                    if (ex.InnerException != null)
                    {
                        IE = "\r\n\r\n" + ex.InnerException.ToString();
                    }
                    Console.WriteLine("Message: " + ex.Message.ToString() + IE, "IO-Fehler (Serialisierung)");
                    return null;
                }
            }

            public static T Deserialize(
            string file, ProgressChangedEventHandler callback)
            {
                using(FileStream stream = new FileStream(file, FileMode.Open))
                {
                    if (stream == null) throw new ArgumentNullException("stream");

                    using (ReadProgressStream cs = new ReadProgressStream(stream))
                    {
                        cs.ProgressChanged += callback;

                        const int defaultBufferSize = 4096;
                        int onePercentSize = (int)Math.Ceiling(stream.Length / 100.0);

                        using (BufferedStream bs = new BufferedStream(cs, 
                            onePercentSize > defaultBufferSize ? 
                            defaultBufferSize : onePercentSize))
                        {

                            XmlSerializer xmlSer = new XmlSerializer(typeof(T));
                            return (T)xmlSer.Deserialize(bs);
                            
                            //BinaryFormatter formatter = new BinaryFormatter();
                            //return (T)formatter.Deserialize(bs);
                        }
                    }
                }

            }
        }
        
        /// <summary>
        /// Generische Implementierung einer BIN-Serialisation
        /// </summary>
        /// <typeparam name="T">
        /// generischer Typ
        /// </typeparam>
        public static class BinSerialisierung<T> where T : class
        {
            /// <summary>
            /// Serialisiert das Objekt
            /// </summary>
            /// <param name="obj">
            /// zu serialisierendes Objekt
            /// </param>
            /// <param name="file">
            /// BIN-Datei in die serialisiert wird
            /// </param>
            public static void Serialisieren(T obj, string file)
            {
                FileStream myStream;
                myStream = new FileStream(@file, FileMode.Create);
                BinaryFormatter binFormatter = new BinaryFormatter();
                binFormatter.Serialize(myStream, obj);
                myStream.Close();
            }

            /// <summary>
            /// Deserialisiert das Objekt
            /// </summary>
            /// <param name="file">
            /// BIN-Datei
            /// </param>
            /// <returns>
            /// deserialisiertes Objekt
            /// </returns>
            public static T Deserialisieren(string file)
            {
                //object oldObj;
                BinaryFormatter binFormatter = new BinaryFormatter();
                FileStream fs = new FileStream(@file, FileMode.Open);
                T data = (T)binFormatter.Deserialize(fs);
                fs.Close();
                return data;
            }
        }

        /// <summary>
        /// Generische Implementierung einer komprimierten BIN-Serialisation
        /// </summary>
        /// <typeparam name="T">
        /// generischer Typ
        /// </typeparam>
        public static class CompSerialisierung<T> where T : class
        {
            /// <summary>
            /// Serialisiert das Objekt
            /// </summary>
            /// <param name="obj">
            /// zu serialisierendes Objekt
            /// </param>
            /// <param name="file">
            /// BIN-Datei in die serialisiert wird
            /// </param>
            public static void Serialisieren(T obj, string file)
            {
                DirectoryInfo di = new FileInfo(file).Directory;
                if (obj != null && di != null && di.Exists)
                {
                    using (FileStream fs = new FileStream(file, FileMode.Create))
                    {
                        using (GZipStream zip = new GZipStream(fs, CompressionMode.Compress))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            bf.Serialize(zip, obj);
                        }
                    }
                }
            }

            /// <summary>
            /// Deserialisiert das Objekt
            /// </summary>
            /// <param name="file">
            /// BIN-Datei
            /// </param>
            /// <returns>
            /// deserialisiertes Objekt
            /// </returns>
            public static T Deserialisieren(string file)
            {
                if (file != null && File.Exists(file))
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                        using (GZipStream zip = new GZipStream(fs, CompressionMode.Decompress))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            return (T)bf.Deserialize(zip);
                        }
                    }
                }
                return default(T);
            }
        
        
        }


        public abstract class ContainerStream : Stream
        {
            private Stream _stream;

            protected ContainerStream(Stream stream)
            {
                if (stream == null) throw new ArgumentNullException("stream");
                _stream = stream;
            }

            protected Stream ContainedStream { get { return _stream; } }

            public override bool CanRead { get { return _stream.CanRead; } }

            public override bool CanSeek { get { return _stream.CanSeek; } }

            public override bool CanWrite { get { return _stream.CanWrite; } }

            public override void Flush() { _stream.Flush(); }

            public override long Length { get { return _stream.Length; } }

            public override long Position
            {
               get { return _stream.Position; }
               set { _stream.Position = value; }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _stream.Read(buffer, offset, count);
            }


        }

        public class ReadProgressStream : ContainerStream
{
    private int _lastProgress = 0;

    public ReadProgressStream(Stream stream) : base(stream) 
    {
        if (!stream.CanRead || !stream.CanSeek || stream.Length <= 0) 
            throw new ArgumentException("stream");
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int amountRead = base.Read(buffer, offset, count);
        if (ProgressChanged != null)
        {
            int newProgress = (int)(Position * 100.0 / Length);
            if (newProgress > _lastProgress)
            {
                _lastProgress = newProgress;
                ProgressChanged(this, 
                    new ProgressChangedEventArgs(_lastProgress, null));
            }
        }
        return amountRead;
    }

    public event ProgressChangedEventHandler ProgressChanged;

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }
}




}
