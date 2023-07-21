using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToGame : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("LobbyCanvas").GetComponent<CharacterSelectScript>().ChooseDifficulty();
        }
    }
}
