using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScript : MonoBehaviour
{
    public GameObject[] walls; //0 is North (Top), 1 is South (bottom), 2 is East (right), 3 is West (left)

    public GameObject[] doors;

    [SerializeField] int EnemyCost;

    public GameObject FCoverPrefab; //prefabs
    public GameObject HCoverPrefab;
    public GameObject RoomObjects;

    [SerializeField] GameObject[] Covers; //an array of covers
    // Start is called before the first frame update
    void Start()
    {
        Covers = new GameObject[10];

        for (int i = 0; i < 10; ++i)
        {
            bool allclear = true;
            float x = Random.Range(-14.5f, 14.5f);
            float y = Random.Range(-14.5f, 14.5f); //random position

            for (int j = 0; j < Covers.Length; ++j) //go through list to see if they can fit
            {
                if (Covers[j] == null) //if the position in the array has nothing, skip it
                    continue;
                else if ((transform.position.x + x < Covers[j].transform.position.x + 1.5 && transform.position.x + x > Covers[j].transform.position.x - 1.5) && (transform.position.y + y < Covers[j].transform.position.y + 1.5 && transform.position.y + y > Covers[j].transform.position.y - 1.5))
                {
                    --i;
                    allclear = false;
                    Debug.Log("not clear"); //if there is another box already in the area, dont put one down
                    break;
                }
            }

            if (allclear)
                Covers[i] = Instantiate(HCoverPrefab, this.transform.position + new Vector3(x, y), Quaternion.identity);
        }
    }

    public void UpdateRoom(int Side) //is there something there?
    {
        doors[Side].SetActive(true);
        walls[Side].SetActive(false);
    }
}
