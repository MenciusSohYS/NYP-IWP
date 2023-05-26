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
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (CostOfEnemy <= 0)
            CostOfEnemy = 10;
        //Debug.Log("First");
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
            //Debug.Log(CostOfEnemy);
            Player.GetComponent<PlayerMechanics>().SetCoins(CostOfEnemy);
            InsideThisRoom.GetComponent<DungeonScript>().RemoveEnemyFromList(gameObject);
            if (Drops.Length != 0)
            {
                //randomise it
                int randomdrop = Random.Range(0, Drops.Length); //randomise the drop
                Instantiate(Drops[1], transform.position, Quaternion.identity); //create
                Instantiate(PortalPrefab, transform.parent.position - new Vector3(0, 0, 0.01f), Quaternion.identity); //create
            }
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
        //Debug.Log("");
    }

    public void MultiplyHP(float HP)
    {
        float newHP = CurrentHealth * HP;

        CurrentHealth = (int)newHP;
    }
}
