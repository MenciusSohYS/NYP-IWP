using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScript : MonoBehaviour
{
    public static int MAX_COST_FOR_ENEMIES = 1;
    public GameObject[] walls; //0 is North (Top), 1 is South (bottom), 2 is East (right), 3 is West (left)

    public GameObject[] doors;

    [SerializeField] int EnemyCost; //change this later when to the variable that affects enemy scaling

    public GameObject FCoverPrefab; //prefabs
    public GameObject HCoverPrefab;
    public GameObject WeaponWorkbenchPrefab;
    public GameObject[] EnemyList;
    public GameObject ChestPrefab;

    //containers
    public GameObject RoomObjects;
    [SerializeField] GameObject RoomEnemies;
    public GameObject Fog;

    [SerializeField] List<GameObject> Enemies;
    public GameObject[] Covers; //an array of covers

    [SerializeField] List<int> SidesWithCorridors;

    // Start is called before the first frame update
    void Start()
    {
        Covers = new GameObject[10];

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

            EnemyCost = 5 * (Globalvariables.CurrentLevel); //change max amount of enemies by cost here
            //creation of enemies
            while (EnemyCost > 0)
            {
                int MaxInt;
                if (MAX_COST_FOR_ENEMIES <= EnemyCost)
                    MaxInt = MAX_COST_FOR_ENEMIES + 1;
                else
                    MaxInt = EnemyCost + 1;

                int RandomEnemyNumber = 1; //Random.Range(1, MaxInt);

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
            }
        }
        else if (transform.tag == "BossRoom")
        {
            //if its a boss room
            //create the boss
            int randomboss = Random.Range(0, EnemyList.Length); //randomise him
            Enemies.Add(Instantiate(EnemyList[randomboss], transform.position, Quaternion.identity)); //create and add him to the list
            //Enemies[0].GetComponent<EnemyMechanics>().MultiplyHP(10);
            Enemies[0].GetComponent<EnemyMechanics>().AlterCost(90);
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
    }

    public void RemoveFog()
    {
        Fog.SetActive(false);
    }

    public void EnableEnemies() //enable enemy ai and also shut the doors so the player cannot escape
    {
        for (int i = 0; i < Enemies.Count; ++i)
        {
            Enemies[i].GetComponent<EnemyMovement>().enabled = true;
            Enemies[i].GetComponent<EnemyMechanics>().enabled = true;
            Enemies[i].transform.GetChild(0).GetComponent<EnemyRangedScript>().enabled = true;
            Enemies[i].GetComponent<EnemyMechanics>().AssignRoom(gameObject);
        }

        for (int i = 0; i < SidesWithCorridors.Count; ++i)
        {
            doors[SidesWithCorridors[i]].SetActive(false);
            walls[SidesWithCorridors[i]].SetActive(true);
        }

        //fix minimap cam to the room
        GameObject MiniMap = GameObject.FindGameObjectWithTag("MiniMap");
        MiniMap.GetComponent<CameraScript>().StopCamera();
        MiniMap.transform.position = this.transform.position + new Vector3(0, 0, -10);
        MiniMap.GetComponent<Camera>().orthographicSize = 17.5f;
        MiniMap.GetComponent<Camera>().backgroundColor = Color.white;

    }

    public void RemoveEnemyFromList(GameObject Enemy)
    {
        Enemies.Remove(Enemy);

        if (Enemies.Count == 0)
        {
            for (int i = 0; i < SidesWithCorridors.Count; ++i)
            {
                doors[SidesWithCorridors[i]].SetActive(true);
                walls[SidesWithCorridors[i]].SetActive(false);
            }

            //fix map cam to player
            GameObject MiniMap = GameObject.FindGameObjectWithTag("MiniMap");
            MiniMap.GetComponent<CameraScript>().ResumeCamera();
            MiniMap.GetComponent<Camera>().orthographicSize = 30f;
            MiniMap.GetComponent<Camera>().backgroundColor = Color.black;

            if (!transform.name.Contains("BossRoom"))
                Instantiate(ChestPrefab, transform.position + new Vector3(0, 0, -0.1f), Quaternion.identity);

            PlayFabHandler.UpdateMoney(GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>().GetCoins() - PlayFabHandler.Coins); //push the update money to playfab
        }
    }

    public int GetEnemyCount()
    {
        return Enemies.Count;
    }
}
