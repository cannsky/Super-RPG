using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InfiniteScrollButton : ScrollButton
{
    InfiniteScroll infiniteScroll;
    protected override void Start()
    {
        base.Start();
        button.onClick.RemoveAllListeners();
        scrollRect.onValueChanged.RemoveAllListeners();
        infiniteScroll = scrollRect.GetComponent<InfiniteScroll>();

        if (type == ScrollButtonType.Up)
            button.onClick.AddListener(() =>
            {
                infiniteScroll.SetDestinationAndAnimate(true);
            });
        else
            button.onClick.AddListener(() =>
            {
                infiniteScroll.SetDestinationAndAnimate(false);
            });
    }

    public override void Load()
    {
    }
}
