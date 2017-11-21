using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {

    Action onDeath;
    float lifeTime;

	// Use this for initialization
	public void Init (Action _onDeath, float _lifeTime) {
        onDeath = _onDeath;
        lifeTime = _lifeTime;
        GetComponentInChildren<Animator>().speed = 1.0f / lifeTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameState.singleton.IsPaused)
            return;

        lifeTime -= Time.deltaTime;

        if(lifeTime<= 0)
        {
            if (onDeath != null)
                onDeath();

            Destroy(gameObject);
        }
	}


}
