using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthSystem : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100;

    private void Update()
    {

    }

    public void Thaw(float temp)
    {
        healthAmount -= temp;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        if (this.gameObject.tag == "player")
        {
            healthBar.fillAmount = healthAmount / 100;
        }
    }

    public void Freeze(float temp)
    {
        healthAmount += temp;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        healthBar.fillAmount = healthAmount / 100;
    }
    public bool Checkup()
    {
        if (healthAmount >= 100)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}