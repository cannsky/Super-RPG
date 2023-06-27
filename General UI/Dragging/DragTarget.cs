using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class DragTarget : MonoBehaviour,IScalable,IRelocatable,IArea
{
    public Canvas outestCanvas;
    RectTransform rect;
    private void Start()
    {
        rect = GetComponent<RectTransform>();    
    }
    public void Drag(Vector3 delta) => transform.position += delta;
    public void SetSiblingIndex(int index) => transform.SetSiblingIndex(index);
    public void SetLocalScale(Vector3 scale) => transform.localScale = scale;
    public Vector3 GetLocalScale() => transform.localScale;
    public void SetLocalPosition(Vector3 pos) => rect.localPosition = pos;
    public Vector3 GetLocalPosition() => rect.localPosition;
    public Vector2 GetSize() => new Vector2(rect.rect.width, rect.rect.height);
    public float GetWidth() => rect.rect.width;
    public float GetHeight() => rect.rect.height;
}