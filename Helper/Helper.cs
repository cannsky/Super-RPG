using System.Collections;
using UnityEngine;
using System;
public static class Helper 
{  
    public static float CalculateDistance(Vector3 pos1,Vector3 pos2)
    {
       float distance = Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2) + Mathf.Pow(pos1.z - pos2.z, 2));

        return distance;
    }

    public static float CalculateDistance(Vector2 pos1,Vector2 pos2)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2));

        return distance;
    }

    public static float CalculateDistance(float x1,float y1,float z1,float x2,float y2,float z2)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2) + Mathf.Pow(z1 - z2, 2));

        return distance;
    }

    public static float CalculateDistance(float x1,float y1,float x2,float y2)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));

        return distance;
    }

    public static Vector3 FindDirectionVectorWithouty(Vector3 startPoint,Vector3 endPoint)
    {
        return new Vector3(endPoint.x - startPoint.x, 0 , endPoint.z - startPoint.z);
    }


    public static float FourthRoot(float value)
    {
        return Mathf.Sqrt(Mathf.Sqrt(value));
    }
    public static Vector2 ConvertFromPolarToCartesian(Vector2 polar)
    {
        return new Vector2(polar.y * Mathf.Cos(polar.x), polar.y * Mathf.Sin(polar.x));
    }

    public static string PrintRoot(Transform gameObject)
    {
        if (!gameObject) return "";
        return PrintRoot(gameObject.transform.parent) + " / " + gameObject.name;
    }
}
