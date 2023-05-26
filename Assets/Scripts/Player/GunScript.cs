using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    Vector3 prevposition;
    float timerforshooting;
    public GameObject[] Bullets;
    public Transform BulletSpawner;
    public WeaponParent WeaponScript;
    private bool Shooting;
    private bool Reloading;
    private float ReloadTimer;
    private int CurrentUpgrade;
    private SpriteRenderer GunSprite;
    private Transform GunTransform;
    [SerializeField] PlayerMechanics PlayerMechanicsScript;
    void Start()
    {
        Shooting = false;
        Reloading = false;
        prevposition = Input.mousePosition;

        Vector3 CenterPivot = Camera.main.WorldToScreenPoint(transform.position);

        float PrevAngle = Mathf.Atan2(CenterPivot.x - prevposition.x, prevposition.y - CenterPivot.y);
        transform.Rotate(new Vector3(0, 0, PrevAngle * Mathf.Rad2Deg));
        //find gun transform
        GunTransform = transform.GetChild(0);
        //find weaponscript
        WeaponScript = GunTransform.GetComponent<WeaponParent>();
        timerforshooting = 0;

        //find weapon sprite
        GunSprite = GunTransform.GetComponent<SpriteRenderer>();


        if (Globalvariables.CurrentLevel > 1) //if the level is not 1, the player might have altered their gun stats, so we override it
        {
            WeaponScript.SetDamageByNumber(Globalvariables.WeaponComponents.Damage);
            WeaponScript.SetFireRateByNumber(Globalvariables.WeaponComponents.FireRate);
            WeaponScript.SetReloadTimeByNumber(Globalvariables.WeaponComponents.ReloadTime);
            WeaponScript.SetMaxMagSize(Globalvariables.WeaponComponents.MagSize);
            WeaponScript.SetCurrentSize(Globalvariables.WeaponComponents.CurrSize);
            WeaponScript.SetSpread(Globalvariables.WeaponComponents.Spread);
            WeaponScript.SetMaxHeat(Globalvariables.WeaponComponents.HeatMax);
            WeaponScript.SetVelocity(Globalvariables.WeaponComponents.Velocity);
            CurrentUpgrade = Globalvariables.WeaponComponents.CurrentUpgrades;
        }
        else
        {
            ApplyBuffPreReq();
            CurrentUpgrade = WeaponScript.GetCurrentUpgrade();
            WeaponScript.SetPickedUp();
        }

        PlayerMechanicsScript = transform.parent.GetComponent<PlayerMechanics>();
        PlayerMechanicsScript.ShowAmmoLeft(WeaponScript.ReturnCurrentMag());
    }
    void Update()
    {
        timerforshooting -= Time.deltaTime;

        Vector3 currPosition = Input.mousePosition;
        //Follow mouse
        {
            Vector3 CenterPivot = Camera.main.WorldToScreenPoint(transform.position);

            float PrevAngle = Mathf.Atan2(CenterPivot.x - prevposition.x, prevposition.y - CenterPivot.y);


            float CurrAngle = Mathf.Atan2(CenterPivot.x - currPosition.x, currPosition.y - CenterPivot.y);

            prevposition = currPosition;

            float AngleDiff = CurrAngle - PrevAngle;

            transform.Rotate(new Vector3(0, 0, AngleDiff * Mathf.Rad2Deg));
        }        

        if (transform.childCount > 0) //if theres a weapon we can do all these
        {
            if (currPosition.x < Screen.width / 2)
            {
                GunSprite.flipY = true;
            }
            else
            {
                GunSprite.flipY = false;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (WeaponScript.ReturnCurrentMag() != WeaponScript.GetMaxMagSize() && WeaponScript.GetReloadTime() != -1) //if max mag no need to reload
                {
                    Shooting = false;
                    Reloading = true;
                    ReloadTimer = WeaponScript.GetReloadTime();
                }
            }
            else if (Input.GetMouseButtonUp(0)) //check if the mouse is being held down
            {
                Shooting = false;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Shooting = true;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                DropWeapon();
                return; //break the update as we dont cant do anything else below
            }

            WeaponScript.SetWeaponHeat(Time.deltaTime);
            GunSprite.material.SetColor("_SpriteColor", new Color(0, -WeaponScript.ReturnCurrentWeaponHeat(), -WeaponScript.ReturnCurrentWeaponHeat(), 0) + new Color(1, 1, 1, 1));


            if (Reloading)
            {
                GunTransform.Rotate(0, 0, 10);
                ReloadTimer = WeaponScript.DoReload(ReloadTimer, Time.deltaTime); //push the calculation to the gun for storing and dynamic reload
                if (ReloadTimer <= 0)
                {
                    GunTransform.localRotation = Quaternion.Euler(0, 0, 90);
                    PlayerMechanicsScript.ShowAmmoLeft(WeaponScript.ReturnCurrentMag());
                    Reloading = false; //completed reload
                }
            }
            else if (Shooting && timerforshooting <= 0 && WeaponScript.ReturnCurrentMag() > 0) //if the player is shooting, can shoot after fire rate cool down and has more than 1 bullet
            {
                timerforshooting = WeaponScript.Attack(BulletSpawner, Bullets[CurrentUpgrade], true); //shoot
                PlayerMechanicsScript.ShowAmmoLeft(WeaponScript.ReturnCurrentMag());
                if (WeaponScript.ReturnCurrentMag() <= 0) //if no more bullets, start the reload
                {
                    ReloadTimer = WeaponScript.GetReloadTime();
                    if (ReloadTimer == -1)
                    {
                        DestroyTempWeapon();
                        return;
                    }
                    Reloading = true;
                }
            }
        }
    }

    public void ChangeFireRate(float ByHowMuch)
    {
        WeaponScript.SetFireRate(ByHowMuch);
    }

    public void UpgradeWeapon(int ByHowMuch)
    {
        CurrentUpgrade += ByHowMuch; //increase the upgrade

        if (CurrentUpgrade > Bullets.Length - 1)
        {
            CurrentUpgrade -= ByHowMuch; //if too high then return it back down
        }
        else
        {
            WeaponScript.SetCurrentUpgrades(ByHowMuch);
            WeaponScript.SetVelocity(WeaponScript.ReturnVelocity() + (ByHowMuch * 5)); //set the new velocity
            WeaponScript.SetMaxMagSize((int)(WeaponScript.GetMaxMagSize() * 1.5f)); //increase the max mag
            WeaponScript.SetDamageByNumber((int)(WeaponScript.GetDamage() * 1.5f)); //damage
            WeaponScript.SetFireRateByNumber((WeaponScript.GetFireRate() * 0.8f)); //lower the firerate the faster the gun shoots
            WeaponScript.SetReloadTimeByNumber(WeaponScript.GetReloadTime() * 0.75f); //lower the reload time the faster
            WeaponScript.SetSpread(WeaponScript.GetSpread() * 0.8f); //lower the spread the better
            WeaponScript.SetMaxHeat(WeaponScript.ReturnMaxHeat() * 0.9f); //lower the heat the better
        }
    }

    public int ReturnCurrentUpgrade()
    {
        return CurrentUpgrade;
    }

    void AssignBuffs(string name, int amount)
    {
        if (name == "Increase Fire Rate")
        {
            float amounttoincreaseby = 1 - (amount * 0.1f);
            WeaponScript.SetFireRate(amounttoincreaseby);
            //Debug.Log("Increased Fire Rate");
        }
        else if (name == "Increase Damage")
        {
            float amounttoincreaseby = 1 + (amount * 0.5f);
            WeaponScript.SetDamage(amounttoincreaseby);
            //Debug.Log("Increased Damage");
        }
        else if (name == "Increase Reload Speed")
        {
            if (WeaponScript.GetReloadTime() == -1)
            {
                return;
            }
            float amounttoincreaseby = WeaponScript.GetReloadTime() * (1 - (amount * 0.05f));
            
            WeaponScript.SetReloadTimeByNumber(amounttoincreaseby);
            //Debug.Log("Amount to decrease by: " + amounttoincreaseby);
        }
    }

    public void AssignNewGun(GameObject NewGun, bool isanoverride) //is the gun an override? if not then replace the current gun
    {
        if (isanoverride)
        {
            //since its an override and there is another weapon that exists, we should:
            //then make the other gun inactive, this code assumes we only have 2 guns
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
        }
        else if (GunTransform != null)
        {
            if (ReloadTimer == -1)
            {
                NewGun.SetActive(true);
                //Debug.Log(GunTransform.name);
                Destroy(GunTransform.gameObject);
            }
            else
            {
                DropWeapon();
            }
        }
        //Debug.Log("CAME HERE TO ASSIGN GUN");
        //set new gun transform
        GunTransform = NewGun.transform;
        //set new parent
        GunTransform.SetParent(transform);
        //place this weapon on top
        GunTransform.SetAsFirstSibling();
        //get the new gun's weapon script
        WeaponScript = NewGun.GetComponent<WeaponParent>();
        if (!WeaponScript.GetPickedUp()) //if it has never been picked up, apply the player's ability
        {
            ApplyBuffPreReq();
            WeaponScript.SetPickedUp(); //tell it that we have picked it up already
        }
        CurrentUpgrade = WeaponScript.GetCurrentUpgrade(); //apply the weapon's current upgrade
        //set its position relative to parent
        GunTransform.localPosition = WeaponScript.GetLocalPosition();
        //reset its angle
        GunTransform.localRotation = Quaternion.Euler(0, 0, 90);
        //set new bullet spawner
        BulletSpawner = NewGun.transform.GetChild(0);
        //find weapon sprite
        GunSprite = NewGun.GetComponent<SpriteRenderer>();
        //set new bullets left
        PlayerMechanicsScript.ShowAmmoLeft(WeaponScript.ReturnCurrentMag());
        if (GunTransform.GetComponent<WeaponDropped>() != null) //if it exists
        {
            GunTransform.GetComponent<WeaponDropped>().enabled = false; //disable the script for when the weapon gets dropped
            GunTransform.GetComponent<CircleCollider2D>().enabled = false; //disable the collider that goes with the script
        }
    }

    void DropWeapon()
    {
        GunTransform.position += new Vector3(0, 0, 0.21f); //set the position on the Z axis
        GunTransform.GetComponent<WeaponDropped>().enabled = true; //enable the script for when the weapon gets dropped
        GunTransform.GetComponent<CircleCollider2D>().enabled = true; //enable the collider that goes with the script
        GunTransform.SetParent(null); //drop weapon
        PlayerMechanicsScript.ShowAmmoLeft(0); //remove ammo indicator
    }

    void DestroyTempWeapon()
    {
        AssignNewGun(transform.GetChild(transform.childCount - 1).gameObject, false);
    }

    void ApplyBuffPreReq()
    {
        for (int i = 0; i < PlayFabHandler.SkillList.Count; ++i)
        {
            //Debug.Log(PlayFabHandler.SkillList[i].Name + " has " + PlayFabHandler.SkillList[i].StackAmount);
            AssignBuffs(PlayFabHandler.SkillList[i].Name, PlayFabHandler.SkillList[i].StackAmount); //assign the correct buffs when the player starts the first level or picks up a new (gun)
        }
    }
}