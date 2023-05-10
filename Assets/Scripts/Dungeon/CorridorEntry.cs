using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorEntry : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //player has entered corridor, reveal the next room
        //Debug.Log(collision.transform.tag);
        if (collision.transform.tag == "Player")
        {
            transform.parent.parent.GetComponent<CorridorScript>().RevealNextRoom();
            //Debug.Log("Player has entered the corridor");
        }
    }
}
