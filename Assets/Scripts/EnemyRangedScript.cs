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
    void Start()
    {
        timerforshooting = 0;
        Player = GameObject.FindGameObjectWithTag("Player");
        timerforshooting = 0;

        prevposition = Player.transform.position;

        Vector3 CenterPivot = transform.parent.position;

        float PrevAngle = Mathf.Atan2(CenterPivot.x - prevposition.x, prevposition.y - CenterPivot.y);
        transform.Rotate(new Vector3(0, 0, PrevAngle * Mathf.Rad2Deg));
    }
    void Update()
    {
        //Follow player
        {

            timerforshooting -= Time.deltaTime;

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
        if (timerforshooting <= 0)
        {
            timerforshooting = WeaponScript.Attack(BulletSpawner, Bullet, false);
        }
    }
}
