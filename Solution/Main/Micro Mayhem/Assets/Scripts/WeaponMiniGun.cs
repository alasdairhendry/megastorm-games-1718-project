using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMiniGun : WeaponBase {

    [SerializeField] Transform shootPoint;
    [SerializeField] private Text ammoHUDTarget;

    private void Start()
    {
        shootPoint = transform.Find("Shootpoint");
        ammoHUDTarget = GameObject.Find("WeaponryCanvas").transform.Find("PrimaryWeapon").Find("Ammo").GetComponent<Text>();
    }

    private void Update()
    {
        rateOfFireCounter += Time.deltaTime;

        if (Input.GetMouseButton(0)) 
            Fire();
    }

    public override void SendAmmoData(Text target)
    {
        if (!isReloading)
        {
            target.text = base.currentClipAmount + " / " + base.clipCapacity;
        }
        else
        {
            target.text = "Reloading...";
        }
    }

    public override void Fire()
    {
        if (currentClipAmount > 0)
        {
            if (rateOfFireCounter >= baseRateOfFire)
            {
                rateOfFireCounter = 0;
                Shoot();
            }
        }
        else
        {
            if (!isReloading)
                StartCoroutine(Reload());
        }

        SendAmmoData(ammoHUDTarget);
    }

    public override void Shoot()
    {
        GameObject bullet = Instantiate(ammo);
        bullet.transform.position = shootPoint.position;

        Vector3 direction = GameObject.Find("Crosshair_Prefab").transform.position - bullet.transform.position;
        bullet.transform.rotation = Quaternion.LookRotation(direction);

        Bullet bulletClass = bullet.GetComponent<Bullet>();
        bulletClass.Init(Vector3.zero, 15, 5);

        currentClipAmount -= 1;
        GetComponent<AudioSource>().Play();
    }

    public override IEnumerator Reload()
    {
        isReloading = true;
        SoundEffectManager.singleton.Play2DSound(reloadSound, false, 0.0f, 1.0f);
        GameObject.FindObjectOfType<PlayerAttack>().GetComponentInChildren<Animator>().SetTrigger("Reload");
        SendAmmoData(ammoHUDTarget);
        yield return new WaitForSeconds(reloadingTime);
        
        if(totalAmmo == -1)
        {
            currentClipAmount = clipCapacity;
        }
        else
        {
            if (totalAmmo < clipCapacity)
            {
                currentClipAmount = totalAmmo;
                totalAmmo = 0;
            }
            else
            {
                currentClipAmount = clipCapacity;
                totalAmmo -= clipCapacity;
            }
        }

        isReloading = false;
        SendAmmoData(ammoHUDTarget);
    }
}
