using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IDamageable {

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject mountPoint;
    [SerializeField] private GameObject crosshair;

    [SerializeField] private float maximumHealth;
    [SerializeField] private float currentHealth;

    float IDamageable.MaximumHealth { get { return maximumHealth; } set { maximumHealth = value; } }
    float IDamageable.CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    [SerializeField] GameObject impactParticle;
    GameObject IDamageable.ImpactParticle { get { return impactParticle; } }

    void IDamageable.Die()
    {
        Debug.Log("Dead");
    }

    void IDamageable.TakeDamage(float damage)
    {
        ((IDamageable)this).CurrentHealth -= damage;

        if (((IDamageable)this).CurrentHealth <= 0)
            ((IDamageable)this).Die();

    }

    // Use this for initialization
    void Start () {
        currentHealth = maximumHealth;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
