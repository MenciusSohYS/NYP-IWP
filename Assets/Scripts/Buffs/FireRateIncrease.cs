using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateIncrease : BuffScript
{
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void ApplyBuffs()
    {
        Player.GetComponent<PlayerMechanics>().IncreaseFireRate(0.8f);
        //Debug.Log("increased fire rate");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            ApplyBuffs();
            Destroy(gameObject);
        }
    }
}
