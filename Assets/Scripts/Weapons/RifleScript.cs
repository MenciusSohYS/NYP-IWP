using UnityEngine;

public class RifleScript : WeaponParent
{

    public RifleScript()
    {
        Damage = 7;
        FireRate = 0.1f;
        Spread = 0.5f;
        MaxMagSize = 30;
        BulletsRemaining = 30;
        ReloadTime = MaxMagSize * 0.1f;
        HeatMax = 1f;
        PositionToParent = new Vector3(0, 0.6f, 0);
    }
}