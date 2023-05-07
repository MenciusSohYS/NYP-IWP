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
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CostOfEnemy = 10;
        CurrentHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MinusHP(int MinusBy)
    {
        if (CurrentHealth > MinusBy)
            CurrentHealth -= MinusBy;
        else
        {
            Player.GetComponent<PlayerMechanics>().SetCoins(CostOfEnemy);
            InsideThisRoom.GetComponent<DungeonScript>().RemoveEnemyFromList(gameObject);
            Destroy(gameObject);
        }
    }

    public void AssignRoom (GameObject room)
    {
        InsideThisRoom = room;
    }

    public void AlterCost(int addhowmuch)
    {
        CostOfEnemy += addhowmuch;
    }
}
