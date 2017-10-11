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

    [Header("Damageable")]
    [SerializeField] protected float maximumHealth;
    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maximumHealth;
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = movementSpeed;
        navMesh.angularSpeed = turnSpeed;
    }

    public virtual void MonitorAwareness() { }

    public virtual void MonitorAttack() { }

    public virtual void Attack() { }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, awarenessRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
