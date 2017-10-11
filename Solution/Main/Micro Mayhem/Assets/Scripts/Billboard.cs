using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    [SerializeField] private GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 lookPosition = target.transform.position - transform.position;        
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        //transform.localRotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5.0f);
	}
}
