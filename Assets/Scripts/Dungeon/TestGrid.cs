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
        if (PathFind == null)
        {
            Origin = transform.position;
            PathFind = new PathFinding(28, 28, (int)transform.position.x, (int)transform.position.y, Origin); //set the height and width here
            //ThisEnemy = Instantiate(testEnemy, Origin, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Vector3 currPosition = Camera.main.transform.position;

            ////Debug.Log((int)currPosition.x + " , " + (int)currPosition.y);

            //List<PathNode> Path = PathFind.FindPath((int)Origin.x - (int)transform.position.x, (int)Origin.y - (int)transform.position.y, (int)currPosition.x, (int)currPosition.y);
            
            ////Debug.Log((int)Origin.x + " , " + (int)Origin.y);

            //if (Path != null)
            //{
            //    Debug.Log("drawing");
            //    for (int i = 0; i < Path.Count - 1; ++i)
            //    {
            //        Debug.DrawLine(new Vector3(Path[i].x + Origin.x - (PathFind.GetGrid().GetWidth() * 0.5f),
            //            Path[i].y + Origin.y - (PathFind.GetGrid().GetHeight() * 0.5f)),
            //            new Vector3(Path[i + 1].x + Origin.x - (PathFind.GetGrid().GetWidth() * 0.5f), Path[i + 1].y +
            //            Origin.y - (PathFind.GetGrid().GetHeight() * 0.5f)), Color.green, 2f);
            //    }
            //}
        }
        if (RoomDScript == null)
        {
            if (transform.position.x == 0 && transform.position.y == 0)
            {
                //Debug.Log(PathFind);
                RoomDScript = GetComponent<DungeonScript>();
                for (int i = 0; i < RoomDScript.Covers.Length; ++i)
                {
                    int X = (int)RoomDScript.Covers[i].transform.position.x + 14;
                    int Y = (int)RoomDScript.Covers[i].transform.position.y + 14;

                    //ListOfUnwalkables.Add(Instantiate(Sqare, RoomDScript.Covers[i].transform.position, Quaternion.identity));
                    PathFind.GetNode(X, Y).SetIsWalkable(false);
                }
            }
        }
    }
}
