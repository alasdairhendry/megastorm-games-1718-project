using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotation : MonoBehaviour {

    [SerializeField] private Vector3 localAxis = new Vector3();
    [SerializeField] private float speed = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(localAxis * Time.deltaTime * speed);
	}
}
