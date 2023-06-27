using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGiver : MonoBehaviour
{

    //Config Params
    [SerializeField] MobExperiencePointBalancer balancer;

    [Range(1, 10)]
    public int mobLevelTier;
    [Range(1, 5)]
    public int mobLevelMin;
    [Range(1, 5)]
    public int mobLevelMax;
    [Range(1, 3)]
    public float mobExperienceConstant;

    //Cached Fields
    [System.NonSerialized] public int level;
    PlayerLevelController player;

    private void Start()
    {
        CalculateLevel();
        player = PlayerLevelController.Instance;
    }

    private void CalculateLevel()
    {
        level = mobLevelTier * 5 - (5- Random.Range(mobLevelMin, mobLevelMax));
    }

    //This will be called when the mob dies
    public void GiveExperience()
    {
        balancer.GiveExperienceToPlayer(player, level, mobExperienceConstant);
    }

}
