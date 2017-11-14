using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTheCleanser : WeaponBase {

    [SerializeField] Transform shootPoint;
    [SerializeField] private GameObject particleSystemPrefab;
    [SerializeField] List<LineRenderBeam> particles;
    [SerializeField] private Text ammoHUDTarget = null;

    private bool isShooting = false;

    [SerializeField]  private List<Damageable> currentTargets = new List<Damageable>();
    //private List<LineRenderBeam> currentTargetLR = new List<LineRenderBeam>();

    private void Start()
    {
        foreach (LineRenderBeam particle in particles)
        {
            if (particle != null)
                particle.Stop();
        }
        shootPoint = transform.Find("Shootpoint");
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Fire();
            ApplyDamage();
        }
        else
        {
            foreach (LineRenderBeam particle in particles)
            {
                if (particle != null)
                    particle.Stop();
            }
            currentTargets.Clear();

            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i] != null)
                    Destroy(particles[i].gameObject);
            }
            particles.Clear();
        }
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
            currentClipAmount -= baseRateOfFire * Time.deltaTime * currentTargets.Count;
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
        FindEnemiesInRange();  // Find the closest damageable
        if (currentTargets.Count == 0)
            foreach (LineRenderBeam particle in particles)
            {
                if (particle != null)
                    particle.Stop();
            }
        else
            foreach (LineRenderBeam particle in particles)
            {
                if (particle != null)
                    particle.Resume();
            }
    }

    public void ApplyDamage()
    {
        for (int i = 0; i < currentTargets.Count; i++)
        {
            if (currentTargets[i] == null)
                currentTargets.RemoveAt(i);
            else
                currentTargets[i].GetComponent<IDamageable>().TakeDamage(baseDamage);
        }
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

    private void FindEnemiesInRange()
    {
        Damageable[] damagables = GameObject.FindObjectsOfType<Damageable>();

        foreach (Damageable damagable in damagables)
        {
            if (Vector3.Distance(this.transform.position, damagable.transform.position) < 15.0f)
            {
                if (CheckStreamContains(damagable) == false)
                {                  
                    GameObject particle = Instantiate(particleSystemPrefab) as GameObject;
                    particle.transform.parent = shootPoint.transform;
                    particle.transform.localPosition = Vector3.zero;
                    particle.transform.localScale = Vector3.one;
                    particles.Add(particle.GetComponent<LineRenderBeam>());

                    print(damagable.gameObject.name);
                    particle.GetComponent<LineRenderBeam>().Play(damagable.gameObject.transform);
                    AddTarget(damagable);
                    //print("BOOP");
                    //print(currentTargets.Count + " count");
                }
            }
            else
            {
                RemoveTarget(damagable);
            }
        }
    }

    public bool CheckStreamContains(Damageable value)
    {
        foreach (Damageable damageable in currentTargets)
        {
            if(currentTargets.Contains(value))
            {
                return true;
            }            
        }

        // We have exited the loop, therefore the value does not exist in the list
        return false;
    }

    public void AddTarget(Damageable target)
    {        
        currentTargets.Add(target);
    }

    public void RemoveTarget(Damageable target)
    {
        for (int i = 0; i < currentTargets.Count; i++)
        {
            if(currentTargets[i] == target)
            {
                currentTargets.RemoveAt(i);
            }
        }

        for (int i = 0; i < particles.Count; i++)
        {
            Destroy(particles[i].gameObject);        
        }
        particles.Clear();
    }
}
