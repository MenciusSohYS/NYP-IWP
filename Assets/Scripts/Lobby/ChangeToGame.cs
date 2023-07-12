using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToGame : MonoBehaviour
{
    private GameObject Player;
    public AudioSource SoundForBGM;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Globalvariables.timerforsound = SoundForBGM.time;
            //Debug.Log(Globalvariables.timerforsound);
            SceneManager.LoadScene("GameScene");
        }
    }
}
