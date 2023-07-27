using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateIncrease : BuffScript
{
    // Start is called before the first frame update
    public override void ApplyBuffs()
    {
        Player.GetComponent<PlayerMechanics>().IncreaseFireRate(0.8f);
        //Debug.Log("increased fire rate");
    }

    protected override void TellPickup()
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>().SetText("Fire Rate Up!", 1);
    }
}
