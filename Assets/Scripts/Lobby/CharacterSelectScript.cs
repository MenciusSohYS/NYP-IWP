using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectScript : MonoBehaviour
{
    //enable audio listener, mechanics, movement, gunscript, flip sprite script
    [SerializeField] GameObject[] Characters;
    [SerializeField] int CurrentIndex;
    public GameObject Camera;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Coins;
    public GameObject Lock;
    public TextMeshProUGUI Notification;
    private bool ConfirmPurchase;
    private bool FadeOut;
    private int CurrentCost;
    // Start is called before the first frame update
    void Start()
    {
        CurrentCost = 0; //cost of buying the character
        FadeOut = false; //fade of the notification text
        ConfirmPurchase = false; //for the buying of characters
        TxtHandler.CheckForCharacters(); //check what characters the player owns
        CurrentIndex = 0; //start from zero
        Characters = GameObject.FindGameObjectsWithTag("Player");
        Camera.transform.position = Characters[0].transform.position - new Vector3(0, 0, 5); //shift the camera to where ever the first character is
        WriteText(); //lock camera to the first player the game can find and write its description
        Coins.text = TxtHandler.FindOneIntValue('C').ToString();
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
        if (CurrentIndex > Characters.Length - 1)
        {
            CurrentIndex = 0;
        }
        Camera.transform.position = Characters[CurrentIndex].transform.position - new Vector3(0, 0, 5);
        WriteText(); //loop through the list and find the next character to be potentially selected, write their description after finding them
    }
    public void PreviousCharacter()
    {
        --CurrentIndex;
        if (CurrentIndex < 0)
        {
            CurrentIndex = Characters.Length - 1;
        }
        Camera.transform.position = Characters[CurrentIndex].transform.position - new Vector3(0, 0, 5);
        WriteText();
    }

    public void EnableCharacter()
    {
        for (int i = 0; i < Characters.Length; ++i)
        {
            if (i != CurrentIndex)
                Destroy(Characters[i]);
        }
        Characters[CurrentIndex].GetComponent<AudioListener>().enabled = true;
        Characters[CurrentIndex].GetComponent<PlayerMovement>().enabled = true;
        Camera.GetComponent<Camera>().fieldOfView = 80;
        Camera.GetComponent<CameraScript>().enabled = true;
        Camera.GetComponent<CameraScript>().SetPlayer(Characters[CurrentIndex]);
        //if player chooses the character, destroy all the others and enable the chosen's scripts
        //then set the camera to follow it

        Description.transform.parent.gameObject.SetActive(false);//disable the panel
        Notification.gameObject.SetActive(false); //disable the notification text
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).GetComponent<Button>() != null)
            {
                transform.GetChild(i).gameObject.SetActive(false); //find eac child in canvas and disable all those that has a button variable
            }
        }

        //lastly, push the gameobject to the global variables so that we know which to instaniate later
        Globalvariables.Playerprefabname = Characters[CurrentIndex].name;
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
            TxtHandler.CurrencyToWrite = "C" + (int.Parse(Coins.text) - CurrentCost).ToString(); //deduct and change currency

            DoUnlock(Characters[CurrentIndex].name); //unlock the character and update the files

            Lock.SetActive(false); //unlock the UI
            FadeOut = true; //fade out the purchase message
            Notification.text = "Purchased";
            Coins.text = TxtHandler.CurrencyToWrite; //update the currency
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
        if (Characters[CurrentIndex].name == "BountyHunter")
        {
            Description.text = "Bounty Hunter, starts with a revolver that deals high damage but low ammo capacity"; //description of class
            Lock.SetActive(!TxtHandler.UnlockedBounty); //unlock or lock the button depending on if the player has unlocked the character
            CurrentCost = 100; //set cost of this character
        }
        else if (Characters[CurrentIndex].name == "Mercenary")
        {
            Description.text = "Mercenary, starts with a rifle with high fire rate and magazine capacity, but has a wide spray and a long reload";
            Lock.SetActive(!TxtHandler.UnlockedMercenary);
            CurrentCost = 150;
        }
    }

    void DoUnlock(string name)
    {
        if (name == "Mercenary")
            TxtHandler.UnlockedMercenary = true;


        TxtHandler.CreateTextFile();
    }
}