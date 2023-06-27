using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DefaultSpriteManager : ScriptableObject
{
    public Sprite backGround;
    public Sprite checkMark;
    public Sprite dropDownArror;
    public Sprite inputFieldBackGround;
    public Sprite knob;
    public Sprite UIMask;
    public Sprite UISprite;

    public static DefaultSpriteManager instance;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
    }
}
