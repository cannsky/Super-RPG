using UnityEngine.UI;
using UnityEngine;

public class ScrollButton : StatsUIControl
{
    protected enum ScrollButtonType
    {
        Up=0,
        Down
    }

    [SerializeField] protected ScrollButtonType type;
    [SerializeField] protected ScrollRect scrollRect; 
    [SerializeField] float maxY;
    [SerializeField] float minY;
    [SerializeField] float errorDistance = 5f;
    [SerializeField] float elementHeight;
    RectTransform content;
    protected Button button;

    protected override void Start()
    {
        base.Start();

        button = GetComponent<Button>();
        content = scrollRect.content;
        scrollRect.onValueChanged.AddListener(Load);

        if (type == ScrollButtonType.Up)
            button.onClick.AddListener(
                () => 
                {
                    content.localPosition = new Vector3(0, content.localPosition.y - elementHeight, 0);
                    FixContent();
                    Load();
                });
        else
            button.onClick.AddListener(
                ()=>
                {
                    content.localPosition = new Vector3(0, content.localPosition.y + elementHeight, 0);
                    FixContent();
                    Load();
                });
        Load();
    }
    public override void Load()
    {
        base.Load();
        if(type==ScrollButtonType.Up)
        {
            if (content.localPosition.y <= minY + errorDistance)
                button.interactable = false;
            else
                button.interactable = true;
        }
        else
        {
            if (content.localPosition.y >= maxY - errorDistance)
                button.interactable = false;
            else
                button.interactable = true;
        }
    }

    protected void Load(Vector2 vector2) => Load();
    void FixContent()
    {
        int GetUppermostElementIndex()
        {
            float dif = content.localPosition.y - minY;
            int index = Mathf.RoundToInt(dif / elementHeight);
            return index;
        }

        void SetUpperMostElement(int index)
        {
            content.localPosition = new Vector3(0, minY + index * elementHeight, 0);
        }

        SetUpperMostElement(GetUppermostElementIndex());
    }
}