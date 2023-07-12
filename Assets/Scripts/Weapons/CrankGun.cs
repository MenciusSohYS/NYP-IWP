using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankGun : WeaponParent
{    public CrankGun()
    {
        Damage = 3;
        FireRate = 0.1f;
        Spread = 0.5f;
        MaxMagSize = 100;
        BulletsRemaining = 100;
        ReloadTime = -1f;
        HeatMax = 1f;
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
