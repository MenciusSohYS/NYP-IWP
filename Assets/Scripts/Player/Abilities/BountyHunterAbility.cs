using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunterAbility : AbilityParent
{
    [SerializeField] GameObject GoldenGunPrefab;
    private GameObject PointerToGun;
    public BountyHunterAbility()
    {
        CoolDown = 10;
        Timer = 0;
        Name = "Big Iron";
    }

    public override int UseAbility()
    {
        if (Timer <= 0)
        {
            //Debug.Log("Bounty Ability");
            PointerToGun = Instantiate(GoldenGunPrefab, transform.position, Quaternion.identity);
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