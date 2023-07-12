using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAbility : AbilityParent
{
    public WizardAbility()
    {
        CoolDown = 1;
        Timer = 0;
        Name = "Fire walker";
    }

    public override int UseAbility()
    {
        if (Timer <= 0)
        {
            //Debug.Log("Wiz Ability");
            Timer = CoolDown;

            return 2;
        }
        return 0;
    }
}