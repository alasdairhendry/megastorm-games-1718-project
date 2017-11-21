using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    [SerializeField] private Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameState.singleton.IsPaused)
            return;

        if (target == null)
            transform.LookAt(Camera.main.transform);
        else
            transform.LookAt(target.transform);
	}
}
