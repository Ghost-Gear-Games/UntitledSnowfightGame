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
    public wave[] waveList = new wave[] { };

    public float slowerThanUpdateTimer = 1;
    public Transform spawnpoint1;
    public Transform spawnpoint2;
    public Transform spawnpoint3;
    public Transform spawnpoint4;

    int enemyCount;



    bool enemiesAreAlive()
    {
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
           else
           {
               return true;
           }

    }
    void Start()
    {
        currentWave = -1;
        waitTime = 3f;
        StartCoroutine(waveSpawner());
    }

    void Update()
    {
        enemiesAreAlive();
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

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
    IEnumerator waveSpawner()
    {
        while (!pauseMenu.GameIsPaused)
        {
            if(currentWave >= 4)
            {
                GenerateWave();
            }
            //GWS in debug log means Gamemaster Wave Script this helps filter the logs
            Debug.Log("GWS ran coroutine wave spawner");
            if (currentWaveState == waveState.waiting)
            {
                Debug.Log("GWS We are in Waiting");
                if (!enemiesAreAlive())
                {
                    Debug.Log("GWS we waited and are now ready to spawn the next wave");
                    currentWave += 1;
                    currentWaveState = waveState.spawning;
                }
                else
                {
                    Debug.Log("GWS Enemies alive and we wait");
                    yield return new WaitForSeconds(1);
                }

            }
            if (currentWaveState == waveState.spawning)
            {

                Debug.Log("GWS spawn the wave's enemies");
                foreach (GameObject enemy in waveList[currentWave].enemies)
                {
                    Instantiate(waveList[currentWave].enemies[enemyCount], Spawnpointer(Random.Range(1, 5)));
                    Debug.Log("GWS spawned enemy");
                    yield return new WaitForSeconds(1f / spawnInterval);
                }
            }

            Debug.Log("GWS finished spawning");
            currentWaveState = waveState.waiting;
        }


    }

    void GenerateWave()
    {
        switch (currentWave)
        {
            case 4:
                switch (Random.Range(1, 4))
                {
                    case 1:
                        waveList[currentWave].enemies = waveList[currentWave - 1].enemies;
                        for (int y = 0; y < 2; y++)
                        {
                            waveList[currentWave].enemies[waveList[currentWave].enemies.Length + 1] = enemyN;
                        }
                        break;
                    case 2:
                        waveList[currentWave].enemies = waveList[currentWave - 1].enemies;
                        for (int x = 0; x < 4; x++)
                        {
                            waveList[currentWave].enemies[waveList[currentWave].enemies.Length + 1] = enemyF;
                        }
                        break;
                    case 3:
                        waveList[currentWave].enemies = waveList[currentWave - 1].enemies;
                        waveList[currentWave].enemies[waveList[currentWave].enemies.Length + 1] = enemyS;
                        break;
                }
                break;
            case >=5:
                for(int i = 0; i < currentWave-4; i++) {
                    switch (Random.Range(1, 4)) {
                        case 1:
                            waveList[currentWave].enemies = waveList[currentWave - 1].enemies;
                            for(int y = 0; y < 2; y++)
                            {
                                waveList[currentWave].enemies[waveList[currentWave].enemies.Length + 1] = enemyN;
                            }
                            break;
                        case 2:
                            waveList[currentWave].enemies = waveList[currentWave - 1].enemies;
                            for(int x = 0; x < 4; x++) {
                                waveList[currentWave].enemies[waveList[currentWave].enemies.Length + 1] = enemyF;
                            }
                            break;
                        case 3:
                            waveList[currentWave].enemies = waveList[currentWave - 1].enemies;
                            waveList[currentWave].enemies[waveList[currentWave].enemies.Length + 1] = enemyS;
                            break;
                    }
                }
                break;

        }
    }
    
}
