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
        public float cooldown;
    };
    public class slush
    {
        public int upgradeCost;
        public int upgradeCount;
        public GameObject slushPrefab;
        public float damage;
        public double damageMultiplier;
        public float slowdownFactor = 0.8f;
        public float cooldown;
    };
}
