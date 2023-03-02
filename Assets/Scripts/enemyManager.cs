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
    // Start is called before the first frame update
    void Start()
    {
        if(eType == "EnemyFast")
        {
            eHPSystem.healthAmount = 5;
            eDamage = 12.5f;
            drops = 5;
        }
        if (eType == "EnemyNormal")
        {
            eHPSystem.healthAmount = 8f;
            eDamage = 25;
            drops = 10;
        }
        if (eType == "EnemySlow")
        {
            eHPSystem.healthAmount = 12f;
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
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("trigger collision enter");
        if (collider.tag == "playerAttack")
        {
            Debug.Log("confirmed it was an attack from the player");
            eAtkMgr = collider.GetComponent<attackManager>();
            switch (collider.name) 
                {
                case "Snowball":
                    Debug.Log("Confirmed it was a snowball");
                    this.gameObject.GetComponent<healthSystem>().healthAmount -= slushed ? (eAtkMgr.snow.damage *  ((float)eAtkMgr.slushPuddle.damageMultiplier)) : eAtkMgr.snow.damage;
                    Destroy(collider.gameObject);
                    if (!this.gameObject.GetComponent<healthSystem>().eCheckup())
                    {
                        Debug.Log("enemy failed checkup");
                        for (int i = 0; i <= (drops / 5); i++) 
                        {
                            Debug.Log("made coin");
                            Instantiate(lunchmoney, this.transform.position + new Vector3(Random.Range(0.1f,0.3f), Random.Range(0.1f, 0.3f)), Quaternion.identity);
                        }
                        Destroy(this.gameObject);
                    }
                    break;
                case "YellowSnowball":
                    this.gameObject.GetComponent<healthSystem>().healthAmount -= slushed ? (eAtkMgr.yellowSnow.damage * ((float)eAtkMgr.slushPuddle.damageMultiplier)) : eAtkMgr.yellowSnow.damage;
                    yellowed = true;
                    if (!this.gameObject.GetComponent<healthSystem>().eCheckup())
                    {
                        Debug.Log("enemy failed checkup");
                        for (int i = 0; i <= (drops / 5); i++)
                        {
                            Debug.Log("made coin");
                            Instantiate(lunchmoney, this.transform.position + new Vector3(Random.Range(0.1f, 0.3f), Random.Range(0.1f, 0.3f)), Quaternion.identity);
                        }
                        Destroy(this.gameObject);
                    }
                    break;
                case "Slush":
                    this.gameObject.GetComponent<AIPath>().maxSpeed *= eAtkMgr.slushPuddle.slowdownFactor;
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
        }
    }
    IEnumerator DamageOverTime()
    {
        for(int i = 0; i <= eAtkMgr.yellowSnow.damageTimeAmount; i++)
        {
            eHPSystem.healthAmount -= eAtkMgr.yellowSnow.damageOverTime;
            yield return new WaitForSeconds(1f);
        }
        yellowed = false;
        yield break;
    }

}
