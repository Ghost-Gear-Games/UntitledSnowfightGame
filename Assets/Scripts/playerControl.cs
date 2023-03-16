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

    /*public clockTimer.timer yellowSnowCooldown;
    public clockTimer.timer slushLiftime;
    public clockTimer.timer slushCooldown;*/

    public int snowballSpeed;

    public int lunchMoneyTotal = 0;

    public Animator animator;

    public void ChangeWeapon(int Weapon)
    {
        switch (Weapon)
        {
            case 1:
                currentWeapon = weapon.BASIC;
                break;
            case 2:
                currentWeapon = weapon.YELLOW;
                break;
            case 3:
                currentWeapon = weapon.SLUSH;
                break;
        }

    }
    void Update()
    {
        //updating movement input
        moveInput = move.ReadValue<Vector2>();

        if(moveInput.x < 0)
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        if (moveInput.x > 0)
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        if(moveInput != new Vector2(0,0))
        {
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
        snowballDistance = new Vector3(0.1f + moveInput.x * 0.1f, 0.1f + moveInput.y * 0.1f, 0);
       /* yellowSnowCooldown.CountTimer(false);
        slushLiftime.CountTimer(true);
        slushCooldown.CountTimer(false);*/
    }
    //movement
    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + speed * Time.fixedDeltaTime * moveInput);

    }
    //input Control
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
    //attack logic
    private void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (currentWeapon)
            {
                case weapon.BASIC:
                    var snowballProjectile = Instantiate(snowball.snowballPrefab, this.transform);
                    snowballProjectile.name = snowball.snowballPrefab.name;
                    snowballProjectile.transform.localScale = 0.5f * new Vector3(((float)context.duration), ((float)context.duration));
                    var desiredDir = this.gameObject.GetComponentInChildren<SpriteRenderer>().flipX? moveInput + new Vector2(Random.Range(-0.2f, -0.1f), 0) : moveInput + new Vector2(Random.Range(0.1f, 0.2f), 0);
                    var desiredVector = desiredDir.normalized * snowballSpeed;
                    var atkMgr = snowballProjectile.GetComponent<attackManager>();
                    atkMgr.snow.size = snowballProjectile.transform.localScale;
                    atkMgr.snow.damage = (snowballProjectile.transform.localScale.x * 2) + (atkMgr.snow.upgradeCount * 5);
                    snowballProjectile.GetComponent<Rigidbody2D>().velocity = desiredVector;
                    Debug.Log("ran basic attack logic");
                    break;
                case weapon.YELLOW:
                    if (yellowSnowball.upgradeCount >= 0)
                    {
                        var yellowSnowProjectile = Instantiate(yellowSnowball.yellowSnowballPrefab, this.transform);
                        yellowSnowProjectile.transform.position = this.transform.position + snowballDistance;
                        var desiredDirY = this.gameObject.GetComponentInChildren<SpriteRenderer>().flipX ? moveInput + new Vector2(Random.Range(-0.2f, -0.1f), 0) : moveInput + new Vector2(Random.Range(0.1f, 0.2f), 0);
                        var desiredVectorY = desiredDirY.normalized * snowballSpeed;
                        yellowSnowProjectile.GetComponent<Rigidbody2D>().velocity = desiredVectorY;
                        var yAtkMgr = yellowSnowProjectile.GetComponent<attackManager>();
                        yAtkMgr.yellowSnow.damage = 3 + (yAtkMgr.yellowSnow.upgradeCount * 2);
                        yAtkMgr.yellowSnow.damageOverTime = 1 + (Mathf.Clamp(yAtkMgr.yellowSnow.upgradeCount * 1, 0, Mathf.Infinity));
                        yAtkMgr.yellowSnow.damageTimeAmount = 12 - (Mathf.Clamp(yAtkMgr.yellowSnow.upgradeCount * 2, 0, 4));
                    }
                    Debug.Log("ran yellow attack logic");
                    break;
                case weapon.SLUSH:
                    if (slush.upgradeCount >= 0)
                    {
                        var slushPuddle = Instantiate(slush.slushPrefab, this.transform);
                        slushPuddle.transform.position = this.transform.position;
                        var sAtkMgr = slushPuddle.GetComponent<attackManager>();
                        sAtkMgr.slushPuddle.size = new Vector3(1 * (sAtkMgr.slushPuddle.upgradeCount + 1), 1 * (sAtkMgr.slushPuddle.upgradeCount + 1), 0);
                        sAtkMgr.slushPuddle.slowdownFactor = 1;
                        sAtkMgr.slushPuddle.damageMultiplier = 1.2 + (sAtkMgr.slushPuddle.upgradeCount * 0.1);
                    }
                    Debug.Log("ran slush attack logic");
                    break;
            }
        }
        if (context.canceled)
        {

        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("entered Trigger collison");
        var temp = this.GetComponent<healthSystem>();
        if (collider.tag == "Enemy")
        {
            Debug.Log("collider is an ememy");
            switch (collider.GetComponent<enemyManager>().eType)
            {
                case "Normal":
                        StartCoroutine(HurtAnim());
                        temp.Freeze(50);
                        Debug.Log("collider is enemy normal");
                    break;
                case "Fast":
                    Debug.Log("collider is enemy fast");
                    StartCoroutine(HurtAnim());
                    temp.Freeze(25);
                    break;
                case "Slow":
                    Debug.Log("collider is enemy slow");
                    StartCoroutine(HurtAnim());
                    temp.Freeze(75);
                    break;

            }
        }
        if(collider.tag == "Collectable")
        {
            lunchMoneyTotal += 1;
            Destroy(collider.gameObject);
        }
        if (!temp.Checkup())
        {
            Debug.Log("gameOver");
            //gameOver
        }
    }
    
    IEnumerator HurtAnim()
    {

        var GFX = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        yield return new WaitForEndOfFrame();
        for(int i = 0; i < 3; i++)
        {
            Debug.Log("ran HurtAnimloop" + i + "times");
            GFX.enabled = true;
            GFX.sprite = HurtSprite;
            yield return new WaitForSeconds(0.25f);
            GFX.enabled = false;
            yield return new WaitForSeconds(0.25f);
        }
        GFX.sprite = Regular;
        GFX.enabled = true;
        Debug.Log("put regular sprite back");
        yield break;
    }
    

}