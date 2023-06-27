using UnityEngine;

public class UIControl : MonoBehaviour
{
    protected bool loaded=false;

    protected virtual void Start()
    {
        
    }

    public bool GetLoaded()
    {
        return loaded;
    }
    
    public void SetLoaded(bool value)
    {
        loaded = value;
    }
    public virtual void Load()
    {
        loaded = true;
    }
}
