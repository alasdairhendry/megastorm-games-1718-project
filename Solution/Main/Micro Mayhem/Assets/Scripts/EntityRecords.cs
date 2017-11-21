using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRecords : MonoBehaviour {

    public static EntityRecords singleton;
    public Action<EnemyBase> onEnemySpawn;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    [SerializeField] List<GameObject> friendlyEntities = new List<GameObject>();
    [SerializeField] List<GameObject> enemyEntities = new List<GameObject>();
    [SerializeField] private GameObject parent;

    private void Start()
    {
        onEnemySpawn += Tutorial.singleton.OnEnemySpawn;
    }

    public void SpawnEnemy(GameObject prefab, Vector3 position)
    {
        GameObject go = Instantiate(prefab);
        go.transform.position = position;
        go.GetComponent<EnemyBase>().AddDeathEvent(() => { enemyEntities.Remove(go); CheckList(); });

        go.transform.parent = parent.transform.Find("Enemies");

        enemyEntities.Add(go);
        if (onEnemySpawn != null)
            onEnemySpawn(go.GetComponent<EnemyBase>());
    }

    //public void SpawnEnemy(GameObject prefab, Transform parent, Vector3 localPosition)
    //{
    //    GameObject go = Instantiate(prefab);
    //    go.transform.parent = parent;
    //    go.transform.localPosition = localPosition;
    //    go.GetComponent<EnemyBase>().AddDeathEvent(() => { enemyEntities.Remove(go); CheckList(); });

    //    enemyEntities.Add(go);
    //}

    public void SpawnFriendly(GameObject prefab, Vector3 position)
    {
        GameObject go = Instantiate(prefab);
        go.transform.position = position;
        go.GetComponent<FriendlyBase>().AddDeathEvent(() => { friendlyEntities.Remove(go); CheckList(); });

        go.transform.parent = parent.transform.Find("Friendlies");

        friendlyEntities.Add(go);
    }

    public float GetInfectionData()
    {
        float infectionRatio = 0.0f;

        if (friendlyEntities.Count == 0 && enemyEntities.Count == 0)
        {
            infectionRatio = 0.0f;
        }
        else if (friendlyEntities.Count == 0 && enemyEntities.Count > 0)
        {
            infectionRatio = 1.0f;
        }
        else if (enemyEntities.Count == 0 && friendlyEntities.Count > 0)
        {
            infectionRatio = -1.0f;
        }
        else    // There is friendly and enemy entities
        {
            if(friendlyEntities.Count == enemyEntities.Count)
            {
                infectionRatio = 0.0f;
            }
            else
            {
                if(friendlyEntities.Count > enemyEntities.Count)
                {
                    infectionRatio = (((float)friendlyEntities.Count / (float)enemyEntities.Count) / (friendlyEntities.Count + enemyEntities.Count)) * -1.0f;
                }
                else if(enemyEntities.Count > friendlyEntities.Count)
                {
                    infectionRatio = (((float)enemyEntities.Count / (float)friendlyEntities.Count) / (friendlyEntities.Count + enemyEntities.Count));
                }
            }
        }

        return infectionRatio;
    }

    public bool GetEnemiesDead()
    {
        if (enemyEntities.Count <= 0) return true;
        else return false;
    }

    private void CheckList ()
    {
        for (int i = 0; i < friendlyEntities.Count; i++)
        {
            if (friendlyEntities[i] == null)
                friendlyEntities.RemoveAt(i);
        }

        for (int i = 0; i < enemyEntities.Count; i++)
        {
            if (enemyEntities[i] == null)
                enemyEntities.RemoveAt(i);
        }
    }
}
