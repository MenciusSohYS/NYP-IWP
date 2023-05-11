using UnityEngine;

public class RifleScript : WeaponParent
{

    public RifleScript()
    {
        Damage = 2;
        FireRate = 0.1f;
        Spread = 0.5f;
        MaxMagSize = 300;
        BulletsRemaining = 300;
        ReloadTime = MaxMagSize * 0.1f;
        HeatMax = 1f;
    }
}