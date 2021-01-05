using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    //Default save path
    public static string path = Path.Combine(Application.persistentDataPath, "saveinfo.sav");
    //
    public static void SaveData (GameManager game)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            DataFile data = new DataFile(game);

            formatter.Serialize(stream, data);
        }
    }

    public static DataFile LoadData()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                DataFile data = formatter.Deserialize(stream) as DataFile;

                return data;
            }
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
