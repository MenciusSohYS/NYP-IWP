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

    public virtual void SetNewCoolDown(float MultiplyBy)
    {
        if (MultiplyBy < 0.1f)
            MultiplyBy = 0.1f;


        //Debug.Log(CoolDown);
        CoolDown *= MultiplyBy;
        //Debug.Log(CoolDown);
    }

    public virtual void CallAtStart()
    {

    }

    private void Start()
    {
        if (Globalvariables.SkillCooldown > 0) //apply ability cooldown buff
        {
            SetNewCoolDown(1f - (Globalvariables.SkillCooldown * 0.1f));
            GetComponent<PlayerMechanics>().UpdatePlayerAndMaxAbilityCooldown();
        }

        CallAtStart();
    }
}