using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDropped : MonoBehaviour
{
    private CircleCollider2D GunCollider;
    private GameObject Player;
    private GunScript PlayerGunScript;
    private SpriteRenderer GunRenderer;
    private bool CanPickUp;

    private void Start()
    {
        CanPickUp = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerGunScript = Player.transform.GetChild(0).GetComponent<GunScript>();
        GunRenderer = GetComponent<SpriteRenderer>();
        if (GunRenderer == null)
            GunRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

    }
    private void Update()
    {
        if (CanPickUp)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerGunScript.AssignNewGun(gameObject, false, true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanPickUp = true;
            GunRenderer.material.SetFloat("_OutlineWidth", 1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanPickUp = false;
            GunRenderer.material.SetFloat("_OutlineWidth", 0);
        }
    }
}
