using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorScript : MonoBehaviour
{
    [SerializeField] GameObject PreviousRoom;
    [SerializeField] GameObject NextRoom;
    [SerializeField] GameObject Entry;
    [SerializeField] GameObject Exit;
    // Start is called before the first frame update

    public void AssignPrev(GameObject Prev)
    {
        PreviousRoom = Prev;
    }

    public void AssignNext(GameObject Next)
    {
        NextRoom = Next;
    }
}
