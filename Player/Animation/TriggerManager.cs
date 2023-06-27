static class TriggerManager
{
    private static bool _meleeAttack_2;
    public static bool meleeAttack2
    {
        get { return _meleeAttack_2; }
        set
        {
            _meleeAttack_2 = value;
            _meleeAttack_3 = false;
            _meleeAttack_4 = false;
            _meleeAttack_5 = false;
        }
    }
    private static bool _meleeAttack_3;
    public static bool meleeAttack3
    {
        get { return _meleeAttack_3; }
        set
        {
            _meleeAttack_3 = value;

            _meleeAttack_2 = false;
            _meleeAttack_4 = false;
            _meleeAttack_5 = false;
        }
    }
    private static bool _meleeAttack_4;
    public static bool meleeAttack4
    {
        get { return _meleeAttack_4; }
        set
        {
            _meleeAttack_4 = value;

            _meleeAttack_2 = false;
            _meleeAttack_3 = false;
            _meleeAttack_5 = false;
        }
    }
    private static bool _meleeAttack_5;
    public static bool meleeAttack5
    {
        get { return _meleeAttack_5; }
        set
        {
            _meleeAttack_5 = value;

            _meleeAttack_2 = false;
            _meleeAttack_3 = false;
            _meleeAttack_4 = false;
        }
    }
    private static bool _combo2;
    public static bool combo2
    {
        get { return _combo2; }
        set
        {
            _combo2 = value;
            _combo3 = false;
        }
    }
    private static bool _combo3;
    public static bool combo3
    {
        get { return _combo3; }
        set
        {
            _combo3 = value;
            _combo2 = false;
        }
    }
    public static bool runningAttack2 { get; set; }
    private static bool _skill_A;
    public static bool skill_A
    {
        get { return _skill_A; }
        set
        {
            _skill_A = value;

            _skill_B = false;
            _skill_C = false;
            _skill_D = false;
            _skill_G = false;
            _skill_H = false;
        }
    }
    private static bool _skill_B;
    public static bool skill_B
    {
        get { return _skill_B; }
        set
        {
            _skill_B = value;

            _skill_A = false;
            _skill_C = false;
            _skill_D = false;
            _skill_G = false;
            _skill_H = false;
        }
    }
    private static bool _skill_C;
    public static bool skill_C
    {
        get { return _skill_C; }
        set
        {
            _skill_C = value;

            _skill_A = false;
            _skill_B = false;
            _skill_D = false;
            _skill_G = false;
            _skill_H = false;
        }
    }
    private static bool _skill_D;
    public static bool skill_D
    {
        get { return _skill_D; }
        set
        {
            _skill_D = value;

            _skill_A = false;
            _skill_B = false;
            _skill_C = false;
            _skill_G = false;
            _skill_H = false;
        }
    }
    private static bool _skill_G;
    public static bool skill_G
    {
        get { return _skill_G; }
        set
        {
            _skill_G = value;

            _skill_A = false;
            _skill_B = false;
            _skill_C = false;
            _skill_D = false;
            _skill_H = false;
        }
    }
    private static bool _skill_H;
    public static bool skill_H
    {
        get { return _skill_H; }
        set
        {
            _skill_H = value;

            _skill_A = false;
            _skill_B = false;
            _skill_C = false;
            _skill_D = false;
            _skill_G = false;
        }
    }
    private static bool _dodgeLeft;
    public static bool dodgeLeft
    {
        get { return _dodgeLeft; }
        set
        {
            _dodgeLeft = value;

            _dodgeRight = false;
            _dodgeFront = false;
        }
    }
    private static bool _dodgeRight;
    public static bool dodgeRight
    {
        get { return _dodgeRight; }
        set
        {
            _dodgeRight = value;

            _dodgeLeft = false;
            _dodgeFront = false;
        }
    }
    private static bool _dodgeFront;
    public static bool dodgeFront
    {
        get { return _dodgeFront; }
        set
        {
            _dodgeFront = value;

            _dodgeLeft = false;
            _dodgeRight = false;
        }
    }
    private static bool _damageLeft;
    public static bool damageLeft
    {
        get { return _damageLeft; }
        set
        {
            _damageLeft = value;

            _damageRight = false;
            _damageFront = false;
            _damageBack = false;
        }
    }
    private static bool _damageRight;
    public static bool damageRight
    {
        get { return _damageRight; }
        set
        {
            _damageRight = value;

            _damageLeft = false;
            _damageFront = false;
            _damageBack = false;
        }
    }
    private static bool _damageFront;
    public static bool damageFront
    {
        get { return _damageFront; }
        set
        {
            _damageFront = value;

            _damageLeft = false;
            _damageRight = false;
            _damageBack = false;
        }
    }
    private static bool _damageBack;
    public static bool damageBack
    {
        get { return _damageBack; }
        set
        {
            _damageBack = value;

            _damageLeft = false;
            _damageRight = false;
            _damageFront = false;
        }
    }
    private static bool _bigDamage;
    public static bool bigDamage
    {
        get { return _bigDamage; }
        set
        {
            _bigDamage = value;
            _smallDamage = false;
        }
    }
    private static bool _smallDamage;
    public static bool smallDamage
    {
        get { return _smallDamage; }
        set
        {
            _smallDamage = value;
            _bigDamage = false;
        }
    }
    private static bool _damageGrade_A;
    public static bool damageGrade_A
    {
        get { return _damageGrade_A; }
        set
        {
            _damageGrade_A = value;

            _damageGrade_B = false;
            _damageGrade_C = false;
            _damageGrade_D = false;
        }
    }
    private static bool _damageGrade_B;
    public static bool damageGrade_B
    {
        get { return _damageGrade_B; }
        set
        {
            _damageGrade_B = value;

            _damageGrade_A = false;
            _damageGrade_C = false;
            _damageGrade_D = false;
        }
    }
    private static bool _damageGrade_C;
    public static bool damageGrade_C
    {
        get { return _damageGrade_C; }
        set
        {
            _damageGrade_C = value;

            _damageGrade_A = false;
            _damageGrade_B = false;
            _damageGrade_D = false;
        }
    }
    private static bool _damageGrade_D;
    public static bool damageGrade_D
    {
        get { return _damageGrade_D; }
        set
        {
            _damageGrade_D = value;

            _damageGrade_A = false;
            _damageGrade_B = false;
            _damageGrade_C = false;
        }
    }
    private static bool _knockDownBack;
    public static bool knockDownBack { get; set; }
    private static bool _knockDown_FW_Standup;
    public static bool knockDown_FW_Standup
    {
        get { return _knockDown_FW_Standup; }
        set
        {
            _knockDown_FW_Standup = value;
            _knockDown_BW_Standup = false;
        }
    }
    private static bool _knockDown_BW_Standup;
    public static bool knockDown_BW_Standup
    {
        get { return _knockDown_BW_Standup; }
        set
        {
            _knockDown_BW_Standup = value;
            _knockDown_FW_Standup = false;
        }
    }
    public static bool runningStateQuitImmediate { get; set; }

    public static void ResetTriggers()
    {
        meleeAttack2 = false;
        combo2 = false;
        runningAttack2 = false;
        skill_A = false;
        dodgeFront = false;
        damageBack = false;
        bigDamage = false;
        damageGrade_A = false;
        knockDownBack = false;
        knockDown_BW_Standup = false;
        runningStateQuitImmediate = false;
    }
}