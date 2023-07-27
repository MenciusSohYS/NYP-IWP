using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerGun;
    public GameObject PlayerBulleSpawner;
    public Slider Health;
    public TextMeshProUGUI Alertmessage;
    public Slider BGHP; //background hpbar
    public TextMeshProUGUI HPNumber;
    public Slider SkillCoolDown;
    public Image SkillCoolDownImage;
    public GameObject GameOverGO;
    private bool isRecharged;
    private CircleCreator CircleCreatorScript;
    public GameObject PausePanel;
    public Slider SliderForBGM;
    public Slider SliderForWeapon;
    public Slider SliderForInteration;
    public GameObject Pause;
    public TextMeshProUGUI PauseTMP;
    public GameObject BigMapPanel;
    public GameObject MiniMap;
    public GameObject LobbyButton;
    public Slider BossHP;
    public TextMeshProUGUI BossValue;
    public RectTransform GunStats;
    int CurrentDialogue;
    public TextMeshProUGUI Description;


    [SerializeField] TextMeshProUGUI Coin;
    [SerializeField] TextMeshProUGUI AmmoText;
    [SerializeField] int MakeDarker;
    [SerializeField] RectTransform AmmoIndicator;
    [SerializeField] RectTransform Crosshair;
    [SerializeField] RectTransform Base;
    [SerializeField] TextMeshProUGUI FPSCounter;
    private Vector3 Offset;
    float TimeElapsed = 0;
    // Start is called before the first frame update
    void Awake()
    {
        GunStats.gameObject.SetActive(false);
        BossHP.gameObject.SetActive(false);
        Cursor.visible = false;
        //find player
        GameOverGO.SetActive(false);
        GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().sortingLayerName = "Default";
        GetComponent<Canvas>().sortingLayerID = 3;
        isRecharged = true;
        CircleCreatorScript = SkillCoolDownImage.transform.GetChild(0).GetComponent<CircleCreator>();

        Offset = new Vector3(45, 30, 0);
        SkillCoolDown.interactable = false;
        Health.interactable = false;
        BGHP.interactable = false;
        PausePanel.SetActive(false);
        SliderForBGM.value = PlayFabHandler.BGMSliderValue;
        SliderForWeapon.value = PlayFabHandler.WeaponSliderValue;
        SliderForInteration.value = PlayFabHandler.InteractionSliderValue;
        MiniMap = GameObject.FindGameObjectWithTag("MiniMap");

        BigMapPanel.SetActive(false);
        CurrentDialogue = 0;
        Description.transform.parent.gameObject.SetActive(false);
        //debug of text file
        {
            //string[] Scores = TxtHandler.ReadFile(@"PlayerStats.txt"); //read from file and return an array

            //for (int i = 0; i < Scores.Length; ++i)
            //{
            //    Debug.Log(Scores[i] + '\t'); //print them out
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeElapsed < 0)
        {
            FPSCounter.text = "FPS: " + Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
            TimeElapsed = 0.5f;
        }
        else
            TimeElapsed -= Time.unscaledTime;



        if (BGHP.value > Health.value)
        {
            BGHP.value -= (Time.deltaTime * (BGHP.maxValue*0.05f));
        }
        if (Alertmessage.IsActive())
        {
            Alertmessage.alpha -= 0.5f * Time.deltaTime * MakeDarker;
            if (Alertmessage.alpha <= 0)
            {
                Alertmessage.gameObject.SetActive(false);
            }
        }
        Vector3 positionToMove = Input.mousePosition;
        positionToMove.z = Base.position.z;

        AmmoIndicator.position = Camera.main.ScreenToWorldPoint(positionToMove + Offset);
        Crosshair.position = Camera.main.ScreenToWorldPoint(positionToMove);

        if (PlayerBulleSpawner)
        {
            Vector3 NewPos = Camera.main.WorldToScreenPoint(PlayerBulleSpawner.transform.position);
            NewPos.z = Base.position.z;
            GunStats.position = Camera.main.ScreenToWorldPoint(NewPos);
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            PauseClicked();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            BigMapInteraction();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!GunStats.gameObject.activeSelf)
            {
                GunStats.gameObject.SetActive(true);
                GunStats.GetComponent<GunStatScript>().enabled = true;
                GunStats.GetComponent<GunStatScript>().AssignPlayerGun(PlayerGun);
            }
            else
            {
                GunStats.gameObject.SetActive(false);
                GunStats.GetComponent<GunStatScript>().enabled = false;
            }
        }
    }

    public bool IsBigMapPanelActive()
    {
        return BigMapPanel.activeSelf;
    }

    public void BigMapInteraction()
    {
        if (MiniMap.GetComponent<CameraScript>().ReturnPauseCamera())
            return;

        if (BigMapPanel.activeSelf)
        {
            BigMapPanel.SetActive(false);

            MiniMap.GetComponent<CameraScript>().ChangeOpenedBigMap(false);
            MiniMap.GetComponent<CameraScript>().ResumeCamera();
            MiniMap.GetComponent<Camera>().orthographicSize = 30f;
            MiniMap.GetComponent<Camera>().backgroundColor = Color.black;
        }
        else
        {
            BigMapPanel.SetActive(true);

            MiniMap.GetComponent<CameraScript>().ChangeOpenedBigMap(true);
            MiniMap.GetComponent<Camera>().orthographicSize = 150f;
            MiniMap.GetComponent<Camera>().backgroundColor = Color.black;
        }
    }

    public void TellPlayerGameObjectHasSpawned()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerGun = Player.transform.GetChild(0).GetChild(0).gameObject;
        PlayerBulleSpawner = PlayerGun.transform.GetChild(0).gameObject;

        SkillCoolDown.maxValue = Player.GetComponent<PlayerMechanics>().ReturnMaxCoolDown();
    }

    public void AssignPlayerGun(GameObject NewPlayerGun)
    {
        PlayerGun = NewPlayerGun;
        PlayerBulleSpawner = PlayerGun.transform.GetChild(0).gameObject;
        GunStats.GetComponent<GunStatScript>().AssignPlayerGun(PlayerGun);
    }

    public void SetMaxHpForBoss(int Max)
    {
        BossHP.maxValue = Max;
        BossHP.value = BossHP.maxValue;
        BossValue.text = BossHP.value.ToString();
    }

    public void SetHpForBoss(int CurrentHp)
    {
        BossHP.value = CurrentHp;
        BossValue.text = CurrentHp.ToString();
        if (BossHP.value <= 0)
        {
            BossHP.gameObject.SetActive(false);
        }
    }

    public void SetBossHpActive()
    {
        BossHP.gameObject.SetActive(true);
    }

    public void SetMaxNCurrentHP(int Max, int Curr)
    {
        Health.maxValue = Max;
        Health.value = Curr;
        HPNumber.text = Health.value.ToString() + "/" + Health.maxValue.ToString();
        BGHP.maxValue = Max;
        BGHP.value = Curr;
    }

    public void SetCurrentHP(int Curr)
    {
        Health.value = Curr;
        HPNumber.text = Health.value.ToString() + "/" + Health.maxValue.ToString();
    }
    public void HealCurrentHP(int Curr)
    {
        Health.value = Curr;
        BGHP.value = Health.value;
        HPNumber.text = Health.value.ToString() + "/" + Health.maxValue.ToString();
    }
    public void SetCoins(int Coins)
    {
        Coin.text = Coins.ToString();
    }

    public int GetCoins()
    {
        return int.Parse(Coin.text);
    }

    public void SetText(string TextToSetTo, int MakeDarker)
    {
        Alertmessage.gameObject.SetActive(true); //set it to true
        Alertmessage.text = TextToSetTo; //set the text

        if (MakeDarker == -1)
            Alertmessage.alpha = 0f; //change alpha to 1 during updtae
        else
            Alertmessage.alpha = 1f; //change alpha to 0 during updtae

        this.MakeDarker = MakeDarker; //make it darker or brighter
    }

    public void SendScore()
    {
        //Debug.Log("C" + Coin.text);
        PlayFabHandler.UpdateMoney(int.Parse(Coin.text) - PlayFabHandler.Coins); //push the update money to playfab
    }

    public void SetGameOver()
    {
        Cursor.visible = true;
        GameOverGO.SetActive(true);
        LobbyButton.SetActive(true);
        PlayFabHandler.PushScore(Globalvariables.EnemiesKilled);
    }

    public void LogOut()
    {
        PlayFabHandler.BGMSliderValue = SliderForBGM.value;
        PlayFabHandler.WeaponSliderValue = SliderForWeapon.value;
        PlayFabHandler.InteractionSliderValue = SliderForInteration.value;
        PlayFabHandler.LogOut();
    }

    public void ReturnToLobby()
    {
        Time.timeScale = 1;
        SendScore();
        Globalvariables.ForgetEverything();
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
    }

    public void UpdateAmmo(int Ammo)
    {
        AmmoText.text = Ammo.ToString();
    }

    public void SetCurrentAbilityCD(float currentTime)
    {
        SkillCoolDown.value = currentTime;
        if (SkillCoolDown.value == SkillCoolDown.maxValue && isRecharged) //if its recharged call it once
        {
            //Debug.Log("Recharged");
            SkillCoolDownImage.color = SkillCoolDownImage.color + new Color(0, 100, 0);
            isRecharged = false;
            CircleCreatorScript.Create(SkillCoolDownImage.color); //create the circle to tell the player that it has recharged
        }
        else if (!isRecharged && SkillCoolDown.value != SkillCoolDown.maxValue) //if its not recharged, change the color once
        {
            //Debug.Log("Not Recharged");
            isRecharged = true;
            SkillCoolDownImage.color = SkillCoolDownImage.color - new Color(0, 100, 0);
        }
    }
    public void PauseClicked()
    {
        if (Pause.activeSelf)
        {
            PauseTMP.text = "II";
            Cursor.visible = false;
            Pause.SetActive(false);
            PlayFabHandler.BGMSliderValue = SliderForBGM.value;
            PlayFabHandler.WeaponSliderValue = SliderForWeapon.value;
            PlayFabHandler.InteractionSliderValue = SliderForInteration.value;
            PlayFabHandler.PushAudioPreferences();
            Time.timeScale = 1;
        }
        else
        {
            Cursor.visible = true;
            PauseTMP.text = ">";
            Pause.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public bool CycleThroughDialogue()
    {
        Description.transform.parent.gameObject.SetActive(true);//enable the panel
        switch (CurrentDialogue)
        {
            case 0:
                Description.text = "<size=37.5>WASD</size> to move, <size=37.5>Left Click</size> to shoot and <size=37.5>R</size> to reload";
                break;
            case 1:
                Description.text = "Rolling (<size=37.5>SPACE</size>) is very crucial, it makes you invulnerable for its duration";
                break;
            case 2:
                Description.text = "Hold <size=37.5>Ctrl</size> to move slowly, helps with positioning";
                break;
            case 3:
                Description.text = "Press <size=37.5>M</size> to open the map, it fills up as you're moving along. Try not to get lost.";
                break;
            case 4:
                Description.text = "You can only have <size=37.5>1</size> weapon at once, upgrades bought with the workbench <size=37.5>only</size> applies to that weapon";
                break;
            case 5:
                Description.text = "<size=37.5>Half</size> covers reduce damage recieved and increases accuracy.";
                break;
            case 6:
                Description.text = " <size=37.5>Full</size> covers protects you from projectiles and reduces your reload time";
                break;
            case 7:
                Description.text = "Press <size=37.5>F</size> to use your ability, different characters different abilities";
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
}