using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private GameObject Player;
    private bool CanInteract;
    private PlayerMechanics PlayerMechs;
    public GameObject[] ThisToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerMechs = Player.GetComponent<PlayerMechanics>();
        CanInteract = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanInteract)
        {
            PlayerMechs.MessagePlayer("Press E to open chest");
            if (Input.GetKeyDown(KeyCode.E))
            {
                int randint = Random.Range(0, ThisToSpawn.Length);
                GameObject NewObj = Instantiate(ThisToSpawn[randint], transform.position, Quaternion.identity);
                NewObj.GetComponent<CircleCollider2D>().enabled = true;
                NewObj.GetComponent<WeaponDropped>().enabled = true;
                GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanInteract = false;
        }
    }
}
