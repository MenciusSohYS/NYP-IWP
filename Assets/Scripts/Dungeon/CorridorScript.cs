using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorScript : MonoBehaviour
{
    [SerializeField] GameObject PreviousRoom;
    [SerializeField] GameObject NextRoom;
    [SerializeField] int Orientation;
    [SerializeField] GameObject Entry;
    [SerializeField] GameObject Exit;
    // Start is called before the first frame update

    public void AssignNeighbours(GameObject Next, GameObject Prev, int Where)
    {
        PreviousRoom = Prev;
        NextRoom = Next;
        Orientation = Where;
    }

    public int ReturnOrientation()
    {
        return Orientation;
    }

    public void RevealNextRoom()
    {
        NextRoom.GetComponent<DungeonScript>().RemoveFog();
    }

    public void EnableEnemyAI()
    {
        NextRoom.GetComponent<DungeonScript>().EnableEnemies();
    }

    public int GetEnemyCount()
    {
        return NextRoom.GetComponent<DungeonScript>().GetEnemyCount();
    }
}