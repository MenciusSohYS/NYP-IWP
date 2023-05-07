using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] List<Vector3> PathVectorList;
    public GameObject SquarePrefab;

    //public GameObject Room;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    //Room = GameObject.FindGameObjectWithTag("RoomObj");
        //    if (Player.transform.position == null)
        //    {
        //        Debug.Log("Player pos not valid");
        //    }
        //    else
        //    {
        //        // MovingToTarget(Player.transform.position);
        //    }
        //}
        if (PathVectorList != null && PathVectorList.Count > 0)
        {
            Vector3 MoveDirection = (PathVectorList[0] - transform.position).normalized;
            transform.position = transform.position + MoveDirection * 6 * Time.deltaTime;
            if (Vector3.Distance(PathVectorList[0], transform.position) < 0.1f)
            {
                PathVectorList.RemoveAt(0);
            }
            else
            {
                //Debug.Log("Moving to " + PathVectorList[0].x + "," + PathVectorList[0].y);
            }
        }
    }

    public void MovingToTarget(Vector3 TargetPos)
    {
        if (PathFinding.Instance.GetNode((int)transform.position.x + 14, (int)transform.position.y + 14).IsWalkable)
        {
            PathVectorList = PathFinding.Instance.FindPath(transform.position, TargetPos);
            PathFinding.Instance.GetNode((int)TargetPos.x + 14, (int)TargetPos.y + 14).SetIsWalkable(false);
            PathFinding.Instance.GetNode((int)transform.position.x + 14, (int)transform.position.y + 14).SetIsWalkable(true);

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
}
