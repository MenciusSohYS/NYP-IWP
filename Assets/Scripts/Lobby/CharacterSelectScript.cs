using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectScript : MonoBehaviour
{
    //enable audio listener, mechanics, movement, gunscript, flip sprite script
    [SerializeField] GameObject[] Characters2;
    [SerializeField] int CurrentIndex;
    public GameObject Camera;
    private GameObject Trader;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Coins;
    public GameObject Lock;
    public TextMeshProUGUI Notification;
    private bool ConfirmPurchase;
    private bool FadeOut;
    private int CurrentCost;

    public class HandlerNumberToLocalNumber
    {
        public int LocalNumber;
        public int HandlerNumber;
        public GameObject CharacterGO;
    }

    List<HandlerNumberToLocalNumber> handlerNumberToLocalNumber;

    // Start is called before the first frame update
    void Start()
    {
        CurrentCost = 0; //cost of buying the character
        FadeOut = false; //fade of the notification text
        ConfirmPurchase = false; //for the buying of characters
        //PlayFabHandler.GetPlayerInventory();
        //TxtHandler.CheckForCharacters(); //check what characters the player owns
        CurrentIndex = 0; //start from zero
        Characters2 = GameObject.FindGameObjectsWithTag("Player");

        handlerNumberToLocalNumber = new List<HandlerNumberToLocalNumber>();

        foreach (GameObject GO in Characters2)
        {
            handlerNumberToLocalNumber.Add(new HandlerNumberToLocalNumber { CharacterGO = GO });
            //Debug.Log(GO.name);
        }

        for (int i = 0; i < PlayFabHandler.Characters.Count; ++i)
        {
            for (int j = 0; j < handlerNumberToLocalNumber.Count; ++j)
            {
                if (PlayFabHandler.Characters[i].Name == handlerNumberToLocalNumber[j].CharacterGO.name)
                {
                    handlerNumberToLocalNumber[j].LocalNumber = j;
                    handlerNumberToLocalNumber[j].HandlerNumber = i;
                    //Debug.Log(handlerNumberToLocalNumber[j].CharacterGO.name);
                    //Debug.Log(PlayFabHandler.Characters[i].Name);
                    //they might have different array co ordinates so we need to use this class to sync them up
                }
            }
        }

        Trader = GameObject.FindGameObjectWithTag("Trader");
        Camera.transform.position = handlerNumberToLocalNumber[0].CharacterGO.transform.position - new Vector3(0, 0, 5); //shift the camera to where ever the first character is
        WriteText(); //lock camera to the first player the game can find and write its description
        Coins.text = PlayFabHandler.Coins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!FadeOut)
            return;

        Notification.color = Notification.color - new Color(0, 0, 0, Time.deltaTime);
        if (Notification.color.a < 0.1f)
        {
            Notification.gameObject.SetActive(false);
            FadeOut = false;
        }
        //reduce alpha channel for fade out
    }

    public void NextCharacter()
    {
        ++CurrentIndex;
        if (CurrentIndex > handlerNumberToLocalNumber.Count - 1)
        {
            CurrentIndex = 0;
        }
        Camera.transform.position = handlerNumberToLocalNumber[CurrentIndex].CharacterGO.transform.position - new Vector3(0, 0, 5);
        WriteText(); //loop through the list and find the next character to be potentially selected, write their description after finding them
    }
    public void PreviousCharacter()
    {
        --CurrentIndex;
        if (CurrentIndex < 0)
        {
            CurrentIndex = handlerNumberToLocalNumber.Count - 1;
        }
        Camera.transform.position = handlerNumberToLocalNumber[CurrentIndex].CharacterGO.transform.position - new Vector3(0, 0, 5);
        WriteText();
    }

    public void EnableCharacter()
    {
        for (int i = 0; i < handlerNumberToLocalNumber.Count; ++i)
        {
            if (i != CurrentIndex)
                Destroy(handlerNumberToLocalNumber[i].CharacterGO);
        }

        //if player chooses the character, destroy all the others and enable the chosen's scripts
        //then set the camera to follow it
        handlerNumberToLocalNumber[CurrentIndex].CharacterGO.GetComponent<AudioListener>().enabled = true;
        handlerNumberToLocalNumber[CurrentIndex].CharacterGO.GetComponent<PlayerMovement>().enabled = true;
        Camera.GetComponent<Camera>().fieldOfView = 80;
        Camera.GetComponent<CameraScript>().enabled = true;
        Camera.GetComponent<CameraScript>().SetPlayer(handlerNumberToLocalNumber[CurrentIndex].CharacterGO);
        //set trader's player
        Trader.GetComponent<TraderScript>().SetPlayer(handlerNumberToLocalNumber[CurrentIndex].CharacterGO);

        Description.transform.parent.gameObject.SetActive(false);//disable the panel
        Notification.gameObject.SetActive(false); //disable the notification text
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).GetComponent<Button>() != null)
            {
                transform.GetChild(i).gameObject.SetActive(false); //find each child in canvas and disable all those that has a button variable
            }
        }

        //lastly, push the gameobject to the global variables so that we know which to instaniate later
        Globalvariables.Playerprefabname = handlerNumberToLocalNumber[CurrentIndex].CharacterGO.name;
    }

    public void BuyCharacter()
    {
        Notification.gameObject.SetActive(true);
        Notification.color = Notification.color + new Color(0, 0, 0, 1);
        if (!ConfirmPurchase)
        {
            Notification.text = "Do you want to buy this character for " + CurrentCost + "?"; //confirm if the user wants to buy
            ConfirmPurchase = true;
            return;
        }

        ConfirmPurchase = false;
        if (int.Parse(Coins.text) >= CurrentCost)
        {
            PlayFabHandler.BuyCharacter(handlerNumberToLocalNumber[CurrentIndex].CharacterGO.name, CurrentCost);

            Coins.text = (int.Parse(Coins.text) - CurrentCost).ToString(); //update the currency

            DoUnlock(handlerNumberToLocalNumber[CurrentIndex].CharacterGO.name); //unlock the character and update the files

            Lock.SetActive(false); //unlock the UI
            FadeOut = true; //fade out the purchase message
            Notification.text = "Purchased";
        }
        else
        {
            FadeOut = true;
            Notification.text = "Not enough money";
        }
    }

    void WriteText()
    {
        ConfirmPurchase = false;
        Notification.text = "";


        Description.text = PlayFabHandler.Characters[handlerNumberToLocalNumber[CurrentIndex].HandlerNumber].Description; //description of class
        CurrentCost = PlayFabHandler.Characters[handlerNumberToLocalNumber[CurrentIndex].HandlerNumber].Cost; //set cost of this character

        if (handlerNumberToLocalNumber[CurrentIndex].CharacterGO.name == "BountyHunter")
        {
            Lock.SetActive(!PlayFabHandler.UnlockedBounty); //unlock or lock the button depending on if the player has unlocked the character
        }
        else if (handlerNumberToLocalNumber[CurrentIndex].CharacterGO.name == "Mercenary")
        {
            Lock.SetActive(!PlayFabHandler.UnlockedMercenary);
        }
        else if (handlerNumberToLocalNumber[CurrentIndex].CharacterGO.name == "Elf")
        {
            Lock.SetActive(!PlayFabHandler.UnlockedElf);
        }
    }

    void DoUnlock(string name)
    {
        if (name == "Mercenary")
            PlayFabHandler.UnlockedMercenary = true;
    }

    public void ShowMessage(string message)
    {
        Notification.color = Notification.color + new Color(0, 0, 0, 1);
        Notification.gameObject.SetActive(true);
        Notification.text = message;
    }
    public void HideMessage()
    {
        Notification.gameObject.SetActive(false);
    }

    public void BackButton()
    {
        GetComponent<ShopScript>().ShopPanel.SetActive(false);
        handlerNumberToLocalNumber[CurrentIndex].CharacterGO.GetComponent<PlayerMovement>().enabled = true;
    }

    public void UpdateCoins(int AmountToMinus)
    {
        Coins.text = (int.Parse(Coins.text) - AmountToMinus).ToString();
    }
}