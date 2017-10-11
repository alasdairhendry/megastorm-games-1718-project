using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMiniGun : WeaponBase {

   [SerializeField] Transform shootPoint;

    private void Start()
    {
        shootPoint = transform.Find("Shootpoint");
    }

    private void Update()
    {
        rateOfFireCounter += Time.deltaTime;

        if (Input.GetMouseButton(0)) 
            Fire();
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
    }

    public override IEnumerator Reload()
    {
        isReloading = true;
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
    }
}
