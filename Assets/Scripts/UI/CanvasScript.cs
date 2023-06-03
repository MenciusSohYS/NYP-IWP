using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    public GameObject Player;
    public Slider Health;
    public TextMeshProUGUI Alertmessage;
    public Slider BGHP; //background hpbar
    public TextMeshProUGUI HPNumber;

    [SerializeField] TextMeshProUGUI Coin;
    [SerializeField] TextMeshProUGUI AmmoText;
    [SerializeField] int MakeDarker;
    [SerializeField] RectTransform AmmoIndicator;
    [SerializeField] RectTransform Crosshair;
    [SerializeField] RectTransform Base;
    private Vector3 Offset;
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = false;
        //find player
        Player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().sortingLayerName = "Default";
        GetComponent<Canvas>().sortingLayerID = 1;
        

        Offset = new Vector3(45, 30, 0);
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
        if (BGHP.value > Health.value)
        {
            BGHP.value -= (Time.deltaTime * (BGHP.maxValue/20));
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
        Debug.Log("C" + Coin.text);
        PlayFabHandler.UpdateMoney(int.Parse(Coin.text) - PlayFabHandler.Coins); //push the update money to playfab
    }

    public void UpdateAmmo(int Ammo)
    {
        AmmoText.text = Ammo.ToString();
    }
}
