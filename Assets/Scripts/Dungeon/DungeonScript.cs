using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScript : MonoBehaviour
{
    public GameObject[] walls; //0 is North (Top), 1 is South (bottom), 2 is East (right), 3 is West (left)

    public GameObject[] doors;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void UpdateRoom(int Side) //is there something there?
    {
        doors[Side].SetActive(true);
        walls[Side].SetActive(false);
    }
}
