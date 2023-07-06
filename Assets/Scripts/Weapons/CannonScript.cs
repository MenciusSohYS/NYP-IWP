using UnityEngine;

public class CannonScript : WeaponParent
{

    public CannonScript()
    {
        Damage = 50;
        FireRate = 0.75f;
        Spread = 0.1f;
        MaxMagSize = 1;
        BulletsRemaining = 1;
        ReloadTime = 4f;
        HeatMax = 1f;
        PositionToParent = new Vector3(0, 1f, 0);
        BulletVelocity = 70;
        Chambered = true;
        Piercing = 2;
    }
    public GameObject CannonBall;
    private Rigidbody2D PlayerRB;

    protected override void AssignAtStart()
    {
        base.AssignAtStart();
        PlayerRB = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Rigidbody2D>();
    }

    public override float Attack(Transform fromhere, GameObject Projectile, bool ShotByPlayer)
    {
        float RandomX = Random.Range(-Spread, Spread) * CurrentHeat;
        float RandomY = Random.Range(-Spread, Spread) * CurrentHeat;

        PlayShootingSound();


        GameObject BulletShot = Instantiate(CannonBall, fromhere.position, fromhere.rotation);
        BulletShot.GetComponent<Bullet>().ShotBy(ShotByPlayer); //shot by enemy or player
        BulletShot.GetComponent<Bullet>().AssignDamage(Damage);
        BulletShot.GetComponent<Bullet>().AssignVelocity(BulletVelocity);
        BulletShot.GetComponent<Bullet>().AssignPierce(Piercing);
        BulletShot.transform.up += new Vector3(RandomX, RandomY, 0);

        Vector2 recoilForce = -BulletShot.transform.up * BulletVelocity * 20;

        PlayerRB.AddForce(recoilForce);

        SetNewBulletsLeft(1); //set new bullets left (-1)
        if (CurrentHeat < HeatMax)
            CurrentHeat += 0.1f;

        //Debug.Log(FireRate);
        return FireRate;
    }
}