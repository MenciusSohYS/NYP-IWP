using UnityEngine;

public class RevolverScript : WeaponParent
{

    public RevolverScript()
    {
        Damage = 10;
        FireRate = 0.5f;
        Spread = 0.1f;
        BulletsRemaining = 6;
        MaxMagSize = 9;
        ReloadTime = 1.2f;
        HeatMax = 0.5f;
        PositionToParent = new Vector3(0, 0.6f, 0);
        Chambered = true;
    }
}