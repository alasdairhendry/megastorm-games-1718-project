using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {

    float MaximumHealth { get; set; }
    float CurrentHealth { get; set; }
    string EntityType { get; set; }
    GameObject ImpactParticle { get; }

    void TakeDamage(float damage);

    void Die();
}
