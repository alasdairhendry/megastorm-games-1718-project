using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private float resetDelay = -1.0f;
    private bool isActive = true;

    public Action OnHover;
    public Action<WeaponBase> OnWeaponPickup;

    private void Start()
    {
        StartCoroutine(ResetWeapon(0.0f));
        OnHover += Tutorial.singleton.OnHoverWeaponPickup;
        OnWeaponPickup += Tutorial.singleton.OnWeaponPickup;
    }

    private void PickWeapon()
    {
        weaponPrefab = weaponPrefabs[UnityEngine.Random.Range(0, weaponPrefabs.Length)];        
    }

    private void SetDisplay()
    {
        if (weaponPrefab == null)
            GetComponentInChildren<TextMesh>().text = "Not Ready";
        else
            GetComponentInChildren<TextMesh>().text = weaponPrefab.GetComponent<WeaponBase>().GetName;

        Transform children = transform.Find("Graphics").Find("Weapons");
        foreach (Transform child in children)
        {
            if(weaponPrefab == null)
            {
                child.gameObject.SetActive(false);
                continue;
            }

            if(child.name == weaponPrefab.GetComponent<WeaponBase>().GetName)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (GameState.singleton.IsPaused)
            return;

        if (isActive)
            Monitor();
    }

    private void Monitor()
    {
        if (Vector3.Distance(GameObject.FindObjectOfType<PlayerAttack>().transform.position, this.transform.position) <= 3.5f)
        {
            if (OnHover != null)
                OnHover();            

            if (Input.GetKeyDown(KeyCode.F))
            {
                GameObject.FindObjectOfType<PlayerAttack>().EquipWeapon(weaponPrefab);
                OnWeaponPickup(weaponPrefab.GetComponent<WeaponBase>());

                if (resetDelay == -1)
                    Destroy(gameObject);
                else
                {
                    StartCoroutine(ResetWeapon(resetDelay));
                    weaponPrefab = null;
                    SetDisplay();
                    isActive = false;
                }
            }

            Color green = new Color();
            ColorUtility.TryParseHtmlString("#3BDF5EFF", out green);
            GetComponentInChildren<Light>().color = green;
        }
        else
        {
            Color blue = new Color();
            ColorUtility.TryParseHtmlString("#B2D5F2FF", out blue);
            GetComponentInChildren<Light>().color = blue;
        }

        Vector3 lookRot = GetComponentInChildren<TextMesh>().transform.position - Camera.main.transform.position;
        lookRot.y = 0;
        GetComponentInChildren<TextMesh>().transform.rotation = Quaternion.LookRotation(lookRot);
    }

    private IEnumerator ResetWeapon(float delay)
    {
        yield return new WaitForSeconds(delay);        
        PickWeapon();
        SetDisplay();
        isActive = true;
    }
}
