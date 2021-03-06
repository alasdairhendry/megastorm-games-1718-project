﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unused.
/// </summary>
public class TEST_ParticleAttractor : MonoBehaviour {

    [SerializeField] private ParticleSystem target;
    ParticleSystem.Particle[] particleList = new ParticleSystem.Particle[1000];

	// Use this for initialization
	public void Init (ParticleSystem _target) {
        target = _target;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameState.singleton.IsPaused)
            return;

        int count = target.GetParticles(particleList);

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particleList[i];      
            
            particle.position = Vector3.Lerp(particle.position, this.transform.position, Time.deltaTime * 1.0f);
            particleList[i] = particle;
        }

        target.SetParticles(particleList, count);
	}

    private void OnDestroy()
    {
        Destroy(target.gameObject);
    }
}
