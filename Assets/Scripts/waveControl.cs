using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveControl : MonoBehaviour
{
    public enum waveState { waiting, spawning }
    public waveState currentWaveState = waveState.waiting;
    public int currentWave = 0;
    public wave[] waves_;
    public GameObject enemy;
    public GameObject enemyFast;
    public GameObject enemySlow;
    [System.Serializable]
    public class wave
    {
        public int enemyCount;
        public int enemyLimit;
        public float spawnInterval;
        public GameObject[] enemyArray;
        public float waitTime;
    }
    IEnumerator waveSpawner()
    {
        if (currentWaveState == waveState.waiting)
        {
            Debug.Log("Wait for 10 seconds");
            yield return new WaitForSeconds(waves_[currentWave].waitTime);
            currentWaveState = waveState.spawning;
        }
        if (currentWaveState == waveState.spawning)
        {
            for (int i = 0; i < waves_[currentWave].enemyLimit; i++)
            {
                waves_[currentWave].enemyCount++;
                Instantiate(enemy);
                currentWave += 1;
            }

        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(currentWaveState == waveState.waiting)
        {
            StartCoroutine(waveSpawner());
        }
    }
    void CreateWave(wave[] waveArray)
    {
         waveArray[0] = new wave();
        
    }

}
