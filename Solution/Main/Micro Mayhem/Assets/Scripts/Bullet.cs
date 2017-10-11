using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private Vector3 initialVelocity;
    private float speed;
    private float damage;

    public void Init(Vector3 _initialVelocity, float _speed, float _damage)
    {
        initialVelocity = _initialVelocity;
        speed = _speed;
        damage = _damage;
    }

	void Start () {
        transform.position += transform.InverseTransformDirection(initialVelocity) * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Time.deltaTime * speed;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().TakeDamage(damage);

            if (other.GetComponent<IDamageable>().ImpactParticle != null)
            {
                GameObject particle = Instantiate(other.GetComponent<IDamageable>().ImpactParticle);
                particle.transform.position = this.transform.position;
            }

            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
