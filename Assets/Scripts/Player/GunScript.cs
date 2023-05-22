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
            for (int i = 0; i < PlayFabHandler.SkillList.Count; ++i)
            {
                Debug.Log(PlayFabHandler.SkillList[i].Name + " has " + PlayFabHandler.SkillList[i].StackAmount);
            }
            CurrentUpgrade = 0;
        }

        PlayerMechanicsScript = transform.parent.GetComponent<PlayerMechanics>();
        PlayerMechanicsScript.ShowAmmoLeft(WeaponScript.ReturnCurrentMag());
    }
    void Update()
    {
        timerforshooting -= Time.deltaTime;
        WeaponScript.SetWeaponHeat(Time.deltaTime);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, -WeaponScript.ReturnCurrentWeaponHeat(), -WeaponScript.ReturnCurrentWeaponHeat(), 0) + new Color(1, 1, 1, 1);


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


        if (Input.GetKeyDown(KeyCode.R))
        {
            Shooting = false;
            Reloading = true;
            ReloadTimer = WeaponScript.GetReloadTime();
        }
        else if (Input.GetMouseButtonUp(0)) //check if the mouse is being held down
        {
            Shooting = false;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Shooting = true;
        }


        if (Reloading)
        {
            transform.GetChild(0).Rotate(0, 0, 10);
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
            timerforshooting = WeaponScript.Attack(BulletSpawner, Bullets[CurrentUpgrade], true); //shoot
            PlayerMechanicsScript.ShowAmmoLeft(WeaponScript.ReturnCurrentMag());
            if (WeaponScript.ReturnCurrentMag() <= 0) //if no more bullets, start the reload
            {
                Reloading = true;
                ReloadTimer = WeaponScript.GetReloadTime();
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
}
