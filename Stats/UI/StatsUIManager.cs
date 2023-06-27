using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsUIManager : UIManager
{
    //Cached Fields
    List<UIControl> controls;
    private static StatsUIManager instance;
    public static StatsUIManager Instance { get => instance; }

    const float statsMenuXPosition = 0f;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this, 0.01f);
    }
    public override void Start()
    {
        base.Start();

        controls = GetComponentsInChildren<UIControl>().ToList();
        //ToggleMenu();
    }

    public override void ToggleMenu()
    {
        bool active = menus[0].GetActive();
        controls?.ForEach(c => c.SetLoaded(!active));
        InputManager.mode = active ? InputManager.UIMode.None : InputManager.UIMode.Stats;
        menus[0].Toggle();
    }
    public override void ChangeScale(float scale)
    {
        currentScale = scale < MinScale ? MinScale : scale > MaxScale ? MaxScale : scale;
        menus[0].SetLocalScale(new Vector3(scale, scale, scale));
        ArrangePositions();
    }

    public override void ArrangePositions() => ResetPositions();
    public override void ResetPositions() => menus[0].SetLocalPosition(new Vector3(statsMenuXPosition, YPosition, 0));
}
