using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public float speed = 5;
    public bool attacking;
    public Vector2 movementInput;
    public Vector3 snowballDistance;
    public Animator animator;
    public Rigidbody2D rb2D;
    public GameObject snowball;
    private void Start()
    {

    }
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
        attacking = Input.GetButtonDown("Fire3");

        animator.SetFloat("XInput", movementInput.x);
        animator.SetFloat("YInput", movementInput.y);
        animator.SetFloat("speed", movementInput.sqrMagnitude);
        animator.SetBool("attacking", attacking);
        if (attacking)
        {
            startAttack("base");
        }
        snowballDistance = new Vector3(movementInput.x, movementInput.y, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movementInput * speed * Time.fixedDeltaTime);
    }
    void startAttack(string attack)
    {

        switch (attack)
        {
            case "base":
                //BASE ATTACK TODO
                //start transition animation
                var baseSnowball = Instantiate(snowball, this.transform);
                //grow 'bullet'
                baseSnowball.transform.position = this.transform.position + snowballDistance;
                baseSnowball.transform.localScale += new Vector3 (Time.deltaTime, Time.deltaTime, 0);
                //stop 'bullet' growth on player input or at certain size
                //start new attack holding animation
                //on player input release 'bullet' in direction player is moving
                //delete 'bullet' after a set time or/and on collision
                break;
            case "attack2":
                break;
            case "attack3":
                break;
            case "attack4":
                break;
            case "attack5":
                break;
        }

    }
}