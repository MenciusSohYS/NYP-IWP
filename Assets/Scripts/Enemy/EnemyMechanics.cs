using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMechanics : MonoBehaviour
{
    [SerializeField] int CurrentHealth;
    [SerializeField] int MaxHP;
    [SerializeField] GameObject InsideThisRoom;
    [SerializeField] int CostOfEnemy;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject[] Drops;
    [SerializeField] GameObject PortalPrefab;
    [SerializeField] float DamageReduction;
    [SerializeField] float DOTDuration; //last for how long
    [SerializeField] float DOTCountDown; //time between damage instance
    [SerializeField] int DOTDamage;
    [SerializeField] GameObject DamageNumber;
    [SerializeField] GameObject FireForEnemy;
    [SerializeField] GameObject FireReference;

    public enum EnemyType
    {
        BasicRanged,
        BasicMelee,
        SniperRanged,
        BossRanged
    }
    [SerializeField] EnemyType enemyType;

    // Start is called before the first frame update
    void Start()
    {
        DOTDuration = 0;
        DOTCountDown = 0.5f;
        DamageReduction = 0;
        Player = GameObject.FindGameObjectWithTag("Player");
        //if (CostOfEnemy <= 0)
        //    CostOfEnemy = 10;
        ////Debug.Log("First");
        //CurrentHealth = 100;

        if (enemyType == EnemyType.BossRanged)
        {
            MaxHP = CurrentHealth;
        }
    }
    private void Update()
    {
        if (DOTDuration <= 0)
            return;

        DOTDuration -= Time.deltaTime;

        if (DOTDuration <= 0)
        {
            Destroy(FireReference);
        }

        if (DOTCountDown > 0)
        {
            DOTCountDown -= Time.deltaTime;
            return;
        }

        MinusHP(DOTDamage);

        GameObject numberobject = Instantiate(DamageNumber, transform.position, Quaternion.identity);

        numberobject.GetComponent<DamageNumbers>().SetNumber(DOTDamage.ToString());
        numberobject.GetComponent<TextMeshPro>().color = new Color(1, 0, 0);

        DOTCountDown = 0.5f;
    }

    public void SetDR(float newDR)
    {
        DamageReduction = newDR;
    }

    public float ReturnDR()
    {
        return DamageReduction;
    }

    public EnemyType ReturnEnemyType()
    {
        return enemyType;
    }

    public int GetHP()
    {
        return CurrentHealth;
    }
    public int GetHPPercentage()
    {
        return (int)(((float)CurrentHealth / (float)MaxHP) * 100);
    }

    public void MinusHP(int MinusBy)
    {
        if (CurrentHealth > MinusBy)
            CurrentHealth -= MinusBy;
        else
        {
            //Debug.Log(CostOfEnemy);
            Player.GetComponent<PlayerMechanics>().SetCoins(CostOfEnemy);
            InsideThisRoom.GetComponent<DungeonScript>().RemoveEnemyFromList(gameObject);
            if (Drops.Length != 0)
            {
                //randomise it
                int randomdrop = Random.Range(0, Drops.Length); //randomise the drop
                Instantiate(Drops[randomdrop], transform.position, Quaternion.identity); //create
                Instantiate(PortalPrefab, transform.parent.position - new Vector3(0, 0, 0.01f), Quaternion.identity); //create
            }
            Destroy(gameObject);
        }
    }

    public void SetDOT(int Damage)
    {
        DOTDuration = 5.1f;

        if (DOTDamage < (int)(Damage * 0.5f))
            DOTDamage = (int)(Damage * 0.5f);

        if (Globalvariables.DOTStacks > 0)
        {
            DOTDamage += Globalvariables.DOTStacks;
        }


        if (FireReference == null)
        {
            FireReference = Instantiate(FireForEnemy, transform.position, Quaternion.identity);
            FireReference.transform.SetParent(transform);
        }
    }

    public void AssignRoom (GameObject room)
    {
        InsideThisRoom = room;
    }

    public Vector3 GetRoomCoordinates()
    {
        return InsideThisRoom.transform.position;
    }

    public void AlterCost(int addhowmuch)
    {
        CostOfEnemy += addhowmuch;
        //Debug.Log("");
    }

    public void MultiplyHP(float HP)
    {
        float newHP = CurrentHealth * HP;

        CurrentHealth = (int)newHP;
        MaxHP = CurrentHealth;

        CostOfEnemy = (int)(CostOfEnemy * HP);
    }
}
