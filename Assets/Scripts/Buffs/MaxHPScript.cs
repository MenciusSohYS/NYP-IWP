using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHPScript : BuffScript
{
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void ApplyBuffs()
    {
        Player.GetComponent<PlayerMechanics>().IncreaseMaxHP(50);
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
