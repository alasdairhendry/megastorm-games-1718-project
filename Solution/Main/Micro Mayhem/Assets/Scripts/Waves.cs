using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour {

    public static Waves singleton;
    [SerializeField] private List<WaveEnemy> waveEnemies = new List<WaveEnemy>();
    [SerializeField] private List<Spawnpoint> spawnPoints = new List<Spawnpoint>();
    [SerializeField] private List<Wave> waves = new List<Wave>();
    public List<Spawnpoint> GetSpawnPoints { get { return spawnPoints; } }

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameState.singleton.IsPaused)
            return;

        MonitorWave();
        CheckWavesFinished();
	}

    private void MonitorWave()
    {        
        foreach (Wave wave in waves)
        {
            if (wave.hasSpawned)
                continue;

            wave.delay -= Time.deltaTime;

            if (wave.delay <= 0.0f)
            {
                WaveEnemy waveEnemy = CheckWave(wave.index);

                if(waveEnemy == null)  // We found no prefab, nullify this wave object
                {
                    wave.hasSpawned = true;
                    continue;
                }
                else    // We found a wave enemy
                {
                    List<GameObject> spawns = GetSpawns(waveEnemy.GetSpawnIndex);

                    if (spawns.Count == 0)   // We found no spawn, nullify this wave object
                    {
                        wave.hasSpawned = true;
                        continue;
                    }
                    else
                    {
                        wave.hasSpawned = true;
                        EntityRecords.singleton.SpawnEnemy(waveEnemy.GetPrefab(), spawns[Random.Range(0, spawns.Count)].transform.position);
                    }
                }
            }
        }
    }

    private void CheckWavesFinished()
    {
        bool wavesFinished = true;

        foreach (Wave wave in waves)
        {
            if(!wave.hasSpawned)
            {
                wavesFinished = false;
                break;
            }
        }

        if(wavesFinished)
        {
            if(EntityRecords.singleton.GetEnemiesDead())
            {
                LevelFinishedOverlay.singleton.PlayerWon();
            }
        }
    }

    private WaveEnemy CheckWave(int index)
    {
        foreach (WaveEnemy waveEnemy in waveEnemies)
        {
            if(waveEnemy.Check(index) != null)
            {
                return waveEnemy.Check(index);
            }
        }

        return null;
    }

    private List<GameObject> GetSpawns(int spawnIndex)
    {
        List<GameObject> spawns = new List<GameObject>();

        foreach (Spawnpoint spawn in spawnPoints)
        {
            if(spawn.Check(spawnIndex) != null)
            {
                spawns.Add(spawn.Check(spawnIndex));               
            }
        }

        return spawns;
    }
}

[System.Serializable]
public class Wave
{
    [SerializeField] public int index = -1;
    [SerializeField] public float delay = 0.0f;
    [HideInInspector] public bool hasSpawned = false;
}

[System.Serializable]
public class WaveEnemy
{
    [SerializeField] private GameObject prefab; // The prefab associated with this class
    [SerializeField] private int index = -1;   // The index associated with this enemy
    [SerializeField] private int spawnIndex = -1;
    public int GetSpawnIndex { get { return spawnIndex; } }

    public WaveEnemy Check(int indexToCheck)
    {
        if(indexToCheck == index)
        {
            return this;
        }
        else
        {
            return null;
        }
    }

    public GameObject GetPrefab()
    {
        return prefab;
    }
}

[System.Serializable]
public class Spawnpoint
{
    [SerializeField] private GameObject spawnPoint; // The prefab associated with this class
    [SerializeField] private int index = -1;   // The index associated with this enemy

    public GameObject Check(int indexToCheck)       // Call this to check if the enemy can spawn at this spawn point
    {
        if (indexToCheck == index)
        {
            return spawnPoint;
        }
        else
        {
            return null;
        }
    }
}