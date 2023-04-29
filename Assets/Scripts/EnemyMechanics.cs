using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMechanics : MonoBehaviour
{
    private int MaxHealth;
    private int CurrentHealth;
    public Slider HPbar;
    // Start is called before the first frame update
    void Start()
    {
        HPbar.maxValue = 100;
        HPbar.value = 75;
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MinusHP(int MinusBy)
    {
        if (HPbar.value <= MinusBy)
        {
            Destroy(gameObject);
        }
        else
        {
            CurrentHealth -= MinusBy;
            HPbar.value = CurrentHealth;
        }
    }
}
