using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHPScript : BuffScript
{
    // Start is called before the first frame update
    public override void ApplyBuffs()
    {
        Player.GetComponent<PlayerMechanics>().IncreaseMaxHP(50);
    }

    protected override void TellPickup()
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>().SetText("Picked Up Health!", 1);
    }
}
