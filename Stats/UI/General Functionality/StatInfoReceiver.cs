using TMPro;
using UnityEngine.UI;

public class StatInfoReceiver : StatsUIControl
{
    //Config Params
    public StatsUIStatTypes statType;
    PlayerLevelController levelController;

    protected override void Start()
    {
        base.Start();
        levelController = PlayerLevelController.Instance;
    }
    //Cached Fields
    private string randomText = "abcytzhzxcnasdkasnk1239214u893hsjandsandknxzcjnı";

    public override void Load()
    {

        base.Start();
        //later on,damage will be updated according to the weapon the player is holding
        //If it's a physical weapon then generalPhysicalDamage field,if it's a magical weapon
        //magicalDamage field will be retrieved.
        //For now it's just assumed it's a physical weapon which is the most likely       

        string text = (statType == StatsUIStatTypes.Strength) ? stats.allStats.strength.ToString() :
        (statType == StatsUIStatTypes.Vitality) ? stats.allStats.vitality.ToString() :
        (statType == StatsUIStatTypes.Focus) ? stats.allStats.focus.ToString() :
        (statType == StatsUIStatTypes.Dexterity) ? stats.allStats.dexterity.ToString() :
        (statType == StatsUIStatTypes.AvailablePoints) ? stats.idlestatPoints.ToString() :
        (statType == StatsUIStatTypes.Health) ? stats.allStats.health.ToString() :
        (statType == StatsUIStatTypes.Mana) ? stats.allStats.energy.ToString() :
        (statType == StatsUIStatTypes.Attack) ? stats.allStats.attack.ToString() :
        (statType == StatsUIStatTypes.Defence) ? stats.allStats.defence.ToString() :
        (statType == StatsUIStatTypes.Armour) ? stats.allStats.armour.ToString() :
        (statType == StatsUIStatTypes.Damage) ? stats.allStats.CalculateTotalPhysicalDamage().ToString() :
        (statType == StatsUIStatTypes.SlashDamage) ? stats.allStats.slashDamage.ToString() :
        (statType == StatsUIStatTypes.PiercingDamage) ? stats.allStats.piercingDamage.ToString() :
        (statType == StatsUIStatTypes.CrushingDamage) ? stats.allStats.crushingDamage.ToString() :
        (statType == StatsUIStatTypes.PoisonDamage) ? stats.allStats.poisonDamage.ToString() :
        (statType == StatsUIStatTypes.MagicDamage) ? stats.allStats.magicDamage.ToString() :
        (statType == StatsUIStatTypes.PhysicalResistance) ? stats.allStats.physicalResistance.ToString() :
        (statType == StatsUIStatTypes.PoisonResistance) ? stats.allStats.poisonResistance.ToString() :
        (statType == StatsUIStatTypes.MagicalResistance) ? stats.allStats.magicalResistance.ToString() :
        (statType == StatsUIStatTypes.Level) ? "Level " + levelController.currentLevel.ToString() :
        (statType == StatsUIStatTypes.IncrementAmount) ? string.Empty :
        randomText;


        if (text == randomText)
        {
            GetComponent<Image>().fillAmount = (float)levelController.currentExp / levelController.totalXpRequiredForLevelup;
        }
        else
        {
            GetComponent<TMP_Text>().text = text;
        }

        loaded = true;
    }

}
