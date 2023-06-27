using System;
public class StatsUIControl:UIControl
{
    protected Player stats;

    protected override void Start()
    {
        base.Start();
        stats = Player.Instance;
    }
}
