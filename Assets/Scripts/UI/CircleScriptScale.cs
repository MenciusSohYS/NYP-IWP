using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScriptScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
    }
}
