using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGun : WeaponParent
{    public MiniGun()
    {
        Damage = 4;
        FireRate = 0.05f;
        Spread = 0.9f;
        MaxMagSize = 300;
        BulletsRemaining = 300;
        ReloadTime = 3f;
        HeatMax = 1f;
    }
}
