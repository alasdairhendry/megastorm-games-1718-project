using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slows the movement of the player when active
/// </summary>
public class FreezeMovement : MonoBehaviour {

    [SerializeField] [Range(0, 1)] private float percentageDecrease = 0.5f;
    [SerializeField] private float lifetime = 1.0f;
    private float initialSpeed = 0;

	// Use this for initialization
	void Start () {        
        initialSpeed = GameObject.FindObjectOfType<PlayerMovement>().MovementSpeed;
        GameObject.FindObjectOfType<PlayerMovement>().MovementSpeed = initialSpeed * percentageDecrease;

        StartCoroutine(Die());
    }
	
	private IEnumerator Die()
    {
        yield return new WaitForSeconds(lifetime);

        while (GameState.singleton.IsPaused)
            yield return null;

        GameObject.FindObjectOfType<PlayerMovement>().MovementSpeed = initialSpeed;
        //GameObject.Find("Frozen_Overlay").transform.GetChild(0).gameObject.SetActive(false);
        Destroy(this);
    }
}
