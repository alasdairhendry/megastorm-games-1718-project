using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : EnemyBase, IDamageable {

    float IDamageable.MaximumHealth { get { return base.maximumHealth; } set { base.maximumHealth = value; } }
    float IDamageable.CurrentHealth { get { return base.currentHealth; } set { base.currentHealth = value; } }

    [SerializeField] GameObject impactParticle;
    GameObject IDamageable.ImpactParticle { get { return impactParticle; } }

    private GameObject player;
    private float timeUntilNextAttack = 6.0f;

    private bool isDying = false;
    private bool isDead = false;

    void IDamageable.Die()
    {
        //animator.SetTrigger("Die");
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
        if (isDead)
        { navMesh.isStopped = true; return; }

        float dist = Vector3.Distance(transform.position, player.transform.position);

        // If this enemy is aware of the player
        if (dist <= awarenessRadius)
        {
            navMesh.SetDestination(player.transform.position);  // Set the target destination to the players location
            RotateToTarget();   // Continue to look at the player
            animator.SetBool("inSight", true);
            animator.SetBool("isWalking", true);    // Ensure the enemy is animated to walk

            if (dist >= attackRadius)   // If the player is outside attack range
            {
                // Resume moving
                navMesh.isStopped = false;              
                animator.SetBool("isWalking", true);
            }
            else
            {
                // Stop moving
                navMesh.isStopped = true;
                //animator.SetBool("inSight", false);
                animator.SetBool("isWalking", false);
            }
        }
        else    // This enemy is too far away from the player
        {            
            navMesh.ResetPath();    // Reset the path to null
            animator.SetBool("inSight", false);
            animator.SetBool("isWalking", false);   // Stop walking animation
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

        if (timeUntilNextAttack >= attackInterval)
        {
            timeUntilNextAttack = 0.0f;
            Attack();
            animator.SetTrigger("Attack");
            foreach (ParticleSystem p in attackParticles)
            {
                p.Play();
            }
        }
    }
    [SerializeField] private ParticleSystem[] attackParticles;

    private void MonitorDeath()
    {
        if (isDying)
        {
            //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            //{
                isDead = true;
                StartCoroutine(DestroyThis(1));
            //}
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
