using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    public void SetNumber(string Number)
    {
        GetComponent<TextMeshPro>().text = Number;
    }


    private void Update()
    {
        transform.position += new Vector3(0, 2) * Time.deltaTime;
    }
}
