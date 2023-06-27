using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Filter 
{
    public Filter(Type type,string searchFilter)
    {
        this.type = type;
        this.searchFilter = searchFilter;
    }

    public Type type;
    public string searchFilter;
}
