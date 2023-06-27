using System;
using UnityEngine;

public class UIMenu:UIToggle,IScalable,IRelocatable,IArea
{
    [NonSerialized] public int index;
    RectTransform rect;

    private void Start() => rect = GetComponent<RectTransform>();
    public void SetLocalScale(Vector3 scale) => transform.localScale = scale;
    public Vector3 GetLocalScale() => transform.localScale;
    public void SetLocalPosition(Vector3 pos) => rect.localPosition = pos;
    public Vector3 GetLocalPosition() => rect.localPosition;
    public Vector2 GetSize() => new Vector2(rect.rect.width,rect.rect.height);
    public float GetWidth() => rect.rect.width;
    public float GetHeight() => rect.rect.height;
}
