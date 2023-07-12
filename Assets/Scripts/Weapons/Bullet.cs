using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] int DamageToDo;
    public bool PlayerFriendly;
    [SerializeField] GameObject DamageNumber;
    public int VelocityOfBullet;
    private bool HitHalfCover;
    int piercecapped; //if have pierce cap do make this false
    [SerializeField] GameObject ExplosionEffect;

    // Start is called before the first frame update
    private void Start()
    {
        if (VelocityOfBullet < 0)
            VelocityOfBullet = 30;
        HitHalfCover = false;
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
        if (!ByPlayer)
        {
            PlayerFriendly = false;
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
            PlayerFriendly = true;
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

        if (collision.transform.name.Contains("Bullet"))
        {
            return;
        }

        if (collision.transform.tag == "Player" && !PlayerFriendly)
        {
            if (HitHalfCover && collision.transform.name.Contains("Dwarf"))
                return;

            HitSomethingWithHealth = collision.transform.GetComponent<PlayerMechanics>().MinusHP(DamageToDo);
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
            HitSomethingWithHealth = true;
            --piercecapped;
        }
        else if (collision.transform.tag == "HalfCover")
        {
            float tempdmg = DamageToDo;
            tempdmg *= 0.5f;
            DamageToDo = (int)tempdmg;
            HitHalfCover = true;
        }
        else if (collision.transform.tag == "Fullcover")
        {
            piercecapped = 0;
        }
        else if (collision.transform.tag == "Walls")
            piercecapped = 0;
        else if (collision.transform.tag == "Melee")
            piercecapped = 0;

        if (HitSomethingWithHealth)
        {
            GameObject numberobject = Instantiate(DamageNumber, transform.position, Quaternion.identity);

            numberobject.GetComponent<DamageNumbers>().SetNumber(DamageToDo.ToString());
        }

        if (piercecapped <= 0)
        {
            if (ExplosionEffect != null)
                Instantiate(ExplosionEffect, transform.position, Quaternion.identity); //create an explosive if you can

            Destroy(gameObject);
        }
    }
}
