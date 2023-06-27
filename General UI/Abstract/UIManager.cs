using System;
using UnityEngine;
using System.Collections.Generic;
public abstract class UIManager : MonoBehaviour
{
    [SerializeField] protected List<UIMenu> menus;
    protected bool isLocked = false;

    [NonSerialized] public bool isOn=false;

    protected float currentScale;
    protected const float YPosition = 1.25f;
    protected const float MaxScale = 1f;
    protected const float MinScale = 0.6f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        int counter = 0;
        foreach (var menu in menus)
        {
            menu.index = counter;
            counter++;
        }
    }
    public virtual Vector2 GetMaximumMenuSize(int index) => menus[0].GetSize();

    public virtual Vector2 GetCurrentMenuSize(int index) => new Vector2(GetCurrentLength(menus[0].GetWidth()), GetCurrentLength(menus[0].GetHeight()));
    public abstract void ToggleMenu();
    public abstract void ChangeScale(float scale);
    public abstract void ArrangePositions();
    public abstract void ResetPositions();
    public virtual void Lock() => isLocked = true;
    public virtual void Unlock() => isLocked = false;
    public virtual bool CanMove() => !isLocked;

    //Will probably be moved in the settings later on
    public static DictionaryNode<float, float> GetResolution()
    {
        //after resolution settings are added this will be fixed
        return new DictionaryNode<float, float>(1920, 1080);
    }
    protected virtual float GetCurrentLength(float maxScaleLength) => (maxScaleLength / MaxScale) * currentScale;
}