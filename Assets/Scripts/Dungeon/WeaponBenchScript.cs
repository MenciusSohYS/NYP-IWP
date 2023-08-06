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
    CanvasScript CVScript;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CanMoveOn = false;
        colliders = GetComponentsInChildren<BoxCollider2D>(true); //true to include those that are inactive
        CVScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>();
        //Debug.Log(colliders.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMoveOn)
        {
            if (Player.transform.GetChild(0).GetComponent<GunScript>().ReturnCurrentUpgrade() == 0)
            {
                Player.GetComponent<PlayerMechanics>().MessagePlayer("Press E to upgrade weapon");
            }
            else
            {
                Player.GetComponent<PlayerMechanics>().MessagePlayer("Press E to spend 100 Coins to upgrade");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Player.transform.GetChild(0).GetComponent<GunScript>().ReturnCurrentUpgrade() > 0 && CVScript.GetCoins() < 100)
                {
                    Player.GetComponent<PlayerMechanics>().MessagePlayer("NOT ENOUGH MONEY!");
                }
                else
                {
                    if (Player.transform.GetChild(0).GetComponent<GunScript>().ReturnCurrentUpgrade() > 0)
                    {
                        PlayFabHandler.DeductCoins(100);
                        CVScript.SetCoins(CVScript.GetCoins() - 100);
                    }
                    Player.GetComponent<PlayerMechanics>().MessagePlayer("Upgraded!");
                    UpgradeWeapon();
                    foreach (BoxCollider2D collider in colliders)
                    {
                        collider.enabled = false;
                        CanMoveOn = false;
                    }
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
        if (Player.transform.GetChild(0).GetComponent<GunScript>().ReturnCurrentUpgrade() == 0)
        {
            Player.transform.GetChild(0).GetComponent<GunScript>().UpgradeWeapon(1);
        }
        else
        {
            CVScript.OpenUpgradePanel();
        }

        AudioSourceField.Play();
        CircleCreatorScript.Create(new Color(0, 255, 0));
    }
}
