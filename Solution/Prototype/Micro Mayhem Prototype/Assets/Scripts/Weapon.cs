using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public string weaponName;

    public bool multiClick;
    public float fireRate;
    public float damage;

    public float ammoCapacity;
    public GameObject ammo_P;
    public GameObject shootPoint;

    public float initialUpwardSpeed = 500.0f;
    public float initialForwardSpeed = 1500.0f;

    public virtual void Start() { }
	
	// Update is called once per frame
	public virtual void Update() { }

    public virtual void Shoot() { }

    public virtual void Reload() { }
}
