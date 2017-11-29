using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTexture : MonoBehaviour {

    private Material[] materials;

	// Use this for initialization
	void Start () {
        materials = GetComponent<Renderer>().materials;
	}
	
	// Update is called once per frame
	void Update () {        
        materials[1].mainTextureOffset += new Vector2(0.01f, 0.02f) * Time.deltaTime;        
    }
}
