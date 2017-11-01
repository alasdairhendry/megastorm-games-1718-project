using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTheCleanser : WeaponBase {

    [SerializeField] Transform shootPoint;
    [SerializeField] private Text ammoHUDTarget = null;

    private bool isShooting = false;

    private void Start()
    {
        shootPoint = transform.Find("Shootpoint");
    }

    private void Update()
    {
        rateOfFireCounter += Time.deltaTime;

        if (Input.GetMouseButton(1))
            Fire();
    }

    public override void SendAmmoData(Text target)
    {
        target.text = base.currentClipAmount.ToString("00") + "%";

        if(ammoHUDTarget==null)
            ammoHUDTarget = target;
    }

    public override void Fire()
    {
        if (currentClipAmount > 0)
        {
            currentClipAmount -= baseRateOfFire * Time.deltaTime;
            Shoot();
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
        GameObject[] damagables = GameObject.FindGameObjectsWithTag("Enemy");

        float minDistance = Mathf.Infinity;
        GameObject target = null;

        foreach (GameObject damagable in damagables)
        {
            if(Vector3.Distance(this.transform.position, damagable.transform.position) < minDistance)
            {
                minDistance = Vector3.Distance(this.transform.position, damagable.transform.position);
                target = damagable;
            }
        }

        print(target.name);
    }

    public override IEnumerator Reload()
    {
        isReloading = true;

        if (totalAmmo == 0)
        {
            GameObject.FindObjectOfType<PlayerAttack>().DropSecondary();
        }

        yield return new WaitForSeconds(reloadingTime);

        if (totalAmmo == -1)    // -1 total ammo indicates that the gun has an unlimited supply of ammo clips
        {
            currentClipAmount = clipCapacity;
        }
        else    // If the total ammo doesnt equal -1, then we have a finite amount of clips we can use. Set total ammo to 0 to make a "one clip" weapon
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
    }
}
