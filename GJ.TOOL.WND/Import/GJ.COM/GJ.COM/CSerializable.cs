using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace GJ.COM
{
    /// <summary>
    /// 序列化
    /// </summary>
    public class CSerializable<T>
    {
        private delegate void WriteHanler(string fileName, T sender);
        private delegate void ReadHanler(string fileName, ref T sender);
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sender"></param>
        public static void Write(string fileName, T sender)
        {
            string fileType = Path.GetExtension(fileName).ToLower();
            if (fileType == ".xml")
            {
                WriteHanler OnWrite = new WriteHanler(WriteXml);
                OnWrite(fileName, sender);
            }
            else
            {
                WriteHanler OnWrite = new WriteHanler(WriteBinary);
                OnWrite(fileName, sender);
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sender"></param>
        public static void Read(string fileName, ref T sender)
        {
            string fileType = Path.GetExtension(fileName).ToLower();
            if (fileType == ".xml")
            {
                ReadHanler OnRead = new ReadHanler(ReadXml);
                OnRead(fileName, ref sender);
            }
            else
            {
                ReadHanler OnRead = new ReadHanler(ReadBinary);
                OnRead(fileName, ref sender);
            }
        }
        /// <summary>
        /// 序列化二进制流
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sender"></param>
        protected static void WriteBinary(string fileName, T sender)
        {
            string folderName = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName); 
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, sender);
            stream.Close();
        }
        /// <summary>
        /// 反序列化二进制流
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sender"></param>
        protected static void ReadBinary(string fileName, ref T sender)
        {
            if (File.Exists(fileName))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                sender = (T)formatter.Deserialize(stream);
                stream.Close();
            }
        }
        /// <summary>
        /// 序列化xml
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sender"></param>
        public static void WriteXml(string fileName, T sender)
        {
            string folderName = Path.GetDirectoryName(fileName);
            if(!Directory.Exists(folderName))
               Directory.CreateDirectory(folderName); 
            XmlSerializer formatter = new XmlSerializer(sender.GetType());
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, sender);
            stream.Close();
        }
        /// <summary>
        /// 反序列化xml
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sender"></param>
        public static void ReadXml(string fileName, ref T sender)
        {
            if (File.Exists(fileName))
            {
                XmlSerializer formatter = new XmlSerializer(sender.GetType());
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                sender = (T)formatter.Deserialize(stream);
                stream.Close();
            }
        }
        /// <summary>
        /// 对象序列转化为字节
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] GetBinaryFormatObject(object msg)
        {
            byte[] binaryDataResult = null;
            MemoryStream memStream = new MemoryStream();
            IFormatter brFormatter = new BinaryFormatter();
            brFormatter.Serialize(memStream, msg);
            binaryDataResult = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return binaryDataResult;
        }
        /// <summary>
        /// 字节反序列为对象
        /// </summary>
        /// <param name="binaryData"></param>
        /// <returns></returns>
        public static object RetrieveObject(byte[] binaryData)
        {
            MemoryStream memStream = new MemoryStream(binaryData);
            IFormatter brFormatter = new BinaryFormatter();
            Object obj = brFormatter.Deserialize(memStream);
            return obj;
        }
    }

}
