using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutor : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] GameObject Canvas;
    private bool CanInteract;
    private bool IsInGame = false;
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
                if (!IsInGame)
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
                else
                {
                    Player.GetComponent<PlayerMovement>().enabled = false;
                    bool HasConvoEnded = Canvas.GetComponent<CanvasScript>().CycleThroughDialogue();
                    Canvas.GetComponent<CanvasScript>().SetText("Press E to continue...", 1);
                    if (HasConvoEnded)
                    {
                        Player.GetComponent<PlayerMovement>().enabled = true; //Disable Player movements
                        Canvas.GetComponent<CanvasScript>().SetText("", 1);
                        CanInteract = false;
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            if (!IsInGame)
            {
                Canvas.GetComponent<CharacterSelectScript>().ShowMessage("Press E to talk to the tutor");
                CanInteract = true;
            }
            else
            {
                FindCanvas();
                Canvas.GetComponent<CanvasScript>().SetText("Press E to talk to the tutor", 1);
                CanInteract = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            if (!IsInGame)
            {
                Canvas.GetComponent<CharacterSelectScript>().HideMessage();
                CanInteract = false;
            }
            else
            {
                FindCanvas();
                Canvas.GetComponent<CanvasScript>().SetText("", 1);
                CanInteract = false;
            }
        }
    }

    public void SetPlayer(GameObject NewPlayer, bool InGame)
    {
        Player = NewPlayer;
        IsInGame = InGame;
    }

    public void FindCanvas()
    {
        if (Canvas != null)
            return;

        Canvas = GameObject.FindGameObjectWithTag("Canvas");
    }
}
