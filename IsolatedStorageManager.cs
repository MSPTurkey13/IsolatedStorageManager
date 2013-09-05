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

        private string prefix;
        private static StorageManager _instance;
        private StorageManager() { prefix = "your_app_name_"; }
        public static StorageManager Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StorageManager();
                }
                return _instance;
            }
        }

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

        public void ClearAll()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string[] relatedfiles = isf.GetFileNames(string.Format("{0}*", prefix));
                foreach (string file in relatedfiles)
                {
                    if (isf.FileExists(file))
                    {
                        isf.DeleteFile(file);
                    }
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
                        throw new NotFoundOnIsolatedStorageException(key);
                    }

                }

            }
            catch (NotFoundOnIsolatedStorageException)
            {
                throw;
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
            return string.Format("{0}{1}.data", prefix, key);
        }

        #endregion

    }
}