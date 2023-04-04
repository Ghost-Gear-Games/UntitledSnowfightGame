using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playerControl : MonoBehaviour,IHasCooldown
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

    public int snowballSpeed;

    public int lunchMoneyTotal = 0;

    public Animator animator;

    public Button yellowSnowButton;
    public Button slushButton;

    Vector3 lastPos;

    [Header("Cooldown Section")]
    [SerializeField] private cooldownSystem cooldownSystm;
    [SerializeField] private int id = 1;
    [SerializeField] private float cooldownDuration = 3;
    public int Id => id;
    public float CooldownDuration => cooldownDuration;

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
    public void UpgradeWeapon(string Weapon)
    {
        //PPS player playerControl SCript
        Debug.Log("PPS ran upgrade weapon");
        switch (Weapon)
        {
            case "base":
                Debug.Log("PPS it was a base upgrade");
                if(lunchMoneyTotal > snowball.upgradeCost)
                {
                    lunchMoneyTotal -= snowball.upgradeCost;
                    snowball.upgradeCount++;
                }
                break;
            case "yellow":
                Debug.Log("PPS it was a yellow upgrade");
                if (lunchMoneyTotal > yellowSnowball.upgradeCost)
                {
                    lunchMoneyTotal -= yellowSnowball.upgradeCost;
                    yellowSnowball.upgradeCount++;
                }
                break;
            case "slush":
                Debug.Log("PPS it was a slush upgrade");
                if (lunchMoneyTotal > slush.upgradeCost)
                {
                    lunchMoneyTotal -= slush.upgradeCost;
                    slush.upgradeCount++;
                }
                break;
        }
    }
    void Update()
    {
        lastPos = this.gameObject.transform.position;
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

        snowball.upgradeCost = 2 * ((int)Mathf.Pow(2, snowball.upgradeCount + 1));
        yellowSnowball.upgradeCost = 3 * ((int)Mathf.Pow(2, yellowSnowball.upgradeCount + 1)) + 2;
        slush.upgradeCost = 4 * ((int)Mathf.Pow(2, slush.upgradeCount + 1)) + 5;

    }
    //movement
    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + speed * Time.fixedDeltaTime * moveInput);
        if (yellowSnowball.upgradeCount >= 0)
        {
            yellowSnowButton.interactable = true;
        }
        if(slush.upgradeCount >= 0)
        {
            slushButton.interactable = true;
        }
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
        snowball.upgradeCount = 0;
        yellowSnowball.upgradeCount = -1;
        slush.upgradeCount = -1;
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
                    var desiredDir = this.gameObject.GetComponentInChildren<SpriteRenderer>().flipX? moveInput + new Vector2(UnityEngine.Random.Range(-0.2f, -0.1f), 0) : moveInput + new Vector2(UnityEngine.Random.Range(0.1f, 0.2f), 0);
                    var desiredVector = desiredDir.normalized * snowballSpeed;
                    var atkMgr = snowballProjectile.GetComponent<attackManager>();
                    atkMgr.snow.size = snowballProjectile.transform.localScale;
                    atkMgr.snow.damage = (snowballProjectile.transform.localScale.x * 2) + (atkMgr.snow.upgradeCount * 5);
                    snowballProjectile.GetComponent<Rigidbody2D>().velocity = desiredVector;
                    Debug.Log("PPS ran basic attack logic");
                    break;
                case weapon.YELLOW:
                    if (cooldownSystm.isOnCooldown(id)) 
                    { 
                        return;
                    }
                    var yellowSnowProjectile = Instantiate(yellowSnowball.yellowSnowballPrefab, this.transform);
                    yellowSnowProjectile.name = yellowSnowball.yellowSnowballPrefab.name;
                    yellowSnowProjectile.transform.position = this.transform.position + snowballDistance;
                    var desiredDirY = this.gameObject.GetComponentInChildren<SpriteRenderer>().flipX ? moveInput + new Vector2(UnityEngine.Random.Range(-0.2f, -0.1f), 0) : moveInput + new Vector2(UnityEngine.Random.Range(0.1f, 0.2f), 0);
                    var desiredVectorY = desiredDirY.normalized * snowballSpeed;
                    yellowSnowProjectile.GetComponent<Rigidbody2D>().velocity = desiredVectorY;
                    var yAtkMgr = yellowSnowProjectile.GetComponent<attackManager>();
                    yAtkMgr.yellowSnow.damage = 3 + (yAtkMgr.yellowSnow.upgradeCount * 2);
                    yAtkMgr.yellowSnow.damageOverTime = 1 + (Mathf.Clamp(yAtkMgr.yellowSnow.upgradeCount * 1, 0, Mathf.Infinity));
                    yAtkMgr.yellowSnow.damageTimeAmount = 12 - (Mathf.Clamp(yAtkMgr.yellowSnow.upgradeCount * 2, 0, 4));
                    Debug.Log("PPS ran yellow attack logic");
                    break;
                case weapon.SLUSH:
                    if (cooldownSystm.isOnCooldown(id)) 
                    {
                        return;
                    }
                    var slushPuddle = Instantiate(slush.slushPrefab, lastPos, Quaternion.identity);
                    var sAtkMgr = slushPuddle.GetComponent<attackManager>();
                    sAtkMgr.slushPuddle.size = new Vector3(1 * (sAtkMgr.slushPuddle.upgradeCount + 1), 1 * (sAtkMgr.slushPuddle.upgradeCount + 1), 0);
                    sAtkMgr.slushPuddle.slowdownFactor = 1;
                    sAtkMgr.slushPuddle.damageMultiplier = 1.2 + (sAtkMgr.slushPuddle.upgradeCount * 0.1);
                    Debug.Log("PPS ran slush attack logic");
                    break;
            }
        }
        if (context.canceled)
        {

        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("PPS entered Trigger collison");
        var temp = this.GetComponent<healthSystem>();
        if (collider.tag == "Enemy")
        {
            Debug.Log("PPS collider is an ememy");
            switch (collider.GetComponent<enemyManager>().eType)
            {
                case "Normal":
                        StartCoroutine(HurtAnim());
                        temp.Freeze(50);
                        Debug.Log("PPS collider is enemy normal");
                    break;
                case "Fast":
                    Debug.Log("PPS collider is enemy fast");
                    StartCoroutine(HurtAnim());
                    temp.Freeze(25);
                    break;
                case "Slow":
                    Debug.Log("PPS collider is enemy slow");
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
            Debug.Log("PPSgameOver");
            //gameOver
        }
    }
    
    IEnumerator HurtAnim()
    {

        var GFX = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        yield return new WaitForEndOfFrame();
        for(int i = 0; i < 3; i++)
        {
            Debug.Log("PPS ran HurtAnimloop" + i + "times");
            GFX.enabled = true;
            GFX.sprite = HurtSprite;
            yield return new WaitForSeconds(0.25f);
            GFX.enabled = false;
            yield return new WaitForSeconds(0.25f);
        }
        GFX.sprite = Regular;
        GFX.enabled = true;
        Debug.Log("PPS put regular sprite back");
        yield break;
    }
    

}