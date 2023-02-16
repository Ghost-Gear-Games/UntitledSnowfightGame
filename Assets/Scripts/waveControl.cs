using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveControl : MonoBehaviour
{
    public enum waveState { waiting, spawning }
    public waveState currentWaveState = waveState.waiting;
    public int currentWave = 0;
    public waveList[] waves_;
    public GameObject enemy1;
    [System.Serializable]
    public class waveList
    {
        public int enemyCount;
        public int enemyLimit;
        public float spawnInterval;
        public GameObject enemy;
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
                Instantiate(enemy1);
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

    }
}
