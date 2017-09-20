using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    [SerializeField] private float currentHealth = 100.0f;
    private float maxHealth = 100.0f;

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
