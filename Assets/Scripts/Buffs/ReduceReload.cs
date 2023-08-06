using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceReload : BuffScript
{
    // Start is called before the first frame update
    public override void ApplyBuffs()
    {
        Player.GetComponent<PlayerMechanics>().ReduceReload(0.95f);
        //Debug.Log("increased fire rate");
    }

    protected override void TellPickup()
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>().SetText("Reduce Reload Time", 1);
    }
}
