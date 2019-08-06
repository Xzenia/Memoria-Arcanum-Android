using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class GameData
{
    public string playerCharacterName;

    public int level;

    private static readonly string saveLocation = Application.persistentDataPath + "/GameData.dat";

    public static void SaveData(GameData data)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = File.Open(saveLocation, FileMode.Create);

        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadData()
    {
        if (GameDataExists())
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = File.Open(saveLocation, FileMode.Open);

            var gameData = (GameData)binaryFormatter.Deserialize(stream);

            stream.Close();

            return gameData;
        }
        else
        {
            Debug.LogWarning("No save file is present.");

            return null;
        }
    }

    public static void DeleteData()
    {
        if (GameDataExists())
        {
            File.Delete(saveLocation);
        }
    }

    public static bool GameDataExists()
    {
        if (File.Exists(saveLocation))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
