using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 Movement;
    int speed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        speed = 12;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(Movement.x * speed, Movement.y * speed);
    }
}
