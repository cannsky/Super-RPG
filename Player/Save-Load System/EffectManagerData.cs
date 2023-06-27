
[System.Serializable]
public class EffectManagerData
{
    //Movement Effects
    public int movementEffectCount;
    public string[] movementEffectSkillNames;
    public float[] movementEffectMagnitudes;
    public float[] movementEffectTimes;
    public float[] movementEffectCurrentTimes;

    //Buff Debuff Effects
    public int buffDebuffEffectCount;
    public string[] buffDebuffEffectSkillNames;
    public int[] buffDebuffStatTypes;
    public int[] buffDebuffEffectMagnitudes;
    public float[] buffDebuffEffectTimes;
    public float[] buffDebuffEffectCurrentTimes;

    //Regeneration Effects
    public int regenerationEffectCount;
    public string[] regenerationEffectSkillNames;
    public int[] regenerationTypes;
    public int[] regenerationEffectMagnitudes;
    public int[] regenerationEffectTimes;
    public float[] regenerationEffectCurrentTimes;

    //Damage Over Time Effects
    public int damageOverTimeEffectCount;
    public string[] damageOverTimeEffectSkillNames;
    public int[] damageStatTypes;
    public int[] damageOverTimeEffectMagnitudes;
    public int[] damageOverTimeEffectTimes;
    public float[] damageOverTimeEffectCurrentTimes;

    public EffectManagerData(EffectManager manager)
    {
        int counter = 0;

        foreach (var effect in manager.movementEffects)
        {
            movementEffectSkillNames[counter] = effect.Key;
            movementEffectMagnitudes[counter] = effect.Value.firstElement;
            movementEffectTimes[counter] = effect.Value.secondElement;
            movementEffectCurrentTimes[counter] = effect.Value.thirdElement;
            counter++;
        }

        movementEffectCount = counter;
        counter = 0;

        foreach (var effect in manager.buffDebuffEffects)
        {
            buffDebuffEffectSkillNames[counter] = effect.Key.firstElement;
            buffDebuffStatTypes[counter] = (int)effect.Key.secondElement;
            buffDebuffEffectMagnitudes[counter] = effect.Value.firstElement;
            buffDebuffEffectTimes[counter] = effect.Value.secondElement;
            buffDebuffEffectCurrentTimes[counter] = effect.Value.thirdElement;
            counter++;
        }

        buffDebuffEffectCount = counter;
        counter = 0;

        foreach (var effect in manager.regenerationEffects)
        {
            regenerationEffectSkillNames[counter] = effect.Key.firstElement;
            regenerationTypes[counter] = (int)effect.Key.secondElement;
            regenerationEffectMagnitudes[counter] = effect.Value.firstElement;
            regenerationEffectTimes[counter] = effect.Value.secondElement;
            regenerationEffectCurrentTimes[counter] = effect.Value.thirdElement;
            counter++;
        }

        regenerationEffectCount = counter;
        counter = 0;

        foreach (var effect in manager.damageOverTimeEffects)
        {
            damageOverTimeEffectSkillNames[counter] = effect.Key.firstElement;
            damageStatTypes[counter] = (int)effect.Key.secondElement;
            damageOverTimeEffectMagnitudes[counter] = effect.Value.firstElement;
            damageOverTimeEffectTimes[counter] = effect.Value.secondElement;
            damageOverTimeEffectCurrentTimes[counter] = effect.Value.thirdElement;
            counter++;
        }

        damageOverTimeEffectCount = counter;
    }
}
