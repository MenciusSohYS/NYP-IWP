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
        Debug.Log("ENEMY POS: " + transform.position);
        if (PathVectorList.Count > 0 && PathVectorList != null)
        {
            Vector3 PositionToChange = PathFinding.Instance.ConvertWorldPos(PathVectorList[PathVectorList.Count - 1]);
            PathFinding.Instance.GetNode((int)PositionToChange.x + 15, (int)PositionToChange.y + 15).SetIsWalkable(true);
        }
        PathVectorList = PathFinding.Instance.FindPath(transform.position, TargetPos);

        Vector3 NewPos = PathFinding.Instance.ConvertWorldPos(transform.position);
        PathFinding.Instance.GetNode((int)NewPos.x + 15, (int)NewPos.y + 15).SetIsWalkable(true);

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
        for (int i = 0; i < PathVectorList.Count - 1; ++i)
        {
            Debug.DrawLine(PathVectorList[i], PathVectorList[i + 1], Color.blue, 15f);
        }
    }
}
