using UnityEngine;

public class RifleScript : WeaponParent
{

    public RifleScript()
    {
        Damage = 2;
        FireRate = 0.1f;
        Spread = 0.5f;
        MaxMagSize = 30;
        BulletsRemaining = 30;
        ReloadTime = 2f;
        HeatMax = 1f;
    }
}