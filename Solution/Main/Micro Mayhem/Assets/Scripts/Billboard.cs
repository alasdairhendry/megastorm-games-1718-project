﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic Class that rotates towards a given target
/// </summary>
public class Billboard : MonoBehaviour {

    [SerializeField] private GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameState.singleton.IsPaused)
            return;

        Vector3 lookPosition = target.transform.position - transform.position;

        lookPosition.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        //transform.localRotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5.0f);
	}
}
