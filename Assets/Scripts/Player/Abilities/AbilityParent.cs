using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityParent : MonoBehaviour
{
    protected float CoolDown = 5;
    protected float Timer = 0;
    protected string Name = "Default";

    public virtual int UseAbility()
    {
        Debug.Log("ability used");
        return 0;
    }
    public virtual float CoolDownAbility(float Time)
    {
        if (Timer <= 0)
            return CoolDown;
        Timer -= Time;
        return CoolDown - Timer;
    }
    public virtual float ReturnMaxAbilityCoolDown()
    {
        return CoolDown;
    }
}