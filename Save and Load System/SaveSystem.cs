using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    private static BinaryFormatter formatter;
    private static FileStream stream;
    private static PlayerData playerData;
    private static EffectManagerData effectManagerData;
    private static LevelControllerData levelData;
    private static BarUIManagerData barUiData;
    private static string path;

    public static void SavePlayer(Player player)
    {
        formatter = new BinaryFormatter();
        path = Application.persistentDataPath + "/Player.data";
        stream = new FileStream(path, FileMode.Create);
        playerData = new PlayerData(player);
        formatter.Serialize(stream, playerData);
        stream.Close();
        SavePlayerEffects(player.effectManager);
    }

    public static void SavePlayerEffects(EffectManager effectManager)
    {
        formatter = new BinaryFormatter();
        path = Application.persistentDataPath + "/PlayerEffects.data";
        stream = new FileStream(path, FileMode.Create);
        effectManagerData = new EffectManagerData(effectManager);
        formatter.Serialize(stream, effectManagerData);
        stream.Close();
    }

    public static void SaveLevelController(PlayerLevelController levelController)
    {
        formatter=new BinaryFormatter();
        path = Application.persistentDataPath + "/PlayerLevel.data";
        stream = new FileStream(path, FileMode.Create);
        levelData = new LevelControllerData(levelController);
        formatter.Serialize(stream, levelData);
        stream.Close();
    }

    public static void SaveBarUI(BarUIManager manager)
    {
        formatter = new BinaryFormatter();
        path = Application.persistentDataPath + "/BarUI.data";

        stream = new FileStream(path, FileMode.Create);
        barUiData = new BarUIManagerData(manager);
        formatter.Serialize(stream, barUiData);
        stream.Close();
    }
}
