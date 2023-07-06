using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    [SerializeField] Vector3 Origin;
    [SerializeField] PathFinding PathFind;
    public GameObject testEnemy;
    public GameObject Sqare;
    [SerializeField] GameObject ThisEnemy;
    [SerializeField] DungeonScript RoomDScript;
    [SerializeField] List<GameObject> ListOfUnwalkables;

    //add width and height of room here
    private void Start()
    {
    }

    private void Update()
    {
                
    }

    public void CreateGrid()
    {
        if (PathFind == null)
        {
            Origin = transform.position;
            //Debug.Log("ORIGIN: " + Origin);
            PathFind = new PathFinding(30, 30, (int)transform.position.x, (int)transform.position.y, Origin); //set the height and width here
            //ThisEnemy = Instantiate(testEnemy, Origin, Quaternion.identity);

            RoomDScript = GetComponent<DungeonScript>();
            for (int i = 0; i < RoomDScript.Covers.Length; ++i)
            {
                if (RoomDScript.Covers[i] != null)
                {
                    int X = (int)RoomDScript.Covers[i].transform.localPosition.x + 15;
                    int Y = (int)RoomDScript.Covers[i].transform.localPosition.y + 15;

                    //ListOfUnwalkables.Add(Instantiate(Sqare, RoomDScript.Covers[i].transform.position, Quaternion.identity));
                    PathFind.GetNode(X, Y).SetIsWalkable(false);
                    //Debug.Log("Set This " + i + " To False");
                }
            }
        }
    }

    public void DestoryGrid()
    {
        if (PathFind != null)
        {
            for (int i = 0; i < 30; ++i)
            {
                for (int j = 0; j < 30; ++j)
                {
                    PathFind.GetNode(i, j).SetIsWalkable(true);
                }
            }
        }
    }

    public bool TestIfGridIsTrue(int Xaxis, int Yaxis)
    {
        //Debug.Log(Xaxis + " and " + Yaxis + " and " + PathFind + " TOF: " + PathFind.GetNode(Xaxis, Yaxis).IsWalkable);

        //Debug.DrawLine(new Vector3(Xaxis - 15, Yaxis - 15) + transform.position, new Vector3(Xaxis + 1 - 15, Yaxis + 1 - 15) + transform.position, Color.cyan, 15f);

        return PathFind.GetNode(Xaxis, Yaxis).IsWalkable;
    }
}
