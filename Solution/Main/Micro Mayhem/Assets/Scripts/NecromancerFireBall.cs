using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerFireBall : MonoBehaviour, IDamageable {

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

    [SerializeField] private float damage;

    void IDamageable.Die()
    {
        DamageFloaters.singleton.AddFloater("DEFLECT", Color.green, this.transform, 1.0f);
        Destroy(gameObject.transform.parent.gameObject);
    }

    void IDamageable.TakeDamage(float damage)
    {
        DamageFloaters.singleton.AddFloater(damage.ToString(), Color.yellow, this.transform, Vector3.zero, 1.0f);
        ((IDamageable)this).CurrentHealth -= damage;

        if (((IDamageable)this).CurrentHealth <= 0)
            ((IDamageable)this).Die();
    }

    // Use this for initialization
    private void Start () {
        player = GameObject.FindObjectOfType<PlayerAttack>().transform;        
	}

    // Update is called once per frame
    private void Update () {
        if (GameState.singleton.IsPaused)
            return;

        speed += Time.deltaTime * 2.5f;
        Vector3 direction = (GameObject.FindObjectOfType<PlayerAttack>().transform.position + new Vector3(0, 1.0f, 0)) - this.transform.position;        

        transform.position += direction * Time.deltaTime * 1.5f * speed;

        if (Vector3.Distance(player.position + new Vector3(0, 1.0f, 0), transform.position) < 1.0f)
        {
            player.GetComponent<IDamageable>().TakeDamage(damage);
            Destroy(gameObject);
        }
	}
}
