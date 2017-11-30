using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An iceball that the necromancer casts towards the player
/// </summary>
[DisallowMultipleComponent]
public class NecromancerIceBall : MonoBehaviour, IDamageable {

    Transform player;
    float speed = 1.0f;

    private float maximumHealth = 5.0f;
    private float currentHealth = 5.0f;
    float IDamageable.MaximumHealth { get { return maximumHealth; } set { maximumHealth = value; } }
    float IDamageable.CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    [SerializeField] GameObject impactParticle;
    GameObject IDamageable.ImpactParticle { get { return impactParticle; } }

    [SerializeField] protected string entityType = "enemy";
    string IDamageable.EntityType { get { return entityType; } set { entityType = value; } }

    // Destroy the iceball
    void IDamageable.Die()
    {
        DamageFloaters.singleton.AddFloater("DEFLECT", Color.green, this.transform, 1.0f);
        Destroy(gameObject.transform.parent.gameObject);
    }

    // The iceball takes damage
    void IDamageable.TakeDamage(float damage)
    {
        DamageFloaters.singleton.AddFloater(damage.ToString(), Color.yellow, this.transform, Vector3.zero, 1.0f);
        ((IDamageable)this).CurrentHealth -= damage;

        if (((IDamageable)this).CurrentHealth <= 0)
            ((IDamageable)this).Die();
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerAttack>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.singleton.IsPaused)
            return;

        // Increase the speed over time, so it eventually reaches the player
        speed += Time.deltaTime * 2.5f;

        // Calculate the direction towards the player
        Vector3 direction = (GameObject.FindObjectOfType<PlayerAttack>().transform.position + new Vector3(0, 1.0f, 0)) - this.transform.position;

        // Move in the calculated direction
        transform.position += direction * Time.deltaTime * 1.5f * speed;

        // If we are close enough to the player, destroy ourselves and apply damage to the player.
        if (Vector3.Distance(player.position + new Vector3(0, 1.0f, 0), transform.position) < 1.0f)
        {
            if (player.gameObject.GetComponent<FreezeMovement>() == null)
                player.gameObject.AddComponent<FreezeMovement>();

            Destroy(gameObject);
        }
    }
}
