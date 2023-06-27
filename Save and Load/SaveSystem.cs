using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    private static BinaryFormatter formatter;
    private static FileStream stream;
    private static PlayerData playerData;
    private static string path;
    public static void SavePlayer (Player player) {
        formatter = new BinaryFormatter();
        path = Application.persistentDataPath + "/player.data";
        stream = new FileStream(path, FileMode.Create);
        playerData = new PlayerData(player);
        formatter.Serialize(stream, playerData);
        stream.Close();
    }
}
