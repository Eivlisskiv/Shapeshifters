using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace IgnitedBox.SaveData
{
    [Serializable]
    public abstract class SavedEntity
    {
        private static readonly string Path = Application.persistentDataPath
            + "/s_v/";

        public static T Load<T>() where T : SavedEntity
        {
            Type t = typeof(T);
            string path = Path + t.Name;
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(path, FileMode.Open))
                    return (T)formatter.Deserialize(stream);
            }

            return (T)Activator.CreateInstance(t);
        }

        public virtual void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(Path + GetType().Name, FileMode.CreateNew))
                formatter.Serialize(stream, this);
        }

        protected virtual void OnLoad() { }
    }
}
