using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHandler : MonoBehaviour
{
    [SerializeField] DungeonScript DungeonScript;
    private int RangedEnemyCount;
    private int MeleeEnemyCount;
    private GameObject PlayerGO;
    private PlayerStates CurrentPlayerState;
    private PlayerStates PreviousPlayerState;
    private float timerforupdate;
    private bool ContinueExecution; //continue purusing, moving or doing other things

    public enum PlayerStates
    {
        Alive,
        Reloading,
        Dead
    }
    private void Start()
    {
        ContinueExecution = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (timerforupdate > 0)
        {
            timerforupdate -= Time.deltaTime;
            if (ContinueExecution)
            {
                StateHandler();
            }
            return;
        }

        CalculateEnemyCount();
        GetPlayerState();
        timerforupdate = 0.5f;
    }

    private void OnEnable()
    {
        CalculateEnemyCount();
        PlayerGO = GameObject.FindGameObjectWithTag("Player");
        GetPlayerState();
    }

    void CalculateEnemyCount()
    {
        RangedEnemyCount = DungeonScript.GetRangedEnemyCount();
        MeleeEnemyCount = DungeonScript.GetMeleeEnemyCount();

        //Debug.Log("Counted Enemies: " + MeleeEnemyCount + " Melee, and " + RangedEnemyCount + " Ranged");

        //no longer needed but here for refence
        {
            //for (int i = 0; i < DungeonScript.Enemies.Count; ++i)
            //{
            //    if (DungeonScript.Enemies[i] != null) //if its not null, count
            //    {
            //        if (DungeonScript.Enemies[i].GetComponent<EnemyMechanics>().ReturnEnemyType() == EnemyMechanics.EnemyType.BasicMelee)
            //        {
            //            ++MeleeEnemyCount;
            //        }
            //        else if (DungeonScript.Enemies[i].GetComponent<EnemyMechanics>().ReturnEnemyType() == EnemyMechanics.EnemyType.BasicRanged)
            //        {
            //            ++RangedEnemyCount;
            //        }
            //    }
            //}
        }
    }

    void StateHandler()
    {
        //Debug.Log(CurrentPlayerState);
        switch(CurrentPlayerState)
        {
            case PlayerStates.Alive:
                break;
            case PlayerStates.Dead:
                break;
            case PlayerStates.Reloading:
                if (MeleeEnemyCount > 0)
                {
                    DungeonScript.SetMeleeEnemiesToAttack(PlayerGO.transform.position);
                }
                break;
        }
    }

    void GetPlayerState()
    {
        CurrentPlayerState = PlayerGO.GetComponent<PlayerMechanics>().GetPlayerState();
        StateHandler();
    }
}
