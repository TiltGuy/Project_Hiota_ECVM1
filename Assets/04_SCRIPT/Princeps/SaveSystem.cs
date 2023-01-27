
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public static class SaveSystem
{
    public static string TutoNameSaveFile = "TutoHiota";
    public static string MainSaveFileName = "Hiota";

    public static void SavePlayerData( PlayerData playerDataToApply)
    {
        string path = GetPath(MainSaveFileName);
        string jsonData;
        if ( File.Exists(MainSaveFileName) )
        {
            string newDataJson = CreateNewSave(playerDataToApply);

            string oldJsonData = File.ReadAllText(GetPath(MainSaveFileName));
            JsonUtility.FromJsonOverwrite(oldJsonData, playerDataToApply);
            Debug.Log("Deck Manager Save File Overwriten : " + path);
            return;
        }
        //BinaryFormatter formatter = GetFormatter();
        //fsSerializer _serializer = new fsSerializer();
        //FileStream stream = new FileStream( path, FileMode.Create);

        PlayerData data = playerDataToApply;
        // Serialize
        //formatter.Serialize(stream, data);
        jsonData = JsonUtility.ToJson(data);

        File.WriteAllText(path, jsonData);
        //stream.Close();

        Debug.Log("New Save Successfull ! " + path);
    }

    private static string CreateNewSave( DeckManager deckManager )
    {
        PlayerData newData = new PlayerData(deckManager);
        string newDataJson = JsonUtility.ToJson(newData);
        return newDataJson;
    }

    private static string CreateNewSave( PlayerData newData)
    {
        string newDataJson = JsonUtility.ToJson(newData);
        return newDataJson;
    }

    //public static void SaveTutoState(PlayerData newData)
    //{
    //    // Overwrite data of the tuto advancement
    //    if (File.Exists(MainSaveFileName))
    //    {
    //        string newDataJson = JsonUtility.ToJson(newData);
    //        string jsonData = File.ReadAllText(GetPath(MainSaveFileName));
    //        PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
    //        JsonUtility.FromJsonOverwrite(newDataJson, data);
    //    }
    //}

    public static PlayerData LoadPlayerData()
    {
        string path = GetPath(MainSaveFileName);

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
            PlayerData data = LoadPlayerData();
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
