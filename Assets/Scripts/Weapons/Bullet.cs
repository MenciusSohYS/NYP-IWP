using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] int DamageToDo;
    public bool PlayerFriendly;
    [SerializeField] GameObject DamageNumber;
    public int VelocityOfBullet;
    [SerializeField] int piercecapped; //if have pierce cap do make this false
    [SerializeField] GameObject ExplosionEffect;
    [SerializeField] bool Crit;

    // Start is called before the first frame update
    private void Start()
    {
        if (VelocityOfBullet < 0)
            VelocityOfBullet = 30;
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3.0f);

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = transform.up * VelocityOfBullet;
    }

    public void ShotBy(bool ByPlayer)
    {
        SpriteRenderer SP = GetComponent<SpriteRenderer>();

        if (!ByPlayer)
        {
            PlayerFriendly = false;
            SP.material.SetColor("_SpriteColor", Color.red);
        }
        else
        {
            PlayerFriendly = true;
        }
        SP.material.SetFloat("_OutlineWidth", 1);
        SP.material.SetColor("_OutlineColor", Color.cyan);
    }

    public void ShotByNoOutline(bool ByPlayer)
    {
        SpriteRenderer SP = GetComponent<SpriteRenderer>();

        if (!ByPlayer)
        {
            PlayerFriendly = false;
            SP.material.SetColor("_SpriteColor", Color.red);
        }
        else
        {
            PlayerFriendly = true;
        }
    }

    public void SetCrit(bool IsCrit)
    {
        Crit = IsCrit;
        DamageToDo *= 2;
    }
    public void AssignDamage(int Dmg)
    {
        DamageToDo = Dmg;
    }
    public void AssignVelocity(int Velocity)
    {
        VelocityOfBullet = Velocity;
    }

    public void AssignPierce(int AmountAvailable)
    {
        piercecapped = AmountAvailable;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool HitSomethingWithHealth = false;

        if (collision.transform.GetComponent<Bullet>())
        {
            return;
        }

        if (collision.transform.tag == "Player" && !PlayerFriendly)
        {
            HitSomethingWithHealth = collision.transform.GetComponent<PlayerMechanics>().MinusHP(DamageToDo);

            if (!collision.transform.GetComponent<PlayerMechanics>().IsTouchinghalfCover())
                collision.GetComponent<Rigidbody2D>().AddForce(rb.velocity * 5);

            --piercecapped;
        }
        else if (collision.transform.tag == "Enemy" && PlayerFriendly)
        {
            float DR = collision.transform.GetComponent<EnemyMechanics>().ReturnDR();
            if (DR > 0)
            {
                //Debug.Log("Old Damage: " + DamageToDo);
                float tempfloat = DamageToDo * DR;
                DamageToDo = (int)tempfloat;
                //Debug.Log("New Damage: " + DamageToDo);
            }

            collision.transform.GetComponent<EnemyMechanics>().MinusHP(DamageToDo);
            if (Globalvariables.FlamingBullet)
            {
                collision.transform.GetComponent<EnemyMechanics>().SetDOT((int)(DamageToDo * 0.3)); //do 1/3 of original damage
            }
            HitSomethingWithHealth = true;
            --piercecapped;
        }
        else if (collision.transform.tag == "HalfCover" && !collision.isTrigger)
        {
            float tempdmg = DamageToDo;
            tempdmg *= 0.5f;
            DamageToDo = (int)tempdmg;
        }
        else if (collision.transform.tag == "Fullcover" && !collision.isTrigger)
        {
            piercecapped = 0;
        }
        else if (collision.transform.tag == "OuterWalls")
        {
            piercecapped = 0;
        }
        else if (collision.transform.tag == "Melee")
            piercecapped = 0;
        else if (collision.transform.tag == "Shield" && !PlayerFriendly)
            piercecapped = 0;
        else if (collision.transform.tag == "BossShield" && PlayerFriendly)
            piercecapped = 0;

        if (HitSomethingWithHealth)
        {
            GameObject numberobject = Instantiate(DamageNumber, transform.position, Quaternion.identity);

            string DamageNum = "";

            if (Crit)
            {
                DamageNum += "CRIT!\n";
            }
            DamageNum += DamageToDo.ToString();

            numberobject.GetComponent<DamageNumbers>().SetNumber(DamageNum, Crit);
        }

        if (piercecapped <= 0)
        {
            if (ExplosionEffect != null)
            {
                GameObject Explosion = Instantiate(ExplosionEffect, transform.position, Quaternion.identity); //create an explosive if you can
                if (Explosion.GetComponent<AOEScript>()) //if aoe script exists
                {
                    Explosion.GetComponent<AOEScript>().AssignDTD((int)(DamageToDo * 0.5f + (Globalvariables.BulletPierce * 0.1f)));
                    //Debug.Log("Exists");
                }
            }

            Destroy(gameObject);
        }
    }

}
