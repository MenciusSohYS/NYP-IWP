using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    private int MaxHealth;
    private int CurrentHealth;
    private GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = 200;
        CurrentHealth = 150;

        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        Canvas.GetComponent<CanvasScript>().MaxNCurrentHP(MaxHealth, CurrentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MinusHP(int howmuchtominus)
    {
        CurrentHealth -= howmuchtominus;
        Canvas.GetComponent<CanvasScript>().SetCurrentHP(CurrentHealth);
    }

    public int GetMaxHP()
    {
        return MaxHealth;
    }
    public int GetCurrentHP()
    {
        return CurrentHealth;
    }
}
