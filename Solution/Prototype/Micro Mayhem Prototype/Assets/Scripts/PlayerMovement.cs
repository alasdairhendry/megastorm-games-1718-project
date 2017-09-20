using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Ray ray;
    private RaycastHit hit;
    private Camera mainCamera;
    private Animator anim;

    private float targetYRotation = 0.0f;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 5.0f;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
        anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Movement();
        Rotation();
        Jump();        
    }

    private void Movement()
    {
        Vector3 inputMovement = Input.GetAxis("Vertical") * transform.forward * Time.deltaTime * movementSpeed;
        GetComponent<Rigidbody>().velocity = new Vector3(inputMovement.x, GetComponent<Rigidbody>().velocity.y, inputMovement.z);
        anim.SetInteger("WalkState", (int)Input.GetAxis("Vertical"));
    }

    private void Rotation()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name == "Ground")
            {
                float angle = AngleBetweenTwoPoints(hit.point, transform.position); 
                targetYRotation = -angle + 90;
            }
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetYRotation, 0), rotationSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 250.0f * Time.deltaTime, ForceMode.Impulse);
        }
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.z - b.z, a.x - b.x) * Mathf.Rad2Deg;
    }
}
