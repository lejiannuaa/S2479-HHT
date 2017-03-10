using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Data;
using System.IO;

namespace HolaCore
{
    //序列化接口
    public interface ISerializable
    {
        void init(object[] param);
        void Serialize(string file);
        void Deserialize(string file);
    }

    [Serializable]
    public class SerializeClass : IDisposable
    {
        private DataSet ds;
        public DataSet DS
        {
            get
            {
                return ds;
            }
            set
            {
                ds = value;
            }
        }

        private string[] data;
        public string[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
    

        public bool Serialize(string file)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);

                XmlSerializer xs = new XmlSerializer(typeof(SerializeClass));
                xs.Serialize(fileStream, this);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        static public SerializeClass Deserialize(string file)
        {
            Stream stream = null;
            SerializeClass sc = null;

            try
            {
                if (File.Exists(file))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(SerializeClass));

                    stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                    sc = xs.Deserialize(stream) as SerializeClass;

                    return sc;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        #region 构造析构
        public SerializeClass()
        {
        }

        ~SerializeClass()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
        #endregion
    }
}
