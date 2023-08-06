using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI TextField;
    public Image SpriteImage;
    CanvasScript CanvasScript;
    string Description;

    private void Start()
    {
        CanvasScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>();
    }
    public void Assigntext(string NewName)
    {
        if (NewName == "MaxMagIncrease")
        {
            TextField.text = "Increase Mag Cap";
            Description = "Increase Mag Cap\nIncreases Maximum Magazine Capacity by 10";
        }
        else if (NewName == "IncreaseReloadSpd")
        {
            TextField.text = "Increase Reload Speed";
            Description = "Increase Reload Speed\nDecreases Reload Time by 10%";
        }
        else if (NewName == "IncreaseAccuracy")
        {
            TextField.text = "Increase Accuracy";
            Description = "Increase Accuracy\nIncreases Accuracy by 10%";
        }
        else if (NewName == "IncreaseFireRate")
        {
            TextField.text = "Increase Fire Rate";
            Description = "Increase Fire Rate\nIncreases Fire Rate by 30%";
        }
        else if (NewName == "IncreaseDmg")
        {
            TextField.text = "Increase Damage";
            Description = "Increase Damage\nIncreases Damage by 20%";
        }
        else if (NewName == "BulletPierce")
        {
            TextField.text = "Increase Bullet Pierce";
            Description = "Increase Bullet Pierce\nIncreases Pierce by 1";
        }
        else if (NewName == "BulletVelocity")
        {
            TextField.text = "Increase Bullet Velocity";
            Description = "Increase Bullet Velocity\nIncreases Bullet Velocity by 20%";
        }
        else if (NewName == "IncreaseCritRate")
        {
            TextField.text = "Increase Crit Rate";
            Description = "Increases Crit Rate\nIncreases Critical Rate by flat 10";
        }
    }

    public void ClickedUpgrade()
    {
        transform.parent.GetComponent<SendUpgradeToPlayer>().UpgradePlayer(TextField.text);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CanvasScript.UpgradeDescriptionOpen(Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CanvasScript.UpgradeDescriptionClose();
    }
}
