using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    [SerializeField] private GameObject weaponPrefab;

    private void Start()
    {
        GetComponentInChildren<TextMesh>().text = weaponPrefab.GetComponent<WeaponBase>().GetName;
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
        }

        Vector3 lookRot = GetComponentInChildren<TextMesh>().transform.position - Camera.main.transform.position;
        lookRot.y = 0;
        GetComponentInChildren<TextMesh>().transform.rotation = Quaternion.LookRotation(lookRot);
    }
}
