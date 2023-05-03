using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    private PathFinding PathFind;
    private void Start()
    {
        PathFind = new PathFinding(28, 28, transform.position - new Vector3(14, 14, 0));
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {

            Vector3 currPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Debug.Log((int)currPosition.x + " , " + (int)currPosition.y);

            List<PathNode> Path = PathFind.FindPath(0, 0, (int)currPosition.x, (int)currPosition.y);

            if (Path != null)
            {
                for (int i = 0; i < Path.Count - 1; ++i)
                {
                    Debug.DrawLine(new Vector3(Path[i].x, Path[i].y), new Vector3(Path[i + 1].x, Path[i + 1].y), Color.green, 2f);
                }
            }
        }
    }
}
