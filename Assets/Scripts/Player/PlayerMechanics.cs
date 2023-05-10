using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    [SerializeField] int MaxHealth;
    [SerializeField] int CurrentHealth;
    [SerializeField] int Currency;

    [SerializeField] CanvasScript Canvas;



    // Start is called before the first frame update
    void Start()
    {
        Currency = 0;
        MaxHealth = 200;
        CurrentHealth = 150;

        Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>();
        Canvas.MaxNCurrentHP(MaxHealth, CurrentHealth);

        SetCoins(TxtHandler.FindOneIntValue('C'));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MinusHP(int howmuchtominus)
    {
        if (CurrentHealth - howmuchtominus <= 0)
        {
            gameObject.SetActive(false);
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            Canvas.SetText("Game Over", -1);
            Canvas.SendScore();
        }

        CurrentHealth -= howmuchtominus;
        Canvas.SetCurrentHP(CurrentHealth);
    }

    public int GetMaxHP()
    {
        return MaxHealth;
    }
    public int GetCurrentHP()
    {
        return CurrentHealth;
    }
    public void SetCoins(int AddHowMuch)
    {
        Currency += AddHowMuch;
        Canvas.SetCoins(Currency);
    }

    public void AnnounceRoomEntered()
    {
        Canvas.SetText("START!", 1);
    }

    public void ShowAmmoLeft(int Ammo)
    {
        if (Canvas == null)
            Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>();

        Canvas.UpdateAmmo(Ammo);
    }
}