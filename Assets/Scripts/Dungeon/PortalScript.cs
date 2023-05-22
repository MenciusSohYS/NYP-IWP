using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
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
        transform.Rotate(0, 0, 1);

        if (CanMoveOn)
        {
            Player.GetComponent<PlayerMechanics>().MessagePlayer("Press E to move on");
            if (Input.GetKeyDown(KeyCode.E))
            {
                //push level, current hp and max hp to global variables
                ++Globalvariables.CurrentLevel;
                Globalvariables.CurrentHP = Player.GetComponent<PlayerMechanics>().GetCurrentHP();
                Globalvariables.MaxHP = Player.GetComponent<PlayerMechanics>().GetMaxHP();

                //set weapon stats and stuff, to be used when you start the new level
                WeaponParent PlayerWeaponScript = Player.transform.GetChild(0).GetChild(0).GetComponent<WeaponParent>();
                GunScript PlayerGunScript = Player.transform.GetChild(0).GetComponent<GunScript>();
                Globalvariables.WeaponComponents.Damage = PlayerWeaponScript.GetDamage();
                Globalvariables.WeaponComponents.FireRate = PlayerWeaponScript.GetFireRate();
                Globalvariables.WeaponComponents.ReloadTime = PlayerWeaponScript.GetReloadTime();
                Globalvariables.WeaponComponents.MagSize = PlayerWeaponScript.GetMaxMagSize();
                Globalvariables.WeaponComponents.CurrSize = PlayerWeaponScript.ReturnCurrentMag();
                Globalvariables.WeaponComponents.Spread = PlayerWeaponScript.GetSpread();
                Globalvariables.WeaponComponents.HeatMax = PlayerWeaponScript.ReturnMaxHeat();
                Globalvariables.WeaponComponents.Velocity = PlayerWeaponScript.ReturnVelocity();
                Globalvariables.WeaponComponents.CurrentUpgrades = PlayerGunScript.ReturnCurrentUpgrade();
                
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);//reload the scene and progess the player
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
}
