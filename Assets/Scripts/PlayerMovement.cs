using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 Movement;
    int speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 12;
    }

    // Update is called once per frame
    void Update()
    {
        Movement.x = (Input.GetAxisRaw("Horizontal"));
        Movement.y = Input.GetAxisRaw("Vertical");
        Vector2 movementDirection = Movement;
        movementDirection.Normalize();
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
    }
}
