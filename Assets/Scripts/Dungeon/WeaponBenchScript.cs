using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBenchScript : MonoBehaviour
{
    private GameObject Player;
    private bool CanMoveOn;
    BoxCollider2D[] colliders;
    public CircleCreator CircleCreatorScript;
    public AudioSource AudioSourceField;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CanMoveOn = false;
        colliders = GetComponentsInChildren<BoxCollider2D>(true); //true to include those that are inactive
        //Debug.Log(colliders.Length);
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
                foreach (BoxCollider2D collider in colliders)
                {
                    collider.enabled = false;
                    CanMoveOn = false;
                }
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
        AudioSourceField.Play();
        Player.transform.GetChild(0).GetComponent<GunScript>().UpgradeWeapon(1);
        CircleCreatorScript.Create(new Color(0, 255, 0));
    }
}
