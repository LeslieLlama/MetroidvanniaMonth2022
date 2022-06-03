using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    //this is the save/load script, it doesn't store the data it just saves, loads and creates the SIN file
    //since its a binary file the format can be anything, I chose SIN 'cause i'm boring

        
    public static void SavePlayer(SaveFile playerCyn)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerCyn.SIN";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerSaveFile cynData = new PlayerSaveFile(playerCyn);

        formatter.Serialize(stream, cynData);
        stream.Close();
    }

    public static PlayerSaveFile loadPlayerCyn()
    {
        string path = Application.persistentDataPath + "/playerCyn.SIN";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerSaveFile cynData = formatter.Deserialize(stream) as PlayerSaveFile;

            stream.Close(); //do not remove this it will make a lot of errors
            return cynData;

        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }

    
}
