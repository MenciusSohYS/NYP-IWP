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
        if (AudioSourceField.clip != ReloadSoundEffect && CurrentHeat > HeatMax * 0.5f)
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

        float Modifier = 0.5f * (4 / ReloadTime);
        if (CurrentHeat > HeatMax)
        {
            Modifier = 0.25f;
        }
        CurrentHeat -= Time.deltaTime * Modifier;
        BulletsRemaining = (int)((HeatMax - CurrentHeat) * 100);
    }

    public override void SetReloadTimeByNumber(float setto)
    {
        ReloadTime = setto;
        ReloadPitch = ReloadSoundDuration / ReloadTime; //pitch for reload will probably only change here
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
        {
            // Convert mouse position to world position
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Set an arbitrary distance from the camera

            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 direction = mouseWorldPosition - BulletShot.transform.position;
            direction.z = 0f; // Ensure the Z-component is zero since it's a 2D game

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            BulletShot.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }

        int IsCrit = Random.Range(0, 101);

        if (IsCrit <= CritRate)
            BulletShot.GetComponent<Bullet>().SetCrit(true);

        BulletShot.transform.up += new Vector3(RandomX, RandomY, 0);


        SetNewBulletsLeft(10); //set new bullets left (-1)

        if (CurrentHeat < HeatMax)
            CurrentHeat += 0.1f;

        //Debug.Log(FireRate);
        return FireRate;
    }
}