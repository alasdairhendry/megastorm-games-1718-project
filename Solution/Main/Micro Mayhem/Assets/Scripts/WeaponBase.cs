using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WeaponBase : MonoBehaviour {

    [SerializeField] protected string weaponName;
    public string GetName { get { return weaponName; } }

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

    public bool isActiveGun = true;

    [SerializeField] private Sprite icon;
    public Sprite GetIcon { get { return icon; } }

    [SerializeField] protected List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] protected AudioClip reloadSound;

    [SerializeField] private GameObject crumbleRoot;
    [SerializeField] private GameObject[] meshObjects;

    public virtual void Fire() { }

    public virtual void Shoot() { }

    public virtual void SendAmmoData(Text target)
    {
        target.text = "0 / 0";
    }

    protected void Crumble()
    {
        foreach (GameObject go in meshObjects)
        {
            go.GetComponent<MeshRenderer>().enabled = false;
        }

        GetComponent<Rigidbody>().isKinematic = true;
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        GetComponent<Rigidbody>().freezeRotation = true;

        crumbleRoot.SetActive(true);
        StartCoroutine(DestroyThis());
    }

    private IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(2.0f);

        while (GameState.singleton.IsPaused)
            yield return null;

        Destroy(gameObject);
    }

    public virtual IEnumerator Reload() { yield return null; }
}
