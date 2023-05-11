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
    // Start is called before the first frame update
    void Start()
    {
        speed = 12;
        Rolling = 0;

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
            }
            else if (Input.GetKey(KeyCode.LeftControl))
                rb.velocity = new Vector2(Movement.x * speed * 0.5f, Movement.y * speed * 0.5f);
            else
                rb.velocity = new Vector2(Movement.x * speed, Movement.y * speed);
        }
        else
        {
            Rolling -= Time.deltaTime;
            rb.velocity = new Vector2(MovementAxisForRolling.x * speed * 2, MovementAxisForRolling.y * speed * 2);
        }

    }

    public float ReturnRollTimer()
    {
        return Rolling;
    }
}
