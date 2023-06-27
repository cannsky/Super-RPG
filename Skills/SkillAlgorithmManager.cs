using UnityEngine;

public static class SkillAlgorithmManager
{
    private static readonly float baseStatAlgorithmConstant;
    private static readonly float skillLevelAlgorithmConstant;

    static SkillAlgorithmManager()
    {
        baseStatAlgorithmConstant = 1f - (Mathf.Log10(Helper.FourthRoot(5f)) * Mathf.PI);
        skillLevelAlgorithmConstant = 1f-(Mathf.Log10(2f)/2f);
    }

    //maybe we can change the algorithm for each skill or each base stat?
    public static float BaseStatAlgorithm(int value, float magnitude)
    {
        return (Mathf.Log10(Helper.FourthRoot(value)) * Mathf.PI + baseStatAlgorithmConstant) * magnitude;
    }
    //when magnitude is 1;
    //when base stat is 5 (the minimum possible value) the multiplier returned is roughly 1 
    //when base stat is 250(the maximum possible value) the multiplier returned is roughly 2.33


    //maybe we can change the algorithm for each skill?
    public static float SkillLevelAlgorithm(int value,float magnitude)
    {
        return (Mathf.Log10(Mathf.Pow(2f,value))/2+skillLevelAlgorithmConstant)*magnitude;
    }
    //when magnitude is 1;
    //when level is 1 (the minimum possible value) the multiplier returned is roughly 1 
    //when level is 10 (the maximum possible value) the multiplier returned is roughly 2.35
}