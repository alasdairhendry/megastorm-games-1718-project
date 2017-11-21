using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDeadeye : WeaponBase {

    [SerializeField] Transform shootPoint;
    [SerializeField] private Text ammoHUDTarget = null;

    private void Start()
    {
        shootPoint = transform.Find("Shootpoint");
    }

    private void Update()
    {
        if (GameState.singleton.IsPaused)
            return;

        rateOfFireCounter += Time.deltaTime;

        if (Input.GetMouseButton(1))
            Fire();
    }

    public override void SendAmmoData(Text target)
    {
        target.text = base.currentClipAmount + " / " + base.clipCapacity;

        if (ammoHUDTarget == null)
            ammoHUDTarget = target;
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
        bulletClass.Init(Vector3.zero, 15, baseDamage);

        currentClipAmount -= 1;
    }

    public override IEnumerator Reload()
    {
        isReloading = true;

        while (GameState.singleton.IsPaused)
            yield return null;

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
