using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] int DamageToDo;
    public bool PlayerFriendly;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = transform.up * 30;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && !PlayerFriendly)
        {
            collision.transform.GetComponent<PlayerMechanics>().MinusHP(DamageToDo);
            Destroy(gameObject); //dont destroy if have pierce
        }
        else if (collision.transform.tag == "Enemy" && PlayerFriendly)
        {
            Destroy(gameObject);
        }
    }
}