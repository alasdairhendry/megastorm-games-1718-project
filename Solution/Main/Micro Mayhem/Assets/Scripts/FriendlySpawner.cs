using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to spawn friendly entities in the scene
/// </summary>
public class FriendlySpawner : MonoBehaviour {

    [SerializeField] private List<FriendlyWave> friendlyWaves = new List<FriendlyWave>();
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] private int initialSpawnCount = 5;

	// Use this for initialization
	void Start () {
        InitializeWaves();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameState.singleton.IsPaused)
            return;

        MonitorWaves();
	}

    // Initialize all of our waves
    private void InitializeWaves()
    {
        foreach (FriendlyWave fw in friendlyWaves)
        {
            fw.Init();
        }

        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnWave(friendlyWaves[Random.Range(0, friendlyWaves.Count)]);
        }
    }

    // Monitor our waves to see if any of them are ready to spawn
    private void MonitorWaves()
    {
        foreach (FriendlyWave fw in friendlyWaves)
        {
            fw.Increment();

            if(fw.CheckCanSpawn())
            {
                SpawnWave(fw);
            }
        }
    }

    // Called when the given wave is ready to spawn
    private void SpawnWave(FriendlyWave wave)
    {
        for (int i = 0; i < wave.GetAmount(); i++)
        {
            EntityRecords.singleton.SpawnFriendly(wave.GetPrefab(), spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position + new Vector3(wave.GetOffset(), 0.0f, wave.GetOffset()));
        }
    }
}

/// <summary>
/// A defined set of rules for spawning a specific entitiy
/// </summary>
[System.Serializable]
public class FriendlyWave
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2 spawnRange = new Vector2(1, 1);
    [SerializeField] private Vector2 spawnOffset = new Vector2(1, 1);
    [SerializeField] private Vector2 spawnDelay = new Vector2(2, 5);
    private float currentSpawnInterval = 0.0f;
    private float currentSpawnDelay = 0.0f;

    public void Init()
    {
        currentSpawnDelay = Random.Range((float)spawnDelay.x, (float)spawnDelay.y);
    }

    public void Increment()
    {
        currentSpawnInterval += Time.deltaTime;        
    }

    public bool CheckCanSpawn()
    {
        if(currentSpawnInterval >= currentSpawnDelay)
        {
            currentSpawnInterval = 0.0f;
            currentSpawnDelay = Random.Range((float)spawnDelay.x, (float)spawnDelay.y);

            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetAmount()
    {
        return (int)Random.Range(spawnRange.x, spawnRange.y);
    }

    public float GetOffset()
    {
        float offset = Random.Range(spawnOffset.x, spawnOffset.y);;
        return offset;
    }

    public GameObject GetPrefab()
    {
        return prefab;
    }
}
