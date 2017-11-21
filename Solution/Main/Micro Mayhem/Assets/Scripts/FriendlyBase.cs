using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyBase : MonoBehaviour, IDamageable {

    [Header("Base Values")]
    [SerializeField]
    protected float awarenessRadius = 5.0f;
    [SerializeField] protected float attackRadius = 2.5f;
    [SerializeField] protected float attackInterval = 2.0f;
    [SerializeField] protected float movementSpeed = 2.0f;
    [SerializeField] protected float turnSpeed = 120.0f;

    [Header("Damageable")]
    [SerializeField] protected float maximumHealth;
    [SerializeField] protected float currentHealth;

    protected NavMeshAgent navMesh;
    protected Animator animator;

    private bool isDying = false;
    private bool isDead = false;

    float IDamageable.MaximumHealth { get { return maximumHealth; } set { maximumHealth = value; } }
    float IDamageable.CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    [SerializeField] GameObject impactParticle;
    GameObject IDamageable.ImpactParticle { get { return impactParticle; } }

    [SerializeField] protected string entityType = "friendly";
    string IDamageable.EntityType { get { return entityType; } set { entityType = value; } }

    public Action eventsOnDeath;

    protected virtual void Start()
    {
        currentHealth = maximumHealth;
        navMesh = GetComponent<NavMeshAgent>();
        //navMesh.speed = movementSpeed;
        //navMesh.angularSpeed = turnSpeed;
        animator = GetComponentInChildren<Animator>();
    }

    protected void Update()
    {
        if (GameState.singleton.IsPaused)
            return;

        MonitorAwareness();
        MonitorDamageFloaters();
        MonitorDeath();
    }

    protected virtual void MonitorAwareness() { }

    protected virtual void MonitorDeath()
    {
        if (isDying)
        {
            isDead = true;
            StartCoroutine(DestroyThis(1));
        }
    }

    public virtual void AddDeathEvent(Action _event)
    {
        if (_event != null)
            eventsOnDeath += _event;
    }

    private IEnumerator DestroyThis(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (eventsOnDeath != null)
            eventsOnDeath();

        Destroy(gameObject);
    }

    void IDamageable.Die()
    {
        //animator.SetTrigger("Die");
        isDying = true;
    }

    void IDamageable.TakeDamage(float damage)
    {
        AddDamageFloater(damage.ToString());
        ((IDamageable)this).CurrentHealth -= damage;

        if (((IDamageable)this).CurrentHealth <= 0)
            ((IDamageable)this).Die();
    }

    private float damageFloatersMinWait = 0.15f;
    private float damageFloaterCurrCounter = 0.0f;

    private List<string> currentFloaters = new List<string>();

    private void MonitorDamageFloaters()
    {
        damageFloaterCurrCounter += Time.deltaTime;

        if (damageFloaterCurrCounter >= damageFloatersMinWait)
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
