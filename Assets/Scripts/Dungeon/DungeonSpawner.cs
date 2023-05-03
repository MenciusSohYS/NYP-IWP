using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour
{
    [SerializeField] int AmountOfRooms;
    public GameObject RoomPrefab;
    public GameObject CorridorPrefab;
    [SerializeField] int [,] GridOfDungeon;
    [SerializeField] int[] TestMap;

    // Start is called before the first frame update
    void Start()
    {
        GridOfDungeon = new int[15, 15];
        for (int i = 0; i < 15; ++i)
        {
            for (int j = 0; j < 15; ++j)
            {
                GridOfDungeon[i, j] = 0;
            }
        }

        GameObject CurrRoom;
        GameObject PrevRoom;
        Vector2 currentcoords = new Vector2(7, 7);

        AmountOfRooms = 6; // Random.Range(5, 7); //randomise amount of rooms

        PrevRoom = Instantiate(RoomPrefab, new Vector3(0, 0, 0), Quaternion.identity); //create the first room
        CurrRoom = PrevRoom;
        PrevRoom.GetComponent<RoomHandler>().SetRoomCoordinates(currentcoords); //tell it where it is on the map


        GridOfDungeon[(int)currentcoords.x, (int)currentcoords.y] = 1; //tell the list its already occupied

        Debug.Log("room 0 created");


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
                    //if (i < 8)
                    //{
                    //    LocationOfNewRoomRelativeToCurrent = TestMap[i];
                    //}
                    //else
                    //{
                    //}
                }
                LocationOfNewRoomRelativeToCurrent = Random.Range(0, 4);

                //check if we can make a room at this place
                switch (LocationOfNewRoomRelativeToCurrent)//see where we can place a room
                {
                    case 0:
                        if (!checkedspot[0] && GridOfDungeon[(int)currentcoords.x, (int)currentcoords.y + 1] == 0)
                        {
                            corridor = Instantiate(CorridorPrefab, PrevRoom.transform.position + new Vector3(0, 30, 0), Quaternion.identity);
                            corridor.transform.Rotate(0, 0, 90);


                            CurrRoom = Instantiate(RoomPrefab, PrevRoom.transform.position + new Vector3(0, 60, 0), Quaternion.identity); //create room
                            CanMakeRoomHere = true; //can make a room at this location
                            currentcoords += new Vector2(0, 1); //set the new co ordinates
                            break;
                        }
                        else
                        {
                            Debug.Log("Unable to create room above room: " + i);
                            checkedspot[0] = true;
                            continue;
                        }
                    case 1:
                        if (!checkedspot[1] && GridOfDungeon[(int)currentcoords.x, (int)currentcoords.y - 1] == 0)
                        {
                            corridor = Instantiate(CorridorPrefab, PrevRoom.transform.position + new Vector3(0, -30, 0), Quaternion.identity);
                            corridor.transform.Rotate(0, 0, -90);


                            CurrRoom = Instantiate(RoomPrefab, PrevRoom.transform.position + new Vector3(0, -60, 0), Quaternion.identity);
                            CanMakeRoomHere = true;
                            currentcoords -= new Vector2(0, 1);
                            break;
                        }
                        else
                        {
                            Debug.Log("Unable to create room below room: " + i);
                            checkedspot[1] = true;
                            continue;
                        }
                    case 2:
                        if (!checkedspot[2] && GridOfDungeon[(int)currentcoords.x + 1, (int)currentcoords.y] == 0)
                        {
                            corridor = Instantiate(CorridorPrefab, PrevRoom.transform.position + new Vector3(30, 0, 0), Quaternion.identity);


                            CurrRoom = Instantiate(RoomPrefab, PrevRoom.transform.position + new Vector3(60, 0, 0), Quaternion.identity);
                            CanMakeRoomHere = true;
                            currentcoords += new Vector2(1, 0);
                            break;
                        }
                        else
                        {
                            Debug.Log("Unable to create room right room: " + i);
                            checkedspot[2] = true;
                            continue;
                        }
                    case 3:
                        Debug.Log("turn left");
                        if (!checkedspot[3] && GridOfDungeon[(int)currentcoords.x - 1, (int)currentcoords.y] == 0)
                        {
                            corridor = Instantiate(CorridorPrefab, PrevRoom.transform.position + new Vector3(-30, 0, 0), Quaternion.identity);
                            corridor.transform.Rotate(0, 0, 180);


                            CurrRoom = Instantiate(RoomPrefab, PrevRoom.transform.position + new Vector3(-60, 0, 0), Quaternion.identity);
                            CanMakeRoomHere = true;
                            currentcoords -= new Vector2(1, 0);
                            break;
                        }
                        else
                        {
                            Debug.Log("Unable to create room left room: " + i);
                            checkedspot[3] = true;
                            continue;
                        }
                    default:
                        break;
                }

                //check if all positions have been checked, if yes, break from loop
                {
                    int havecheckedhowmany = 0;
                    for (int z = 0; z < checkedspot.Length; ++z)
                    {
                        if (checkedspot[z])
                        {
                            ++havecheckedhowmany;
                        }
                    }

                    if (havecheckedhowmany >= 3) //if checked all 4 corners
                    {
                        Debug.Log("Checked all sides");
                        i = 100;
                        CanMakeRoomHere = true;
                    }
                }
            }
            if (i != 100)
            {
                //Debug.Log("Room " + i + " created");
                
                LetRoomsKnow(LocationOfNewRoomRelativeToCurrent, PrevRoom, CurrRoom); //let them know their parent and children
                corridor.GetComponent<CorridorScript>().AssignNext(CurrRoom); //tell the corridor who is infront
                corridor.GetComponent<CorridorScript>().AssignPrev(PrevRoom); //tell the corridor who is behind
                GridOfDungeon[(int)currentcoords.x, (int)currentcoords.y] = 1; //tell the list that the area is occupied


                PrevRoom = CurrRoom;
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
}