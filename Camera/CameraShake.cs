using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;

    void Start()
    {
        impulseSource = transform.GetComponent<CinemachineImpulseSource>();

        Invoke("Shake", 3f);
    }

    void Shake()
    {
        impulseSource.GenerateImpulse();
        Invoke("Shake", 3f);
    }
}
