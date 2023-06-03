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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool HitSomethingWithHealth = false;
        bool piercecapped = false; //if have pierce cap do make this false

        if (collision.transform.name.Contains("Bullet"))
        {
            return;
        }

        if (collision.transform.tag == "Player" && !PlayerFriendly)
        {
            HitSomethingWithHealth = collision.transform.GetComponent<PlayerMechanics>().MinusHP(DamageToDo);
            collision.GetComponent<Rigidbody2D>().AddForce(rb.velocity * 5);
            piercecapped = true;
        }
        else if (collision.transform.tag == "Enemy" && PlayerFriendly)
        {
            collision.transform.GetComponent<EnemyMechanics>().MinusHP(DamageToDo);
            HitSomethingWithHealth = true;
            piercecapped = true;
        }
        else if (collision.transform.tag == "HalfCover")
        {
            float tempdmg = DamageToDo;
            tempdmg *= 0.5f;
            DamageToDo = (int)tempdmg;
        }
        else if (collision.transform.tag == "FullCover")
        {
            piercecapped = true;
        }
        else if (collision.transform.tag == "Walls")
            piercecapped = true;
        else if (collision.transform.tag == "Melee")
            piercecapped = true;

        if (HitSomethingWithHealth)
        {
            GameObject numberobject = Instantiate(DamageNumber, transform.position, Quaternion.identity);

            numberobject.GetComponent<DamageNumbers>().SetNumber(DamageToDo.ToString());
        }

        if (piercecapped)
        {
            Destroy(gameObject);
        }
    }
}
