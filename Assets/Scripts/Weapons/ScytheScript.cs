using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheScript : WeaponParent
{
    public ScytheScript()
    {
        Damage = 50;
        FireRate = 0.5f; //duration for 1 swing
        Spread = 0.1f;
        BulletsRemaining = 1;
        MaxMagSize = 1;
        ReloadTime = 0.1f; //cool down before attacking again
        HeatMax = 0.1f;
        PositionToParent = new Vector3(0, 1f, 0);
        isMelee = true;
    }

    bool isAttacking;
    float timerforattacking;
    private Animator AnimatorField;
    private BoxCollider2D SwordCollider;
    public GameObject DamageNumber;
    //private List<(float, float)> NumberOfAttacks;
    private List<float> Animations;
    private int AttackNumber;
    //private string[] animationNames = { "SwordAttack", "SwordAttack2" }; //append here
    private TrailRenderer Trail;
    private bool IsPlayer;
    public GameObject SlashWave;
    public GameObject SlashSpawnPoint;

    private void Start()
    {
        Animations = new List<float>();
        AttackNumber = 0;
        AnimatorField = transform.parent.GetComponent<Animator>();
        SwordCollider = transform.GetComponent<BoxCollider2D>();
        Trail = transform.GetChild(0).GetComponent<TrailRenderer>();
        Trail.emitting = false;

        AnimationClip[] AnimClips = AnimatorField.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in AnimClips)
        {
            if (clip.name.Contains("Idle"))
                continue;

            Animations.Add(clip.length);
        }

        AudioSourceField = GetComponent<AudioSource>();
        AudioSourceField.volume = 0.5f;
        AssignAtStart();
        IsPlayer = true;

    }

    public override float Attack(Transform fromhere, GameObject Projectile, bool ShotByPlayer)
    {
        //using
        isAttacking = true;

        //Debug.Log(AnimatorField.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //SwordCollider.isTrigger = false;
        if (AttackNumber == 0)
        {
            GameObject BulletShot = Instantiate(SlashWave, SlashSpawnPoint.transform.position, transform.parent.parent.rotation);
            BulletShot.GetComponent<Bullet>().ShotByNoOutline(ShotByPlayer); //shot by enemy or player
            BulletShot.GetComponent<Bullet>().AssignDamage(Damage);
            BulletShot.GetComponent<Bullet>().AssignVelocity(20);
        }

        AnimatorField.SetInteger("Attack", AttackNumber); //attack number is the sequence of the attack

        //Debug.Log("NumberOfAttacks[" + AttackNumber + "] out of " + NumberOfAttacks.Count);
        MeleeSound();
        timerforattacking = Animations[AttackNumber]; //timerforattacking is to tell the sword if the attack is over

        Trail.emitting = true;

        return Animations[AttackNumber] + 0.1f;
    }

    void MeleeSound()
    {
        if (AudioSourceField.clip != ShootingSoundEffects[AttackNumber])
        {
            AudioSourceField.clip = ShootingSoundEffects[AttackNumber];
            AudioSourceField.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking)
        {
            if (collision.gameObject.tag == "Enemy" && IsPlayer)
            {
                collision.transform.GetComponent<EnemyMechanics>().MinusHP(Damage);

                GameObject numberobject = Instantiate(DamageNumber, transform.position, Quaternion.identity);

                numberobject.GetComponent<DamageNumbers>().SetNumber(Damage.ToString());
            }
            else if (collision.transform.tag == "Player" && !IsPlayer)
            {
                collision.transform.GetComponent<PlayerMechanics>().MinusHP(Damage);

                GameObject numberobject = Instantiate(DamageNumber, transform.position, Quaternion.identity);

                numberobject.GetComponent<DamageNumbers>().SetNumber(Damage.ToString());
            }
        }
    }

    private void Update()
    {
        if (!isAttacking)
            return;

        timerforattacking -= Time.deltaTime;
        //Debug.Log(timerforattacking);
        if (timerforattacking < 0)
        {
            ++AttackNumber; //increase the value so that the next time we attack its the next pattern

            if (AttackNumber > Animations.Count - 2)
            {
                AttackNumber = 0;
                //Debug.Log("NumberOfAttacks[" + AttackNumber + "] out of " + NumberOfAttacks.Count);
            }
            //Debug.Log(AttackNumber);

            isAttacking = false;
            AnimatorField.SetInteger("Attack", 4); //4 sets it back to idle, there's actually only 2 animations
            Trail.emitting = false;
            //SwordCollider.isTrigger = true;
        }
        else
        {
            //Debug.Log(AnimatorField.GetCurrentAnimatorStateInfo(0).normalizedTime + " " + AttackNumber);
        }
    }

    public void SetIsPlayer(bool setto)
    {
        IsPlayer = setto;
    }
}