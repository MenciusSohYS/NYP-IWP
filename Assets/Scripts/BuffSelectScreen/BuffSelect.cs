using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelect : MonoBehaviour
{
    public Image ImageOfBuff;
    public TextMeshProUGUI NameOfBuff;
    [SerializeField] string Description;

    public void Assign(string Name, Sprite NewSprite, string newdescription)
    {
        NameOfBuff.text = Name;
        ImageOfBuff.sprite = NewSprite;
        Description = newdescription;
    }

    public string GetDescription()
    {
        string tempstring = Description;
        if (NameOfBuff.text.Contains("Bullets Pierce"))
        {
            tempstring += "\n\nCurrent Amount: " + Globalvariables.BulletPierce;
        }
        else if (NameOfBuff.text.Contains("Skill Cooldown"))
        {
            tempstring += "\n\nCurrent Amount: " + Globalvariables.SkillCooldown;
        }
        else if (NameOfBuff.text.Contains("D.O.T lover"))
        {
            tempstring += "\n\nCurrent Amount: " + Globalvariables.DOTStacks;
        }
        else if (NameOfBuff.text.Contains("Increase Health"))
        {
            tempstring += "\n\nCurrent Amount: " + Globalvariables.HealthOrb;
        }

        return tempstring;
    }

    public void DoNothing()
    {

    }
}
