using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Cleanser : Ammo {

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float damageRadius = 5.0f;

    public override void Start()
    {
        base.Start();

    }

    public override void Update()
    {
        base.Update();

 
    }

    public override void Collision()
    {
        if (hasActivated)
            return;

        hasActivated = true;
        GameObject prefab = Instantiate(explosionPrefab);
        prefab.transform.position = transform.position;
        AOEDamage(damageRadius, damage);

        Destroy(gameObject, 0.5f);
    }

    public override void OnCollisionEnter(Collision col)
    {
        Collision();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
