using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{

    float MouseX;
    bool Flipped;
    // Start is called before the first frame update
    void Start()
    {
        Flipped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (MouseX != Input.mousePosition.x)
        {
            MouseX = Input.mousePosition.x;
        }
        else
        {
            return;
        }

        if (MouseX < Screen.width / 2 && Flipped == false)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).Rotate(0, 180, 0);
            }
            Flipped = true;
            //Debug.Log("Flipped");
        }
        else if (MouseX >= Screen.width / 2 && Flipped == true)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).Rotate(0, 180, 0);
            }
            Flipped = false;
            //Debug.Log("Not Flipped");
        }
    }
}
