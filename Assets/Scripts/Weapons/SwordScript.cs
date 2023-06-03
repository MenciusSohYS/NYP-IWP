using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : WeaponParent
{
    public SwordScript()
    {
        Damage = 70;
        FireRate = 0.5f; //duration for 1 swing
        Spread = 0.1f;
        BulletsRemaining = 1;
        MaxMagSize = 1;
        ReloadTime = 0.01f; //cool down before attacking again
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
    private string[] animationNames = { "SwordAttack", "SwordAttack2" }; //append here
    private TrailRenderer Trail;

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
            //Debug.Log("Animation duration: " + duration + " seconds");
            Animations.Add(clip.length);
        }

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

        //Debug.Log(AttackNumber);
        //SwordCollider.isTrigger = false;
        AnimatorField.SetInteger("Attack", AttackNumber); //attack number is the sequence of the attack, if one combo has 5 attacks then maximum number should be 4

        //Debug.Log(FireRate + "*" + Animations[AttackNumber]);
        AnimatorField.SetFloat("Speed",  Animations[AttackNumber] / FireRate); //converts the animation to the proper multiplier
        //Debug.Log("NumberOfAttacks[" + AttackNumber + "] out of " + NumberOfAttacks.Count);

        timerforattacking = FireRate; //timerforattacking is to tell the sword if the attack is over

        Trail.emitting = true;
        return FireRate + ReloadTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && isAttacking)
        {
            collision.transform.GetComponent<EnemyMechanics>().MinusHP(Damage);

            GameObject numberobject = Instantiate(DamageNumber, transform.position, Quaternion.identity);

            numberobject.GetComponent<DamageNumbers>().SetNumber(Damage.ToString());
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

            if (AttackNumber > Animations.Count - 1)
            {
                AttackNumber = 0;
                //Debug.Log("NumberOfAttacks[" + AttackNumber + "] out of " + NumberOfAttacks.Count);
            }
            //Debug.Log("Time up");

            isAttacking = false;
            AnimatorField.SetInteger("Attack", 4);
            Trail.emitting = false;
            //SwordCollider.isTrigger = true;
        }
    }
}