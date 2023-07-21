using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 Movement;
    int speed;
    Rigidbody2D rb;
    [SerializeField] float Rolling;
    [SerializeField] float RollingCooldown;
    [SerializeField] float FlyingTimer;
    [SerializeField] Vector2 MovementAxisForRolling;
    private bool rollingleft;
    public GameObject Fire;
    private Vector3 StartPositionOfFlight;
    Quaternion currentRotation;
    GunScript PlayerGS;
    bool touchinghalfcover;
    // Start is called before the first frame update
    void Start()
    {
        touchinghalfcover = false;
        CheckSpeed();
        Rolling = 0;
        RollingCooldown = 0;
        FlyingTimer = 0;
        rollingleft = true;
        rb = GetComponent<Rigidbody2D>();

        if (Globalvariables.CurrentLevel > 1)
        {
            speed = Globalvariables.Speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");

        if (Rolling < 1 && RollingCooldown > 0)
        {
            RollingCooldown -= Time.deltaTime;
        }

        if (Rolling < 0 && FlyingTimer <= 0)
        {
            if (Input.GetKey(KeyCode.Space) && RollingCooldown <= 0)
            {
                float radians = (transform.GetChild(0).transform.eulerAngles.z + 90) * Mathf.Deg2Rad; //roll according to angle of shooting
                MovementAxisForRolling.x = Mathf.Cos(radians);
                MovementAxisForRolling.y = Mathf.Sin(radians);
                Rolling = 0.5f;

                if (transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>() != null)
                {
                    if (transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().flipY)
                        rollingleft = true;
                    else
                        rollingleft = false;
                }
                else
                {
                    if (transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().flipY)
                        rollingleft = true;
                    else
                        rollingleft = false;
                }
            }
            else if (Input.GetKey(KeyCode.LeftControl))
                rb.velocity = new Vector2(Movement.x * speed * 0.5f, Movement.y * speed * 0.5f);
            else
                rb.velocity = new Vector2(Movement.x * speed, Movement.y * speed);
        }
        else if (FlyingTimer <= 0)
        {
            if (rollingleft)
                transform.Rotate(0, 0, 1000 * Time.deltaTime);
            else
                transform.Rotate(0, 0, -1000 * Time.deltaTime);

            Rolling -= Time.deltaTime;
            rb.velocity = new Vector2(MovementAxisForRolling.x * (speed + 2), MovementAxisForRolling.y * (speed + 2));
            if (Rolling <= 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                RollingCooldown = 0.5f;
            }
        }
        else
        {
            rb.velocity = new Vector2(MovementAxisForRolling.x * speed * 2, MovementAxisForRolling.y * speed * 2);
            FlyingTimer -= Time.deltaTime; //if i am flying, minus the time
            if (FlyingTimer <= 0) //if time runs out for flying, do this
            {
                float Distance = Vector3.Distance(transform.position, StartPositionOfFlight);

                // Calculate the backtracking direction based on the current rotation
                float angle = currentRotation.eulerAngles.z;
                Vector2 backtrackDirection = Quaternion.Euler(0f, 0f, angle + 180f) * Vector2.up;

                // Calculate the initial position based on the current position and backtrack direction
                Vector2 initialPosition = new Vector2(transform.position.x, transform.position.y) + 0.5f * Distance * backtrackDirection;

                GameObject LineOfFire = Instantiate(Fire, initialPosition, currentRotation);

                Vector2 TempVector2 = new(Distance, 1);

                LineOfFire.GetComponent<SpriteRenderer>().size = TempVector2;
                LineOfFire.GetComponent<BoxCollider2D>().size = TempVector2;

                LineOfFire.transform.rotation = LineOfFire.transform.rotation * Quaternion.Euler(0, 0, 90);

                if ((LineOfFire.transform.rotation.eulerAngles.z > 90 || LineOfFire.transform.rotation.eulerAngles.z < -90) && LineOfFire.transform.rotation.eulerAngles.z < 270)
                {
                    //Debug.Log(LineOfFire.transform.rotation.eulerAngles.z);
                    LineOfFire.GetComponent<SpriteRenderer>().flipY = true;
                }
            }
        }

    }

    public bool IsRolling()
    {
        return (Rolling > 0);
    }

    public void FlyForward()
    {
        StartPositionOfFlight = transform.position;
        //Debug.Log(StartPositionOfFlight);
        float radians = (transform.GetChild(0).transform.eulerAngles.z + 90) * Mathf.Deg2Rad; //roll according to angle of shooting
        MovementAxisForRolling.x = Mathf.Cos(radians);
        MovementAxisForRolling.y = Mathf.Sin(radians);
        FlyingTimer = 0.5f;
        currentRotation = transform.GetChild(0).transform.rotation;
    }

    public float ReturnRollTimer()
    {
        return Rolling;
    }

    public int ReturnSpeed()
    {
        return speed;
    }

    public void DecreaseSpeed(int DecreaseBy)
    {
        speed -= DecreaseBy;
    }

    public void IncreaseSpeed(int IncreaseBy)
    {
        CheckSpeed();
        speed += (IncreaseBy * 2);
        //Debug.Log("Speed: " + speed);
    }

    void CheckSpeed()
    {
        if (speed < 1)
        {
            speed = 12;
            if (gameObject.name.Contains("Mercenary"))
            {
                float temp = speed * 1.2f;
                speed = (int)temp;
                //Debug.Log("Merc");
            }
        }
        else
        {
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Contains("HalfCover"))
        {
            if (PlayerGS == null)
            {
                PlayerGS = transform.GetChild(0).GetComponent<GunScript>();
            }

            if (PlayerGS) //if dont have means its a melee and melee isnt affected by this
            {
                PlayerGS.IncreaseAccuracy();
            }
            touchinghalfcover = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag.Contains("HalfCover"))
        {
            if (PlayerGS == null)
            {
                PlayerGS = transform.GetChild(0).GetComponent<GunScript>();
            }

            if (PlayerGS) //if dont have means its a melee and melee isnt affected by this
            {
                PlayerGS.DecreaseAccuracy();
            }
            touchinghalfcover = false;
        }
    }

    public bool IsTouchingHalfCover()
    {
        return touchinghalfcover;
    }
}
