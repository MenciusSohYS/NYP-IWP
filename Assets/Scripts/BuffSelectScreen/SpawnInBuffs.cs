using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnInBuffs : MonoBehaviour
{
    public GameObject BuffPrefab;
    public GameObject WhereToPlaceBuffs;
    public TextMeshProUGUI DescriptionOfBuff;

    public Sprite[] ArrayOfSprites;
    public string[] ArrayOfNames;
    public string[] ArrayOfDescription;

    private void Start()
    {
        int AlreadyUsedBuff = -1;
        for (int i = 0; i < 2; ++i)
        {
            GameObject NewBuff = Instantiate(BuffPrefab, transform.position, Quaternion.identity);
            NewBuff.transform.SetParent(WhereToPlaceBuffs.transform, false);

            bool SuitableBuff = false;
            while (!SuitableBuff)
            {
                int RandomBuff = Random.Range(0, ArrayOfNames.Length); //randomise

                if (AlreadyUsedBuff == RandomBuff)
                    continue;

                SuitableBuff = true;
                switch (RandomBuff) //check if its a 1 time only buff
                {
                    case 0:
                        if (Globalvariables.FlamingBullet)
                        {
                            SuitableBuff = false;
                            Debug.Log("Not Suitable");
                            break;
                        }
                        break;
                    case 2:
                        if (Globalvariables.RadialShield)
                        {
                            SuitableBuff = false;
                            Debug.Log("Not Suitable");
                            break;
                        }
                        break;
                    default:
                        break;
                }

                if (SuitableBuff) //if it is okay to give to player assign the values
                {
                    NewBuff.GetComponent<BuffSelect>().Assign(ArrayOfNames[RandomBuff], ArrayOfSprites[RandomBuff], ArrayOfDescription[RandomBuff]);

                    NewBuff.GetComponent<Button>().onClick.AddListener(() =>ChangeDescription(NewBuff));

                    AlreadyUsedBuff = RandomBuff;
                }
            }
        }
    }

    public void ChangeDescription(GameObject Buffselect)
    {
        DescriptionOfBuff.text = Buffselect.GetComponent<BuffSelect>().GetDescription();
    }

    public void Continue()
    {
        if (DescriptionOfBuff.text.Contains("burn"))
        {
            Globalvariables.FlamingBullet = true;
        }
        else if (DescriptionOfBuff.text.Contains("pierce"))
        {
            ++Globalvariables.BulletPierce;
        }
        else if (DescriptionOfBuff.text.Contains("radial blast"))
        {
            Globalvariables.RadialShield = true;
        }
        else if (DescriptionOfBuff.text.Contains("Skill cooldown"))
        {
            ++Globalvariables.SkillCooldown;
        }
        else if (DescriptionOfBuff.text.Contains("damage over time"))
        {
            ++Globalvariables.DOTStacks;
            Debug.Log(Globalvariables.DOTStacks);
        }
        else if (DescriptionOfBuff.text.Contains("effectiveness"))
        {
            ++Globalvariables.HealthOrb;
            Debug.Log(Globalvariables.HealthOrb);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}