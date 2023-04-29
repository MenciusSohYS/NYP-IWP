using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    protected int Damage = 5;
    protected float FireRate = 0.5f;

    public float Attack(Transform fromhere, GameObject Projectile, bool ShotByPlayer)
    {
        GameObject BulletShot = Instantiate(Projectile, fromhere.position, fromhere.rotation);
        BulletShot.GetComponent<Bullet>().ShotBy(ShotByPlayer); //shot by enemy
        BulletShot.GetComponent<Bullet>().AssignDamage(Damage);

        return FireRate;
    }
}
