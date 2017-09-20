using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Cleanser : Weapon {

    public override void Start()
    {
        base.Start();

        weaponName = "The Cleanser";
        multiClick = false;
        fireRate = 0.15f;
        ammoCapacity = 15.0f;
    }

    public override void Update()
    {
        base.Update();

        Shoot();
    }

    public override void Shoot()
    {
        base.Shoot();

        if(Input.GetMouseButtonDown(0))
        {
            GameObject ammo = Instantiate(ammo_P);
            ammo.transform.position = shootPoint.transform.position;
            ammo.transform.rotation = transform.rotation;
            ammo.GetComponent<Ammo>().damage = damage;

            ammo.GetComponent<Rigidbody>().AddForce(ammo.transform.forward * Time.deltaTime * initialForwardSpeed, ForceMode.Impulse);
            ammo.GetComponent<Rigidbody>().AddForce(ammo.transform.up * Time.deltaTime * initialUpwardSpeed, ForceMode.Impulse);
        }
    }

    public override void Reload()
    {
        base.Reload();


    }
}
