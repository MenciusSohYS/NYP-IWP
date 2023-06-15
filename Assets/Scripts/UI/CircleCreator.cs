using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCreator : MonoBehaviour
{
    public GameObject CirclePrefab;
    private Color ColorToAssign;
    public void Create(Color NewColor)
    {
        ColorToAssign = NewColor;
        createcircle();
        Invoke("createcircle", 0.3f);
    }

    void createcircle()
    {
        GameObject NewCircle = Instantiate(CirclePrefab, transform.position, Quaternion.identity);
        NewCircle.GetComponent<SpriteRenderer>().color = ColorToAssign;
        NewCircle.transform.SetParent(transform);
    }
}
