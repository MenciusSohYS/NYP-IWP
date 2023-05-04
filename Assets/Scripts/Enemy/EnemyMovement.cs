using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] List<Vector3> PathVectorList;
    [SerializeField] GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MovingToTarget(Player.transform.position);
        }
        if (PathVectorList != null && PathVectorList.Count > 0)
        {
            Vector3 MoveDirection = (PathVectorList[0] - transform.position).normalized;
            transform.position = transform.position + MoveDirection * 6 * Time.deltaTime;
            if (Vector3.Distance(PathVectorList[0], transform.position) < 0.5f)
            {
                PathVectorList.RemoveAt(0);
            }
            else
            {
                Debug.Log("Moving to " + PathVectorList[0].x + "," + PathVectorList[0].y);
            }
        }
    }

    public void MovingToTarget(Vector3 TargetPos)
    {
        PathVectorList = PathFinding.Instance.FindPath(transform.position, TargetPos);
    }
}
