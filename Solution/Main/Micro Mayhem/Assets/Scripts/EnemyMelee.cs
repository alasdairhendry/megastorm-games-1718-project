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
    private float timeUntilNextAttack = 0.0f;

    private bool isDying = false;
    private bool isDead = false;

    void IDamageable.Die()
    {
        animator.SetTrigger("Die");
        isDying = true;
    }

    void IDamageable.TakeDamage(float damage)
    {
        ((IDamageable)this).CurrentHealth -= damage;
        print(damage);

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
        MonitorDeath();
    }

    public override void MonitorAwareness()
    {        
        if(isDead)
        { navMesh.isStopped = true; return; }

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if(dist <= awarenessRadius)
        {
            navMesh.SetDestination(player.transform.position);
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

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist > attackRadius)
            return;

        timeUntilNextAttack += Time.deltaTime;

        if(timeUntilNextAttack >= attackInterval)
        {
            timeUntilNextAttack = 0.0f;
            Attack();
            animator.SetTrigger("Attack");
        }
    }

    private void MonitorDeath()
    {
        if(isDying)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            {
                isDead = true;
                StartCoroutine(DestroyThis(1));
            }
        }
    }

    private IEnumerator DestroyThis(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }

    private void RotateToTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public override void Attack()
    {
        player.GetComponent<IDamageable>().TakeDamage(1);
    }
}
