using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfAbility : AbilityParent
{
    [SerializeField] GameObject BowAndArrow;
    private GameObject PointerToGun;
    public ElfAbility()
    {
        CoolDown = 15;
        Timer = 0;
        Name = "Bow and arrow";
    }

    public override int UseAbility()
    {
        if (Timer <= 0)
        {
            //Debug.Log("Bounty Ability");
            PointerToGun = Instantiate(BowAndArrow, transform.position, Quaternion.identity);
            transform.GetChild(0).GetComponent<GunScript>().AssignNewGun(PointerToGun, true, true);
            Timer = CoolDown;
        }
        return 0;
    }
    public override float CoolDownAbility(float Time)
    {
        if (Timer <= 0 || PointerToGun != null)
            return CoolDown;
        Timer -= Time;
        return CoolDown - Timer;
    }
}