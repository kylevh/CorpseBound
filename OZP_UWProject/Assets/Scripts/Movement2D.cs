using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;

    public float movementSpeed = 5f;
    Vector2 movement;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        DoTheMovementThing();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    void DoTheMovementThing() //does the movement thing
    {
        //Input
        if (Mathf.Abs(movement.y) >= .7f && Mathf.Abs(movement.x) >= .7f)
        {
            movement.x = Input.GetAxisRaw("Horizontal") * .7f;
            movement.y = Input.GetAxisRaw("Vertical") * .7f;
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Magnitude", movement.magnitude);
    }
}
