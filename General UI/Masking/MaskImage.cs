using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ProgrammableMask))]
public class MaskImage : Image
{
    ProgrammableMask mask;
    protected override void Start()
    {
        base.Start();
        mask = GetComponent<ProgrammableMask>();
    }

    public override Material materialForRendering => mask.Mask(base.materialForRendering);
}
