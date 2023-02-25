using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    public float eHP;
    public float eDamage;
    public int drops;
    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.name == "EnemyFast")
        {
            eHP = 5;
            eDamage = 25;
            drops = 5;
        }
        if (this.gameObject.name == "EnemyNormal")
        {
            eHP = 15;
            eDamage = 50;
            drops = 10;
        }
        if (this.gameObject.name == "EnemySlow")
        {
            eHP = 45;
            eDamage = 75;
            drops = 20;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collider = collision.collider.gameObject;
        if (collision.collider.tag == "playerAttack")
        {
            switch (collision.collider.name) 
                {
                case "Snowball":
                    var projectileData = collider.GetComponent<attackManager>();
                    break;
                }

        }
    }

}
