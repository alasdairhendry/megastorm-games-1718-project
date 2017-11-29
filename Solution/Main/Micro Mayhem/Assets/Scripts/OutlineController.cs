using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(GetOutlines());
    }
	
	// Update is called once per frame
	void Update () {
  
	}

    private IEnumerator GetOutlines()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);

            Outline[] outlines = GameObject.FindObjectsOfType<Outline>();

            foreach (Outline outline in outlines)
            {
                Ray ray = new Ray(Camera.main.transform.position, outline.transform.position - Camera.main.transform.position);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit, 500.0f))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Geometry"))
                    {
                        outline.enabled = true;
                    }
                    else outline.enabled = false;
                }
                else
                {
                    outline.enabled = false;
                }
            }

            yield return null;
        }
    }
}
