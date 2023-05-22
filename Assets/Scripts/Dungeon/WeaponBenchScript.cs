using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBenchScript : MonoBehaviour
{
    private GameObject Player;
    private bool CanMoveOn;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CanMoveOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMoveOn)
        {
            Player.GetComponent<PlayerMechanics>().MessagePlayer("Press E to upgrade weapon");
            if (Input.GetKeyDown(KeyCode.E))
            {
                UpgradeWeapon();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanMoveOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanMoveOn = false;
        }
    }

    private void UpgradeWeapon()
    {
        Player.transform.GetChild(0).GetComponent<GunScript>().UpgradeWeapon(1);
    }
}
