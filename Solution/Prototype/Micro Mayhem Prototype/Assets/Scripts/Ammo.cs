using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {

    [HideInInspector] public float damage = 0.0f;
    protected bool hasActivated = false;

    public virtual void Start() { }

    public virtual void Update() { }

    public virtual void Collision() { }

    public virtual void OnCollisionEnter(Collision col) { }

    protected void AOEDamage(float radius, float damage)
    {
        RaycastHit[] collisions = Physics.SphereCastAll(new Ray(transform.position, transform.forward), radius);
        foreach (RaycastHit hit in collisions)
        {
            if(hit.collider.gameObject.GetComponent<EnemyBase>()!= null)
            {
                hit.collider.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
            }
        }
    }
}
