using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEffect
{
    public bool useEffect;
    public EffectType effectType;
    public int id;
    public int amount;
}

public enum EffectType{
    SetGoldReward,
    SetExpReward,
    SetRepoputationReward,
    GiveItem,
    GetItem,
    SetQuestID
}