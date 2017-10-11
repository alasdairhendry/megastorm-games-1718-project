using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour {

    [SerializeField] protected string weaponName;

    [SerializeField] protected float baseDamage;
    [SerializeField] protected float baseRateOfFire;
    protected float rateOfFireCounter = 0.0f;

    public enum FireType { Single, Auto }
    [SerializeField] protected FireType fireType = FireType.Single;

    public enum AmmoType { Integer, Percentage }
    [SerializeField] protected AmmoType ammoType = AmmoType.Integer;

    [SerializeField] protected float totalAmmo;     // How much total ammo we have
    [SerializeField] protected float clipCapacity;  // How much each clip holds
    [SerializeField] protected float currentClipAmount;   // Current ammo inside the ammo clip

    [SerializeField] protected GameObject ammo; // The prefab to spawn when weapon shoots

    [SerializeField] protected float reloadingTime = 2.0f;
    [SerializeField] protected bool isReloading = false;

    public virtual void Fire() { }

    public virtual void Shoot() { }

    public virtual IEnumerator Reload() { yield return null; }

}
