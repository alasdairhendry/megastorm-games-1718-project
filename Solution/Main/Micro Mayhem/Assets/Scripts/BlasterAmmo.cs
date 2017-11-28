using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ammo used by the blaster gun
/// </summary>
public class BlasterAmmo : Bullet {

    [SerializeField] private GameObject particleEffect;

    public override void Init(Vector3 _initialVelocity, float _speed, float _damage)
    {
        initialVelocity = _initialVelocity;
        GetComponent<Rigidbody>().AddRelativeForce(_initialVelocity, ForceMode.Impulse);
        speed = _speed;
        damage = _damage;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameState.singleton.IsPaused)
            return;

        // Translate forward with given speed
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // Ensure we dont collide with the character or the gun
        if (other.gameObject.name == "Character" || other.gameObject.name == "Blaster_PREF(Clone)")
        {
            return;
        }

        // Spawn a particle effect, and call Destroy on this gameobject.
        GameObject particle = Instantiate(particleEffect);
        particle.transform.position = this.transform.position + new Vector3(0, 0.5f, 0);
        Destroy(this.gameObject);        

        Rigidbody[] rigidbodies = GameObject.FindObjectsOfType<Rigidbody>();
        
        // Set up raycast
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(this.transform.position, 5.5f, Vector3.forward);
        
        // Loop through each object the raycast hit
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>() != null)
            {
                // If we collide with a physics-based object, propel it away from the origin of the explosion
                // - The explosion knock-back, and damage taken on an object, is greather the closer it is to the origin - Determined by the Position Deficit
                Vector3 direction = hit.collider.gameObject.transform.position - this.transform.position;
                float positionDeficit = Mathf.Lerp(1.0f, 0.0f, Vector3.Distance(this.transform.position, hit.collider.gameObject.transform.position) / 5.5f);

                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(direction * positionDeficit * 5.0f, ForceMode.VelocityChange);

                if (hit.collider.gameObject.GetComponent<IDamageable>() != null)
                {
                    hit.collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage * positionDeficit);
                }
            }            
        }
    }
}
