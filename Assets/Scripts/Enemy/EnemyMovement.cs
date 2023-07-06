using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] List<Vector3> PathVectorList;
    public GameObject SquarePrefab;
    private GameObject PlayerGO;
    private EnemyMechanics enemyMechanicsScript;
    private Vector3 RoomOffset;

    //public GameObject Room;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerGO = GameObject.FindGameObjectWithTag("Player");
        enemyMechanicsScript = GetComponent<EnemyMechanics>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug here
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    //Room = GameObject.FindGameObjectWithTag("RoomObj");
        //    if (PlayerGO == null)
        //        PlayerGO = GameObject.FindGameObjectWithTag("Player");

        //    //RoomOffset = enemyMechanicsScript.GetRoomCoordinates();
        //    MovingToTarget(PlayerGO.transform.position);

        //}
        if (PathVectorList != null && PathVectorList.Count > 0)
        {
            Quaternion CurrentRotation = transform.GetChild(0).rotation;

            Vector3 MoveDirection = (PathVectorList[0] - transform.position).normalized;
            transform.position = transform.position + MoveDirection * 6 * Time.deltaTime;

            transform.GetChild(0).rotation = CurrentRotation;

            if (Vector3.Distance(PathVectorList[0], transform.position) < 0.1f)
            {
                PathVectorList.RemoveAt(0);
            }
        }
    }

    public void MovingToTarget(Vector3 TargetPos)
    {
        //Debug.Log(transform.localPosition - RoomOffset);
        //Debug.Log("ENEMY POS: " + transform.position);
        if (PathVectorList != null && PathVectorList.Count > 0)
        {
            Vector3 RoundVector3Part1 = new Vector3((int)Mathf.Round(PathVectorList[PathVectorList.Count - 1].x), (int)Mathf.Round(PathVectorList[PathVectorList.Count - 1].y));
            Vector3 PositionToChange = PathFinding.Instance.ConvertWorldPos(RoundVector3Part1);
            PathFinding.Instance.GetNode((int)(PositionToChange.x), (int)(PositionToChange.y)).SetIsWalkable(true);
        }

        Vector3 RoundVector3 = new Vector3((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y));
        Vector3 NewPos = PathFinding.Instance.ConvertWorldPos(RoundVector3);
        PathFinding.Instance.GetNode((int)(NewPos.x), (int)(NewPos.y)).SetIsWalkable(true);

        PathVectorList = PathFinding.Instance.FindPath(transform.position, TargetPos);

        DrawDebugRay();
        //if (PathFinding.Instance.GetNode((int)transform.position.x + 14, (int)transform.position.y + 14).IsWalkable)
        {
            //PathFinding.Instance.GetNode((int)TargetPos.x + 14, (int)TargetPos.y + 14).SetIsWalkable(false);
            //PathFinding.Instance.GetNode((int)transform.position.x + 14, (int)transform.position.y + 14).SetIsWalkable(true);

            //debug
            {
                //GameObject Sqare = Instantiate(SquarePrefab, TargetPos, Quaternion.identity);

                //Sqare.transform.SetParent(Room.transform);

                //for (int i = 0; i < Sqare.transform.childCount; ++i)
                //{
                //    if (Sqare.transform.GetChild(i).position == transform.position)
                //    {
                //        Destroy(Sqare);
                //        break;
                //    }
                //}
            }
        }
    }

    public void DrawDebugRay()
    {
        if (PathVectorList != null && PathVectorList.Count > 0)
        {
            for (int i = 0; i < PathVectorList.Count - 1; ++i)
            {
                Debug.DrawLine(PathVectorList[i], PathVectorList[i + 1], Color.blue, 15f);
            }
        }
    }

    public int VectorListCount()
    {
        return PathVectorList.Count;
    }

    public Vector3 ReturnLastLocation()
    {
        return PathVectorList[PathVectorList.Count - 1];
    }
}
