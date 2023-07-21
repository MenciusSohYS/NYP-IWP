using UnityEngine;
using UnityEngine.Audio;

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
    protected Vector3 PositionToParent = new Vector3(0, 0.1f, 0);
    protected bool PickedUp = false;
    protected int CurrentUpgrade = 0;
    protected bool Chambered = false;
    protected bool isMelee = false;
    protected AudioSource AudioSourceField;
    public AudioClip ReloadSoundEffect;
    public AudioClip[] ShootingSoundEffects;
    protected float ReloadSoundDuration;
    protected float ReloadPitch;
    protected bool CanUpdate = false;
    protected int Piercing = 1;

    private void Start()
    {
        if (AudioSourceField == null)
            AudioSourceField = GetComponent<AudioSource>();
        AssignAtStart();
    }

    protected virtual void AssignAtStart()
    {
        if (ReloadSoundEffect != null)
        {
            ReloadSoundDuration = ReloadSoundEffect.length; //get the reload sound's duration, will not change during run time for now
            ReloadPitch = ReloadSoundDuration / ReloadTime;
        }
    }

    public virtual void CallFlipped(bool ToF)
    {

    }

    public void StopSound()
    {
        if (CurrentReload <= 0 && (!Chambered && !isMelee))
            AudioSourceField.Pause();
    }

    public virtual void TellitsAttachedToPlayer()
    {
        if (AudioSourceField == null)
            AudioSourceField = GetComponent<AudioSource>();
        AudioSourceField.spatialBlend = 0;
    }

    public virtual float Attack(Transform fromhere, GameObject Projectile, bool ShotByPlayer)
    {
        float RandomX = Random.Range(-Spread, Spread) * CurrentHeat;
        float RandomY = Random.Range(-Spread, Spread) * CurrentHeat;

        PlayShootingSound();

        GameObject BulletShot = Instantiate(Projectile, fromhere.position, fromhere.rotation);
        BulletShot.GetComponent<Bullet>().ShotBy(ShotByPlayer); //shot by enemy or player
        BulletShot.GetComponent<Bullet>().AssignDamage(Damage);
        BulletShot.GetComponent<Bullet>().AssignVelocity(BulletVelocity);
        BulletShot.GetComponent<Bullet>().AssignPierce(Piercing + Globalvariables.BulletPierce);
        BulletShot.transform.up += new Vector3(RandomX, RandomY, 0);

        SetNewBulletsLeft(1); //set new bullets left (-1)
        if (CurrentHeat < HeatMax)
            CurrentHeat += 0.1f;

        //Debug.Log(FireRate);
        return FireRate;
    }

    protected virtual void PlayShootingSound()
    {
        if (ShootingSoundEffects.Length < 1 || (AudioSourceField.isPlaying && !Chambered))
        {
            return;
        }

        //int RandomShootingSound = Random.Range(0, ShootingSoundEffects.Length); //use if I decide to put in more sound
        AudioSourceField.clip = ShootingSoundEffects[0];
        //Debug.Log(ShootingSoundEffects[RandomShootingSound].name);
        AudioSourceField.Play();
        //Debug.Log("Playing Revolver Sound");
    }

    public void SetVelocity(int newVelocity)
    {
        BulletVelocity = newVelocity;
    }
    public void MultiplyVelocity(float multiplyby)
    {
        float tempvelo = BulletVelocity;
        tempvelo *= multiplyby;
        BulletVelocity = (int)tempvelo;
    }

    public void SetMaxHeat(float newheat)
    {
        HeatMax = newheat;

        //Debug.Log("New Heat");
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
    public virtual void SetReloadTimeByNumber(float setto)
    {
        ReloadTime = setto;
        ReloadPitch = ReloadSoundDuration / ReloadTime; //pitch for reload will probably only change here
    }
    public void SetDamageByNumber(int setto)
    {
        Damage = setto;
    }
    public virtual void SetFireRateByNumber(float setto)
    {
        FireRate = setto;
    }

    public virtual void SetFireRate(float multiplyby)
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

    public virtual void StartReload()
    {
    }

    public virtual float GetReloadTime()
    {
        if (CurrentReload <= 0)
        {
            //Debug.Log(ReloadTime); //if gun has no current reload
            return ReloadTime; //this is for dynamic/reload saving
        }
        else
        {
            //Debug.Log(CurrentReload); //if it has been reloaded partially
            return CurrentReload; //this is for dynamic/reload saving
        }
    }

    public float ReturnFullReload()
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

    public virtual float DoReload(float TimeLeftToGo, float ElapsedTime)
    {
        if (AudioSourceField.clip != ReloadSoundEffect) //if its a shooting sound
        {
            if (AudioSourceField.isPlaying && AudioSourceField.clip.length < 0.5f) //if it is still playing the shooting sound
            {
                return TimeLeftToGo;
            }
            else
            {
                AudioSourceField.clip = ReloadSoundEffect;
                AudioSourceField.pitch = ReloadPitch;
                AudioSourceField.Play();
            }
        }

        TimeLeftToGo -= ElapsedTime;
        CurrentReload = TimeLeftToGo; //store reload time

        if (TimeLeftToGo <= 0 && AudioSourceField != null)
        {
            BulletsRemaining = MaxMagSize; //set new bullets left (max)
            AudioSourceField.pitch = 1;
            AudioSourceField.clip = ShootingSoundEffects[0];
            AudioSourceField.time = 0;
        }

        return TimeLeftToGo;
    }

    public virtual void SetWeaponHeat(float ElapsedTime)
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

    public Vector3 GetLocalPosition()
    {
        return PositionToParent;
    }

    public bool GetPickedUp()
    {
        return PickedUp;
    }

    public int GetCurrentUpgrade()
    {
        return CurrentUpgrade;
    }

    public virtual void SetCurrentUpgrades(int ByHowMuch)
    {
        CurrentUpgrade += ByHowMuch;
    }

    public virtual void SetPickedUp()
    {
        PickedUp = !PickedUp;
    }

    public bool ReturnChambered()
    {
        return Chambered;
    }
    public bool ReturnMelee()
    {
        return isMelee;
    }

    public void StartCanUpdate()
    {
        CanUpdate = true;
    }
}
