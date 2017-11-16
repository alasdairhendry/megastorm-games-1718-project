using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerFireBall : MonoBehaviour {

    Transform player;
    float speed = 1.0f;

	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<PlayerAttack>().transform;        
	}
	
	// Update is called once per frame
	void Update () {
        speed += Time.deltaTime * 2.5f;
        Vector3 direction = (GameObject.FindObjectOfType<PlayerAttack>().transform.position + new Vector3(0, 1.0f, 0)) - this.transform.position;        

        transform.position += direction * Time.deltaTime * 1.5f * speed;

        if (Vector3.Distance(player.position + new Vector3(0, 1.0f, 0), transform.position) < 1.0f)
        {
            player.GetComponent<IDamageable>().TakeDamage(10);
            Destroy(gameObject);
        }
	}
}
