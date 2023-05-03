using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    public GameObject Player;
    public Slider Health;
    public Slider BGHP; //background hpbar
    // Start is called before the first frame update
    void Awake()
    {
        //find player
        Player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (BGHP.value > Health.value)
        {
            BGHP.value -= (Time.deltaTime * (BGHP.maxValue/20));
        }
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
}
