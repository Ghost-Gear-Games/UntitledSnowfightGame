using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slushLogic : MonoBehaviour
{
    public float maxLifetime = 10;
    public float currentLifetime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLifetime < maxLifetime)
        {
            currentLifetime += Time.deltaTime;

        }
        else
        {
            Destroy(this);
        }
    }
}
