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

    [SerializeField] private float healthRegenRate = 5.0f;

    float IDamageable.MaximumHealth { get { return maximumHealth; } set { maximumHealth = value; } }
    float IDamageable.CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    string IDamageable.EntityType { get { return entityType; } set { entityType = value; } }
    [SerializeField] protected string entityType = "friendly";

    [SerializeField] GameObject impactParticle;
    GameObject IDamageable.ImpactParticle { get { return impactParticle; } }

    [SerializeField] List<WeaponBase> weapons = new List<WeaponBase>();
    [SerializeField] GameObject secondaryMountPoint;
    [SerializeField] private GameObject secondaryWeaponGUIContainer;
    [SerializeField] private RectTransform healthBar;    

    void IDamageable.Die()
    {
        //Debug.Log("Dead");
        LevelFinishedOverlay.singleton.PlayerDied();
    }

    void IDamageable.TakeDamage(float damage)
    {
        //DamageFloaters.singleton.AddFloater(damage.ToString("00"), Color.red, this.transform, new Vector3(0, 1.5f, -1.0f), 1);
        AddDamageFloater(damage.ToString("00"));
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
            weapons.RemoveAt(1);

            GameObject secondaryWeapon = secondaryMountPoint.transform.GetChild(0).gameObject;
            secondaryWeapon.AddComponent<Rigidbody>();
            secondaryWeapon.GetComponent<Rigidbody>().AddForce(Vector3.up * 5.0f, ForceMode.Impulse);
            secondaryWeapon.transform.parent = null;
            secondaryWeapon.GetComponent<WeaponBase>().isActiveGun = false;
        }
    }

    private void UpdateHealthBar()
    {
        float t = currentHealth / maximumHealth;
        float width = Mathf.Lerp(0.0f, 277.0f, t);

        healthBar.sizeDelta = new Vector2(width, healthBar.sizeDelta.y);
    }

    private void RegenerateHealth()
    {
        if (currentHealth < maximumHealth * 0.25f && currentHealth > 0.0f)
        {
            currentHealth += Time.deltaTime * healthRegenRate;

            if (currentHealth >= maximumHealth * 0.25f)
                currentHealth = maximumHealth * 0.25f;
        }

        if (currentHealth < maximumHealth * 0.50f && currentHealth > maximumHealth * 0.25f)
        {
            currentHealth += Time.deltaTime * healthRegenRate;

            if (currentHealth >= maximumHealth * 0.50f)
                currentHealth = maximumHealth * 0.50f;
        }

        if (currentHealth < maximumHealth * 0.75f && currentHealth > maximumHealth * 0.50f)
        {
            currentHealth += Time.deltaTime * healthRegenRate;

            if (currentHealth >= maximumHealth * 0.75f)
                currentHealth = maximumHealth * 0.75f;
        }

        if (currentHealth < maximumHealth * 1.0f && currentHealth > maximumHealth * 0.75f)
        {
            currentHealth += Time.deltaTime * healthRegenRate;

            if (currentHealth >= maximumHealth * 1.0f)
                currentHealth = maximumHealth * 1.0f;
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
        if (GameState.singleton.IsPaused)
            return;

        UpdateHealthBar();
        RegenerateHealth();
        MonitorDamageFloaters();
	}

    private float damageFloatersMinWait = 0.15f;
    private float damageFloaterCurrCounter = 0.0f;

    private List<string> currentFloaters = new List<string>();

    private void MonitorDamageFloaters()
    {
        damageFloaterCurrCounter += Time.deltaTime;

        if (damageFloaterCurrCounter >= damageFloatersMinWait)
        {
            SendDamageFloaters();
            damageFloaterCurrCounter = 0.0f;
        }
    }

    private void SendDamageFloaters()
    {
        float allDamage = 0.0f;

        foreach (string floater in currentFloaters)
        {
            allDamage += float.Parse(floater);
        }

        if (allDamage != 0)
            DamageFloaters.singleton.AddFloater(allDamage.ToString("00"), Color.red, this.transform, new Vector3(0.0f, 2.0f, -1.0f), 1);

        currentFloaters.Clear();
    }

    protected void AddDamageFloater(string text)
    {
        if (currentFloaters.Count == 0)
            damageFloaterCurrCounter = 0;

        currentFloaters.Add(text);
    }
}
