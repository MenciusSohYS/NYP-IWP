using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendUpgradeToPlayer : MonoBehaviour
{
    public CanvasScript CanvasScript;

    public void UpgradePlayer(string Name)
    {
        GunScript PlayerGS = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<GunScript>();

        if (Name == "Increase Mag Cap")
        {
            PlayerGS.IncreaseMaxMagCap(10);
        }
        else if (Name == "Increase Reload Speed")
        {
            PlayerGS.ReduceRelaodBy(0.9f);
        }
        else if (Name == "Increase Accuracy")
        {
            PlayerGS.ReduceSpread(0.9f);
        }
        else if (Name == "Increase Fire Rate")
        {
            PlayerGS.ChangeFireRate(0.7f);
        }
        else if (Name == "Increase Damage")
        {
            PlayerGS.IncreaseDamage(1.2f);
        }
        else if (Name == "Increase Bullet Pierce")
        {
            PlayerGS.IncreasePierce(1);
        }
        else if (Name == "Increase Bullet Velocity")
        {
            PlayerGS.IncreaseVelocity(1.2f);
        }
        else if (Name == "Increase Crit Rate")
        {
            PlayerGS.IncreaseCrit(10);
        }

        CanvasScript.CloseUpgradePanel();
    }
}
