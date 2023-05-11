using UnityEngine;

public class RevolverScript : WeaponParent
{

    public RevolverScript()
    {
        Damage = 10;
        FireRate = 0.5f;
        Spread = 0.1f;
        BulletsRemaining = 6;
        MaxMagSize = 6;
        ReloadTime = MaxMagSize * 0.1f;
        HeatMax = 0.5f;
    }
}