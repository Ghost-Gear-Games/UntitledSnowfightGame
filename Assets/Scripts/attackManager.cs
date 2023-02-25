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
    public class yellowSnowball
    {
        public int upgradeCost;
        public int upgradeCount;
        public GameObject yellowSnowballPrefab;
        public float damage;
        public int damageOverTime;
        public int damageTimeAmount;
        public float cooldown;
    };
    public class slush
    {
        public int upgradeCost;
        public int upgradeCount;
        public GameObject slushPrefab;
        public Vector3 size;
        public double damageMultiplier;
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
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(this.gameObject.layer == 6 && collision.collider.gameObject.layer == 3)
        {
            Destroy(this.gameObject);
        }
    }
}
