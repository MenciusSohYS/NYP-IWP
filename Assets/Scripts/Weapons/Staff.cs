using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : WeaponParent
{

    public Staff()
    {
        Damage = 25;
        FireRate = 0.75f;
        Spread = 0.1f;
        MaxMagSize = 100;
        BulletsRemaining = 100;
        ReloadTime = 4f;
        HeatMax = 1f;
        PositionToParent = new Vector3(0, 1f, 0);
        BulletVelocity = 35;
        Chambered = true;
    }
    public GameObject CannonBall;
    private float cooldownbeforerestore = 0.5f;

    public override void CallFlipped(bool ToF)
    {
        if (ToF)
        {
            transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, -2.5f, transform.GetChild(0).localPosition.z);
        }
        else
        {
            transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, 2.5f, transform.GetChild(0).localPosition.z);
        }
    }

    public override void SetWeaponHeat(float ElapsedTime)
    {
        if (AudioSourceField.clip != ReloadSoundEffect && CurrentHeat > HeatMax * 0.3f)
        {
            AudioSourceField.clip = ReloadSoundEffect;
            AudioSourceField.Play();
            //Debug.Log("Playing");
        }

        if (cooldownbeforerestore > 0)
        {
            cooldownbeforerestore -= Time.deltaTime;
            return;
        }

        //Debug.Log("Start Cooldown");

        if (CurrentHeat <= 0)
        {
            if (AudioSourceField.clip == ReloadSoundEffect)
            {
                AudioSourceField.Pause();
            }
            CurrentHeat = 0;
            return;
        }

        CurrentHeat -= Time.deltaTime * 0.5f;
        BulletsRemaining = (int)((HeatMax - CurrentHeat) * 100);
    }

    public override float Attack(Transform fromhere, GameObject Projectile, bool ShotByPlayer)
    {
        if (CurrentHeat >= HeatMax)
            return 0;

        cooldownbeforerestore = 0.5f;

        float RandomX = Random.Range(-Spread, Spread) * CurrentHeat;
        float RandomY = Random.Range(-Spread, Spread) * CurrentHeat;

        PlayShootingSound();

        GameObject BulletShot = Instantiate(CannonBall, fromhere.position, fromhere.rotation); //technically fireball but too lazy to change
        BulletShot.GetComponent<Bullet>().ShotBy(ShotByPlayer); //shot by enemy or player
        BulletShot.GetComponent<Bullet>().AssignDamage(Damage);
        BulletShot.GetComponent<Bullet>().AssignVelocity(BulletVelocity);
        BulletShot.GetComponent<Bullet>().AssignPierce(Piercing + Globalvariables.BulletPierce);
        BulletShot.transform.up += new Vector3(RandomX, RandomY, 0);


        SetNewBulletsLeft(10); //set new bullets left (-1)

        if (CurrentHeat < HeatMax)
            CurrentHeat += 0.1f;

        //Debug.Log(FireRate);
        return FireRate;
    }
}