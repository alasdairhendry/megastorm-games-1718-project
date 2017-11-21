using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour {

    [Header("Base Values")]
    [SerializeField] protected float awarenessRadius = 5.0f;
    [SerializeField] protected float attackRadius = 2.5f;
    [SerializeField] protected float attackInterval = 2.0f;
    [SerializeField] protected float movementSpeed = 2.0f;
    [SerializeField] protected float turnSpeed = 120.0f;
    protected NavMeshAgent navMesh;
    protected Animator animator;

    [Header("Damageable")]
    [SerializeField] protected float maximumHealth;
    [SerializeField] protected float currentHealth;

    [SerializeField] protected float damage;
    [SerializeField] protected string entityType = "enemy";

    protected Action eventsOnDeath;

    protected virtual void Start()
    {
        currentHealth = maximumHealth;
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = movementSpeed;
        navMesh.angularSpeed = turnSpeed;
        animator = GetComponentInChildren<Animator>();
    }

    public virtual void MonitorAwareness() { }

    public virtual void MonitorAttack() { }

    public virtual void Attack() { }    

    public virtual void AddDeathEvent(Action _event)
    {
        if (_event != null)
            eventsOnDeath += _event;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, awarenessRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    protected void Update()
    {
        if (GameState.singleton.IsPaused)
            return;

        MonitorDamageFloaters();
    }

    private float damageFloatersMinWait = 0.15f;
    private float damageFloaterCurrCounter = 0.0f;

    private List<string> currentFloaters = new List<string>();

    private void MonitorDamageFloaters()
    {
        damageFloaterCurrCounter += Time.deltaTime;

        if(damageFloaterCurrCounter >= damageFloatersMinWait)
        {
            SendDamageFloaters();
            damageFloaterCurrCounter = 0.0f;
        }
    }

    private void SendDamageFloaters()
    {
        float allDamage = 0.0f;

        foreach (string floater in currentFloaters)
        {
            allDamage += float.Parse(floater);            
        }

        if (allDamage != 0)
            DamageFloaters.singleton.AddFloater(allDamage.ToString("00"), Color.yellow, this.transform, new Vector3(0.0f, 2.0f, -1.0f), 1);

        currentFloaters.Clear();
    }

    protected void AddDamageFloater(string text)
    {
        if (currentFloaters.Count == 0)
            damageFloaterCurrCounter = 0;

        currentFloaters.Add(text);
    }
}
