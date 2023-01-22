using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveControl : MonoBehaviour
{
    public enum waveState { waiting, spawning }
    public waveState currentWaveState = waveState.waiting;
    public waves[] waves_;
    IEnumerator waveSpawner() {
        if (currentWaveState == waveState.waiting)
        {
            Debug.Log("Wait for 10 seconds");
            yield return new WaitForSeconds(waves_.waitTime);
            currentWaveState = waveState.spawning;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
