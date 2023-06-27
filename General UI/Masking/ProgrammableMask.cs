using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(MaskImage))]
public class ProgrammableMask : MonoBehaviour
{
    [SerializeField] CompareFunction compareMode;
    
    public Material Mask(Material mat)
    {
        mat.SetInt("_StencilComp", (int)compareMode);
        return mat;
    }
}