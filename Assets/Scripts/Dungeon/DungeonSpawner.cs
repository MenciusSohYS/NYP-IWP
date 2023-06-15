using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour
{
    [SerializeField] int AmountOfRooms;
    public GameObject RoomPrefab;
    public GameObject StartRoomPrefab;
    public GameObject CorridorPrefab;
    public GameObject BossRoomPrefab;
    public GameObject UpgradeRoomPrefab;
    private int[][] GridOfDungeon = new int[GridSize][]; //create a grid
    [SerializeField] int[] TestMap;
    public GameObject[] PlayerPrefabs;
    public GameObject[] WeaponPrefabs;
    [SerializeField] List<GameObject> EndRooms;
    public GameObject[] Buffs;
    private const int GridSize = 15;
    GameObject mainCamera;
    GameObject miniMap;
    GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        miniMap = GameObject.FindGameObjectWithTag("MiniMap");
        canvas = GameObject.Find("Canvas");

        for (int i = 0; i < PlayerPrefabs.Length; ++i) //create player
        {
            if (PlayerPrefabs[i].name == Globalvariables.Playerprefabname)
            {
                GameObject PlayerGO = Instantiate(PlayerPrefabs[i], new Vector3(0, 0, -0.05f), Quaternion.identity);
                mainCamera.GetComponent<CameraScript>().SetPlayer(PlayerGO);
                miniMap.GetComponent<CameraScript>().SetPlayer(PlayerGO);


                if (Globalvariables.CurrentLevel > 1)
                {
                    for (int j = 0; j < WeaponPrefabs.Length; ++j)
                    {
                        //Debug.Log(Globalvariables.WeaponComponents.WeaponName.Contains(WeaponPrefabs[j].transform.name));
                        if (Globalvariables.WeaponComponents.WeaponName.Contains(WeaponPrefabs[j].transform.name))
                        {
                            GameObject NewWeapon = Instantiate(WeaponPrefabs[j], PlayerGO.transform.position, Quaternion.identity);
                            //Destroy(PlayerWeapon.transform.GetChild(0).GetChild(0).gameObject);
                            PlayerGO.transform.GetChild(0).GetComponent<GunScript>().AssignNewGun(NewWeapon, false, false); //replace current gun and tell the game that we have picked it up, just instantiating
                            PlayerGO.transform.GetChild(0).GetComponent<GunScript>().AssignWeaponBuffsAfterLevelOne(); //Assign the buffs
                            Destroy(PlayerGO.transform.GetChild(0).GetChild(PlayerGO.transform.GetChild(0).childCount - 1).gameObject); //remove the last gameobject
                        }
                    }
                }

                break;
            }
        }


        for (int i = 0; i < GridSize; i++)
        {
            GridOfDungeon[i] = new int[GridSize]; //finish creating the second part of the jagged array
        }

        GameObject CurrRoom;
        GameObject PrevRoom;
        Vector2 currentcoords = new Vector2(7, 7);

        AmountOfRooms = Random.Range(8, 11); //randomise amount of rooms

        int changedirections = (int)((AmountOfRooms - 1) * 0.5f); //when to branch off

        int UpgradeRoomLocation = Random.Range(1, AmountOfRooms - 2);

        PrevRoom = Instantiate(StartRoomPrefab, new Vector3(0, 0, 0), Quaternion.identity); //create the first room
        CurrRoom = PrevRoom;
        PrevRoom.GetComponent<RoomHandler>().SetRoomCoordinates(currentcoords); //tell it where it is on the map


        GridOfDungeon[(int)currentcoords.x][(int)currentcoords.y] = 1; //tell the list its already occupied

        canvas.GetComponent<CanvasScript>().SetText("Level " + Globalvariables.CurrentLevel.ToString(), 1);


        for (int i = 1; i < AmountOfRooms; ++i) //for loop to create everything
        {
            bool CanMakeRoomHere = false;

            bool[] checkedspot = new bool[4];

            int LocationOfNewRoomRelativeToCurrent = 0;

            for (int z = 0; z < checkedspot.Length; ++z) //have not checked spot
                checkedspot[z] = false;


            GameObject corridor = PrevRoom;
            while (!CanMakeRoomHere) //will keep running loop until we have no more rooms
            {
                //testing
                {
                    //if (i < 11)
                    //{
                    //    LocationOfNewRoomRelativeToCurrent = TestMap[i];
                    //}
                    //else
                    //{
                    //}
                }

                LocationOfNewRoomRelativeToCurrent = Random.Range(0, 4);

                //check if it hit the boundaries

                switch (LocationOfNewRoomRelativeToCurrent)
                {
                    case 0:
                        if ((int)currentcoords.y + 1 > 14)
                        {
                            LocationOfNewRoomRelativeToCurrent = 5;
                            checkedspot[0] = true;
                        }
                        break;
                    case 1:
                        if ((int)currentcoords.y - 1 < 0)
                        {
                            LocationOfNewRoomRelativeToCurrent = 5;
                            checkedspot[1] = true;
                        }
                        break;
                    case 2:
                        if ((int)currentcoords.x + 1 > 14)
                        {
                            LocationOfNewRoomRelativeToCurrent = 5;
                            checkedspot[2] = true;
                        }
                        break;
                    case 3:
                        if ((int)currentcoords.x - 1 < 0)
                        {
                            LocationOfNewRoomRelativeToCurrent = 5;
                            checkedspot[3] = true;
                        }
                        break;
                    default:
                        break;
                }

                //change room according to which one it should be now, there are 3 so far, boss, normal and upgrades
                GameObject CreateThisRoom;

                if (i == UpgradeRoomLocation)
                    CreateThisRoom = UpgradeRoomPrefab;
                else if (i != AmountOfRooms - 1)
                    CreateThisRoom = RoomPrefab;
                else
                    CreateThisRoom = BossRoomPrefab;
                //check all boxes
                //check if we can make a room at this place
                switch (LocationOfNewRoomRelativeToCurrent)//see where we can place a room
                {
                    case 0:
                        if (!checkedspot[0] && GridOfDungeon[(int)currentcoords.x][(int)currentcoords.y + 1] == 0)
                        {
                            corridor = Instantiate(CorridorPrefab, PrevRoom.transform.position + new Vector3(0, 30, 0), Quaternion.identity);
                            corridor.transform.Rotate(0, 0, 90);

                            CurrRoom = Instantiate(CreateThisRoom, PrevRoom.transform.position + new Vector3(0, 60, 0), Quaternion.identity); //create room

                            CanMakeRoomHere = true; //can make a room at this location
                            currentcoords += new Vector2(0, 1); //set the new co ordinates
                            break;
                        }
                        else
                        {
                            //Debug.Log("Unable to create room above room: " + i);
                            checkedspot[0] = true;
                            continue;
                        }
                    case 1:
                        if (!checkedspot[1] && GridOfDungeon[(int)currentcoords.x][(int)currentcoords.y - 1] == 0)
                        {
                            corridor = Instantiate(CorridorPrefab, PrevRoom.transform.position + new Vector3(0, -30, 0), Quaternion.identity);
                            corridor.transform.Rotate(0, 0, -90);

                            CurrRoom = Instantiate(CreateThisRoom, PrevRoom.transform.position + new Vector3(0, -60, 0), Quaternion.identity); //create room

                            CanMakeRoomHere = true;
                            currentcoords -= new Vector2(0, 1);
                            break;
                        }
                        else
                        {
                            //Debug.Log("Unable to create room below room: " + i);
                            checkedspot[1] = true;
                            continue;
                        }
                    case 2:
                        if (!checkedspot[2] && GridOfDungeon[(int)currentcoords.x + 1][(int)currentcoords.y] == 0)
                        {
                            corridor = Instantiate(CorridorPrefab, PrevRoom.transform.position + new Vector3(30, 0, 0), Quaternion.identity);

                            CurrRoom = Instantiate(CreateThisRoom, PrevRoom.transform.position + new Vector3(60, 0, 0), Quaternion.identity); //create room

                            CanMakeRoomHere = true;
                            currentcoords += new Vector2(1, 0);
                            break;
                        }
                        else
                        {
                            //Debug.Log("Unable to create room right room: " + i);
                            checkedspot[2] = true;
                            continue;
                        }
                    case 3:
                        if (!checkedspot[3] && GridOfDungeon[(int)currentcoords.x - 1][(int)currentcoords.y] == 0)
                        {
                            corridor = Instantiate(CorridorPrefab, PrevRoom.transform.position + new Vector3(-30, 0, 0), Quaternion.identity);
                            corridor.transform.Rotate(0, 0, 180);

                            CurrRoom = Instantiate(CreateThisRoom, PrevRoom.transform.position + new Vector3(-60, 0, 0), Quaternion.identity); //create boss room

                            CanMakeRoomHere = true;
                            currentcoords -= new Vector2(1, 0);
                            break;
                        }
                        else
                        {
                            //Debug.Log("Unable to create room left room: " + i);
                            checkedspot[3] = true;
                            continue;
                        }
                    default:
                        break;
                }

                //check if all positions have been checked, if yes, break from loop (no need this anymore)
                {
                //    int havecheckedhowmany = 0;
                //    for (int z = 0; z < checkedspot.Length; ++z)
                //    {
                //        if (checkedspot[z])
                //        {
                //            ++havecheckedhowmany;
                //        }
                //    }

                //    if (havecheckedhowmany == 4) //if checked all 4 corners
                //    {
                //        Debug.Log("Checked all sides");
                //        i = 100;
                //        CanMakeRoomHere = true;
                //    }
                }
            }


            //letting room know which is where
            {
                //Debug.Log("Room " + i + " created");
                
                LetRoomsKnow(LocationOfNewRoomRelativeToCurrent, PrevRoom, CurrRoom); //let them know their parent and children
                corridor.GetComponent<CorridorScript>().AssignNeighbours(CurrRoom, PrevRoom, LocationOfNewRoomRelativeToCurrent); //tell the corridor who is infront and tell the corridor who is behind
                GridOfDungeon[(int)currentcoords.x][(int)currentcoords.y] = 1; //tell the list that the area is occupied

                if (i != UpgradeRoomLocation) //if the room is not a upgrade room (or rooms without enemies, you do not need grids for pathfinding)
                    CurrRoom.GetComponent<TestGrid>().enabled = false;

                PrevRoom = CurrRoom;
                PrevRoom.GetComponent<RoomHandler>().SetRoomCoordinates(currentcoords);

                //checking deadend
                {
                    if (i == AmountOfRooms - 2)
                    {
                        EndRooms.Add(PrevRoom); //add last room
                        bool checkingfordeadends = false;

                        while (!checkingfordeadends) //check list until the room location is fine
                        {
                            //randomise locations the boss could spawn in
                            int randomnumberforbossroom = Random.Range(0, EndRooms.Count);

                            PrevRoom = EndRooms[randomnumberforbossroom];
                            currentcoords = PrevRoom.GetComponent<RoomHandler>().ReturnCoords(); //match the coords of the new room

                            checkingfordeadends = CheckIfThereIsAnythingThere(currentcoords);
                        }
                    }
                    else if (!CheckIfThereIsAnythingThere(currentcoords) || i == changedirections)
                    {
                        //Debug.Log("Found dead end!"); //found dead end
                        EndRooms.Add(PrevRoom);
                        bool nodeadends = false;
                        while (!nodeadends)
                        {
                            int BackTrackHowMuch = Random.Range(2, i - 1); //randomly back track

                            for (int j = 0; j < BackTrackHowMuch; ++j)
                            {
                                PrevRoom = PrevRoom.GetComponent<RoomHandler>().ReturnPreviousRoom(); //keep backtracking until the wanted value
                            }

                            currentcoords = PrevRoom.GetComponent<RoomHandler>().ReturnCoords(); //match the coords of the new room

                            nodeadends = CheckIfThereIsAnythingThere(currentcoords);
                        }
                    }
                }
            }
        }
    }

    void LetRoomsKnow(int LocationOfNewRoomRelativeToCurrent, GameObject PreviousRoom, GameObject CurrentRoom)
    {
        //let the previous room know where the next is
        PreviousRoom.GetComponent<RoomHandler>().AssignNext(CurrentRoom);
        //let the previous room know where is it relatively
        PreviousRoom.GetComponent<RoomHandler>().AssignRoomTo(LocationOfNewRoomRelativeToCurrent, CurrentRoom);

        //let the next room know where the previous is
        CurrentRoom.GetComponent<RoomHandler>().AssignPrev(PreviousRoom);
        //let the next room know where is it relatively
        if (LocationOfNewRoomRelativeToCurrent % 2 == 0 || LocationOfNewRoomRelativeToCurrent == 0)
            CurrentRoom.GetComponent<RoomHandler>().AssignRoomTo(LocationOfNewRoomRelativeToCurrent + 1, PreviousRoom);
        else
            CurrentRoom.GetComponent<RoomHandler>().AssignRoomTo(LocationOfNewRoomRelativeToCurrent - 1, PreviousRoom);

        // 1 or 3 will be west or north so just need to minus 1 for both of them as its the opposite (vice versa for 0 or 2 but plus instead of minus)
    }

    bool CheckIfThereIsAnythingThere(Vector2 currentcoords)
    {
        //Debug.Log(currentcoords);
        bool[] IsThisDeadEnd = new bool[4];
        int IsThisAreaOccupied = 0;

        for (int j = 0; j < IsThisDeadEnd.Length; ++j)
        {
            IsThisDeadEnd[j] = false;
        }

        //now check to see if this final room is a dead end
        if ((int)currentcoords.y + 1 > 14)
        {
            IsThisDeadEnd[0] = true;
            IsThisAreaOccupied++;
        }
        if ((int)currentcoords.y - 1 < 0)
        {
            IsThisDeadEnd[1] = true;
            IsThisAreaOccupied++;
        }
        if ((int)currentcoords.x + 1 > 14)
        {
            IsThisDeadEnd[2] = true;
            IsThisAreaOccupied++;
        }
        if ((int)currentcoords.x - 1 < 0)
        {
            IsThisDeadEnd[3] = true;
            IsThisAreaOccupied++;
        }

        //if (IsThisAreaOccupied > 0)
        //{
        //    Debug.Log("Hit grid limit " + IsThisAreaOccupied);
        //}

        //see if any adjacent room is occupied
        for (int j = 0; j < IsThisDeadEnd.Length; ++j)
        {
            if (!IsThisDeadEnd[j])
            {
                switch (j)
                {
                    case 0:
                        if (GridOfDungeon[(int)currentcoords.x][(int)currentcoords.y + 1] == 1)
                        {
                            IsThisAreaOccupied++;
                            //Debug.Log("Top is occupied, current room is number: " + i);
                            //if (i == 9)
                            //{
                            //    Debug.Log(IsThisAreaOccupied);
                            //}
                        }
                        break;
                    case 1:
                        if (GridOfDungeon[(int)currentcoords.x][(int)currentcoords.y - 1] == 1)
                        {
                            IsThisAreaOccupied++;
                            //Debug.Log("Bottom is occupied, current room is number: " + i);
                            //if (i == 9)
                            //{
                            //    Debug.Log(IsThisAreaOccupied);
                            //}
                        }
                        break;
                    case 2:
                        if (GridOfDungeon[(int)currentcoords.x + 1][(int)currentcoords.y] == 1)
                        {
                            IsThisAreaOccupied++;
                            //Debug.Log("Right is occupied, current room is number: " + i);
                            //if (i == 9)
                            //{
                            //    Debug.Log(IsThisAreaOccupied);
                            //}
                        }
                        break;
                    case 3:
                        if (GridOfDungeon[(int)currentcoords.x - 1][(int)currentcoords.y] == 1)
                        {
                            IsThisAreaOccupied++;
                            //Debug.Log("Left is occupied, current room is number: " + i);
                            //if (i == 9)
                            //{
                            //    Debug.Log(IsThisAreaOccupied);
                            //}
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        if (IsThisAreaOccupied >= 4)
            return false;
        else
            return true;

    }
}