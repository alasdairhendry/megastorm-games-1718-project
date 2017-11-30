using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unused.
/// </summary>
public class TEST_Moveforward : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameState.singleton.IsPaused)
            return;

        transform.position += transform.forward * Time.deltaTime * 20.0f;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().TakeDamage(15);
            GameObject particle = Instantiate(other.GetComponent<IDamageable>().ImpactParticle);
            particle.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
