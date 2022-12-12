using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDown2DMovement : MonoBehaviour
{

    public Vector2 movementInput;
    public float speed = 5;
    public Rigidbody2D rb2D;
    public bool attacking;
    public Animator animator;

    void Update()
    {
        //Input is collected and set to our variable every frame Input in this case is also a class just like this scripts 'TopDown2DMovement' script

        /*
        Input.GetAxisRaw(string "axis") is a method 
        that gives us a value = to the inputed axis
        basically if the player presses the right arrow 
        or pushes a joystick right, this returns the value 1
        if instead it was left, then it would return -1
        if its inbetween(when your using a joystick this can happen)
        it returns a value inbetween -1 and 1 depending on its direction
        */
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        /*
        Input.GetButtonDown(string "button") is a method that returns a bool
        true if the button is currently bieng held down on this frame
        false if it isnt
        */
        attacking = Input.GetButtonDown("Fire1");

        animator.SetFloat("XInput", movementInput.x);
        animator.SetFloat("YInput", movementInput.y);
        animator.SetFloat("speed", movementInput.sqrMagnitude);
        animator.SetBool("attacking", attacking);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movementInput * speed * Time.fixedDeltaTime);
    }
}
