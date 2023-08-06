using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowScript : WeaponParent
{
    const float BOWSCALE = 3.333333333333f;
    public BowScript()
    {
        Damage = (int)(100 * (1 + (Globalvariables.CurrentLevel * 0.2f)));
        FireRate = 0.2f;
        Spread = 0.01f;
        BulletsRemaining = 5;
        MaxMagSize = 5;
        ReloadTime = -1;
        HeatMax = 1f; //we will use heat to determine how much the string has been pulled
        PositionToParent = new Vector3(0, 0.6f, 0);
        BulletVelocity = 40;
        Piercing = (int)(4 * (1 + (Globalvariables.CurrentLevel * 0.2f)));
        CritRate = 70;
    }

    public Sprite[] Sprites;
    private SpriteRenderer BowStringSpriteRenderer;
    private Transform BowStringTransform;
    public GameObject ArrowPrefab;
    private Transform FromHere;
    private bool isShotByPlayer;
    public AudioClip[] ArrowSFX;

    private void Start()
    {
        BowStringTransform = transform.GetChild(transform.childCount - 1);
        BowStringSpriteRenderer = BowStringTransform.GetComponent<SpriteRenderer>(); //bow string should be the last child
    }

    public override float Attack(Transform fromhere, GameObject Projectile, bool ShotByPlayer) //we arent going to use Projectile as bow has its own unique projectile that will be used
    {
        if (CurrentHeat == 0)
        {
            PlayShootingSound();
            BowStringSpriteRenderer.sprite = Sprites[Sprites.Length - 1];
            BowStringTransform.GetChild(0).gameObject.SetActive(true);
        }
        FromHere = fromhere;
        isShotByPlayer = ShotByPlayer;
        if (CurrentHeat < HeatMax)
        {
            CurrentHeat += Time.deltaTime;
            float ScaleM = CurrentHeat * 0.3f;
            SetBowStringScale(ScaleM);
        }
        return 0;
    }
    protected override void PlayShootingSound()
    {
        //int RandomShootingSound = Random.Range(0, ShootingSoundEffects.Length); //use if I decide to put in more sound
        AudioSourceField.clip = ShootingSoundEffects[0];
        //Debug.Log(ShootingSoundEffects[RandomShootingSound].name);
        AudioSourceField.Play();
        //Debug.Log("Playing Revolver Sound");
    }

    public override void SetWeaponHeat(float ElapsedTime)
    {
        if (CurrentHeat > 0)
        {

            GameObject BulletShot = Instantiate(ArrowPrefab, FromHere.position, FromHere.rotation);
            BulletShot.GetComponent<Bullet>().ShotBy(isShotByPlayer); //shot by enemy or player
            BulletShot.GetComponent<Bullet>().AssignDamage((int)(Damage * CurrentHeat));
            BulletShot.GetComponent<Bullet>().AssignVelocity((int)(20 + (BulletVelocity * CurrentHeat)));
            BulletShot.GetComponent<Bullet>().AssignPierce((int)(Piercing * CurrentHeat) + 1 + Globalvariables.BulletPierce);

            int IsCrit = Random.Range(0, 101);

            if (IsCrit <= CritRate)
                BulletShot.GetComponent<Bullet>().SetCrit(true);
            //Debug.Log((int)(BulletVelocity * CurrentHeat));
            if (CurrentHeat >= HeatMax)
            {
                AudioSourceField.clip = ArrowSFX[1];
                AudioSourceField.Play();
            }
            else
            {
                GetComponent<AudioSource>().clip = ArrowSFX[0];
                AudioSourceField.Play();
            }

            SetNewBulletsLeft(1); //set new bullets left (-1)
            SetBowStringScale(0.1f);

            BowStringSpriteRenderer.sprite = Sprites[0];
            BowStringTransform.GetChild(0).gameObject.SetActive(false);

            CurrentHeat = 0;
        }
    }

    void SetBowStringScale(float ScaleM) //scale of bow
    {
        BowStringTransform.localScale = new Vector3(ScaleM, 1, 1);
        for (int i = 0; i < BowStringTransform.childCount; ++i)
        {
            BowStringTransform.GetChild(i).localScale = new Vector3(BOWSCALE * (1 / ScaleM), BOWSCALE, 1); //replace 3.333333333 by 1 / the bow's default scale
        }
    }
}
