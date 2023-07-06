using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemScript : MonoBehaviour
{
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Price;
    public TextMeshProUGUI MaxedText;
    public TMP_InputField AmountOwnedText;
    public Image Image;
    public GameObject ImageGO;
    private int OriginalAmount;
    private int currentcost;
    private string ItemID;
    private GameObject CanvasGO;
    public GameObject Upgradeables;

    private void Start()
    {
        if (OriginalAmount < 1)
        {
            OriginalAmount = 0;
            AmountOwnedText.text = "0";
        }
    }

    public void SetDescription(string newstring)
    {
        //Debug.Log(newstring);
        Description.text = newstring;
    }
    public void SetItemID(string newstring)
    {
        //Debug.Log(newstring);
        ItemID = newstring;
    }

    public void SetItemPrice(int newint)
    {
        currentcost = newint;
        Price.text = "0 Coins";
    }
    public void SetCanvasGO(GameObject Canvas)
    {
        CanvasGO = Canvas;
    }

    public void ChangeItemTotalCost(int amount)
    {
        Price.text = (amount * currentcost) + " Coins";
    }

    public void SetItemName(string newstring)
    {
        Name.text = newstring;
    }
    public void SetImage(Texture2D newImage)
    {
        Sprite sprite = Sprite.Create(newImage, new Rect(0, 0, newImage.width, newImage.height), Vector2.one * 0.5f); //convert the texture2d to a sprite

        Image.sprite = sprite;
    }

    public void SetItemAmount(int newAmount) //may not be referenced upon creation
    {
        if (newAmount == 5)
        {
            Upgradeables.SetActive(false);
            MaxedText.gameObject.SetActive(true);
        }

        AmountOwnedText.GetComponent<InputfieldScript>().SetMinimumNumber(newAmount);
        OriginalAmount = newAmount;
        AmountOwnedText.text = newAmount.ToString();
    }

    public void InfoButtonClick()
    {
        if (ImageGO.activeSelf)
        {
            ImageGO.SetActive(false);
            Description.gameObject.SetActive(true);
        }
        else
        {
            ImageGO.SetActive(true);
            Description.gameObject.SetActive(false);
        }
    }
    public void BuyMore()
    {
        AmountOwnedText.text = (int.Parse(AmountOwnedText.text) + 1).ToString();
    }

    public void BuyLess()
    {
        AmountOwnedText.text = (int.Parse(AmountOwnedText.text) - 1).ToString();
    }

    public void Buy()
    {
        if (int.Parse(AmountOwnedText.text) != 0 && currentcost * (int.Parse(AmountOwnedText.text) - OriginalAmount) <= int.Parse(CanvasGO.GetComponent<CharacterSelectScript>().Coins.text))
        {
            PlayFabHandler.BuySkill(ItemID, currentcost, 0, int.Parse(AmountOwnedText.text) - OriginalAmount); //buy using the function
            CanvasGO.GetComponent<CharacterSelectScript>().UpdateCoins(currentcost * (int.Parse(AmountOwnedText.text) - OriginalAmount)); //update the current coins
            SetItemAmount(int.Parse(AmountOwnedText.text));
            CanvasGO.GetComponent<ShopScript>().SetText("Bought");
        }
        else if (int.Parse(AmountOwnedText.text) != 0)
        {
            CanvasGO.GetComponent<ShopScript>().SetText("Not Enough Money!");
        }
    }
}
