using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectScript : MonoBehaviour
{
    public AudioSource SoundForBGM;
    //enable audio listener, mechanics, movement, gunscript, flip sprite script
    [SerializeField] GameObject[] Characters2;
    [SerializeField] int CurrentIndex;
    public GameObject Camera;
    private GameObject Trader;
    private GameObject Tutor;
    private GameObject Leaderboard;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Coins;
    public GameObject Lock;
    public GameObject Pause;
    public GameObject Difficulty;
    public TextMeshProUGUI Notification;
    public TextMeshProUGUI PauseTMP;
    public TextMeshProUGUI DifficultyDescription;
    public TextMeshProUGUI DifficultyLevelText;
    private bool ConfirmPurchase;
    private bool FadeOut;
    private int CurrentCost;
    private int CurrentDialogue;
    public Slider SliderForBGM;
    public Slider SliderForWeapon;
    public Slider SliderForInteraction;
    int DifficultyLevel;
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
        Time.timeScale = 1;
        DifficultyLevel = 5;
        CurrentCost = 0; //cost of buying the character
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
        Tutor = GameObject.FindGameObjectWithTag("Tutor");
        Leaderboard = GameObject.FindGameObjectWithTag("Leaderboard");
        Camera.transform.position = handlerNumberToLocalNumber[0].CharacterGO.transform.position - new Vector3(0, 0, 5); //shift the camera to where ever the first character is
        WriteText(); //lock camera to the first player the game can find and write its description
        Coins.text = PlayFabHandler.Coins.ToString();
        Pause.SetActive(false);


        Notification.gameObject.SetActive(true);
        Notification.text = "Character Select";
        Notification.color = new Color(1, 1, 1, 1);
        FadeOut = true; //fade of the notification text

        CurrentDialogue = 0;

        SliderForBGM.value = PlayFabHandler.BGMSliderValue;
        SliderForWeapon.value = PlayFabHandler.WeaponSliderValue;
        SliderForInteraction.value = PlayFabHandler.InteractionSliderValue;
        Difficulty.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!FadeOut)
            return;

        Notification.color = Notification.color - new Color(0, 0, 0, Time.deltaTime * 0.2f);
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
        handlerNumberToLocalNumber[CurrentIndex].CharacterGO.GetComponent<PlayerMovement>().enabled = true;
        handlerNumberToLocalNumber[CurrentIndex].CharacterGO.GetComponent<AudioListener>().enabled = true;
        Camera.GetComponent<Camera>().fieldOfView = 80;
        Camera.GetComponent<CameraScript>().enabled = true;
        Camera.GetComponent<CameraScript>().SetPlayer(handlerNumberToLocalNumber[CurrentIndex].CharacterGO);
        //set trader's player
        Trader.GetComponent<TraderScript>().SetPlayer(handlerNumberToLocalNumber[CurrentIndex].CharacterGO);
        Tutor.GetComponent<Tutor>().SetPlayer(handlerNumberToLocalNumber[CurrentIndex].CharacterGO, false);
        Leaderboard.GetComponent<LeaderboardHandler>().SetPlayer(handlerNumberToLocalNumber[CurrentIndex].CharacterGO);

        Description.transform.parent.gameObject.SetActive(false);//disable the panel
        Notification.gameObject.SetActive(false); //disable the notification text
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).GetComponent<Button>() != null && transform.GetChild(i).name != "Pause")
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
        else if (handlerNumberToLocalNumber[CurrentIndex].CharacterGO.name == "Dwarf")
        {
            Lock.SetActive(!PlayFabHandler.UnlockedDwarf);
        }
        else if (handlerNumberToLocalNumber[CurrentIndex].CharacterGO.name == "Wizard")
        {
            Lock.SetActive(!PlayFabHandler.UnlockedWizard);
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
    public void BackButton2()
    {
        GetComponent<LeaderBoardScript>().LeaderboadGO.SetActive(false);
        handlerNumberToLocalNumber[CurrentIndex].CharacterGO.GetComponent<PlayerMovement>().enabled = true;
    }

    public void UpdateCoins(int AmountToMinus)
    {
        Coins.text = (int.Parse(Coins.text) - AmountToMinus).ToString();
    }

    public void PauseClicked()
    {
        if (Pause.activeSelf)
        {
            PauseTMP.text = "II";
            Pause.SetActive(false);
            PlayFabHandler.BGMSliderValue = SliderForBGM.value;
            PlayFabHandler.WeaponSliderValue = SliderForWeapon.value;
            PlayFabHandler.InteractionSliderValue = SliderForInteraction.value;
            PlayFabHandler.PushAudioPreferences();
        }
        else
        {
            PauseTMP.text = ">";
            Pause.SetActive(true);
        }
    }

    public void PressLogOut()
    {
        PlayFabHandler.BGMSliderValue = SliderForBGM.value;
        PlayFabHandler.WeaponSliderValue = SliderForWeapon.value;
        PlayFabHandler.InteractionSliderValue = SliderForInteraction.value;
        PlayFabHandler.LogOut();
    }

    public bool CycleThroughDialogue()
    {
        Description.transform.parent.gameObject.SetActive(true);//enable the panel
        switch(CurrentDialogue)
        {
            case 0:
                Description.text = "Alright, basics, <size=37.5>WASD</size> to move, <size=37.5>Left Click</size> to shoot and <size=37.5>R</size> to reload";
                break;
            case 1:
                Description.text = "Got that? Well on to the next part, <size=37.5>Space</size> to roll, you can avoid damage with it.";
                break;
            case 2:
                Description.text = "Hold <size=37.5>Ctrl</size> to move slowly, helps with positioning";
                break;
            case 3:
                Description.text = "Press <size=37.5>F</size> to use your ability, different characters different abilities";
                break;
            case 4:
                Description.text = "Press <size=37.5>E</size> to interact with things, normally a message will show up and tell you to do so, like what you've been doing";
                break;
            case 5:
                Description.text = "You can only have <size=37.5>1</size> weapon at once, upgrades bought with the workbench <size=37.5>only</size> applies to that weapon";
                break;
            case 6:
                Description.text = "There's two types of cover, <size=37.5>Half</size> and <size=37.5>Full</size>, first reduces damage received by half, the other destorys projectiles";
                break;
            case 7:
                Description.text = "Press <size=37.5>M</size> to open the map, it fills up as you're moving along. Try not to get lost.";
                break;
            case 8:
                Description.text = "Hold <size=37.5>Tab</size> to see your weapon's stats";
                break;
            case 9:
                Description.text = "And thats it, good luck out there, <size=38>You're Gonna Need It</size>";
                break;
            default:
                CurrentDialogue = 0;
                Description.transform.parent.gameObject.SetActive(false);
                return true;
        }
        ++CurrentDialogue;
        return false;
    }

    public void LoadIntoGame()
    {
        Globalvariables.timerforsound = SoundForBGM.time;
        Globalvariables.Difficulty = DifficultyLevel;
        //Debug.Log(Globalvariables.timerforsound);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void ChooseDifficulty()
    {
        for (int i = 0; i < transform.childCount - 1; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
    }

    public void SliderChange(float SliderValue)
    {
        DifficultyLevel = (int)SliderValue;
        DifficultyLevelText.text = "Difficulty: " + DifficultyLevel;
        if (SliderValue > 9)
        {
            DifficultyDescription.text = "MAX Difficulty, you may or may not survive till the end, all the best";
        }
        else if (SliderValue > 7)
        {
            DifficultyDescription.text = "Hard difficulty, enemies on this level are very very painful to deal with";
        }
        else if (SliderValue > 4)
        {
            DifficultyDescription.text = "Medium Difficulty, enemies are hard but honestly, with good positioning, its easy";
        }
        else if (SliderValue > 1)
        {
            DifficultyDescription.text = "Easy Difficulty, great if you just started or want a stroll in the park";
        }
        else
        {
            DifficultyDescription.text = "Well, baby steps am I right";
        }
    }

}