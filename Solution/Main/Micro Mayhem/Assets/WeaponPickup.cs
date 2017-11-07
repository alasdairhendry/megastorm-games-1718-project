using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponPrefabs;
    private GameObject weaponPrefab;

    private void Start()
    {
        PickWeapon();
        SetDisplay();
    }

    private void PickWeapon()
    {
        weaponPrefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Length - 1)];        
    }

    private void SetDisplay()
    {
        GetComponentInChildren<TextMesh>().text = weaponPrefab.GetComponent<WeaponBase>().GetName;

        Transform children = transform.Find("Graphics");
        foreach (Transform child in children)
        {
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
        if (Vector3.Distance(GameObject.FindObjectOfType<PlayerAttack>().transform.position, this.transform.position) <= 3.5f)
        {
            print("F to pickup " + weaponPrefab.GetComponent<WeaponBase>().GetName);

            if(Input.GetKeyDown(KeyCode.F))
            {
                GameObject.FindObjectOfType<PlayerAttack>().EquipWeapon(weaponPrefab);
                Destroy(this.gameObject);
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
}
