using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public static class PlayFabHandler
{
    public static bool UnlockedMercenary = false;
    public static bool UnlockedBounty = true;

    public struct PlayerBoughtSkills
    {
        public string Name;
        public int StackAmount;
    };

    public static List<PlayerBoughtSkills> SkillList;

    //playfab things
    public static int Coins = 0;
    public static string PlayFabID;

    public static void GetVirtualCurrencies() //call playfab to get currency
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            r =>
            {
                Coins = r.VirtualCurrency["CN"];
                GetPlayerInventory();
            }, OnError);
    }

    public static void GetPlayerInventory()
    {
        SkillList = new List<PlayerBoughtSkills>();

        var UserInv = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(UserInv,
            result =>
            {
                List<ItemInstance> ii = result.Inventory;

                foreach (ItemInstance i in ii)
                {
                    if (i.DisplayName == "Mercenary")
                        UnlockedMercenary = true;
                    else if (i.CatalogVersion == "Skills") //if we find one belonging to the skills catalogue, we will want to create a struct about it
                    {
                        SkillList.Add(new PlayerBoughtSkills
                        {
                            Name = i.DisplayName,
                            StackAmount = i.RemainingUses ?? 0 //a int? is a nullable int so we need to use ?? incase its a null to default it to a zero
                        });
                    }
                }

                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LoginScene")
                    UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");

            }, OnError);
    }

    public static void BuyCharacter(string NameOfCharacer, int cost)
    {
        var buyreq = new PurchaseItemRequest
        {
            CatalogVersion = "Characters",
            ItemId = NameOfCharacer,
            VirtualCurrency = "CN",
            Price = cost
        };
        PlayFabClientAPI.PurchaseItem(buyreq,
            result =>
            {
                Debug.Log("Bought!");
                UnlockedMercenary = true;
                GetVirtualCurrencies();
            }, OnError);
    }
    public static void BuySkill(string NameOfSkill, int cost, int TimesDone, int DoThisAmountOfTimes)
    {
        if (TimesDone < DoThisAmountOfTimes)
        {
            var buyreq = new PurchaseItemRequest
            {
                CatalogVersion = "Skills",
                ItemId = NameOfSkill,
                VirtualCurrency = "CN",
                Price = cost
            };
            PlayFabClientAPI.PurchaseItem(buyreq,
                result =>
                {
                    Debug.Log("Bought!");
                    BuySkill(NameOfSkill, cost, ++TimesDone, DoThisAmountOfTimes);
                }, OnError);
        }
        else
            GetVirtualCurrencies();
    }

    public static void UpdateMoney(int AmountToAdd)
    {
        var updatereq = new AddUserVirtualCurrencyRequest
        {
            Amount = AmountToAdd,
            VirtualCurrency = "CN"
        };
        PlayFabClientAPI.AddUserVirtualCurrency(updatereq,
            result =>
            {
                Debug.Log("ADDED "+ updatereq.Amount + " DOLLARS");
                Coins += AmountToAdd;

            }, OnError);
    }

    static void OnError(PlayFabError e)
    {
        Debug.Log("ERROR WITH PLAYFAB");
    }
}
