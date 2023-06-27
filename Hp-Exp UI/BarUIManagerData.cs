using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BarUIManagerData 
{
    public int expPeriod;

    public BarUIManagerData(BarUIManager manager)
    {
        expPeriod = manager.expPeriod;
    }
}
