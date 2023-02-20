using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public float speed = 5;
    public bool baseAttackPressed;
    public Vector2 movementInput;
    public Vector3 snowballDistance;
    public Animator animator;
    public Rigidbody2D rb2D;
    public attackManager.snowball snowball;
    public attackManager.yellowSnowball yellowSnowball;
    public attackManager.slush slush;
    private void Start()
    {

    }
    void Update()
    {
        //Input is collected and set to our variable every frame Input in this case is class

        /*

        */

        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        /*
        Input.GetButton(string "button") is a method that returns a bool value
        this value is true if the button is currently bieng held down
        and this value is false if it isnt
        */
        baseAttackPressed = Input.GetButton("Fire3");
        //NOTE: will change "Fire3" to right click mouse button later

        animator.SetFloat("XInput", movementInput.x);
        animator.SetFloat("YInput", movementInput.y);
        animator.SetFloat("speed", movementInput.sqrMagnitude);
        animator.SetBool("attacking", baseAttackPressed);
        if (Input.GetButtonDown("Fire3"))
        {
            StartCoroutine(startAttack("base"));
        }
        if (Input.GetButtonUp("Fire3"))
        {
            StopCoroutine(startAttack("base"));
        }
        snowballDistance = new Vector3(0.1f + movementInput.x * 0.1f, 0.1f + movementInput.y * 0.1f, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movementInput * speed * Time.fixedDeltaTime);
    }
    IEnumerator startAttack(string attack)
    {

        switch (attack)
        {
            case "base":
                //BASE ATTACK OLD
                  float rateOfGrowth = 0f;
                //start 'basic attack charge' animation
                //start attackTimer
                //
                //create 'bullet'
                var baseSnowball = Instantiate(snowball.snowballPrefab, this.transform);
                //place bullet in the direction the player is moving
                baseSnowball.transform.position = this.transform.position + snowballDistance;
                //grow 'bullet'
                //stop 'bullet' growth on player input or at certain size
                Vector3 scaleO = baseSnowball.transform.localScale;
                Vector3 maxSize = new Vector3(.5f, .5f, 0);
                while (baseAttackPressed && baseSnowball.transform.localScale != maxSize)
                {
                    baseSnowball.transform.localScale = Vector3.Lerp(scaleO, maxSize, rateOfGrowth);
                    baseSnowball.transform.position = this.transform.position + snowballDistance;
                    snowball.size = baseSnowball.transform.localScale;
                    snowball.damage = baseSnowball.transform.localScale.x * 10;
                    rateOfGrowth += 0.005f;
                    yield return null;
                }
                rateOfGrowth = 0;
                baseSnowball.transform.position = this.transform.position + new Vector3(0,.5f);
                //start new attack holding animation
                //on player input release 'bullet' in direction player is moving
                Rigidbody2D snowballrb = baseSnowball.GetComponent<Rigidbody2D>();
                snowballrb.velocity += 2 * (movementInput + new Vector2(0.1f, 0.1f));
                //delete 'bullet' after a set time or/and on collision
                

                break;
            case "attack2":
                break;
            case "attack3":
                break;
        }
        yield break;
    }
}