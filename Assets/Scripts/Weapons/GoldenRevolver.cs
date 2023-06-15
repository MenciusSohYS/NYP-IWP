using UnityEngine;

public class GoldenRevolver : WeaponParent
{
    public GoldenRevolver()
    {
        Damage = 70;
        FireRate = 0.25f;
        Spread = 0.01f;
        BulletsRemaining = 6;
        MaxMagSize = 6;
        ReloadTime = -1;
        HeatMax = 0.5f;
        PositionToParent = new Vector3(0, 0.6f, 0);
        Chambered = true;
    }
}