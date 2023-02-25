using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public PlayerActions playerControls;
    public InputAction move;
    public InputAction fire;
    public Vector2 moveInput = Vector2.zero;

    private void Start()
    {

    }
    void Update()
    {
        //Input is collected and set to our variable every frame Input in this case is class

        /*

        */

        moveInput = move.ReadValue<Vector2>();

        /*
        Input.GetButton(string "button") is a method that returns a bool value
        this value is true if the button is currently bieng held down
        and this value is false if it isnt
        */
        baseAttackPressed = Input.GetButton("Fire3");
        //NOTE: will change "Fire3" to right click mouse button later

        animator.SetFloat("XInput", moveInput.x);
        animator.SetFloat("YInput", moveInput.y);
        animator.SetFloat("speed", moveInput.sqrMagnitude);
        animator.SetBool("attacking", baseAttackPressed);
        snowballDistance = new Vector3(0.1f + movementInput.x * 0.1f, 0.1f + movementInput.y * 0.1f, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + moveInput * speed * Time.fixedDeltaTime);

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
                Rigidbody2D snowballrb = baseSnowball.GetComponent<Rigidbody2D>();
                //place bullet in the direction the player is moving
                baseSnowball.transform.position = this.transform.position + snowballDistance;
                //grow 'bullet'
                //stop 'bullet' growth on player input or at certain size
                Vector3 scaleO = baseSnowball.transform.localScale;
                Vector3 maxSize = new Vector3(.5f, .5f, 0);
                while (baseAttackPressed && baseSnowball.transform.localScale.x < maxSize.x)
                {
                    baseSnowball.transform.localScale = Vector3.Lerp(scaleO, maxSize, rateOfGrowth);
                    baseSnowball.transform.position = this.transform.position + snowballDistance;
                    snowball.size = baseSnowball.transform.localScale;
                    snowball.damage = ((baseSnowball.transform.localScale.x * 10) + (snowball.upgradeCount * 5));
                    rateOfGrowth += 0.001f;
                }
                while(baseAttackPressed)
                {
                    baseSnowball.transform.position = this.transform.position + new Vector3(0, .5f);
                }
                    snowballrb.velocity += 2 * (moveInput);
                rateOfGrowth = 0;
                
                //start new attack holding animation
                //on player input release 'bullet' in direction player is moving

                //delete 'bullet' after a set time or/and on collision
                

                break;
            case "attack2":
                break;
            case "attack3":
                break;
        }
        yield break;
    }

    private void OnEnable()
    {
        move = playerControls.Gameplay.Move;
        fire = playerControls.Gameplay.Fire1;
        fire.Enable();
        fire.performed += Attack;
        move.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
    }
    private void Awake()
    {
        playerControls = new PlayerActions();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(startAttack("base"));
        }
        if (context.canceled)
        {
            StopCoroutine(startAttack("base"));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var temp = this.GetComponent<healthSystem>();
        if (collision.collider.tag == "Enemy")
        {
            switch (collision.collider.name)
            {
                case "EnemyNormal":
                    temp.Freeze(50);
                    break;
                case "EnemyFast":
                    temp.Freeze(25);
                    break;
                case "EnemySlow":
                    temp.Freeze(75);
                    break;

            }
        }
        if (temp.Checkup())
        {

        }
    }
}