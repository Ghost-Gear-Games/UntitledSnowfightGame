using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackManager : MonoBehaviour
{
    [System.Serializable]
    public class snowball
    {
        public int upgradeCount;
        public int upgradeCost;
        public GameObject snowballPrefab;
        public Vector2 size;
        public float damage;
    };
    [System.Serializable]
    public class yellowSnowball
    {
        public int upgradeCost;
        public int upgradeCount;
        public GameObject yellowSnowballPrefab;
        public float damage;
        public float damageOverTime;
        public float damageTimeAmount;
        public float cooldown;
    };
    [System.Serializable]
    public class slush
    {
        public int upgradeCost;
        public int upgradeCount;
        public GameObject slushPrefab;
        public Vector3 size;
        public double damageMultiplier = 1;
        public float slowdownFactor = 0.8f;
        public float cooldown;
    };
    public snowball snow;
    public yellowSnowball yellowSnow;
    public slush slushPuddle;
    private void Awake()
    {
        switch (this.name) { 
            case "Snowball":
                snow.upgradeCost = 50;
                break;
            case "YellowSnowball":
                yellowSnow.upgradeCount = -1;
                yellowSnow.upgradeCost = 50;
                break;
            case "Slush":
                slushPuddle.upgradeCount = -1;
                slushPuddle.upgradeCost = 75;
                break;
        }
    }
    private void Update()
    {
        
        snow.upgradeCost = 2 * ((int)Mathf.Pow(5, snow.upgradeCount + 1));
        yellowSnow.upgradeCost = 2 * ((int)Mathf.Pow(7.5f, yellowSnow.upgradeCount + 1));
        slushPuddle.upgradeCost = 2 * ((int)Mathf.Pow(10, slushPuddle.upgradeCount + 1));

        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(this.gameObject.tag == "playerAttack" && collision.collider.gameObject.layer == 3 && this.gameObject != slushPuddle.slushPrefab)
        {
            Debug.Log("attack collided with edge of screen");
            Destroy(this.gameObject);
        }
    }
}
