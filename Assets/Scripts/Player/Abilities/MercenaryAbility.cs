using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryAbility : AbilityParent
{

    public MercenaryAbility()
    {
        CoolDown = 15;
        Timer = 0;
        Name = "Mercenary Heal";        
    }

    public override int UseAbility()
    {
        if (Timer <= 0)
        {
            //Debug.Log("Merc Ability");
            Timer = CoolDown;
            return 1;
        }
        return 0;
    }
}
