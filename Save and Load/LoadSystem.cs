using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadSystem
{
    private static BinaryFormatter formatter;
    private static FileStream stream;
    private static PlayerData playerData;
    public static PlayerData LoadPlayer () {
        string path = Application.persistentDataPath + "/player.data";
        if(File.Exists(path)){
            formatter = new BinaryFormatter();
            stream = new FileStream(path, FileMode.Open);
            playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return playerData;
        }
        else{
            return null;
        }
    }
}
