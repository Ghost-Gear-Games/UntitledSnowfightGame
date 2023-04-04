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
    public bool yellowed = false;
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
        if (yellowed)
        {
            StartCoroutine(DamageOverTime());
        }
        if(pathfinding.destination.x > this.transform.position.x)
        {
            GFX.flipX = false;
        }
        if(pathfinding.destination.x < this.transform.position.x)
        {
            GFX.flipX = true;
        }
        if (slushed)
        {
            this.gameObject.GetComponent<AIPath>().maxSpeed *= eAtkMgr.slushPuddle.slowdownFactor;
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
                    if (!eHPSystem.eCheckup())
                    {
                        Debug.Log("EMS enemy failed checkup");
                        for (int i = 0; i <= (drops / 5); i++) 
                        {
                            Debug.Log("EMS made coin");
                            Instantiate(lunchmoney, this.transform.position + new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.identity);
                        }
                        Destroy(this.gameObject);
                    }

                    break;
                case "YellowSnowball":
                    Debug.Log("EMS Confirmed it was a snowball");
                    eHPSystem.healthAmount -= slushed ? (eAtkMgr.yellowSnow.damage * ((float)eAtkMgr.slushPuddle.damageMultiplier)) : eAtkMgr.yellowSnow.damage;
                    yellowed = true;
                    if (!eHPSystem.eCheckup())
                    {
                        Debug.Log("EMS enemy failed checkup");
                        for (int i = 0; i <= (drops / 5); i++)
                        {
                            Debug.Log("EMS made coin");
                            Instantiate(lunchmoney, this.transform.position + new Vector3(UnityEngine.Random.Range(0.1f, 0.3f), UnityEngine.Random.Range(0.1f, 0.3f)), Quaternion.identity);
                        }
                        Destroy(this.gameObject);
                    }
                    break;
                case "Slush":
                    slushed = true;
                    break;
                }

        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "playerAttack" && collider.name == "Slush")
        {
            slushed = false;
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
        for(int i = 0; i <= eAtkMgr.yellowSnow.damageTimeAmount; i++)
        {
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
            yield return new WaitForSeconds(1f);
        }

        yellowed = false;
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
}
