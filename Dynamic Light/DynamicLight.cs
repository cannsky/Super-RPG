using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLight : StyledMonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(5f * Time.deltaTime, 0, 0);
    }
}
