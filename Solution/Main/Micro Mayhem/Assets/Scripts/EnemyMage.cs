﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// AI Behaviour for the Necromancer enemy
/// </summary> 
public class EnemyMage : EnemyBase, IDamageable {

    float IDamageable.MaximumHealth { get { return base.maximumHealth; } set { base.maximumHealth = value; } }
    float IDamageable.CurrentHealth { get { return base.currentHealth; } set { base.currentHealth = value; } }
    string IDamageable.EntityType { get { return entityType; } set { entityType = value; } }

    [SerializeField] List<GameObject> projectilePrefabs = new List<GameObject>();
    [SerializeField] GameObject shootPoint;

    [SerializeField] GameObject impactParticle;
    GameObject IDamageable.ImpactParticle { get { return impactParticle; } }

    private GameObject player;
    private float timeUntilNextAttack = 0.0f;

    private bool isDying = false;
    private bool isDead = false;

    [SerializeField] private bool findingClosestPoint = false;
    private bool isWalkingTo = false;

    // Kill this enemy
    void IDamageable.Die()
    {
        animator.SetTrigger("Die");
        isDying = true;
    }

    // Tell this enemy to take damage
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
        currentHealth = maximumHealth;
        player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
    }

    private new void Update()
    {
        if (GameState.singleton.IsPaused)
            return;

        base.Update();
        MonitorAwareness();
        MonitorAttack();
        MonitorDeath();
    }

    // Monitor where the enemy is in respect to the character
    public override void MonitorAwareness()
    {
        if (isDead)
        { navMesh.isStopped = true; return; }

        float dist = Vector3.Distance(transform.position, player.transform.position);

        // If the target is too close, move away from it
        if(dist <= awarenessRadius)
        {
            if (!findingClosestPoint)
            {
                if (navMesh.hasPath == false)
                {
                    StartCoroutine(FindClosestPoint());
                }
            }

            if (navMesh.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            { navMesh.isStopped = true; animator.SetBool("isWalking", false); }
            else
            { navMesh.isStopped = false; animator.SetBool("isWalking", true); }
        }
        else if(dist>= attackRadius)
        {
            isWalkingTo = true;
            navMesh.SetDestination(player.transform.position);  // Set the target destination to the players location
            RotateToTarget();   // Continue to look at the player

            animator.SetBool("isWalking", true);    // Ensure the enemy is animated to walk
        }
        else
        {
            //navMesh.ResetPath();
            animator.SetBool("isWalking", false);
        }

        if (isWalkingTo && dist <= attackRadius)
        {
            navMesh.ResetPath();
            isWalkingTo = false;
            animator.SetBool("isWalking", false);
        }
    }

    // Monitor our attack range
    public override void MonitorAttack()
    {
        if (isDead)
            return;

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist > attackRadius)
            return;

        RotateToTarget();
        timeUntilNextAttack += Time.deltaTime;

        // Attack our target if we can
        if (timeUntilNextAttack >= attackInterval)
        {
            timeUntilNextAttack = 0.0f;
            Attack();
            animator.SetTrigger("Attack");
        }
    }

    // Events called on Death
    public override void AddDeathEvent(Action _event)
    {
        if (_event != null)
            eventsOnDeath += _event;
    }

    // Monitor if we are dying
    private void MonitorDeath()
    {
        if (isDying)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            {
                isDead = true;
                StartCoroutine(DestroyThis(1));
            }
        }
    }

    // Kill this enemy, after a given interval
    private IEnumerator DestroyThis(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (GameState.singleton.IsPaused)
            yield return null;

        if (eventsOnDeath != null)
            eventsOnDeath();

        Destroy(gameObject);
    }

    // Choose our attack
    public override void Attack()
    {
        StartCoroutine(SpawnProjectile());
        PlayAttackSound();
    }

    // After a delay, attack our target
    private IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(0.5f);

        while (GameState.singleton.IsPaused)
            yield return null;

        GameObject proj = Instantiate(projectilePrefabs[UnityEngine.Random.Range(0, projectilePrefabs.Count)]);
        proj.transform.position = shootPoint.transform.position;
        proj.transform.parent = this.transform;
    }

    // Face our target
    private void RotateToTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    // Find the closest place that is further than our "Flee" radius
    private IEnumerator FindClosestPoint()
    {
        findingClosestPoint = true;
        Vector2 origin = new Vector2(player.transform.position.x, player.transform.position.z);
        int angle = 30;
        float radius = awarenessRadius + 5.0f;
        bool foundPoint = false;
        List<Vector3> possiblePoints = new List<Vector3>();
        List<Vector3> validPoints = new List<Vector3>();

        for (int i = 0; i < 12; i++)
        {
            Vector2 pointOnCircle = new Vector2 { x = (origin.x + radius) * Mathf.Cos((angle * i) * Mathf.Deg2Rad), y = (origin.y + radius) * Mathf.Sin((angle * i) * Mathf.Deg2Rad) };
            possiblePoints.Add(new Vector3(pointOnCircle.x, 0, pointOnCircle.y));                       
        }

        foreach (Vector3 point in possiblePoints)
        {
            navMesh.SetDestination(point);

            //print("CheckingPoint");

            while (navMesh.pathPending)
            {
                //print("Awaiting Path");
                yield return null;
            }

            if (navMesh.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                //print("Found True Path - " + point);

                validPoints.Add(point);
                //foundPoint = true;
                //print("Found True Path ");
            }            
        }

        float closestDistance = Mathf.Infinity;
        foreach (Vector3 point in validPoints)
        {
            if(Vector3.Distance(point, Vector3.zero) < closestDistance)
            {
                closestDistance = Vector3.Distance(point, Vector3.zero);
                navMesh.SetDestination(point);
            }
        }
        
        //print("Setting False");
        findingClosestPoint = false;
    }

    public void PlayAttackSound()
    {
        base.PlaySFX(0, false, 0.1f, 1.0f, 10.0f, 500.0f);
    }
}
