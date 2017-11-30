using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to lock the rotation of a transform
/// </summary>
public class RotationLock : MonoBehaviour {

    private enum LockType { Global, Local }
    [SerializeField] private LockType lockType;
    [SerializeField] private bool x;
    [SerializeField] private bool y;
    [SerializeField] private bool z;
    [SerializeField] private Vector3 rotation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (lockType == LockType.Global)
        {
            Vector3 newRotation = new Vector3();

            if (x)
                newRotation.x = rotation.x;
            else
                newRotation.x = transform.eulerAngles.x;

            if (y)
                newRotation.y = rotation.y;
            else
                newRotation.y = transform.eulerAngles.y;

            if (x)
                newRotation.z = rotation.z;
            else
                newRotation.z = transform.eulerAngles.z;

            transform.eulerAngles = newRotation;
        }
        else if (lockType == LockType.Local)
        {
            Vector3 newRotation = new Vector3();

            if (x)
                newRotation.x = rotation.x;
            else
                newRotation.x = transform.localEulerAngles.x;

            if (y)
                newRotation.y = rotation.y;
            else
                newRotation.y = transform.localEulerAngles.y;

            if (x)
                newRotation.z = rotation.z;
            else
                newRotation.z = transform.localEulerAngles.z;

            transform.localEulerAngles = newRotation;
        }
    }
}
