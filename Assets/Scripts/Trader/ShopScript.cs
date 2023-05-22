using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class ShopScript : MonoBehaviour
{
    public GameObject ShopContentArea;
    public GameObject ShopItemPrefab;
    public GameObject ShopPanel;
    public TextMeshProUGUI ShopNotif;
    private float TimerForNotif;
    private bool NotifTrue;
    // Start is called before the first frame update
    void Start()
    {
        GetCatalog();
        ShopPanel.SetActive(false);
    }

    private void Update()
    {
        if (!NotifTrue)
            return;

        TimerForNotif -= Time.deltaTime;
        if (TimerForNotif <= 0)
        {
            NotifTrue = false;
            ShopNotif.text = "";
        }
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

        ShopUI.GetComponent<ItemScript>().SetItemName(name);
        ShopUI.GetComponent<ItemScript>().SetItemPrice(Price);
        ShopUI.GetComponent<ItemScript>().SetDescription(Description);
        ShopUI.GetComponent<ItemScript>().SetItemID(ItemID);
        ShopUI.GetComponent<ItemScript>().SetCanvasGO(gameObject);

        for (int i = 0; i < PlayFabHandler.SkillList.Count; ++i)
        {
            if (PlayFabHandler.SkillList[i].Name == name)
            {
                ShopUI.GetComponent<ItemScript>().SetItemAmount(PlayFabHandler.SkillList[i].StackAmount);
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
