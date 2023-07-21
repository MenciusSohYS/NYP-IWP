using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAbility : AbilityParent
{
    public WizardAbility()
    {
        CoolDown = 2;
        Timer = 0;
        Name = "Fire walker";
    }
    PlayerMovement PlayerMovementScript;

    public override void CallAtStart()
    {
        PlayerMovementScript = GetComponent<PlayerMovement>();
    }

    public override int UseAbility()
    {
        if (Timer <= 0 && !PlayerMovementScript.IsRolling())
        {
            //Debug.Log("Wiz Ability");
            Timer = CoolDown;

            return 2;
        }
        return 0;
    }
}