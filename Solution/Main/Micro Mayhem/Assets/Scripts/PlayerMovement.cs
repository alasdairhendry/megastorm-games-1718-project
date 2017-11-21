using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Animator animator;
    new private Rigidbody rigidbody;

    [SerializeField] private LayerMask rotationRayMask;
    [SerializeField] private GameObject upperRotationalBone;
    [SerializeField] private float movementSpeed = 15.0f; 
    public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }

    [SerializeField] private GameObject crosshair;

	// Use this for initialization
	void Start () {
        Physics.autoSimulation = true;
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GameState.singleton.IsPaused)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }

        LowerBodyMovement();
    }

    void Update () {
        if (GameState.singleton.IsPaused)
            return;

        UpperBodyMovement();
	}

    private void LowerBodyMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");        

        //transform.position += new Vector3(horizontalInput, 0.0f, verticalInput).normalized * movementSpeed * Time.deltaTime;
        rigidbody.velocity = new Vector3(horizontalInput, 0.0f, verticalInput).normalized * movementSpeed * Time.deltaTime;

        animator.SetFloat("ForwardMotion", verticalInput);
        animator.SetFloat("SidewardMotion", horizontalInput);
    }

    private void UpperBodyMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 500.0f, rotationRayMask))
        {
            float angle = Mathf.Atan2(hit.point.z - transform.position.z, hit.point.x - transform.position.x) * Mathf.Rad2Deg - 90;
            if (angle < 0)
                angle += 360;            

            upperRotationalBone.transform.eulerAngles = new Vector3(upperRotationalBone.transform.eulerAngles.x, -angle, upperRotationalBone.transform.eulerAngles.z);
        }

        if(Physics.Raycast(ray, out hit))
        {
            //print(hit.collider.gameObject.name);
            crosshair.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            crosshair.transform.position += crosshair.transform.InverseTransformDirection(crosshair.transform.Find("Graphics").forward).normalized * 0.1f;

            Quaternion newRot = Quaternion.LookRotation(hit.normal);
            crosshair.transform.Find("Graphics").rotation = Quaternion.Slerp(crosshair.transform.Find("Graphics").rotation, newRot, Time.deltaTime * 5.0f);            
        }
    }
}
