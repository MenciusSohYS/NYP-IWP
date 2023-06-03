using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 Movement;
    int speed;
    Rigidbody2D rb;
    [SerializeField] float Rolling;
    [SerializeField] Vector2 MovementAxisForRolling;
    private bool rollingleft;
    // Start is called before the first frame update
    void Start()
    {
        CheckSpeed();
        Rolling = 0;
        rollingleft = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");


        if (Rolling < 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                float radians = (transform.GetChild(0).transform.eulerAngles.z + 90) * Mathf.Deg2Rad; //roll according to angle of shooting
                MovementAxisForRolling.x = Mathf.Cos(radians);
                MovementAxisForRolling.y = Mathf.Sin(radians);
                Rolling = 0.5f;

                if (transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>() != null)
                {
                    if (transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>())
                        rollingleft = true;
                    else
                        rollingleft = false;
                }
                else
                {
                    if (transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>())
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
        else
        {
            if (rollingleft)
                transform.Rotate(0, 0, 5);
            else
                transform.Rotate(0, 0, -5);

            Rolling -= Time.deltaTime;
            rb.velocity = new Vector2(MovementAxisForRolling.x * speed * 2, MovementAxisForRolling.y * speed * 2);
            if (Rolling <= 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

    }

    public float ReturnRollTimer()
    {
        return Rolling;
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
}
