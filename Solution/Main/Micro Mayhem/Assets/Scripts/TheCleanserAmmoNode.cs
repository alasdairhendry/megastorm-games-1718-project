using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCleanserAmmoNode : MonoBehaviour
{

    //    private WeaponTheCleanser root;

    //    private Damageable previousTarget;
    //    private Damageable currentTarget;

    //    // Use this for initialization
    //    public void Init (WeaponTheCleanser _root) {
    //        root = _root;
    //	}

    //    // Update is called once per frame
    //    private void Update () {
    //        CalculateTargets();

    //        if(currentTarget != null)
    //        Debug.DrawRay(this.transform.position, (currentTarget.transform.position - this.transform.position) * Vector3.Distance(this.transform.position, currentTarget.transform.position));
    //    }

    //    private void CalculateTargets()
    //    {
    //        previousTarget = currentTarget;
    //        currentTarget = ReturnClosestDamage();  // Find the closest damageable

    //        if (currentTarget == previousTarget)
    //            return;

    //        // Our current closest target is new, we need to destroy our old and create our new
    //        if (previousTarget != null)
    //        {
    //            if (previousTarget.GetComponent<TheCleanserAmmoNode>() != null)
    //            {
    //                Destroy(previousTarget.GetComponent<TheCleanserAmmoNode>());
    //            }

    //            root.RemoveTarget(previousTarget);
    //        }

    //        if (currentTarget != null)
    //        {
    //            if (currentTarget.GetComponent<TheCleanserAmmoNode>() == null)
    //            {
    //                currentTarget.gameObject.AddComponent<TheCleanserAmmoNode>();
    //                currentTarget.GetComponent<TheCleanserAmmoNode>().Init(root);
    //                root.AddTarget(currentTarget);
    //            }
    //        }
    //    }

    //    private void OnDestroy()
    //    {
    //        if (currentTarget != null)
    //            DestroyImmediate(currentTarget.GetComponent<TheCleanserAmmoNode>());
    //    }

    //    private Damageable ReturnClosestDamage()
    //    {
    //        Damageable[] damagables = GameObject.FindObjectsOfType<Damageable>();

    //        float minDistance = Mathf.Infinity;
    //        Damageable target = null;

    //        foreach (Damageable damagable in damagables)
    //        {
    //            if (Vector3.Distance(this.transform.position, damagable.transform.position) < minDistance)
    //            {
    //                if (root.CheckStreamContains(damagable.GetComponent<IDamageable>()) == false)
    //                {
    //                    if (damagable.GetComponent<TheCleanserAmmoNode>() != null)
    //                        continue;

    //                    minDistance = Vector3.Distance(this.transform.position, damagable.transform.position);
    //                    target = damagable;
    //                }
    //            }
    //        }

    //        return target;
    //    }
}
