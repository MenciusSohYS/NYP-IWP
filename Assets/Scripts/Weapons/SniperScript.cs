using UnityEngine;

public class SniperScript : WeaponParent
{

    public SniperScript()
    {
        Damage = 15;
        FireRate = 0.75f;
        Spread = 0.1f;
        MaxMagSize = 5;
        BulletsRemaining = 5;
        ReloadTime = 3f;
        HeatMax = 1f;
        PositionToParent = new Vector3(0, 0.7f, 0);
        BulletVelocity = 70;
        Chambered = true;
    }
}