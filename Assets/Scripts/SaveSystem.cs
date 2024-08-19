using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
    public static void Save(int coinCount, int bestScore)
    {
        BinaryFormatter bf = new BinaryFormatter();
        GameData gameData = new GameData(coinCount, bestScore);

        FileStream fs = new FileStream(GetPath(), FileMode.Create);

        bf.Serialize(fs, gameData);

        fs.Close();

    }

    public static GameData Load()
    {
        if (File.Exists(GetPath()))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(GetPath(), FileMode.Open);
            GameData gameData =  bf.Deserialize(fs) as GameData;
            fs.Close();
            return gameData;
        }
        else
        {
            GameData gameData = new GameData();
            return gameData;
        }
    }

    static string GetPath()
    {
        return Application.persistentDataPath + "game.sav";
    }
}
