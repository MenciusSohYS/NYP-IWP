using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxMagCapScript : BuffScript
{
    // Start is called before the first frame update
    public override void ApplyBuffs()
    {
        Player.GetComponent<PlayerMechanics>().IncreaseMaxMagCap(5);
        //Debug.Log("increased fire rate");
    }

    protected override void TellPickup()
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>().SetText("Increased Mag Capacity!", 1);
    }
}
