using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveExpandScript : MonoBehaviour
{
    private void Update()
    {
        transform.localScale += new Vector3(0.7f, 0.7f, 0.7f) * Time.deltaTime;
    }
}
