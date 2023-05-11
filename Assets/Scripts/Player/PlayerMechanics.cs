using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMechanics : MonoBehaviour
{
    [SerializeField] int MaxHealth;
    [SerializeField] int CurrentHealth;
    [SerializeField] int Currency;

    [SerializeField] CanvasScript Canvas;

    [SerializeField] float InvulnTime;

    // Start is called before the first frame update
    void Start()
    {
        InvulnTime = 0;
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
        if (InvulnTime > 0)
        {
            InvulnTime -= Time.deltaTime;
            if (InvulnTime <= 0)
            {
                transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    public bool MinusHP(int howmuchtominus)
    {
        if (GetComponent<PlayerMovement>().ReturnRollTimer() > 0)
        {
            return false;
        }
        else if (InvulnTime <= 0)
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
            InvulnTime = 0.5f;
            transform.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            return true;
        }
        return false;
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
        if (Canvas == null && SceneManager.GetActiveScene().name != "LobbyScene")
            Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>();

        Canvas.UpdateAmmo(Ammo);
    }
}