using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    public TextMeshPro textfield;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    public void SetNumber(string Number, bool Crit)
    {
        textfield.text = Number;
        if (Crit)
        {
            textfield.color = Color.red;
        }
    }


    private void Update()
    {
        transform.position += new Vector3(0, 2) * Time.deltaTime;
    }
}
