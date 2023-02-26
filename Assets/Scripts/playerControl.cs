using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerControl : MonoBehaviour
{
    public float speed = 5;
    public Vector3 snowballDistance;
    public Rigidbody2D rb2D;
    public Sprite HurtSprite;
    public Sprite Regular;

    public attackManager.snowball snowball;
    public attackManager.yellowSnowball yellowSnowball;
    public attackManager.slush slush;

    public PlayerActions playerControls;
    public InputAction move;
    public InputAction fire;
    public Vector2 moveInput = Vector2.zero;

    public enum weapon { BASIC, YELLOW, SLUSH };
    public weapon currentWeapon;

    public clockTimer.timer snowballChargetimer = new clockTimer.timer();
    public clockTimer.timer yellowSnowCooldown = new clockTimer.timer();
    public clockTimer.timer slushLiftime = new clockTimer.timer();
    public clockTimer.timer slushCooldown = new clockTimer.timer();

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
        //NOTE: will change "Fire3" to right click mouse button later

        snowballDistance = new Vector3(0.1f + moveInput.x * 0.1f, 0.1f + moveInput.y * 0.1f, 0);
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
            case "basic":
                var visualSnowball = Instantiate(snowball.snowballPrefab, this.transform);
                visualSnowball.transform.position = this.transform.position + snowballDistance;
                snowballChargetimer.timerDuration = 5;
                snowballChargetimer.timerOn = true;
                snowballChargetimer.StartTimerCountUp();
                if (!snowballChargetimer.timerOn)
                {
                    Destroy(visualSnowball);
                    var snowballProjectile = Instantiate(snowball.snowballPrefab, this.transform);
                    snowballProjectile.transform.localScale *= snowballChargetimer.timerTime;
                    var atkMgr = snowballProjectile.GetComponent<attackManager>();
                    atkMgr.snow.size = snowballProjectile.transform.localScale;
                    atkMgr.snow.damage = snowballProjectile.transform.localScale.x + (atkMgr.snow.upgradeCount * 5);
                    snowballProjectile.GetComponent<Rigidbody2D>().velocity += 2 * moveInput;
                }
                yield return new WaitForSeconds(0.05f);
                
                break;
            case "yellow":
                if (yellowSnowball.upgradeCount >= 0) {
                    var yellowSnowProjectile = Instantiate(yellowSnowball.yellowSnowballPrefab, this.transform);
                    yellowSnowProjectile.transform.position = this.transform.position + snowballDistance;
                    yellowSnowProjectile.GetComponent<Rigidbody2D>().velocity = 2 * moveInput;
                    var yAtkMgr = yellowSnowProjectile.GetComponent<attackManager>();
                    yAtkMgr.yellowSnow.damage = 3 + (yAtkMgr.yellowSnow.upgradeCount * 2);
                    yAtkMgr.yellowSnow.damageOverTime = 1 + (Mathf.Clamp(yAtkMgr.yellowSnow.upgradeCount * 1, 0, 3));
                    yAtkMgr.yellowSnow.damageTimeAmount = 12 - (Mathf.Clamp(yAtkMgr.yellowSnow.upgradeCount * 2, 0, 4));
                }
                break;
            case "slush":
                if(slush.upgradeCount >= 0)
                {
                    var slushPuddle = Instantiate(slush.slushPrefab, this.transform);
                    slushPuddle.transform.position = this.transform.position;
                    var sAtkMgr = slushPuddle.GetComponent<attackManager>();
                    sAtkMgr.slushPuddle.size = new Vector3(1 * (sAtkMgr.slushPuddle.upgradeCount), 1 * (sAtkMgr.slushPuddle.upgradeCount), 0);
                    sAtkMgr.slushPuddle.slowdownFactor = 1;
                    sAtkMgr.slushPuddle.damageMultiplier = 1.2 + (sAtkMgr.slushPuddle.upgradeCount * 0.1);
                }
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
            switch (currentWeapon)
            {
                case weapon.BASIC:
                    StartCoroutine(startAttack("basic"));
                    break;
                case weapon.YELLOW:
                    StartCoroutine(startAttack("yellow"));
                    break;
                case weapon.SLUSH:
                    StartCoroutine(startAttack("slush"));
                    break;
            }
        }
        if (context.canceled)
        {
            StopCoroutine(startAttack("basic"));
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var temp = this.GetComponent<healthSystem>();
        if (collider.tag == "Enemy")
        {

            switch (collider.name)
            {
                case "EnemyNormal":
                    StartCoroutine(HurtAnim());
                    temp.Freeze(50);
                    break;
                case "EnemyFast":
                    StartCoroutine(HurtAnim());
                    temp.Freeze(25);
                    break;
                case "EnemySlow":
                    StartCoroutine(HurtAnim());
                    temp.Freeze(75);
                    break;

            }
        }
        if (!temp.Checkup())
        {
            //gameOver
        }
    }
    
    IEnumerator HurtAnim()
    {
        var GFX = this.gameObject.GetComponentInChildren<SpriteRenderer>();

        for(int i = 0; i < 3; i++)
        {
            GFX.enabled = true;
            GFX.sprite = HurtSprite;
            yield return new WaitForSeconds(0.25f);
            GFX.enabled = false;
            yield return new WaitForSeconds(0.25f);
        }
        GFX.sprite = Regular;
        yield break;
    }
}