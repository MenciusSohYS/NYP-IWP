using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScript : MonoBehaviour
{
    public static int MAX_COST_FOR_ENEMIES = 3;
    public GameObject[] walls; //0 is North (Top), 1 is South (bottom), 2 is East (right), 3 is West (left)
    public GameObject[] outerwalls; //0 is North (Top), 1 is South (bottom), 2 is East (right), 3 is West (left)

    public GameObject[] doors;

    [SerializeField] int EnemyCost; //change this later when to the variable that affects enemy scaling

    public GameObject FCoverPrefab; //prefabs
    public GameObject HCoverPrefab;
    public GameObject WeaponWorkbenchPrefab;
    public GameObject[] EnemyList;
    public GameObject[] EnemyReinforcementList;
    public GameObject ChestPrefab;

    //containers
    public GameObject RoomObjects;
    [SerializeField] GameObject RoomEnemies;
    public GameObject Fog;

    [SerializeField] List<GameObject> Enemies; //all enemies
    public GameObject[] Covers; //an array of covers
    public List<GameObject> MeleeEnemies; //all enemies
    public List<GameObject> RangedEnemies; //all enemies

    [SerializeField] List<int> SidesWithCorridors;

    private TestGrid GridReference;

    bool SpawnMoreEnemies;

    float Scalevalue;

    // Start is called before the first frame update
    void Start()
    {
        Scalevalue = 0;
        SpawnMoreEnemies = false;
        Covers = new GameObject[10];
        GridReference = GetComponent<TestGrid>();
        //if its a normal room
        if (transform.tag == "Room")
        {
            for (int i = 0; i < 10; ++i) //create obstacles
            {
                bool allclear = true;
                int randomnum = Random.Range(0, 2);
                int x = Random.Range(-13, 13);
                int y = Random.Range(-13, 13); //random position

                if (x < 1 && x > -1 && y < 1 && y > -1)
                    continue; //we want to reserve this space for the chest later

                for (int j = 0; j < Covers.Length; ++j) //go through list to see if they can fit
                {
                    if (Covers[j] == null) //if the position in the array has nothing, skip it
                        continue;
                    else if ((transform.position.x + x < Covers[j].transform.position.x + 1.5 && transform.position.x + x > Covers[j].transform.position.x - 1.5) && (transform.position.y + y < Covers[j].transform.position.y + 1.5 && transform.position.y + y > Covers[j].transform.position.y - 1.5))
                    {
                        --i;
                        allclear = false;
                        //Debug.Log("not clear"); //if there is another box already in the area, dont put one down, break out of the loop looking for other overlaps
                        break;
                    }
                }

                if (allclear)
                {
                    if (randomnum == 0)
                        Covers[i] = Instantiate(HCoverPrefab, this.transform.position + new Vector3(x, y), Quaternion.identity); //create object and push into array for future reference
                    else
                        Covers[i] = Instantiate(FCoverPrefab, this.transform.position + new Vector3(x, y), Quaternion.identity); //create object and push into array for future reference

                    Covers[i].transform.SetParent(RoomObjects.transform); //set parent so that it doesnt mess up the scene and it actually stays as an object
                }
            }

            EnemyCost = 12 * (Globalvariables.CurrentLevel); //change max amount of enemies by cost here
            //creation of enemies
            while (EnemyCost > 0)
            {
                int MaxInt;
                if (MAX_COST_FOR_ENEMIES <= EnemyCost)
                    MaxInt = MAX_COST_FOR_ENEMIES + 1;
                else
                    MaxInt = EnemyCost + 1;

                int RandomEnemyNumber = Random.Range(1, MaxInt);

                int x = 0;
                int y = 0;
                bool allclear = false;
                while (!allclear)
                {
                    allclear = true;
                    x = Random.Range(-13, 13);
                    y = Random.Range(-13, 13); //random position

                    for (int j = 0; j < Covers.Length; ++j) //go through list to see if they can fit
                    {
                        if (Covers[j] == null) //if the position in the array has nothing, skip it
                            continue;
                        else if ((transform.position.x + x < Covers[j].transform.position.x + 1 && transform.position.x + x > Covers[j].transform.position.x - 1) && (transform.position.y + y < Covers[j].transform.position.y + 1 && transform.position.y + y > Covers[j].transform.position.y - 1))
                        {
                            allclear = false;
                            //Debug.Log("not clear"); 
                            //if there is another box already in the area, dont put one down, break out of the loop looking for other overlaps
                            break;
                        }
                    }
                    for (int j = 0; j < Enemies.Count; ++j) //go through list to see if they can fit
                    {
                        if (Enemies[j] == null) //if the position in the array has nothing, skip it
                            continue;
                        else if ((transform.position.x + x < Enemies[j].transform.position.x + 1 && transform.position.x + x > Enemies[j].transform.position.x - 1) && (transform.position.y + y < Enemies[j].transform.position.y + 1 && transform.position.y + y > Enemies[j].transform.position.y - 1))
                        {
                            allclear = false;
                            //Debug.Log("not clear"); 
                            //if there is another box already in the area, dont put one down, break out of the loop looking for other overlaps
                            break;
                        }
                    }
                }
                //create enemy


                Enemies.Add(Instantiate(EnemyList[RandomEnemyNumber - 1], this.transform.position + new Vector3(x, y), Quaternion.identity));
                EnemyCost -= RandomEnemyNumber;
                Enemies[Enemies.Count - 1].transform.SetParent(RoomEnemies.transform); //put enemies inside of the room's container, for easier reference

                if (Enemies.Count > 12)
                {
                    Scalevalue = EnemyCost * 0.1f;
                    Debug.Log(Scalevalue);
                    EnemyCost = 0;
                }
            }
            //Debug.Log("difficulty " + Globalvariables.Difficulty);
            //add these new enemies to the list
            for (int i = 0; i < Enemies.Count; ++i)
            {
                if (Enemies[i] != null) //if its not null, count
                {
                    if (Enemies[i].GetComponent<EnemyMechanics>().ReturnEnemyType() == EnemyMechanics.EnemyType.BasicMelee)
                    {
                        MeleeEnemies.Add(Enemies[i]);
                    }
                    else if (Enemies[i].GetComponent<EnemyMechanics>().ReturnEnemyType() == EnemyMechanics.EnemyType.BasicRanged || Enemies[i].GetComponent<EnemyMechanics>().ReturnEnemyType() == EnemyMechanics.EnemyType.SniperRanged)
                    {
                        RangedEnemies.Add(Enemies[i]);
                    }
                }
            }
        }
        else if (transform.tag == "BossRoom")
        {
            //if its a boss room
            //create the boss
            int randomboss = Random.Range(0, EnemyList.Length); //randomise him
            Enemies.Add(Instantiate(EnemyList[randomboss], transform.position, Quaternion.identity)); //create and add him to the list            
            Enemies[0].transform.SetParent(RoomEnemies.transform);
        }
        else if (transform.tag == "UpgradeRoom")
        {
            Instantiate(WeaponWorkbenchPrefab, transform.position + new Vector3(0, 0, -0.1f), Quaternion.identity);
        }
    }

    public void UpdateRoom(int Side) //is there something there?
    {
        SidesWithCorridors.Add(Side);
        doors[Side].SetActive(true);
        walls[Side].SetActive(false);
        //Debug.Log(transform.name);
        outerwalls[Side].SetActive(false);
    }

    public void RemoveFog()
    {
        Fog.SetActive(false);
    }

    public void EnableEnemies() //enable enemy ai and also shut the doors so the player cannot escape
    {

        for (int i = 0; i < Enemies.Count; ++i)
        {
            EnemyRangedScript Enemyrangedscript = Enemies[i].transform.GetChild(0).GetComponent<EnemyRangedScript>();
            EnemyMechanics EnemyMechanicScript = Enemies[i].GetComponent<EnemyMechanics>();

            Enemies[i].GetComponent<EnemyMovement>().enabled = true;
            EnemyMechanicScript.enabled = true;
            Enemyrangedscript.enabled = true;
            EnemyMechanicScript.AssignRoom(gameObject);

            if (Scalevalue > 1)
            {
                if (Scalevalue > 9)
                    Scalevalue = 9;

                Enemyrangedscript.IncreaseDamage(Scalevalue);
                EnemyMechanicScript.MultiplyHP(1 + (Scalevalue));
            }
            else if (transform.tag == "BossRoom" && Globalvariables.CurrentLevel > 1)
            {
                Enemyrangedscript.IncreaseDamage(Globalvariables.CurrentLevel * 0.75f);
                EnemyMechanicScript.MultiplyHP(Globalvariables.CurrentLevel * 0.75f);
            }
            
            float MultiplicationValueForDifficulty = (Globalvariables.Difficulty * 0.1f) + 0.4f;

            //change damage, hp and bullet velo based on difficulty
            Enemyrangedscript.IncreaseDamage(MultiplicationValueForDifficulty);
            Enemyrangedscript.ChangeBulletVelo(MultiplicationValueForDifficulty);
            EnemyMechanicScript.MultiplyHP(MultiplicationValueForDifficulty);

        }
        for (int i = 0; i < SidesWithCorridors.Count; ++i)
        {
            doors[SidesWithCorridors[i]].SetActive(false);
            walls[SidesWithCorridors[i]].SetActive(true);
            outerwalls[SidesWithCorridors[i]].SetActive(true);
        }

        //fix minimap cam to the room
        GameObject MiniMap = GameObject.FindGameObjectWithTag("MiniMap");

        if (MiniMap.GetComponent<CameraScript>().ReturnOpenedBigMap())
        {
            GameObject.Find("Canvas").GetComponent<CanvasScript>().BigMapInteraction();
        }

        MiniMap.GetComponent<CameraScript>().StopCamera();
        MiniMap.transform.position = this.transform.position + new Vector3(0, 0, -10);
        MiniMap.GetComponent<Camera>().orthographicSize = 17.5f;
        MiniMap.GetComponent<Camera>().backgroundColor = Color.white;

    }

    public void RemoveEnemyFromList(GameObject Enemy)
    {
        ++Globalvariables.EnemiesKilled;
        Enemies.Remove(Enemy);
        if (RangedEnemies.Contains(Enemy))
        {
            RangedEnemies.Remove(Enemy);
        }
        else if (MeleeEnemies.Contains(Enemy))
        {
            MeleeEnemies.Remove(Enemy);
        }

        if (Enemies.Count == 0)
        {
            for (int i = 0; i < SidesWithCorridors.Count; ++i)
            {
                doors[SidesWithCorridors[i]].SetActive(true);
                walls[SidesWithCorridors[i]].SetActive(false);
                outerwalls[SidesWithCorridors[i]].SetActive(false);
            }
            GridReference.DestoryGrid();

            //fix map cam to player
            GameObject MiniMap = GameObject.FindGameObjectWithTag("MiniMap");
            MiniMap.GetComponent<CameraScript>().ResumeCamera();
            MiniMap.GetComponent<Camera>().orthographicSize = 30f;
            MiniMap.GetComponent<Camera>().backgroundColor = Color.black;

            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            int HealAmount = (int)(2 * (10 - Globalvariables.Difficulty));
            Player.GetComponent<PlayerMechanics>().Heal(HealAmount);
            //Debug.Log(HealAmount);

            if (!transform.name.Contains("BossRoom"))
                Instantiate(ChestPrefab, transform.position + new Vector3(0, 0, -0.1f), Quaternion.identity);

            PlayFabHandler.UpdateMoney(GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>().GetCoins() - PlayFabHandler.Coins); //push the update money to playfab

            GameObject.FindGameObjectWithTag("BGM").GetComponent<BGMList>().PlayBGM();
        }
    }

    public int GetEnemyCount()
    {
        return Enemies.Count;
    }

    public int GetRangedEnemyCount()
    {
        return RangedEnemies.Count;
    }

    public int GetMeleeEnemyCount()
    {
        return MeleeEnemies.Count;
    }

    public void RangedTryToLookForPlayer(Vector3 PlayerLocation) //world coords
    {
        foreach (GameObject RangedEnemy in RangedEnemies)
        {
            if (RangedEnemy.transform.GetChild(0).GetComponent<EnemyRangedScript>().ReturnIsReloading()) //if they are reloading, they should run
            {
                EnemyGoAndHide(RangedEnemy, PlayerLocation);
            }
            else
            {
                EnemyFindPlayer(RangedEnemy, PlayerLocation);
            }
        }
    }

    public void LookForPlayer(Vector3 PlayerLocation) //since melee has a function for itself, melee will have one for itself too
    {
        PlayerLocation = new Vector3(Mathf.Round(PlayerLocation.x), Mathf.Round(PlayerLocation.y));

        foreach (GameObject Enemies in RangedEnemies)
        {
            EnemyFindPlayer(Enemies, PlayerLocation);
        }
    }

    public void GoHide(Vector3 PlayerLocation, bool SetRanged) //true for ranged and false for melee
    {
        List<GameObject> NewEnemyList = RangedEnemies;

        if (!SetRanged)
            NewEnemyList = MeleeEnemies;



        PlayerLocation = new Vector3(Mathf.Round(PlayerLocation.x), Mathf.Round(PlayerLocation.y));

        //see if enemy can hit player first
        foreach (GameObject Enemies in NewEnemyList)
        {
            EnemyGoAndHide(Enemies, PlayerLocation);
            Enemies.transform.GetChild(0).GetComponent<EnemyRangedScript>().SetShootNow(false);
        }
    }

    void EnemyFindPlayer(GameObject Enemies, Vector3 PlayerLocation)
    {
        if (!Enemies.transform.GetChild(0).GetComponent<EnemyRangedScript>().ReturnShootNow())
        {
            Enemies.transform.GetChild(0).GetComponent<EnemyRangedScript>().SetShootNow(true);
        }
        EnemyMovement EnemyMovementScript = Enemies.GetComponent<EnemyMovement>();

        if (EnemyMovementScript.VectorListCount() > 0) //if there's already a path
        {
            if (CanSeePlayer(EnemyMovementScript.ReturnLastLocation(), PlayerLocation)) //find out if that position can already hide from player
            {
                return;
            }
        }

        Vector3 EnemiesLocation = new Vector3(Mathf.Round(Enemies.transform.position.x), Mathf.Round(Enemies.transform.position.y));
        //Ray RayFromEnemyCurrentToPlayer = new Ray(EnemiesLocation, PlayerLocation - EnemiesLocation);

        if (!CanSeePlayer(EnemiesLocation, PlayerLocation))
        {
            for (int i = 0; i < Covers.Length; ++i)
            {
                if (Covers[i] == null)
                    continue;

                bool CanMoveHere = false;

                List<Vector3> ArrayOfUnavailableLocations = new List<Vector3>(); //this includes the position where the enemies can see the player
                while (!CanMoveHere)
                {
                    Vector3 NewPosition = LookForValidLocation(Covers[i].transform.position, 3, ArrayOfUnavailableLocations);
                    if (NewPosition != Vector3.negativeInfinity)
                    {
                        if (CanSeePlayer(NewPosition, PlayerLocation))
                        {
                            EnemyMovementScript.MovingToTarget(NewPosition);
                            CanMoveHere = true;
                            i = Covers.Length; //no need to find anymore spots
                        }
                        else if (ArrayOfUnavailableLocations.Count < 48)
                        {
                            ArrayOfUnavailableLocations.Add(NewPosition); //unavaliable position, add a to the avoid list
                        }
                        else
                        {
                            //Debug.Log("No valid location at cover number: " + i);
                            break;
                        }
                    }
                }
            }
        }
    }

    void EnemyGoAndHide(GameObject Enemies, Vector3 PlayerLocation)
    {
        EnemyMovement EnemyMovementScript = Enemies.GetComponent<EnemyMovement>();

        if (EnemyMovementScript.VectorListCount() > 0) //if there's already a path
        {
            if (!CanSeePlayer(EnemyMovementScript.ReturnLastLocation(), PlayerLocation)) //find out if that position can already hide from player
            {
                return; //no need this any longer
            }
        }

        Vector3 EnemiesLocation = new Vector3(Mathf.Round(Enemies.transform.position.x), Mathf.Round(Enemies.transform.position.y));
        //Ray RayFromEnemyCurrentToPlayer = new Ray(EnemiesLocation, PlayerLocation - EnemiesLocation);

        if (CanSeePlayer(EnemiesLocation, PlayerLocation))
        {
            for (int i = 0; i < Covers.Length; ++i)
            {
                if (Covers[i] == null)
                    continue;

                List<Vector3> ArrayOfUnavailableLocations = new List<Vector3>(); //this includes the position where the enemies can see the player
                bool CanMoveHere = false;
                while (!CanMoveHere)
                {
                    Vector3 NewPosition = LookForValidLocation(Covers[i].transform.position, 3, ArrayOfUnavailableLocations);
                    if (NewPosition != Vector3.negativeInfinity)
                    {
                        if (!CanSeePlayer(NewPosition, PlayerLocation))
                        {
                            EnemyMovementScript.MovingToTarget(NewPosition);
                            CanMoveHere = true;
                            i = Covers.Length; //no need to find anymore spots
                        }
                        else if (ArrayOfUnavailableLocations.Count < 48)
                        {
                            ArrayOfUnavailableLocations.Add(NewPosition); //unavaliable position, add a to the avoid list
                        }
                        else
                        {
                            //Debug.Log("No valid location at cover number: " + i);
                            break;
                        }
                    }
                }
            }
        }
    }

    bool CanSeePlayer(Vector3 fromhere, Vector3 tohere)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(fromhere, tohere - fromhere);

        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].transform.CompareTag("HalfCover") || hit[i].transform.CompareTag("Fullcover"))
            {
                //Debug.Log("Hit Cover");
                //Debug.DrawRay(fromhere, hit[i].transform.position - fromhere, Color.cyan, 1f);
                //already hiding
                return false;
            }
            else if (hit[i].transform.CompareTag("Player"))
            {
                //Debug.Log("Hit Player");
                //Debug.DrawRay(fromhere, hit[i].transform.position - fromhere, Color.green, 1f);
                //Debug.Log(hit[i].transform.tag);
                //if hit player, move away to another spot
                return true;
            }
        }
        return true;
    }

    public void SetMeleeEnemiesToAttack(Vector3 PlayerLocation)
    {
        Vector3 PlaceToGoToAttack = PlayerLocation;
        foreach (GameObject Enemies in MeleeEnemies)
        {
            //GridReference.MakeWalkableForEnemy(Enemies.transform.position - transform.position);
            PlaceToGoToAttack = LookForValidLocation(PlayerLocation, 2); //it to go here
            if (PlaceToGoToAttack != Vector3.negativeInfinity)
            {
                Enemies.GetComponent<EnemyMovement>().MovingToTarget(PlaceToGoToAttack);
                if (!Enemies.transform.GetChild(0).GetComponent<EnemyRangedScript>().ReturnShootNow())
                {
                    Enemies.transform.GetChild(0).GetComponent<EnemyRangedScript>().SetShootNow(true);
                }
            }
        }
    }


    public void SetBossToAttack(Vector3 PlayerPos)
    {
        if (Enemies.Count < 1)
            return;

        GameObject Boss = Enemies[0];

        if (Boss.name.Contains("Boss"))
        {
            CheckHPOfBoss(Boss);

            Boss.transform.GetChild(0).GetComponent<EnemyRangedScript>().SetShootNow(true);
            //make boss move randomly
            if (Boss.GetComponent<EnemyMovement>().ReturnIfPathVectorListIsNull()) //finished moving
            {
                if (Boss.GetComponent<EnemyMovement>().VectorListCount() > 0)
                    return;

                int RandomX = (int)PlayerPos.x;
                int RandomY = (int)PlayerPos.y;
                bool CanEnemySeePlayer = false;
                while (!CanEnemySeePlayer)
                {
                    //randomise new location to walk/run to
                    RandomX = Random.Range(-14, 14) + (int)transform.position.x;
                    RandomY = Random.Range(-14, 14) + (int)transform.position.y;
                    //Debug.Log(RandomX + " " + RandomY);
                    CanEnemySeePlayer = CanSeePlayer(new Vector3(RandomX, RandomY, 0), PlayerPos);
                }
                Vector3 PlaceToGoToAttack = LookForValidLocation(new Vector3(RandomX, RandomY, 0), 2); //look for valid location around the random spot
                                                                                                       //Debug.Log(PlaceToGoToAttack.x + " " + PlaceToGoToAttack.y);
                Boss.GetComponent<EnemyMovement>().MovingToTarget(PlaceToGoToAttack); //move the boss to that spot
            }
        }
        if (Enemies.Count > 1)
        {
            SetMeleeEnemiesToAttack(PlayerPos);
        }
    }

    public bool GetSpawnMoreEnemies()
    {
        return SpawnMoreEnemies;
    }

    void CheckHPOfBoss(GameObject Boss)
    {
        SpecialBossScript SPBossScript = Boss.transform.GetComponent<SpecialBossScript>();
        int Health = Boss.transform.GetComponent<EnemyMechanics>().GetHPPercentage();
        //Debug.Log(Health);
        bool Changed = SPBossScript.CheckForPhaseChange(Health); //check phase

        if (Changed)
        {
            switch (SPBossScript.ReturnCurrentPhaseNumber())
            {
                case SpecialBossScript.Phase.Phase1:
                    Debug.Log("Increased Speed");
                    Boss.transform.GetComponent<EnemyMovement>().SetSpeed(12); //boss gets increased speed
                    break;
                case SpecialBossScript.Phase.Phase2:
                    Debug.Log("Increased Accuracy");
                    Boss.transform.GetChild(0).GetComponent<EnemyRangedScript>().WeaponScript.SetMaxHeat(0.5f);
                    break;
                case SpecialBossScript.Phase.Phase3:
                    Debug.Log("Reinforcements");
                    SpawnMoreEnemies = true;
                    break;
                case SpecialBossScript.Phase.Phase4:
                    Debug.Log("Decreased damage taken");
                    Boss.transform.GetComponent<EnemyMechanics>().SetDR(0.5f); //boss gets 50% damage reduction
                    break;
            }
        }
    }

    public int SpawnReinforcements(int WhichIteration)
    {
        if (!SpawnMoreEnemies)
            return 0;

        int RandInt = Random.Range(0, EnemyReinforcementList.Length);
        switch (WhichIteration)
        {
            case 0:
                ++WhichIteration;
                Enemies.Add(Instantiate(EnemyReinforcementList[RandInt], this.transform.position + new Vector3(14, 14), Quaternion.identity));
                break;
            case 1:
                ++WhichIteration;
                Enemies.Add(Instantiate(EnemyReinforcementList[RandInt], this.transform.position + new Vector3(14, -14), Quaternion.identity));
                break;
            case 2:
                ++WhichIteration;
                Enemies.Add(Instantiate(EnemyReinforcementList[RandInt], this.transform.position + new Vector3(-14, 14), Quaternion.identity));
                break;
            case 3:
                ++WhichIteration;
                Enemies.Add(Instantiate(EnemyReinforcementList[RandInt], this.transform.position + new Vector3(-14, -14), Quaternion.identity));
                SpawnMoreEnemies = false;
                break;
        }
        Enemies[Enemies.Count - 1].transform.SetParent(RoomEnemies.transform); //put enemies inside of the room's container, for easier reference

        Enemies[Enemies.Count - 1].GetComponent<EnemyMovement>().enabled = true;
        Enemies[Enemies.Count - 1].GetComponent<EnemyMechanics>().enabled = true;
        Enemies[Enemies.Count - 1].transform.GetChild(0).GetComponent<EnemyRangedScript>().enabled = true;
        Enemies[Enemies.Count - 1].GetComponent<EnemyMechanics>().AssignRoom(gameObject);
        MeleeEnemies.Add(Enemies[Enemies.Count - 1]);

        return WhichIteration;
    }

    Vector3 LookForValidLocation(Vector3 TargetPosition, int radius, List<Vector3> ArrayOfVector3 = null)
    {
        TargetPosition = new Vector3(Mathf.Round(TargetPosition.x), Mathf.Round(TargetPosition.y));
        Vector3 NewLocation = TargetPosition;

        //breakdown, start with -1 and check each location, then increase the max num to check by 1 (to check 1,1 first)
        //each time we abs the number (to change to positive if negative) to find out if the Number is 

        int MaxNum = 1;

        //Debug.Log("Came Here Again");
        while (MaxNum <= radius)
        {
            for (int i = -MaxNum; i <= MaxNum; ++i) //this will represent X
            {
                for (int j = -MaxNum; j <= MaxNum; ++j) //this will represent Y
                {
                    if ((i == 0 && j == 0) || (Mathf.Abs(i) < MaxNum && Mathf.Abs(j) < MaxNum))
                        continue; //we do not want 0, 0 or repeat numbers (if max num is 2, it will check for 1 again)

                    //Debug.Log("Checking : " + ((int)TargetPosition.x - (int)transform.position.x + 15) + " " + i + " " + ((int)TargetPosition.y - (int)transform.position.y + 15) + " " + j);

                    int TargetXPosition = (int)TargetPosition.x + i - (int)transform.position.x + 15;
                    int TargetYPosition = (int)TargetPosition.y + j - (int)transform.position.y + 15;

                    //Debug.Log(TargetXPosition);
                    //Debug.Log(TargetYPosition);

                    if (TargetXPosition > 29 || TargetXPosition < 0 ||
                        TargetYPosition > 29 || TargetYPosition < 0)
                    {
                        continue; //we dont want anything outside of the grid
                    }
                    else if (ArrayOfVector3 != null)
                    {
                        if (ArrayOfVector3.Contains(new Vector3(TargetXPosition, TargetYPosition)))
                        {
                            continue; //we also dont want anything that was determined to be unable to be accessed
                        }
                    }

                    if (GridReference.TestIfGridIsTrue(TargetXPosition, TargetYPosition))
                    {
                        return NewLocation + new Vector3(i, j);
                    }
                }
            }
            ++MaxNum;
        }

        return Vector3.negativeInfinity;
    }
}