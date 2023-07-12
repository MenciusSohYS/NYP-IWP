using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMechanics : MonoBehaviour
{
    [SerializeField] int CurrentHealth;
    [SerializeField] GameObject InsideThisRoom;
    [SerializeField] int CostOfEnemy;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject[] Drops;
    [SerializeField] GameObject PortalPrefab;
    [SerializeField] float DamageReduction;
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
        DamageReduction = 0;
        Player = GameObject.FindGameObjectWithTag("Player");
        if (CostOfEnemy <= 0)
            CostOfEnemy = 10;
        //Debug.Log("First");
        CurrentHealth = 100;

        if (enemyType == EnemyType.BossRanged)
        {
            CurrentHealth = 1000;
            CostOfEnemy = 90;
        }
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
    }
}
