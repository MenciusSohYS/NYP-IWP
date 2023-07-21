using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public static class PlayFabHandler
{
    public static bool UnlockedMercenary = false;
    public static bool UnlockedElf = false;
    public static bool UnlockedBounty = true;
    public static bool UnlockedDwarf = false;
    public static bool UnlockedWizard = false;


    public static float BGMSliderValue = 1;
    public static float WeaponSliderValue = 1;
    public static int HighScore = 0;

    public struct PlayerBoughtSkills
    {
        public string Name;
        public int StackAmount;
    };

    public static List<PlayerBoughtSkills> SkillList;

    //playfab things
    public static int Coins = 0;
    public static string PlayFabID;
    public struct Character
    {
        public string Name;
        public int Cost;
        public string Description;
        public GameObject CharacterGO;
    };

    public static List<Character> Characters;

    public static void GetVirtualCurrencies() //call playfab to get currency
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            r =>
            {
                Coins = r.VirtualCurrency["CN"];
                GetPlayerInventory();
            }, OnError);
    }

    public static void GetCatalog()
    {
        Characters = new List<Character>();

        var catreq = new GetCatalogItemsRequest
        {
            CatalogVersion = "Characters" //get from characters
        };
        PlayFabClientAPI.GetCatalogItems(catreq,
            result =>
            {
                List<CatalogItem> items = result.Catalog;

                foreach (CatalogItem i in items) //everytime we find a skill, we add it to the list in global variables
                {
                    Characters.Add(new Character { Name = i.DisplayName, Cost = (int)i.VirtualCurrencyPrices["CN"], Description = i.Description });
                    //Debug.Log(i.DisplayName);
                }

                UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
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
                    else if (i.DisplayName == "Elf")
                        UnlockedElf = true;
                    else if (i.DisplayName == "Dwarf")
                        UnlockedDwarf = true;
                    else if (i.DisplayName == "Wizard")
                        UnlockedWizard = true;
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
                    GetCatalog();

            }, OnError);
    }

    public static void BuyCharacter(string NameOfCharacter, int cost)
    {
        var buyreq = new PurchaseItemRequest
        {
            CatalogVersion = "Characters",
            ItemId = NameOfCharacter,
            VirtualCurrency = "CN",
            Price = cost
        };
        PlayFabClientAPI.PurchaseItem(buyreq,
            result =>
            {
                Debug.Log("Bought!");

                if (NameOfCharacter == "Mercenary")
                    UnlockedMercenary = true;
                else if (NameOfCharacter == "Elf")
                    UnlockedElf = true;
                else if (NameOfCharacter == "Dwarf")
                    UnlockedDwarf = true;
                else if (NameOfCharacter == "Wizard")
                    UnlockedWizard = true;

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
                //Debug.Log("ADDED "+ updatereq.Amount + " DOLLARS");
                Coins += AmountToAdd;

            }, OnError);
    }

    static void OnError(PlayFabError e)
    {
        Debug.Log("ERROR WITH PLAYFAB");
    }

    public static void LogOut()
    {
        PushAudioPreferences();
        UnlockedMercenary = false;
        UnlockedElf = false;
        UnlockedBounty = true;
        UnlockedDwarf = false;
        UnlockedWizard = false;
        Coins = 0;
        PlayFabID = "";
        SkillList = new List<PlayerBoughtSkills>();
        Characters = new List<Character>();
        BGMSliderValue = 1;
        WeaponSliderValue = 1;
        HighScore = 0;
        Globalvariables.ForgetEverything();
        PlayFabClientAPI.ForgetAllCredentials();
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
    }

    public static void PushAudioPreferences()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"BGM", BGMSliderValue.ToString() },
                    {"Weapon", WeaponSliderValue.ToString() }
                }
        },
            result => Debug.Log("Pushed " + BGMSliderValue + " and "+ WeaponSliderValue),
            error =>
            {
                Debug.Log("Error in getting user data");
                Debug.Log(error.GenerateErrorReport());
            });
    }

    public static void PushScore(int ScoreToCompare)
    {
        if (ScoreToCompare < HighScore)
        {
            return;
        }
        Debug.Log("Higher Score");
        HighScore = ScoreToCompare;

        var req = new PlayFab.ClientModels.UpdatePlayerStatisticsRequest
        {
            Statistics = new List<PlayFab.ClientModels.StatisticUpdate>
            {
                new PlayFab.ClientModels.StatisticUpdate
                {
                    StatisticName = "HighScore",
                    Value=HighScore
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(req, 
            result=>
            {
                //Debug.Log("Successful leaderboard sent" + result.ToString());
            }
            , OnError);
    }

}
