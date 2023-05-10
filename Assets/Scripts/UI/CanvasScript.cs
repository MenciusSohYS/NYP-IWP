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

    [SerializeField] TextMeshProUGUI Coin;
    [SerializeField] TextMeshProUGUI AmmoText;
    [SerializeField] int MakeDarker;
    [SerializeField] RectTransform AmmoIndicator;
    [SerializeField] RectTransform Base;
    private Vector3 Offset;
    // Start is called before the first frame update
    void Awake()
    {
        //find player
        Player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<Canvas>().worldCamera = Camera.main;

        Offset = new Vector3(10, 10, 0);
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
    }

    public void MaxNCurrentHP(int Max, int Curr)
    {
        Health.maxValue = Max;
        Health.value = Curr;
        BGHP.maxValue = Max;
        BGHP.value = Curr;
    }

    public void SetCurrentHP(int Curr)
    {
        Health.value = Curr;
    }
    public void SetCoins(int Coins)
    {
        Coin.text = Coins.ToString();
    }

    public void SetText(string TextToSetTo, int MakeDarker)
    {
        Alertmessage.gameObject.SetActive(true); //set it to true
        Alertmessage.text = TextToSetTo; //set the text

        if (MakeDarker == -1)
            Alertmessage.alpha = 0f; //change alpha to 1
        else
            Alertmessage.alpha = 1f; //change alpha to 0

        this.MakeDarker = MakeDarker; //make it darker or brighter
    }

    public void SendScore()
    {
        Debug.Log("C" + Coin.text);
        TxtHandler.CurrencyToWrite = "C" + Coin.text;
        TxtHandler.CreateTextFile();
    }

    public void UpdateAmmo(int Ammo)
    {
        AmmoText.text = Ammo.ToString();
    }
}
