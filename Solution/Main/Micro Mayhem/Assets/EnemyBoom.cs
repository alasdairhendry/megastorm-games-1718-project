using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoom : EnemyBase, IDamageable {

    float IDamageable.MaximumHealth { get { return base.maximumHealth; } set { base.maximumHealth = value; } }
    float IDamageable.CurrentHealth { get { return base.currentHealth; } set { base.currentHealth = value; } }

    [SerializeField] GameObject impactParticle;
    GameObject IDamageable.ImpactParticle { get { return impactParticle; } }

    private GameObject player;
    private float timeUntilNextAttack = 0.0f;

    [SerializeField] private bool isDying = false;
     [SerializeField] private bool isDead = false;

    void IDamageable.Die()
    {        
        isDying = true;
    }

    void IDamageable.TakeDamage(float damage)
    {
        AddDamageFloater(damage.ToString());
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

    private new void Update()
    {
        base.Update();
        MonitorAwareness();
        MonitorAttack();
        MonitorDeath();
    }

    public override void MonitorAwareness()
    {
        if (isDead || isDying)
        { navMesh.isStopped = true; return; }

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist <= awarenessRadius)
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

        if (timeUntilNextAttack >= attackInterval)
        {
            timeUntilNextAttack = 0.0f;
            Attack();
            animator.SetTrigger("Attack");
        }
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
        isDying = true;
    }

    private IEnumerator DoAttackDamage()
    {
        yield return new WaitForSeconds(1.5f);

        RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, 10, Vector3.up);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == this.gameObject)
                continue;

            if (hit.collider.gameObject.GetComponent<IDamageable>() != null)
            {
                print("found");
                hit.collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }
    }
}
