using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : EnemyBase, IDamageable
{
    float IDamageable.MaximumHealth { get { return base.maximumHealth; } set { base.maximumHealth = value; } }
    float IDamageable.CurrentHealth { get { return base.currentHealth; } set { base.currentHealth = value; } }

    [SerializeField] GameObject impactParticle;
    GameObject IDamageable.ImpactParticle { get { return impactParticle; } }

    private GameObject player;

    void IDamageable.Die()
    {
        Destroy(gameObject);
    }

    void IDamageable.TakeDamage(float damage)
    {
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

    private void Update()
    {
        MonitorAwareness();
        MonitorAttack();
    }

    public override void MonitorAwareness()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if(dist <= awarenessRadius)
        {
            navMesh.SetDestination(player.transform.position);

            if (dist >= attackRadius)
            {
                navMesh.isStopped = false;
            }
            else
            {
                navMesh.isStopped = true;
            }
        }
        else
        {
            navMesh.ResetPath();
        }
    }

    private float timeUntilNextAttack = 0.0f;
    public override void MonitorAttack()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist > attackRadius)
            return;

        timeUntilNextAttack += Time.deltaTime;

        if(timeUntilNextAttack >= attackInterval)
        {
            timeUntilNextAttack = 0.0f;
            Attack();
        }
    }

    public override void Attack()
    {
        player.GetComponent<IDamageable>().TakeDamage(15);
    }
}
