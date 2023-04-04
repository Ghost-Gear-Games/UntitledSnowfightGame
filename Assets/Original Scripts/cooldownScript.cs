using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cooldownScript : MonoBehaviour
{
    public float cooldownTime;
    public float nextTimeUsable = 0;

    bool CheckCooldown()
    {
        if(Time.time > nextTimeUsable)
        {
           nextTimeUsable = Time.time + cooldownTime;
           return true;
        }
        return false;
    }
}
