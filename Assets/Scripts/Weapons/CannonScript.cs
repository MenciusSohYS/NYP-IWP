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
        ReloadTime = 3f;
        HeatMax = 1f;
        BulletVelocity = 65;
        PositionToParent = new Vector3(0, 1f, 0);
        Chambered = true;
        Piercing = 0;
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
        BulletShot.GetComponent<Bullet>().AssignPierce(1);
        BulletShot.transform.up += new Vector3(RandomX, RandomY, 0);

        int IsCrit = Random.Range(0, 101);

        if (IsCrit <= CritRate)
            BulletShot.GetComponent<Bullet>().SetCrit(true);

        Vector2 recoilForce = 20 * BulletVelocity * -BulletShot.transform.up;

        PlayerRB.AddForce(recoilForce);

        SetNewBulletsLeft(1); //set new bullets left (-1)
        if (CurrentHeat < HeatMax)
            CurrentHeat += 0.1f;

        //Debug.Log(FireRate);
        return FireRate;
    }
}