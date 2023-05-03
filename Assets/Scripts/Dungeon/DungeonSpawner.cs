using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour
{
    [SerializeField] int AmountOfRooms;
    public GameObject RoomPrefab;
    [SerializeField] List<bool> MapOfDungeon;
    [SerializeField] int [,] GridOfDungeon;

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

        MapOfDungeon = new List<bool>();
        for (int i = 0; i < 15; ++i)
        {
            for (int j = 0; j < 15; ++j)
            {
                MapOfDungeon.Add(false);
            }
        }

        GameObject CurrRoom;
        GameObject PrevRoom;
        Vector2 currentcoords = new Vector2(7, 7);

        AmountOfRooms = Random.Range(5, 7); //randomise amount of rooms

        PrevRoom = Instantiate(RoomPrefab, new Vector3(0, 0, 0), Quaternion.identity); //create the first room
        CurrRoom = PrevRoom;
        PrevRoom.GetComponent<RoomHandler>().SetRoomCoordinates(currentcoords); //tell it where it is on the map


        MapOfDungeon.RemoveAt((int)currentcoords.x * (int)currentcoords.y);
        MapOfDungeon.Insert((int)currentcoords.x * (int)currentcoords.y, true); //tell the list its already occupied

        Debug.Log("room 0 created");


        for (int i = 1; i < AmountOfRooms; ++i) //for loop to create everything
        {
            bool CanMakeRoomHere = false;

            bool[] checkedspot = new bool[4];

            int LocationOfNewRoomRelativeToCurrent = 0;

            for (int z = 0; z < checkedspot.Length; ++z) //have not checked spot
                checkedspot[z] = false;

            while (!CanMakeRoomHere) //will keep running loop until we have no more rooms
            {
                LocationOfNewRoomRelativeToCurrent = Random.Range(0, 3);

                //check if we can make a room at this place
                switch (LocationOfNewRoomRelativeToCurrent)//see where we can place a room
                {
                    case 0:
                        if (!checkedspot[0] && !MapOfDungeon[(int)currentcoords.x * ((int)currentcoords.y + 1)])
                        {
                            CurrRoom = Instantiate(RoomPrefab, PrevRoom.transform.position + new Vector3(0, 30, 0), Quaternion.identity); //create room
                            CanMakeRoomHere = true; //can make a room at this location
                            currentcoords += new Vector2(0, 1); //set the new co ordinates
                            break;
                        }
                        else
                        {
                            checkedspot[0] = true;
                            continue;
                        }
                    case 1:
                        if (!checkedspot[1] && !MapOfDungeon[(int)currentcoords.x * ((int)currentcoords.y - 1)])
                        {
                            CurrRoom = Instantiate(RoomPrefab, PrevRoom.transform.position + new Vector3(0, -30, 0), Quaternion.identity);
                            CanMakeRoomHere = true;
                            currentcoords -= new Vector2(0, 1);
                            break;
                        }
                        else
                        {
                            checkedspot[1] = true;
                            continue;
                        }
                    case 2:
                        if (!checkedspot[2] && !MapOfDungeon[((int)currentcoords.x + 1) * (int)currentcoords.y])
                        {
                            CurrRoom = Instantiate(RoomPrefab, PrevRoom.transform.position + new Vector3(30, 0, 0), Quaternion.identity);
                            CanMakeRoomHere = true;
                            currentcoords += new Vector2(1, 0);
                            break;
                        }
                        else
                        {
                            checkedspot[2] = true;
                            continue;
                        }
                    case 3:
                        if (!checkedspot[3] && !MapOfDungeon[((int)currentcoords.x - 1 )* (int)currentcoords.y])
                        {
                            CurrRoom = Instantiate(RoomPrefab, PrevRoom.transform.position + new Vector3(-30, 0, 0), Quaternion.identity);
                            CanMakeRoomHere = true;
                            currentcoords -= new Vector2(1, 0);
                            break;
                        }
                        else
                        {
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

                    if (havecheckedhowmany > 3) //if checked all 4 corners
                    {
                        Debug.Log("Checked all sides");
                        i = 100;
                        break;
                    }
                }
            }
            Debug.Log("Room " + i + " created");
            LetRoomsKnow(LocationOfNewRoomRelativeToCurrent, PrevRoom, CurrRoom); //let them know their parent and children

            MapOfDungeon.RemoveAt((int)currentcoords.x * (int)currentcoords.y); //tell the list that the area is occupied
            MapOfDungeon.Insert((int)currentcoords.x * (int)currentcoords.y, true);

            PrevRoom = CurrRoom;
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