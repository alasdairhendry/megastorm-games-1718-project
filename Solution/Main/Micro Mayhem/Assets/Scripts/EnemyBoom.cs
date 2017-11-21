using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoom : EnemyBase, IDamageable {

    float IDamageable.MaximumHealth { get { return base.maximumHealth; } set { base.maximumHealth = value; } }
    float IDamageable.CurrentHealth { get { return base.currentHealth; } set { base.currentHealth = value; } }

    [SerializeField] GameObject impactParticle;
    GameObject IDamageable.ImpactParticle { get { return impactParticle; } }

    private GameObject target;
    private float timeUntilNextAttack = 0.0f;

    [SerializeField] private bool isDying = false;
    [SerializeField] private bool isDead = false;

    string IDamageable.EntityType { get { return entityType; } set { entityType = value; } }

    void IDamageable.Die()
    {        
        isDying = true;
    }

    void IDamageable.TakeDamage(float damage)
    {
        AddDamageFloater(damage.ToString());
        ((IDamageable)this).CurrentHealth -= damage;

        if (((IDamageable)this).CurrentHealth <= 0)
            ((IDamageable)this).Die();
    }

    protected override void Start()
    {
        base.Start();

        FindClosestTarget();

        currentHealth = maximumHealth;        
    }

    private new void Update()
    {
        if (GameState.singleton.IsPaused)
            return;

        base.Update();
        MonitorAwareness();
        MonitorAttack();
        MonitorDeath();

        if (target == null)
            FindClosestTarget();
    }

    public override void MonitorAwareness()
    {
        if (isDead || isDying)
        { navMesh.isStopped = true; return; }

        if (target == null)
            FindClosestTarget();

        float dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist <= awarenessRadius)
        {
            navMesh.SetDestination(target.transform.position);
            RotateToTarget();
            animator.SetBool("isWalking", true);

            if (dist >= attackRadius)
            {
                navMesh.isStopped = false;
                animator.SetBool("isWalking", true);
            }
            else
            {
                navMesh.isStopped = true;
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            navMesh.ResetPath();
            animator.SetBool("isWalking", false);
        }
    }

    public override void MonitorAttack()
    {
        if (isDead)
            return;

        if (target == null)
            FindClosestTarget();

        float dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist > attackRadius)
            return;

        timeUntilNextAttack += Time.deltaTime;

        if (timeUntilNextAttack >= attackInterval)
        {
            timeUntilNextAttack = 0.0f;
            Attack();
            animator.SetTrigger("Attack");
        }
    }

    public override void AddDeathEvent(Action _event)
    {
        if (_event != null)
            eventsOnDeath += _event;
    }

    private void MonitorDeath()
    {
        if (isDying)
        {
            isDead = true;
            StartCoroutine(DestroyThis(1.7f));
        }
    }

    private IEnumerator DestroyThis(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (GameState.singleton.IsPaused)
            yield return null;

        if (eventsOnDeath != null)
            eventsOnDeath();

        Destroy(gameObject);
    }

    private void FindClosestTarget()
    {
        Damageable[] damagables = GameObject.FindObjectsOfType<Damageable>();
        List<Damageable> validTargets = new List<Damageable>();

        foreach (Damageable d in damagables)
        {
            if (d.GetComponent<IDamageable>().EntityType == "friendly")
            {
                validTargets.Add(d);
            }
        }

        //print("Found " + validTargets.Count + " valid targets");

        float closestPoint = Mathf.Infinity;
        Damageable closestGO = new Damageable();

        foreach (Damageable g in validTargets)
        {
            if (Vector3.Distance(this.transform.position, g.transform.position) < closestPoint)
            {
                closestGO = g;
                closestPoint = Vector3.Distance(this.transform.position, g.transform.position);
            }
        }

        if (closestGO != null) 
        target = closestGO.gameObject;
    }

    private void RotateToTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public override void Attack()
    {
        StartCoroutine(DoAttackDamage());   // Wait for attack animation to finish before applying damage
        isDying = true;
    }

    private IEnumerator DoAttackDamage()
    {
        yield return new WaitForSeconds(1.5f);

        while (GameState.singleton.IsPaused)
            yield return null;

        RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, 10, Vector3.up);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == this.gameObject)
                continue;

            if (hit.collider.gameObject.GetComponent<IDamageable>() != null)
            {
                if (hit.collider.gameObject.GetComponent<IDamageable>().EntityType == "friendly")
                    hit.collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }
    }
}
