
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public static class SaveSystem
{
    public static void SavePlayerData( CharacterSpecs playerSpecs, DeckManager deckManager )
    {
        //BinaryFormatter formatter = GetFormatter();
        //fsSerializer _serializer = new fsSerializer();
        string path = GetPath();
        //FileStream stream = new FileStream( path, FileMode.Create);

        PlayerData data = new PlayerData(playerSpecs, deckManager);
        // Serialize
        //formatter.Serialize(stream, data);
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(path, jsonData);
        //stream.Close();

        Debug.Log("Save Successfull ! " + path);
    }

    public static PlayerData LoadPlayerData()
    {
        string path = GetPath();

        if ( !File.Exists(path) )
        {
            Debug.LogWarning("Save File not found in " + path);
            return null;
        }
        try
        {
            string jsonData = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
            Debug.Log("loading successfull");
            return data;
        }
        catch
        {
            Debug.LogWarningFormat("Failed to load File SAve" + path);
            return null;
        }
    }

    //private static BinaryFormatter GetFormatter()
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    return formatter;
    //}

    public static string GetPath()
    {
        return Application.persistentDataPath + "/player.json";
    }
}
