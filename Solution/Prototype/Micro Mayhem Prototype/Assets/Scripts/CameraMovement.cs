using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField] private Vector3 offsetPosition = new Vector3();
    private Transform player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player_EMP").transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(45.0f, transform.eulerAngles.y, transform.eulerAngles.z);

        Movement();
    }

    private void Movement()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = player.position + offsetPosition;

        Vector3 bounds = GameObject.FindObjectOfType<CameraBounds>().bounds;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -(bounds.x / 2), bounds.x / 2);
        targetPosition.z = Mathf.Clamp(targetPosition.z, -(bounds.z / 2), bounds.z / 2);

        transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref velocity, Time.deltaTime);
    }
}
