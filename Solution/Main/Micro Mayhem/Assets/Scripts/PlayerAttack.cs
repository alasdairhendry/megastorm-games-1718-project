using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IDamageable {

    private Animator animator;

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject mountPoint;
    [SerializeField] private GameObject crosshair;

    [SerializeField] private float maximumHealth;
    [SerializeField] private float currentHealth;

    float IDamageable.MaximumHealth { get { return maximumHealth; } set { maximumHealth = value; } }
    float IDamageable.CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    [SerializeField] GameObject impactParticle;
    GameObject IDamageable.ImpactParticle { get { return impactParticle; } }

    [SerializeField] List<WeaponBase> weapons = new List<WeaponBase>();
    [SerializeField] GameObject secondaryMountPoint;

    void IDamageable.Die()
    {
        Debug.Log("Dead");
    }

    void IDamageable.TakeDamage(float damage)
    {
        ((IDamageable)this).CurrentHealth -= damage;

        if (((IDamageable)this).CurrentHealth <= 0)
            ((IDamageable)this).Die();

    }

    public void EquipWeapon(GameObject weapon)
    {
        if(weapons.Count == 1) // We only have our mini gun
        {
            weapons.Add(weapon.GetComponent<WeaponBase>());

            GameObject go = Instantiate(weapon);
            go.transform.parent = secondaryMountPoint.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;

            animator.SetBool("SecondaryWeapon", true);
        }
        else if(weapons.Count == 2) // We currently are equipping another weapon
        {
            Destroy(secondaryMountPoint.transform.GetChild(0).gameObject);

            weapons[1] = weapon.GetComponent<WeaponBase>();

            GameObject go = Instantiate(weapon);
            go.transform.parent = secondaryMountPoint.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;

            animator.SetBool("SecondaryWeapon", true);
        }
    }

    // Use this for initialization
    void Start () {
        currentHealth = maximumHealth;

        animator = GetComponentInChildren<Animator>();
        weapons.Add(GameObject.FindObjectOfType<WeaponMiniGun>());
	}
	
	// Update is called once per frame
	void Update () {

	}
}
