using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : WeaponParent
{
    public SwordScript()
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
    public Sprite[] SwordSprites;

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

        //tesing
        {
        //    NumberOfAttacks = new List<(float, float)>();
        //    foreach (string AnimationName in animationNames)
        //    {
        //        AnimationClip CorrectClip = null;

        //        AnimationClip[] ArrayOfClips = AnimatorField.runtimeAnimatorController.animationClips;
        //        foreach (AnimationClip animationClip in ArrayOfClips)
        //        {
        //            if (animationClip.name == AnimationName)
        //            {
        //                CorrectClip = animationClip;
        //                break;
        //            }
        //        }

        //        if (CorrectClip != null)
        //        {
        //            float speed = 1f; // Default speed is 1

        //            AnimatorStateInfo AnimationInfo = AnimatorField.GetCurrentAnimatorStateInfo(0);
        //            if (AnimationInfo.IsName(CorrectClip.name))
        //            {
        //                speed = AnimationInfo.speed; // Get the speed from the current state
        //            }

        //            NumberOfAttacks.Add((CorrectClip.length, speed)); //length will be the first item and speed will be the second
        //            Debug.Log("Animation: " + AnimationName + " Duration: " + CorrectClip.length + " Speed: " + speed);
        //        }
        //        else
        //        {
        //            Debug.LogError("Clip not found: " + AnimationName);
        //        }
        //    }
        }
    }

    public override float Attack(Transform fromhere, GameObject Projectile, bool ShotByPlayer)
    {
        //using
        isAttacking = true;

        //Debug.Log(AnimatorField.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //SwordCollider.isTrigger = false;
        AnimatorField.SetInteger("Attack", AttackNumber); //attack number is the sequence of the attack

        //Debug.Log(FireRate + "*" + Animations[AttackNumber]);
        AnimatorField.SetFloat("Speed",  Animations[AttackNumber] / FireRate); //converts the animation to the proper multiplier
        //Debug.Log("NumberOfAttacks[" + AttackNumber + "] out of " + NumberOfAttacks.Count);
        MeleeSound();
        timerforattacking = FireRate; //timerforattacking is to tell the sword if the attack is over

        Trail.emitting = true;

        return FireRate + 0.1f;
    }

    public void MeleeSound()
    {
        if (AudioSourceField.clip != ShootingSoundEffects[AttackNumber])
        {
            AudioSourceField.clip = ShootingSoundEffects[AttackNumber];
            AudioSourceField.Play();
            ++AttackNumber; //increase the value so that the next time we attack its the next pattern

            if (AttackNumber > Animations.Count - 1)
            {
                AttackNumber = 0;
                //Debug.Log("NumberOfAttacks[" + AttackNumber + "] out of " + NumberOfAttacks.Count);
            }
        }
    }

    public override void SetCurrentUpgrades(int ByHowMuch)
    {
        base.SetCurrentUpgrades(ByHowMuch);
        if (CurrentUpgrade > 0)
        {
            GetComponent<SpriteRenderer>().sprite = SwordSprites[1];
            GetComponent<BoxCollider2D>().size = new Vector2 (2, 4.5f);
            GetComponent<BoxCollider2D>().offset = new Vector2 (3, -0.7f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && IsPlayer)
        {
            int DamageToDo = Damage;
            int IsCrit = Random.Range(0, 101);
            bool DidCrit = false;

            if (IsCrit <= CritRate)
            {
                DidCrit = true;
                DamageToDo = (int)(DamageToDo * 1.5f);
            }

            collision.transform.GetComponent<EnemyMechanics>().MinusHP(DamageToDo);

            GameObject numberobject = Instantiate(DamageNumber, transform.position, Quaternion.identity);

            numberobject.GetComponent<DamageNumbers>().SetNumber(DamageToDo.ToString(), DidCrit);
        }
        else if (collision.transform.tag == "Player" && !IsPlayer)
        {
            collision.transform.GetComponent<PlayerMechanics>().MinusHP(Damage);

            GameObject numberobject = Instantiate(DamageNumber, transform.position, Quaternion.identity);

            numberobject.GetComponent<DamageNumbers>().SetNumber(Damage.ToString(), false);
        }
    }

    public void SetBoxActive(bool SetTo)
    {
        SwordCollider.enabled = SetTo;
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