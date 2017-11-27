using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected virtual void Start () {
        transform.position += transform.InverseTransformDirection(initialVelocity) * Time.deltaTime;
	}

    // Update is called once per frame
    protected virtual void Update () {
        if (GameState.singleton.IsPaused)
            return;

        transform.position += transform.forward * Time.deltaTime * speed;
	}

    protected virtual void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        if (other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().TakeDamage(damage);
           
            if (other.GetComponent<IDamageable>().ImpactParticle != null)
            {
                //GameObject particle = Instantiate(other.GetComponent<IDamageable>().ImpactParticle);
                //particle.transform.position = this.transform.position;
            }

            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
