
using UnityEngine;
using System.IO;
using FullSerializer;
using System;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public static class SaveSystem
{
    public static void SavePlayerData( CharacterSpecs playerSpecs, DeckManager deckManager )
    {
        BinaryFormatter formatter = GetFormatter();
        //fsSerializer _serializer = new fsSerializer();
        string path = GetPath();
        FileStream stream = new FileStream( path, FileMode.Create);

        PlayerData data = new PlayerData(playerSpecs, deckManager);
        // Serialize
        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Save Successfull ! ");
    }

    public static PlayerData LoadPlayerData ()
    {
        string path = GetPath();

        if ( !File.Exists( path ) )
        {
            Debug.LogWarning("Save File not found in " + path);
            return null;
        }

        FileStream fileStream = File.Open(path, FileMode.Open);

        BinaryFormatter formatter = GetFormatter();

        try
        {
            PlayerData data = formatter.Deserialize(fileStream) as PlayerData;
            fileStream.Close();
            Debug.Log("loading successfull");
            return data;
        }
        catch
        {
            Debug.LogWarningFormat("Failed to load File SAve", path);
            fileStream.Close();
            return null;
        }
    }

    private static BinaryFormatter GetFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter( );
        return formatter;
    }

    public static string GetPath()
    {
        return Application.persistentDataPath + "/player.hio";
    }
}
