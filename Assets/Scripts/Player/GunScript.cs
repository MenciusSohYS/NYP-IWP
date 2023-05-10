using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    Vector3 prevposition;
    float timerforshooting;
    public GameObject Bullet;
    public Transform BulletSpawner;
    public WeaponParent WeaponScript;
    private bool Shooting;
    private bool Reloading;
    private float ReloadTimer;
    [SerializeField] PlayerMechanics PlayerMechanicsScript;
    void Start()
    {
        Shooting = false;
        Reloading = false;
        prevposition = Input.mousePosition;

        Vector3 CenterPivot = Camera.main.WorldToScreenPoint(transform.position);

        float PrevAngle = Mathf.Atan2(CenterPivot.x - prevposition.x, prevposition.y - CenterPivot.y);
        transform.Rotate(new Vector3(0, 0, PrevAngle * Mathf.Rad2Deg));

        //find weaponscript
        WeaponScript = transform.GetChild(0).GetComponent<WeaponParent>();
        timerforshooting = 0;


        PlayerMechanicsScript = transform.parent.GetComponent<PlayerMechanics>();
        PlayerMechanicsScript.ShowAmmoLeft(WeaponScript.ReturnCurrentMag());

        WeaponScript.SetFireRate(0.1f);
        WeaponScript.SetDamage(7);
    }
    void Update()
    {
        timerforshooting -= Time.deltaTime;
        WeaponScript.SetWeaponHeat(Time.deltaTime);
        //Follow mouse
        {
            Vector3 CenterPivot = Camera.main.WorldToScreenPoint(transform.position);

            float PrevAngle = Mathf.Atan2(CenterPivot.x - prevposition.x, prevposition.y - CenterPivot.y);

            Vector3 currPosition = Input.mousePosition;

            float CurrAngle = Mathf.Atan2(CenterPivot.x - currPosition.x, currPosition.y - CenterPivot.y);

            prevposition = currPosition;

            float AngleDiff = CurrAngle - PrevAngle;

            transform.Rotate(new Vector3(0, 0, AngleDiff * Mathf.Rad2Deg));

            if (currPosition.x < Screen.width / 2)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = true;
            }
            else
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = false;
            }
        }

        if (Reloading)
        {
            transform.GetChild(0).Rotate(0, 0, 3);
        }

        if (Input.GetMouseButtonUp(0)) //check if the mouse is being held down
        {
            Shooting = false;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Shooting = true;
        }


        if (Reloading)
        {
            ReloadTimer = WeaponScript.DoReload(ReloadTimer, Time.deltaTime); //push the calculation to the gun for storing and dynamic reload
            if (ReloadTimer <= 0)
            {
                transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 90);
                PlayerMechanicsScript.ShowAmmoLeft(WeaponScript.ReturnCurrentMag());
                Reloading = false; //completed reload
            }
        }
        else if (Shooting && timerforshooting <= 0 && WeaponScript.ReturnCurrentMag() > 0) //if the player is shooting, can shoot after fire rate cool down and has more than 1 bullet
        {
            timerforshooting = WeaponScript.Attack(BulletSpawner, Bullet, true); //shoot
            PlayerMechanicsScript.ShowAmmoLeft(WeaponScript.ReturnCurrentMag());
            if (WeaponScript.ReturnCurrentMag() <= 0) //if no more bullets, start the reload
            {
                Reloading = true;
                ReloadTimer = WeaponScript.GetReloadTime();
            }
        }
    }
}
