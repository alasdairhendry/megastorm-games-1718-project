using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

    [SerializeField] private AudioClip clip;

	// Use this for initialization
	void Start () {
        SoundEffectManager.singleton.Play2DSound(clip, true, 0.0f, 0.25f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
