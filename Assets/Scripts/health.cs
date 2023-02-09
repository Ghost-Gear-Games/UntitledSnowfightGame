using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    private void Start()
    {
        healthDamage healthSystem = new healthDamage(100);
        Debug.Log("Health: "+healthSystem.GetHealth());
        healthSystem.Damage(10);
        Debug.Log("Health: "+healthSystem.GetHealth());
        healthSystem.Heal(10);
        Debug.Log("Health: "+healthSystem.GetHealth());
        CMDebug.ButtonUI(new Vector2(100, 100), "damage", () =>
        {
            healthSystem.Damage(10);
            Debug.Log("Damaged: " + healthSystem.GetHealth());
        });
        CMDebug.ButtonUI(new Vector2(-100, 100), "heal", () =>
        {
            healthSystem.Damage(10);
            Debug.Log("Healed: " + healthSystem.GetHealth());
        });
    }
}
