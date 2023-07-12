using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutor : MonoBehaviour
{
    private GameObject Player;
    private GameObject Canvas;
    private bool CanInteract;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Canvas = GameObject.FindGameObjectWithTag("LobbyCanvas");
        CanInteract = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Player.GetComponent<PlayerMovement>().enabled = false; //Disable Player movements
                bool HasConvoEnded = Canvas.GetComponent<CharacterSelectScript>().CycleThroughDialogue();
                Canvas.GetComponent<CharacterSelectScript>().ShowMessage("Press E to continue...");
                if (HasConvoEnded)
                {
                    Player.GetComponent<PlayerMovement>().enabled = true; //Disable Player movements
                    Canvas.GetComponent<CharacterSelectScript>().HideMessage();
                    CanInteract = false;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            Canvas.GetComponent<CharacterSelectScript>().ShowMessage("Press E to talk to the tutor");
            CanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            Canvas.GetComponent<CharacterSelectScript>().HideMessage();
            CanInteract = false;
        }
    }

    public void SetPlayer(GameObject NewPlayer)
    {
        Player = NewPlayer;
    }
}
