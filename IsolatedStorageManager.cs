using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace MSPTurkey13
{
    public class StorageManager
    {
        #region Singleton

        private static StorageManager _instance;
        private StorageManager() { }
        public static StorageManager Current;

        #endregion

        #region Public Methods

        public void Remove(string key)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(GetFileName(key)))
                {
                    isf.DeleteFile(key);
                }
            }
        }

        public T Get<T>(string key)
        {
            T ObjToLoad = default(T);

            try
            {

                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isf.FileExists(GetFileName(key)))
                    {
                        using (IsolatedStorageFileStream fs = isf.OpenFile(GetFileName(key), System.IO.FileMode.Open))
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(T));
                            ObjToLoad = (T)ser.Deserialize(fs);
                        }
                    }
                    else
                    {
                        // may be throw not found exception
                    }

                }

            }
            catch (Exception error)
            {
                throw new NotImplementedException(error.Message);
            }

            return ObjToLoad;
        }

        public void Save<T>(string key, T ObjectToSave)
        {
            TextWriter writer;

            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fs = isf.OpenFile(GetFileName(key), System.IO.FileMode.Create))
                {
                    writer = new StreamWriter(fs);
                    XmlSerializer ser = new XmlSerializer(typeof(T));
                    ser.Serialize(writer, ObjectToSave);
                    writer.Close();
                }
            }
        }

        #endregion

        #region Helper Methods

        private string GetFileName(string key)
        {
            return string.Format("{0}.data", key);
        }

        #endregion

    }
}