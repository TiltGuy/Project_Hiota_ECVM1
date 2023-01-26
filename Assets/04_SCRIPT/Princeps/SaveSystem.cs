
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public static class SaveSystem
{
    public static string TutoNameSaveFile = "TutoHiota";
    public static string MainSaveFileName = "Hiota";
    public static void SavePlayerData( CharacterSpecs playerSpecs, DeckManager deckManager, string nameFile )
    {
        //BinaryFormatter formatter = GetFormatter();
        //fsSerializer _serializer = new fsSerializer();
        string path = GetPath(nameFile);
        //FileStream stream = new FileStream( path, FileMode.Create);

        PlayerData data = new PlayerData(playerSpecs, deckManager);
        // Serialize
        //formatter.Serialize(stream, data);
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(path, jsonData);
        //stream.Close();

        Debug.Log("Save Successfull ! " + path);
    }

    public static void SaveTutoState()
    {
        // Overwrite data of the tuto advancement
    }

    public static PlayerData LoadPlayerData( string nameFile )
    {
        string path = GetPath(nameFile);

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

    public static void ClearData(string nameFile)
    {
        string path = GetPath(nameFile);
        if ( !File.Exists(path) )
        {
            Debug.LogWarning("Save File not found in " + path);
            return;
        }

        File.Delete(path);
        Debug.Log("File Save Deleted" + path);
    }

    public static string GetPath(string nameFile)
    {
        return Application.persistentDataPath + "/" + nameFile + ".json";
    }

    public static bool CheckIfTutoPassed()
    {
        if(File.Exists(GetPath(MainSaveFileName)))
        {
            PlayerData data = LoadPlayerData(MainSaveFileName);
            return data.b_HasPassedTutorial;
        }
        return false;
    }

    public static bool CheckIfMainSaveExists()
    {
        if ( File.Exists(GetPath(MainSaveFileName)) )
        {
            return true;
        }
        return false;
    }
}
