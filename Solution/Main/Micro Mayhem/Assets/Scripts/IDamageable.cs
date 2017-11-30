using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface used to define the variables of an Object that can take damage
/// </summary>
public interface IDamageable {

    float MaximumHealth { get; set; }
    float CurrentHealth { get; set; }
    string EntityType { get; set; }
    GameObject ImpactParticle { get; }

    void TakeDamage(float damage);

    void Die();
}
