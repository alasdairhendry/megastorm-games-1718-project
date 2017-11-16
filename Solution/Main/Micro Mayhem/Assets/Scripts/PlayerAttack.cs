using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private GameObject secondaryWeaponGUIContainer;

    void IDamageable.Die()
    {
        Debug.Log("Dead");
        SceneManager.LoadScene(0);
    }

    void IDamageable.TakeDamage(float damage)
    {
        DamageFloaters.singleton.AddFloater(damage.ToString(), Color.red, this.transform, new Vector3(0, 1.5f, -1.0f), 1);
        ((IDamageable)this).CurrentHealth -= damage;

        if (((IDamageable)this).CurrentHealth <= 0)
            ((IDamageable)this).Die();

    }

    public void EquipWeapon(GameObject weapon)
    {
        if(weapons.Count == 1) // We only have our mini gun
        {            
            GameObject go = Instantiate(weapon);

            weapons.Add(go.GetComponent<WeaponBase>());

            go.transform.parent = secondaryMountPoint.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;

            secondaryWeaponGUIContainer.transform.Find("Name").GetComponent<Text>().text = weapon.GetComponent<WeaponBase>().GetName;
            go.GetComponent<WeaponBase>().SendAmmoData(secondaryWeaponGUIContainer.transform.Find("Ammo").GetComponent<Text>());
            secondaryWeaponGUIContainer.transform.Find("Image").GetComponent<Image>().sprite = weapon.GetComponent<WeaponBase>().GetIcon;
            secondaryWeaponGUIContainer.transform.parent.GetComponent<Animator>().SetBool("SecondaryWeapon", true);

            animator.SetBool("SecondaryWeapon", true);
        }
        else if(weapons.Count == 2) // We currently are equipping another weapon
        {
            DropSecondary();            

            GameObject go = Instantiate(weapon);

            weapons.Add(go.GetComponent<WeaponBase>());

            go.transform.parent = secondaryMountPoint.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;

            secondaryWeaponGUIContainer.transform.Find("Name").GetComponent<Text>().text = weapon.GetComponent<WeaponBase>().GetName;
            go.GetComponent<WeaponBase>().SendAmmoData(secondaryWeaponGUIContainer.transform.Find("Ammo").GetComponent<Text>());
            secondaryWeaponGUIContainer.transform.Find("Image").GetComponent<Image>().sprite = weapon.GetComponent<WeaponBase>().GetIcon;
            secondaryWeaponGUIContainer.transform.parent.GetComponent<Animator>().SetBool("SecondaryWeapon", true);

            animator.SetBool("SecondaryWeapon", true);
        }
    }

    public void DropSecondary()
    {
        if(weapons.Count == 2)
        {                     
            weapons[1] = null;
            secondaryWeaponGUIContainer.transform.parent.GetComponent<Animator>().SetBool("SecondaryWeapon", false);
            animator.SetBool("SecondaryWeapon", false);
            StartCoroutine(DropSecondaryModel());
            weapons.RemoveAt(1);
        }
    }

    private IEnumerator DropSecondaryModel()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject secondaryWeapon = secondaryMountPoint.transform.GetChild(0).gameObject;
        secondaryWeapon.AddComponent<Rigidbody>();
        secondaryWeapon.GetComponent<Rigidbody>().AddForce(Vector3.up * 15.0f, ForceMode.Impulse);
        secondaryWeapon.transform.parent = null;
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
