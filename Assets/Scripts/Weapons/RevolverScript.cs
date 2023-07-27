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
        ReloadTime = 1.5f;
        HeatMax = 0.5f;
        PositionToParent = new Vector3(0, 0.6f, 0);
        Chambered = true;
        CritRate = 70;
    }
    private float timeforonebullet;

    public override void StartReload()
    {
        //Debug.Log(ReloadSound.clip.length + "/" + remainingReloadTime + "=" + ReloadSound.clip.length / remainingReloadTime);
        //Debug.Log(ReloadPitch);

        timeforonebullet = (MaxMagSize / (ReloadTime - ReduceNextReload));
        ReduceNextReload = 0;
    }

    public override void SetReloadTimeByNumber(float setto)
    {
        if (ReloadSoundDuration <= 0)
            ReloadSoundDuration = ReloadSoundEffect.length; //get the reload sound's duration, will not change during run time for now

        ReloadTime = setto;
        ReloadPitch = ReloadSoundDuration / (ReloadTime / MaxMagSize); //pitch for reload will probably only change here

        //Debug.Log(setto);
        //Debug.Log(ReloadSoundDuration);
        //Debug.Log("Called in Set " + ReloadPitch);
    }

    protected override void AssignAtStart()
    {
        if (ReloadSoundDuration <= 0)
            ReloadSoundDuration = ReloadSoundEffect.length; //get the reload sound's duration, will not change during run time for now
        ReloadPitch = ReloadSoundDuration / (ReloadTime / MaxMagSize);
        //Debug.Log("Called in Assign " + ReloadPitch);
    }

    public override float GetReloadTime()
    {
        if (CurrentReload <= 0)
        {
            //Debug.Log(ReloadTime); //if gun has no current reload

            float remainingReloadTime = ReloadTime * (1 - (float)BulletsRemaining / MaxMagSize);

            return remainingReloadTime;  //reload from current mag size
        }
        return CurrentReload;
    }

    public override float DoReload(float TimeLeftToGo, float ElapsedTime)
    {
        if (AudioSourceField.clip != ReloadSoundEffect) //if its a shooting sound
        {
            if (AudioSourceField.isPlaying) //if it is still playing the shooting sound
            {
                return TimeLeftToGo;
            }
            else
            {
                AudioSourceField.clip = ReloadSoundEffect;
                AudioSourceField.pitch = ReloadPitch;
            }
        }

        TimeLeftToGo -= ElapsedTime;
        CurrentReload = TimeLeftToGo; //store reload time

        int HowManyBulletsNow = (int)(timeforonebullet * (ReloadTime - CurrentReload));
        if (BulletsRemaining < HowManyBulletsNow)
        {
            BulletsRemaining = HowManyBulletsNow;
            if (BulletsRemaining == MaxMagSize)
            {
                AudioSourceField.pitch = 1;
                AudioSourceField.Pause();
            }
        }
        else if (!AudioSourceField.isPlaying)
        {
            AudioSourceField.Play();
        }

        return TimeLeftToGo;
    }
}