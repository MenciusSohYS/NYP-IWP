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
    void Start()
    {

        prevposition = Input.mousePosition;

        Vector3 CenterPivot = Camera.main.WorldToScreenPoint(transform.position);

        float PrevAngle = Mathf.Atan2(CenterPivot.x - prevposition.x, prevposition.y - CenterPivot.y);
        transform.Rotate(new Vector3(0, 0, PrevAngle * Mathf.Rad2Deg));


        //find weaponscript
        WeaponScript = transform.GetChild(0).GetComponent<WeaponParent>();
        timerforshooting = 0;

        WeaponScript.SetFireRate(0.1f);
        WeaponScript.SetDamage(7);
    }
    void Update()
    {
        timerforshooting -= Time.deltaTime;
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

        if (Input.GetMouseButtonDown(0) && timerforshooting <= 0)
        {
            timerforshooting = WeaponScript.Attack(BulletSpawner, Bullet, true);
        }
    }
}
