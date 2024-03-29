using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankGun : WeaponParent
{    public CrankGun()
    {
        Damage = (int)(8 * (1 + (Globalvariables.CurrentLevel * 0.1f)));
        FireRate = 0.1f;
        Spread = 0.4f;
        MaxMagSize = (int)(100 * (1 + (Globalvariables.CurrentLevel * 0.2f)));
        BulletsRemaining = (int)(100 * (1 + (Globalvariables.CurrentLevel * 0.2f)));
        ReloadTime = -1f;
        HeatMax = 1f;
        CritRate = 30;
    }

    public override void CallFlipped(bool ToF)
    {
        if (ToF)
        {
            transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, 0.8f, transform.GetChild(0).localPosition.z);
        }
        else
        {
            transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, -0.8f, transform.GetChild(0).localPosition.z);
        }
    }

    public override void TellitsAttachedToPlayer()
    {
        base.TellitsAttachedToPlayer();
        Debug.Log(BulletsRemaining * (1 + (8 * 0.2f)));
    }
    public override float Attack(Transform fromhere, GameObject Projectile, bool ShotByPlayer)
    {
        if (AudioSourceField.isPlaying)
        {
            if (AudioSourceField.time < 0.633f)
            {
                return 0;
            }
        }
        else
            PlayShootingSound();

        return base.Attack(fromhere, Projectile, ShotByPlayer);
    }

    protected override void PlayShootingSound()
    {
        if (AudioSourceField.isPlaying)
        {
            if (AudioSourceField.time > 3.1f)
            {
                AudioSourceField.time = 0.633f;
                return;
            }
            return;
        }
        //if (ShootingSoundEffects.Length < 1 || ( && !Chambered))
        //{
        //    return;
        //}

        //int RandomShootingSound = Random.Range(0, ShootingSoundEffects.Length); //use if I decide to put in more sound
        AudioSourceField.clip = ShootingSoundEffects[0];
        //Debug.Log(ShootingSoundEffects[RandomShootingSound].name);
        AudioSourceField.Play();
        //Debug.Log("Playing Revolver Sound");
    }
}
