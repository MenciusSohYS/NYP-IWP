using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedScript : MonoBehaviour
{
    Vector3 prevposition;
    float timerforshooting;
    public GameObject Bullet;
    public Transform BulletSpawner;
    private GameObject Player;
    public WeaponParent WeaponScript;
    private bool Reloading;
    private float ReloadTimer;
    void Start()
    {
        Reloading = false;
        ReloadTimer = 0f;

        Player = GameObject.FindGameObjectWithTag("Player");
        timerforshooting = 0;

        prevposition = Player.transform.position;

        Vector3 CenterPivot = transform.parent.position;

        float PrevAngle = Mathf.Atan2(CenterPivot.x - prevposition.x, prevposition.y - CenterPivot.y);
        transform.Rotate(new Vector3(0, 0, PrevAngle * Mathf.Rad2Deg));

        WeaponScript = transform.GetChild(0).GetComponent<WeaponParent>();
    }
    void Update()
    {
        timerforshooting -= Time.deltaTime;
        WeaponScript.SetWeaponHeat(Time.deltaTime);
        //Follow player
        {
            Vector3 CenterPivot = transform.parent.position;

            float PrevAngle = Mathf.Atan2(CenterPivot.x - prevposition.x, prevposition.y - CenterPivot.y);

            Vector3 currPosition = Player.transform.position;

            float CurrAngle = Mathf.Atan2(CenterPivot.x - currPosition.x, currPosition.y - CenterPivot.y);

            prevposition = currPosition;

            float AngleDiff = CurrAngle - PrevAngle;

            transform.Rotate(new Vector3(0, 0, AngleDiff * Mathf.Rad2Deg));

            if (currPosition.x < transform.position.x)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = true;
            }
            else
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = false;
            }
        }

        //shooting part
        if (Reloading)
        {
            transform.GetChild(0).Rotate(0, 0, 3);
        }

        if (timerforshooting <= 0 && WeaponScript.ReturnCurrentMag() > 0) //if the player is shooting, can shoot after fire rate cool down and has more than 1 bullet
        {
            timerforshooting = WeaponScript.Attack(BulletSpawner, Bullet, false); //shoot
            if (WeaponScript.ReturnCurrentMag() <= 0) //if no more bullets, start the reload
            {
                Reloading = true;
                ReloadTimer = WeaponScript.GetReloadTime();
            }
        }
        else if (Reloading)
        {
            ReloadTimer = WeaponScript.DoReload(ReloadTimer, Time.deltaTime); //push the calculation to the gun for storing and dynamic reload
            if (ReloadTimer <= 0)
            {
                transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 90);
                Reloading = false; //completed reload
            }
        }
    }
}
