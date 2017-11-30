using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : EnemyBase, IDamageable {

    float IDamageable.MaximumHealth { get { return base.maximumHealth; } set { base.maximumHealth = value; } }
    float IDamageable.CurrentHealth { get { return base.currentHealth; } set { base.currentHealth = value; } }
    string IDamageable.EntityType { get { return entityType; } set { entityType = value; } }

    [SerializeField] GameObject impactParticle;
    GameObject IDamageable.ImpactParticle { get { return impactParticle; } }

    private GameObject player;
    private float timeUntilNextAttack = 6.0f;

    private bool isDying = false;
    private bool isDead = false;

    [SerializeField] private GameObject crumbleTransform;

    void IDamageable.Die()
    {
        //animator.SetTrigger("Die");
        isDying = true;
    }

    void IDamageable.TakeDamage(float damage)
    {
        AddDamageFloater(damage.ToString());
        ((IDamageable)this).CurrentHealth -= damage;
        //print(damage);

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

    public override void AddDeathEvent(Action _event)
    {
        if (_event != null)
            eventsOnDeath += _event;
    }

    [SerializeField] private ParticleSystem[] attackParticles;

    private void MonitorDeath()
    {
        if (isDying)
        {
            //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            //{
                isDead = true;
                StartCoroutine(DestroyThis(3));
            animator.SetBool("isWalking", false);
            animator.SetBool("inSight", false);
            transform.Find("Graphics").Find("RockFist").Find("RockFist").GetComponent<SkinnedMeshRenderer>().enabled = false;
            crumbleTransform.SetActive(true);
            crumbleTransform.GetComponent<Animator>().SetTrigger("Die");
            //}
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

    private void RotateToTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public override void Attack()
    {
        StartCoroutine(DoAttackDamage());   // Wait for attack animation to finish before applying damage
    }

    private IEnumerator DoAttackDamage()
    {
        yield return new WaitForSeconds(1.75f);

        while (GameState.singleton.IsPaused)
            yield return null;


        RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, 10, Vector3.up);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == this.gameObject)
                continue;

            if (hit.collider.gameObject.GetComponent<IDamageable>() != null)
            {                
                hit.collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }

        PlayAttackSound();
    }

    public void PlayMovementSound()
    {
        base.PlaySFX(0, false, 0.0f, 1.0f, 0.1f, 50.0f);
    }

    public void PlayAttackSound()
    {
        base.PlaySFX(1, false, 0.0f, 1.0f, 5.0f, 500.0f);
    }
}
