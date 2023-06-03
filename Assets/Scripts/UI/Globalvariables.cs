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

}
