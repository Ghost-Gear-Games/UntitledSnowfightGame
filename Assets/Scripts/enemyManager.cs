using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    public string eType;
    public float eDamage;
    public int drops;
    public healthSystem eHPSystem;
    public bool slushed = false;
    public GameObject lunchmoney;
    public attackManager eAtkMgr;
   
    public Sprite Regular;
    public Sprite HurtSprite;

    public SpriteRenderer GFX;

    public AIPath pathfinding;
    // Start is called before the first frame update
    void Start()
    {
        pathfinding = this.gameObject.GetComponent<AIPath>();
        GFX = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        if (eType == "Fast")
        {
            eHPSystem.healthAmount = 4f;
            eDamage = 12.5f;
            drops = 5;
        }
        if (eType == "Normal")
        {
            eHPSystem.healthAmount = 8f;
            eDamage = 25;
            drops = 10;
        }
        if (eType == "Slow")
        {
            eHPSystem.healthAmount = 16f;
            eDamage = 50;
            drops = 20;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pathfinding.destination.x > this.transform.position.x)
        {
            GFX.flipX = false;
        }
        if(pathfinding.destination.x < this.transform.position.x)
        {
            GFX.flipX = true;
        }

    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        //EMS mean enemy manager script makes the debuglogs easier to find
        Debug.Log("EMS trigger collision enter");
        if (collider.tag == "playerAttack")
        {
            Debug.Log("EMS confirmed it was an attack from the player");
            eAtkMgr = collider.GetComponent<attackManager>();
            switch (collider.gameObject.name) 
                {
                case "Snowball":
                    Debug.Log("EMS Confirmed it was a snowball");

                    StartCoroutine(HurtAnim());
                    eHPSystem.healthAmount -= slushed ? (eAtkMgr.snow.damage * ((float)eAtkMgr.slushPuddle.damageMultiplier)) : eAtkMgr.snow.damage;
                    Destroy(collider.gameObject);
                    enemyDeath();
                    break;
                case "YellowSnowball":
                    Debug.Log("EMS Confirmed it was a yellow snowball");
                    eHPSystem.healthAmount -= slushed ? (eAtkMgr.yellowSnow.damage * ((float)eAtkMgr.slushPuddle.damageMultiplier)) : eAtkMgr.yellowSnow.damage;
                    StartCoroutine(DamageOverTime());
                    Destroy(collider.gameObject);
                    enemyDeath();
                    break;
                case "Slush":
                    Debug.Log("EMS slushed Enemy");
                    slushed = true;
                    this.gameObject.GetComponent<AIPath>().maxSpeed *= eAtkMgr.slushPuddle.slowdownFactor;
                    break;
                }

        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        eAtkMgr = collision.GetComponent<attackManager>();
        if (slushed)
        {
            Debug.Log("EMS slowed enemy");
            GFX.color += Color.blue;
        }
        if(collision.name == "Slush")
        {
            slushed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "playerAttack" && collider.name == "Slush")
        {
            slushed = false;
            GFX.color -= Color.blue * 2;
            switch (eType)
            {
                case "Normal":
                    this.gameObject.GetComponent<AIPath>().maxSpeed = 2;
                    break;
                case "Fast":
                    this.gameObject.GetComponent<AIPath>().maxSpeed = 4;
                    break;
                case "Slow":
                    this.gameObject.GetComponent<AIPath>().maxSpeed = 1;
                    break;
            }
        }
    }
    IEnumerator DamageOverTime()
    {
        Debug.Log("EMS ran Dmg/Time Coroutine");
        GFX.color = new Color(255,255,100);
        for(int i = 0; i <= eAtkMgr.yellowSnow.damageTimeAmount; i++)
        {
            Debug.Log("EMS ran Dmg/Time hurt loop" + i.ToString() + "times");
            eHPSystem.healthAmount -= eAtkMgr.yellowSnow.damageOverTime;

            if (!eHPSystem.eCheckup())
            {
                Debug.Log("EMS enemy failed checkup");
                for (int x = 0; x <= (drops / 5); x++)
                {
                    Debug.Log("EMS made coin");
                    Instantiate(lunchmoney, this.transform.position + new Vector3(UnityEngine.Random.Range(0.1f, 0.3f), UnityEngine.Random.Range(0.1f, 0.3f)), Quaternion.identity);
                }
                Destroy(this.gameObject);
            }
            yield return new WaitForSeconds(0.5f);
        }
        GFX.color = Color.white;
        yield break;
    }
    IEnumerator HurtAnim()
    {
        if(eType != "Slow")
        {
            this.gameObject.GetComponent<AIPath>().maxSpeed = 0;
        }
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("EMS ran HurtAnimloop" + i + "times");
            GFX.enabled = true;
            GFX.sprite = HurtSprite;
            yield return new WaitForSeconds(0.25f);
            GFX.enabled = false;
            yield return new WaitForSeconds(0.25f);
        }
        GFX.sprite = Regular;
        GFX.enabled = true;
        switch (eType)
        {
            case "Normal":
                this.gameObject.GetComponent<AIPath>().maxSpeed = 2;
                break;
            case "Fast":
                this.gameObject.GetComponent<AIPath>().maxSpeed = 4;
                break;
        }
        Debug.Log("EMS put regular sprite back");
        yield break;
    }
    void enemyDeath()
    {
        if (!eHPSystem.eCheckup())
        {
            Debug.Log("EMS enemy failed checkup");
            for (int i = 0; i <= (drops / 5); i++)
            {
                Debug.Log("EMS made coin");
                Instantiate(lunchmoney, this.transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}
