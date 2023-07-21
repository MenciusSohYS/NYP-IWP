using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globalvariables
{
    public static int currency = 0;
    public static string Playerprefabname;
    public static int CurrentLevel = 1;
    public static int MaxHP = 200;
    public static int CurrentHP = 200;
    public static int Speed = 12;
    public static float timerforsound;
    public static float Difficulty = 1;
    public static bool FlamingBullet = false;
    public static int BulletPierce = 0;
    public static bool RadialShield = false;
    public static int SkillCooldown = 0;
    public static int DOTStacks = 0;
    public static int HealthOrb = 0;
    public static int EnemiesKilled = 0;

    //Weapon stats
    public struct WeaponComponents
    {
        public static int Damage;
        public static float FireRate;
        public static float ReloadTime;
        public static int MagSize;
        public static int CurrSize;
        public static float Spread;
        public static float HeatMax;
        public static int Velocity;
        public static int CurrentUpgrades;
        public static string WeaponName;
    };

    public static void ForgetEverything()
    {
        MaxHP = 200;
        CurrentHP = 200;
        Speed = 12;
        CurrentLevel = 1;
        FlamingBullet = false;
        BulletPierce = 0;
        RadialShield = false;
        SkillCooldown = 0;
        DOTStacks = 0;
        HealthOrb = 0;
        EnemiesKilled = 0;
    }
}
