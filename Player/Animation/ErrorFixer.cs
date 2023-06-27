using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorFixer : MonoBehaviour
{
    public Animator anim;
    private void Update()
    {
        // this is for running
        if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement2")) transform.localPosition = new Vector3(-0.0016f, 0.06f, -0.025f);
        else if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement3")) transform.localPosition = new Vector3(-0.0016f, 0.07f, -0.025f);
        // this is for holding
        else transform.localPosition = new Vector3(-0.0016f, -0.045f, -0.092f);
    }
}
