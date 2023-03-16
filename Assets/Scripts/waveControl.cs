using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveControl : MonoBehaviour
{
    [System.Serializable]
    public class wave
    {

        public GameObject[] enemies;

    }
    public enum waveState { waiting, spawning }
    public waveState currentWaveState = waveState.waiting;
    public float spawnInterval = 1;
    public float waitTime;
    public int currentWave;

    public GameObject enemyN;
    public GameObject enemyF;
    public GameObject enemyS;
    public wave[] waveList;

    public float slowerThanUpdateTimer = 1;
    public Transform spawnpoint1;
    public Transform spawnpoint2;
    public Transform spawnpoint3;
    public Transform spawnpoint4;


    bool enemiesAreAlive()
    {
        slowerThanUpdateTimer -= Time.deltaTime;
        if (slowerThanUpdateTimer <= 0)
        {
            slowerThanUpdateTimer = 1;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                if (currentWaveState == waveState.waiting)
                {
                    StartCoroutine(waveSpawner(waveList[currentWave + 1]));
                }
                return false;
            }

        }
        return true;
    }
    void Start()
    {
        currentWave = 0;
        waitTime = 5f;
        StartCoroutine(waveSpawner(waveList[currentWave]));
    }

    void Update()
    {
        enemiesAreAlive();


    }
    Transform Spawnpointer(float spawnpoint)
    {
        if (spawnpoint == 1)
        {
            return spawnpoint1;
        }
        if(spawnpoint == 2)
        {
            return spawnpoint2;
        }
        if(spawnpoint == 3)
        {
            return spawnpoint3;
        }
        if(spawnpoint == 4)
        {
            return spawnpoint4;
        }
        else
        {
            return spawnpoint4;
        }
    }
    IEnumerator waveSpawner(wave wave_)
    {
        int enemyCount;
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (currentWaveState == waveState.waiting)
        {
            if (!enemiesAreAlive())
            {
                Debug.Log("Wait for 10 seconds");
                yield return new WaitForSeconds(waitTime);
                currentWaveState = waveState.spawning;
            }

        }
        if (currentWaveState == waveState.spawning)
        {

            Debug.Log("spawn the wave's enemies");
            foreach (GameObject enemy in waveList[currentWave].enemies)
            {
                    Instantiate(waveList[currentWave].enemies[enemyCount], Spawnpointer(Random.Range(1, 5)));
                    enemyCount++;
                    Debug.Log("spawned enemy");
                    yield return new WaitForSeconds(1f / spawnInterval);
            }
        }
        Debug.Log("finished spawning");
        currentWaveState = waveState.waiting;
        currentWave += 1;
        yield break;

    }
}
