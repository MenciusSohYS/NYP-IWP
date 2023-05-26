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
    [SerializeField] AbilityParent CharacterAbility;
    [SerializeField] PlayerMovement PlayerMoveScript;
    [SerializeField] SpriteRenderer SpriteRender;

    // Start is called before the first frame update
    void Start()
    {
        InvulnTime = 0;
        Currency = 0;
        CharacterAbility = GetComponent<AbilityParent>();
        PlayerMoveScript = GetComponent<PlayerMovement>();
        SpriteRender = GetComponent<SpriteRenderer>();
        if (Globalvariables.CurrentLevel == 1)
        {
            for (int i = 0; i < PlayFabHandler.SkillList.Count; ++i)
            {
                //Debug.Log(PlayFabHandler.SkillList[i].Name + " has " + PlayFabHandler.SkillList[i].StackAmount);
                AssignBuffs(PlayFabHandler.SkillList[i].Name, PlayFabHandler.SkillList[i].StackAmount); //assign the correct buffs when the player starts the first level (mechanics)
            }
        }

        MaxHealth = Globalvariables.MaxHP;
        CurrentHealth = Globalvariables.CurrentHP;

        Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>();
        Canvas.SetMaxNCurrentHP(MaxHealth, CurrentHealth);

        SetCoins(PlayFabHandler.Coins);
    }

    // Update is called once per frame
    void Update()
    {
        if (InvulnTime > 0)
        {
            InvulnTime -= Time.deltaTime;
            if (InvulnTime <= 0)
            {
                SpriteRender.color = new Color(1, 1, 1, 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            UseAbility(CharacterAbility.UseAbility()); //use the character's abilites
        }
        else
        {
            CharacterAbility.CoolDownAbility(Time.deltaTime); //cool down the ability when not in use
        }
    }

    public bool MinusHP(int howmuchtominus)
    {
        if (PlayerMoveScript.ReturnRollTimer() > 0)
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
            SpriteRender.color = new Color(1, 0, 0, 1);
            return true;
        }
        return false;
    }

    public void Heal(int HowMuchToHeal)
    {
        CurrentHealth += HowMuchToHeal;
        if (MaxHealth < CurrentHealth)
            CurrentHealth = MaxHealth;

        Canvas.HealCurrentHP(CurrentHealth);
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

    public void IncreaseMaxHP(int ByHowMuch)
    {
        MaxHealth += ByHowMuch;
        CurrentHealth += ByHowMuch;
        Canvas.SetMaxNCurrentHP(MaxHealth, CurrentHealth);
    }

    public void IncreaseFireRate(float byhowmuch)
    {
        transform.GetChild(0).GetComponent<GunScript>().ChangeFireRate(byhowmuch);
    }

    public void MessagePlayer(string Message)
    {
        Canvas.SetText(Message, 1);
    }

    void AssignBuffs(string name, int amount)
    {
        if (name == "Increase Health")
        {
            Globalvariables.MaxHP += (amount * 25);
            Globalvariables.CurrentHP = Globalvariables.MaxHP;
            //Debug.Log("Increase Health");
        }
        else if (name == "Increase Speed")
        {
            PlayerMoveScript.IncreaseSpeed(amount);
            //Debug.Log("Increase Speed");
        }
    }

    void UseAbility(int WhichAbility)
    {
        switch (WhichAbility)
        {
            case 0:
                return;
            case 1:
                {
                    Heal(20);
                    return;
                }
            default:
                return;
        }        
    }
}