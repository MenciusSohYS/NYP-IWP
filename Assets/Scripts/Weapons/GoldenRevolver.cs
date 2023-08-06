using UnityEngine;

public class GoldenRevolver : WeaponParent
{
    public GoldenRevolver()
    {
        Damage = (int)(70 * (1 + (Globalvariables.CurrentLevel * 0.1f)));
        FireRate = 0.25f;
        Spread = 0.01f;
        BulletsRemaining = 6 + (int)(Globalvariables.CurrentLevel * 0.5f);
        MaxMagSize = 6 + (int)(Globalvariables.CurrentLevel * 0.5f);
        ReloadTime = -1;
        HeatMax = 0.5f;
        PositionToParent = new Vector3(0, 0.6f, 0);
        Chambered = true;
        BulletVelocity = 75;
    }
}