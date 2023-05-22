using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    protected int Damage = 5;
    protected float FireRate = 0.5f;
    protected float ReloadTime = 0.5f;
    protected int MaxMagSize = 6;
    protected int BulletsRemaining = 6;
    protected float Spread = 0.2f;
    protected float CurrentReload = 0f; //this is for dynamic reloading, if player partially reloads and swaps weapons, the timer will just pause, no need to start from 0
    protected float HeatMax = 1f; //heat determines the spread of the gun
    protected float CurrentHeat = 0f; //current heat value
    protected int BulletVelocity = 30; //current heat value

    public float Attack(Transform fromhere, GameObject Projectile, bool ShotByPlayer)
    {
        float RandomX = Random.Range(-Spread, Spread) * CurrentHeat;
        float RandomY = Random.Range(-Spread, Spread) * CurrentHeat;

        GameObject BulletShot = Instantiate(Projectile, fromhere.position, fromhere.rotation);
        BulletShot.GetComponent<Bullet>().ShotBy(ShotByPlayer); //shot by enemy
        BulletShot.GetComponent<Bullet>().AssignDamage(Damage);
        BulletShot.GetComponent<Bullet>().AssignVelocity(BulletVelocity);
        BulletShot.transform.up += new Vector3(RandomX, RandomY, 0);

        SetNewBulletsLeft(1); //set new bullets left (-1)
        if (CurrentHeat < HeatMax)
            CurrentHeat += 0.1f;

        //Debug.Log(CurrentHeat + "/" + HeatMax);
        return FireRate;
    }
    public void SetVelocity(int newVelocity)
    {
        BulletVelocity = newVelocity;
    }

    public void SetMaxHeat(float newheat)
    {
        HeatMax = newheat;
    }
    public void SetSpread(float spreadtoset)
    {
        Spread = spreadtoset;
    }
    public void SetCurrentSize(int newCurrentmag)
    {
        BulletsRemaining = newCurrentmag;
    }
    public void SetMaxMagSize(int newMaxMagSize)
    {
        MaxMagSize = newMaxMagSize;
    }
    public void SetReloadTimeByNumber(float setto)
    {
        ReloadTime = setto;
    }
    public void SetDamageByNumber(int setto)
    {
        Damage = setto;
    }
    public void SetFireRateByNumber(float setto)
    {
        FireRate = setto;
    }

    public void SetFireRate(float multiplyby)
    {
        FireRate *= multiplyby;
    }
    public void SetDamage(float multiplyby)
    {
        float tempdamage = Damage;
        tempdamage *= multiplyby;
        Damage = (int)tempdamage;
    }


    public int GetDamage()
    {
        return Damage;
    }

    public float GetFireRate()
    {
        return FireRate;
    }
    public float GetSpread()
    {
        return Spread;
    }

    public float GetReloadTime()
    {
        return ReloadTime;
    }

    public int GetMaxMagSize()
    {
        return MaxMagSize;
    }

    public int ReturnCurrentMag()
    {
        return BulletsRemaining;
    }

    public void SetNewBulletsLeft(int MinusHowMuch)
    {
        BulletsRemaining -= MinusHowMuch;
    }

    public float DoReload(float TimeLeftToGo, float ElapsedTime)
    {
        TimeLeftToGo -= ElapsedTime;
        CurrentReload = TimeLeftToGo;

        if (TimeLeftToGo <= 0)
            BulletsRemaining = MaxMagSize; //set new bullets left (max)

        return TimeLeftToGo;
    }

    public void SetWeaponHeat(float ElapsedTime)
    {
        if (CurrentHeat <= 0)
        {
            CurrentHeat = 0;
            return;
        }
        CurrentHeat -= Time.deltaTime * 0.5f;
    }

    public float ReturnCurrentWeaponHeat()
    {
        return CurrentHeat;
    }
    public float ReturnMaxHeat()
    {
        return HeatMax;
    }
    public int ReturnVelocity()
    {
        return BulletVelocity;
    }
}
