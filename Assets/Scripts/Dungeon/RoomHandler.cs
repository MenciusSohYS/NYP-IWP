using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    [SerializeField] GameObject[] Rooms; //these will be the rooms next to the current room
    [SerializeField] GameObject PreviousRoom;
    [SerializeField] GameObject NextRoom;
    [SerializeField] Vector2 Coordinates;

    // Start is called before the first frame update
    void Start()
    {
        if (Rooms.Length != 4)
        {
            Rooms = new GameObject[4];

            for (int i = 0; i < Rooms.Length; ++i)
            {
                Rooms[i] = null;
            }
        }
    }

    public void AssignRoomTo(int where, GameObject ReferenceRoom) //where will follow DungeonScript's 0123
    {
        if (Rooms.Length != 4)
        {
            Rooms = new GameObject[4];

            for (int i = 0; i < Rooms.Length; ++i)
            {
                Rooms[i] = null;
            }
        }
        Rooms[where] = ReferenceRoom;
        GetComponent<DungeonScript>().UpdateRoom(where);
    }

    public void AssignPrev(GameObject Prev)
    {
        PreviousRoom = Prev;
    }

    public void AssignNext(GameObject Next)
    {
        NextRoom = Next;
    }

    public void SetRoomCoordinates(Vector2 currentcoords)
    {
        Coordinates = currentcoords;
    }
}
