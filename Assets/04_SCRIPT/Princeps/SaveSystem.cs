
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer( CharacterSpecs playerSpecs, DeckManager deckManager )
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetPath();
        FileStream stream = new FileStream( path, FileMode.Create);

        PlayerData data = new PlayerData(playerSpecs, deckManager);
        // Serialize
        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Save Successfull ! ");
    }

    public static PlayerData LoadPlayer ()
    {
        string path = GetPath();

        if ( !File.Exists( path ) )
        {
            Debug.LogError("Save File not found in " + path);
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream( path, FileMode.Open);
        // Deserialize
        PlayerData data = formatter.Deserialize(fileStream) as PlayerData;
        fileStream.Close();
        Debug.Log("loading successfull");
        return data;
    }

    private static string GetPath()
    {
        return Application.persistentDataPath + "/Hiota.hio";
    }
}
