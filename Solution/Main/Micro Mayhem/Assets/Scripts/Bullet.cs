using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class that most/all ammo should derive from 
/// </summary>
public class Bullet : MonoBehaviour {

    protected Vector3 initialVelocity;
    protected float speed;
    protected float damage;

    public virtual void Init(Vector3 _initialVelocity, float _speed, float _damage)
    {
        initialVelocity = _initialVelocity;
        speed = _speed;
        damage = _damage;
    }

    // On spawn, propel the bullet with the given initialVelocity
    protected virtual void Start () {
        transform.position += transform.InverseTransformDirection(initialVelocity) * Time.deltaTime;
	}

    // Update is called once per frame
    protected virtual void Update () {
        if (GameState.singleton.IsPaused)
            return;

        // Move forward locally, in respect to our speed
        transform.position += transform.forward * Time.deltaTime * speed;
	}

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Check we collided with an Object that can be damaged
        if (other.GetComponent<IDamageable>() != null)
        {
            // Send damage to the object
            other.GetComponent<IDamageable>().TakeDamage(damage);
           
            // Check if this object has a specific impact particle, if it does, spawn it in.
            if (other.GetComponent<IDamageable>().ImpactParticle != null)
            {
                //GameObject particle = Instantiate(other.GetComponent<IDamageable>().ImpactParticle);
                //particle.transform.position = this.transform.position;
            }

            // Destroy the gameobject
            Destroy(this.gameObject);
        }
        else
        {
            // We collided with something that is not damagable, Destroy the bullet.
            Destroy(this.gameObject);
        }
    }
}
