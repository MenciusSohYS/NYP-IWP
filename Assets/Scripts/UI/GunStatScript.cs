using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunStatScript : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public GameObject PlayerGun;
    public WeaponParent PlayerWeaponScript;
    // Update is called once per frame
    void Update()
    {
        if (!isActiveAndEnabled)
            return;


        Text.text = "";
        Text.text += "Damage: " + PlayerWeaponScript.GetDamage() + '\n';
        Text.text += "Fire Rate: " + PlayerWeaponScript.GetFireRate().ToString("F2") + '\n';
        Text.text += "Reload: " + PlayerWeaponScript.ReturnFullReload().ToString("F2") + '\n';
        Text.text += "Mag Cap: " + PlayerWeaponScript.GetMaxMagSize() + '\n';
        Text.text += "Bullet Speed: " + PlayerWeaponScript.ReturnVelocity() + '\n';
        Text.text += "Bullet Pierce: " + PlayerWeaponScript.ReturnPiercing() + '\n';
        Text.text += "Max Heat: " + PlayerWeaponScript.ReturnMaxHeat().ToString("F2") + '\n';
        Text.text += "Max Spread: " + PlayerWeaponScript.GetSpread().ToString("F2") + '\n';
        Text.text += "Crit Rate: " + PlayerWeaponScript.ReturnCrit() + '\n';
    }

    public void AssignPlayerGun(GameObject PlayerGunGO)
    {
        PlayerGun = PlayerGunGO;

        PlayerWeaponScript = PlayerGun.GetComponent<WeaponParent>();
        if (PlayerWeaponScript == null)
        {
            PlayerWeaponScript = PlayerGun.transform.GetChild(0).GetComponent<WeaponParent>();
        }
    }
}
