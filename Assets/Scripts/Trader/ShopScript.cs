using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class ShopScript : MonoBehaviour
{
    public GameObject ShopContentArea;
    public GameObject ShopItemPrefab;
    public GameObject ShopPanel;
    public TextMeshProUGUI ShopNotif;
    public Texture2D[] ImagesForShop;
    [SerializeField] RectTransform UpgradeDescription;
    private float TimerForNotif;
    private bool NotifTrue;
    private Vector3 OffsetForUpgradeDescription;
    // Start is called before the first frame update
    void Start()
    {
        GetCatalog();
        ShopPanel.SetActive(false);
        UpgradeDescription.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (ShopPanel.activeSelf)
        {
            Vector3 positionToMove = Input.mousePosition;
            //Debug.Log(positionToMove);
            positionToMove.z = -10;
            float Y = UpgradeDescription.sizeDelta.y + 10;
            float X = UpgradeDescription.sizeDelta.x + 10;
            if (Input.mousePosition.x > Screen.width * 0.5f)
            {
                X = -X;
            }
            if (Input.mousePosition.y > Screen.height * 0.5f)
            {
                Y = -Y;
            }
            OffsetForUpgradeDescription = new Vector3(X, Y, 0);
            UpgradeDescription.position = (positionToMove + OffsetForUpgradeDescription);
        }

        if (!NotifTrue)
            return;

        TimerForNotif -= Time.deltaTime;
        if (TimerForNotif <= 0)
        {
            NotifTrue = false;
            ShopNotif.text = "";
        }
    }

    public void SetUpgradeTextOn(string NewText)
    {
        UpgradeDescription.gameObject.SetActive(true);
        UpgradeDescription.GetChild(0).GetComponent<TextMeshProUGUI>().text = NewText;
    }
    public void SetUpgradeTextOff()
    {
        UpgradeDescription.gameObject.SetActive(false);
    }

    public void GetCatalog()
    {
        var catreq = new GetCatalogItemsRequest
        {
            CatalogVersion = "Skills" //get from skills
        };
        PlayFabClientAPI.GetCatalogItems(catreq,
            result =>
            {
                List<CatalogItem> items = result.Catalog;

                foreach (CatalogItem i in items) //everytime we find a skill, we add it to the shop (by creating a gameobject to represent it
                {
                    //Debug.Log(i.DisplayName + "," + i.VirtualCurrencyPrices["CN"]);
                    CreateShopItem(i.DisplayName, (int)i.VirtualCurrencyPrices["CN"], i.Description, i.ItemId); //change it to string and push it to the creation
                }
            }, OnError);
    }
    void CreateShopItem(string name, int Price, string Description, string ItemID)
    {
        GameObject ShopUI = Instantiate(ShopItemPrefab, transform.position, Quaternion.identity);

        ShopUI.transform.SetParent(ShopContentArea.transform, false);

        ItemScript ShopItemScript = ShopUI.GetComponent<ItemScript>();

        ShopItemScript.SetItemName(name); //assign name
        ShopItemScript.SetItemPrice(Price); //assign price
        ShopItemScript.SetDescription(Description); //assign description
        ShopItemScript.SetItemID(ItemID); //assign itemid (hidden from player)
        ShopItemScript.SetCanvasGO(gameObject); //assign Canvas to refer to when we want to buy an item

        for (int i = 0; i < PlayFabHandler.SkillList.Count; ++i)
        {
            if (PlayFabHandler.SkillList[i].Name == name)
            {
                ShopItemScript.SetItemAmount(PlayFabHandler.SkillList[i].StackAmount); //assign the current stack amount of the item
            }
        }

        for (int i = 0; i < ImagesForShop.Length; ++i)
        {
            if (ImagesForShop[i].name == ItemID)
            {
                ShopItemScript.SetImage(ImagesForShop[i]); //assign image to the shop
            }
        }

        //Debug.Log(PlayFabHandler.SkillList[PlayFabHandler.SkillList.Count - 1].Name + " " + PlayFabHandler.SkillList[PlayFabHandler.SkillList.Count - 1].StackAmount);
    }

    void OnError(PlayFabError e)
    {
        Debug.Log("ERROR WITH PLAYFAB");
    }

    public void SetText(string newtext)
    {
        NotifTrue = true;
        TimerForNotif = 0.5f;
        ShopNotif.text = newtext;
    }
}
