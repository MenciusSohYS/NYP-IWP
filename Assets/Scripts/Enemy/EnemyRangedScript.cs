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
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer ParentspriteRenderer;
    private bool ShootNow;
    void Start()
    {
        ShootNow = false;
        Reloading = false;
        ReloadTimer = 0f;

        Player = GameObject.FindGameObjectWithTag("Player");
        timerforshooting = 0;

        prevposition = Player.transform.position;

        Vector3 CenterPivot = transform.parent.position;

        float PrevAngle = Mathf.Atan2(CenterPivot.x - prevposition.x, prevposition.y - CenterPivot.y);
        transform.Rotate(new Vector3(0, 0, PrevAngle * Mathf.Rad2Deg));

        WeaponScript = transform.GetChild(0).GetComponent<WeaponParent>();

        if (WeaponScript == null)
        {
            WeaponScript = transform.GetChild(0).GetChild(0).GetComponent<WeaponParent>(); //if its a melee weapon
            if (WeaponScript.GetComponent<SwordScript>())
                WeaponScript.GetComponent<SwordScript>().SetIsPlayer(false);
            else
                WeaponScript.GetComponent<ScytheScript>().SetIsPlayer(false);
        }

        WeaponScript.SetDamage(0.5f);
        spriteRenderer = WeaponScript.GetComponent<SpriteRenderer>();
        ParentspriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        timerforshooting -= Time.deltaTime;
        WeaponScript.SetWeaponHeat(Time.deltaTime);
        //Follow player
        {
            Vector3 CenterPivot = transform.parent.position;
            Vector3 currPosition = Player.transform.position;

            float CurrAngle = Mathf.Atan2(CenterPivot.x - currPosition.x, currPosition.y - CenterPivot.y);
            transform.rotation = Quaternion.Euler(0, 0, CurrAngle * Mathf.Rad2Deg);


            if (currPosition.x < transform.position.x && !spriteRenderer.flipY)
            {
                spriteRenderer.flipY = true;
                if (BulletSpawner)
                    BulletSpawner.transform.localPosition = new Vector3(BulletSpawner.transform.localPosition.x, -BulletSpawner.transform.localPosition.y);
                ParentspriteRenderer.flipX = true;
            }
            else if (currPosition.x > transform.position.x && spriteRenderer.flipY)
            {
                spriteRenderer.flipY = false;
                if (BulletSpawner)  
                    BulletSpawner.transform.localPosition = new Vector3(BulletSpawner.transform.localPosition.x, -BulletSpawner.transform.localPosition.y);
                ParentspriteRenderer.flipX = false;
            }
        }

        //shooting part

        if (timerforshooting <= 0 && WeaponScript.ReturnCurrentMag() > 0 && ShootNow) //if the player is shooting, can shoot after fire rate cool down and has more than 1 bullet
        {
            timerforshooting = WeaponScript.Attack(BulletSpawner, Bullet, false); //shoot
            if (WeaponScript.ReturnCurrentMag() <= 0) //if no more bullets, start the reload
            {
                Reloading = true;
                WeaponScript.StartReload();
                ReloadTimer = WeaponScript.GetReloadTime();
            }
        }
        else if (Reloading)
        {
            transform.GetChild(0).Rotate(0, 0, 3);
            ReloadTimer = WeaponScript.DoReload(ReloadTimer, Time.deltaTime); //push the calculation to the gun for storing and dynamic reload
            if (ReloadTimer <= 0)
            {
                transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 90);
                Reloading = false; //completed reload
            }
        }
    }

    public void SetShootNow(bool setto)
    {
        ShootNow = setto;
    }

    public void ForceReload()
    {
        Reloading = true;
        WeaponScript.StartReload();
        ReloadTimer = WeaponScript.GetReloadTime();
    }

    public void IncreaseDamage(float ByHowMuch)
    {
        if (WeaponScript == null)
        {
            WeaponScript = transform.GetChild(0).GetChild(0).GetComponent<WeaponParent>(); //if its a melee weapon
            if (WeaponScript.GetComponent<SwordScript>())
                WeaponScript.GetComponent<SwordScript>().SetIsPlayer(false);
            else
                WeaponScript.GetComponent<ScytheScript>().SetIsPlayer(false);
        }
        WeaponScript.SetDamage(ByHowMuch);
        if (WeaponScript.GetDamage() > 100)
        {
            WeaponScript.SetDamageByNumber(100); //max cap the damage at 100 or the player might get 1 shotted and it might be too unfair
        }
    }

    public void SetMaxMagCap(int AmountToSet)
    {
        WeaponScript.SetMaxMagSize(AmountToSet);
    }

    public void ChangeBulletVelo(float ByHowMuch)
    {
        WeaponScript.MultiplyVelocity(ByHowMuch);
    }

    public bool ReturnShootNow()
    {
        return ShootNow;
    }

    public bool ReturnIsReloading()
    {
        return Reloading;
    }
    private void OnEnable()
    {
        WeaponScript = transform.GetChild(0).GetComponent<WeaponParent>();

        if (WeaponScript != null)
            WeaponScript.StartCanUpdate();
    }
}
